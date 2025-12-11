namespace DistanceMicroservice.Tests.Fixtures;

public class DistanceMicroserviceApp(string environment = "Development", HttpResponseMessage? fakeHttpResponseMessage = null) : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment(environment);

        builder.ConfigureServices(services =>
        {
            // Add mock/test services to the builder here


            if (fakeHttpResponseMessage != null)
            {
                services.ConfigureHttpClientDefaults(c => c.ConfigurePrimaryHttpMessageHandler(() => new FakeHttpMessageHandler(fakeHttpResponseMessage)));
                
                var oldSettings = services.FirstOrDefault(s => s.ServiceType == typeof(GoogleRouteSettings));
                if (oldSettings != null)
                {
                    services.Remove(oldSettings);
                }

                // ensure they're both not blank and not the real service
                var testSettings = new GoogleRouteSettings
                {
                    ApiUrl = "http://fake.api/",
                    ApiKey = "test-key"
                };
                services.AddSingleton(testSettings);
            }

        });

        return base.CreateHost(builder);
    }
}
