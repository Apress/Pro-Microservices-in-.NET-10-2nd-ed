namespace DistanceMicroservice.Tests.Integration;

public class DistanceMicroservice_Tests
{
    [Fact]
    public async Task CallFakeGoogle_ReturnsResults()
    {
        // Arrange
        Addresses addresses = new Addresses(
            OriginAddress: "123 Main St, Anytown, CA",
            DestinationAddress: "456 Lincoln Ave, Anytown, CA"
        );
        Routes expectedRoutes = new(routes: [
            new Route(distanceMeters: 123, duration: "days"),
            new Route(distanceMeters: 124, duration: "minutes")
        ]);
        var fakeResponseBody = JsonSerializer.Serialize(expectedRoutes);
        var expectedMessage = "Number of routes found: 2";

        var fakeMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(fakeResponseBody, Encoding.UTF8, "application/json")
        };

        using var siteApp = new DistanceMicroserviceApp(fakeHttpResponseMessage: fakeMessage);
        using var httpClient = siteApp.CreateClient();

        // Act
        var res = await httpClient.PostAsJsonAsync("/getdistanceinfo", addresses);
        res.EnsureSuccessStatusCode();
        var body = await res.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<DiscoveredRoutes>(body, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Assert
        result.ShouldNotBeNull();
        result.Routes.ShouldBe(expectedRoutes.routes);
        result.Message.ShouldBe(expectedMessage);
    }

    [Fact]
    public async Task CallFakeGoogle_ReturnsNull()
    {
        // Arrange
        Addresses addresses = new Addresses(
            OriginAddress: "123 Main St, Anytown, CA",
            DestinationAddress: "456 Lincoln Ave, Anytown, CA"
        );
        Routes expectedRoutes = new([]);
        var fakeResponseBody = "";
        var expectedMessage = "Error deserializing response to Routes.";

        var fakeMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(fakeResponseBody, Encoding.UTF8, "application/json")
        };

        using var siteApp = new DistanceMicroserviceApp(fakeHttpResponseMessage: fakeMessage);
        using var httpClient = siteApp.CreateClient();

        // Act
        var res = await httpClient.PostAsJsonAsync("/getdistanceinfo", addresses);
        res.EnsureSuccessStatusCode();
        var body = await res.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<DiscoveredRoutes>(body, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Assert
        result.ShouldNotBeNull();
        result.Routes.ShouldBe(expectedRoutes.routes);
        result.Message.ShouldBe(expectedMessage);
    }

    [Fact]
    public async Task CallFakeGoogle_Returns500()
    {
        // Arrange
        Addresses addresses = new Addresses(
            OriginAddress: "123 Main St, Anytown, CA",
            DestinationAddress: "456 Lincoln Ave, Anytown, CA"
        );
        Routes expectedRoutes = new([]);
        var fakeResponseBody = "";
        var expectedMessage = "Error: 500 - Internal Server Error";

        var fakeMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
            Content = new StringContent(fakeResponseBody, Encoding.UTF8, "application/json")
        };

        using var siteApp = new DistanceMicroserviceApp(fakeHttpResponseMessage: fakeMessage);
        using var httpClient = siteApp.CreateClient();

        // Act
        var res = await httpClient.PostAsJsonAsync("/getdistanceinfo", addresses);
        res.EnsureSuccessStatusCode();
        var body = await res.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<DiscoveredRoutes>(body, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Assert
        result.ShouldNotBeNull();
        result.Routes.ShouldBe(expectedRoutes.routes);
        result.Message.ShouldBe(expectedMessage);
    }
}
