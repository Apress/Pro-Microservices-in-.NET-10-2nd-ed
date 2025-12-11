import { CompositePropagator, W3CBaggagePropagator, W3CTraceContextPropagator } from '@opentelemetry/core';
import { WebTracerProvider } from '@opentelemetry/sdk-trace-web';
import { BatchSpanProcessor } from '@opentelemetry/sdk-trace-base';
import { registerInstrumentations } from '@opentelemetry/instrumentation';
import { getWebAutoInstrumentations } from '@opentelemetry/auto-instrumentations-web';
import { resourceFromAttributes, detectResources } from '@opentelemetry/resources';
import { browserDetector } from '@opentelemetry/opentelemetry-browser-detector';
import { ATTR_SERVICE_NAME, ATTR_SERVICE_VERSION } from '@opentelemetry/semantic-conventions';
import { OTLPTraceExporter } from '@opentelemetry/exporter-trace-otlp-http';

export interface TelemetryConfig {
  OTEL_SERVICE_NAME?: string;
  OTEL_SERVICE_VERSION?: string;
  OTEL_EXPORTER_OTLP_ENDPOINT?: string;
  OTEL_EXPORTER_OTLP_HEADERS?: string;
}

const FrontendTracer = async (config: TelemetryConfig) => {
  if (!config.OTEL_SERVICE_NAME || !config.OTEL_EXPORTER_OTLP_ENDPOINT) {
    return; // don't setup if it won't get sent anywhere
  }
  console.log('initializing OpenTelemetry with config:', config);

  const { ZoneContextManager } = await import('@opentelemetry/context-zone');

  let resource = resourceFromAttributes({
    [ATTR_SERVICE_NAME]: config.OTEL_SERVICE_NAME
  });
  if (config.OTEL_SERVICE_VERSION) {
    resource = resource.merge(resourceFromAttributes({
      [ATTR_SERVICE_VERSION]: config.OTEL_SERVICE_VERSION
    }));
  }
  const detectedResources = detectResources({detectors: [browserDetector]});
  resource = resource.merge(detectedResources);

  const provider = new WebTracerProvider({
    resource,
    spanProcessors: [
      new BatchSpanProcessor(
        new OTLPTraceExporter({
          headers: {}, // so it passes CORS checks and doesn't send `credentials: 'include'` in preflight
          url: config.OTEL_EXPORTER_OTLP_ENDPOINT +'/v1/traces'
        }),
        {
          scheduledDelayMillis: 500
        }
      )
    ]
  });

  const contextManager = new ZoneContextManager();

  provider.register({
    contextManager,
    propagator: new CompositePropagator({
      propagators: [
        new W3CBaggagePropagator(),
        new W3CTraceContextPropagator()]
    })
  });

  registerInstrumentations({
    tracerProvider: provider,
    instrumentations: [
      getWebAutoInstrumentations({
        '@opentelemetry/instrumentation-fetch': {
          propagateTraceHeaderCorsUrls: /.*/,
          clearTimingResources: true
        }
      })
    ]
  });
};

export default FrontendTracer;
