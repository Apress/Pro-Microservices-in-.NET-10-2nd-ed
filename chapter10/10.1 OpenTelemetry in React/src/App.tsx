import './App.css';
import FrontendTracer from './telemetry';
import Counter from './components/Counter';

const env = import.meta.env;
const OTEL_SERVICE_NAME = env.VITE_OTEL_SERVICE_NAME;
const OTEL_SERVICE_VERSION = env.VITE_OTEL_SERVICE_VERSION;
const OTEL_EXPORTER_OTLP_ENDPOINT = env.VITE_OTEL_EXPORTER_OTLP_ENDPOINT;
FrontendTracer({
  OTEL_SERVICE_NAME,
  OTEL_SERVICE_VERSION,
  OTEL_EXPORTER_OTLP_ENDPOINT
});

function App() {
  return (
    <>
      <h1>Pro Microservices in .NET 10</h1>
      <div className="card">
        <Counter />
      </div>
    </>
  );
}

export default App;
