namespace DistanceMicroservice.Tests.Unit;

public class GoogleRouteServices_Tests
{
    [Fact]
    public async Task CallMockGoogle_ReturnsResults()
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

        var ioc = new Fixture().Customize(new AutoNSubstituteCustomization());
        ioc.MakeFakeHttpClientFactory(fakeMessage);

        // Act
        var sut = ioc.Create<GoogleRouteServices>();
        var result = await sut.GetRouteInfo(addresses);

        // Assert
        result.ShouldNotBeNull();
        result.Routes.ShouldBe(expectedRoutes.routes);
        result.Message.ShouldBe(expectedMessage);
    }

    [Fact]
    public async Task CallMockGoogle_ReturnsNull()
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

        var ioc = new Fixture().Customize(new AutoNSubstituteCustomization());
        ioc.MakeFakeHttpClientFactory(fakeMessage);

        // Act
        var sut = ioc.Create<GoogleRouteServices>();
        var result = await sut.GetRouteInfo(addresses);

        // Assert
        result.ShouldNotBeNull();
        result.Routes.ShouldBe(expectedRoutes.routes);
        result.Message.ShouldBe(expectedMessage);
    }
    [Fact]
    public async Task CallMockGoogle_Returns500()
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

        var ioc = new Fixture().Customize(new AutoNSubstituteCustomization());
        ioc.MakeFakeHttpClientFactory(fakeMessage);

        // Act
        var sut = ioc.Create<GoogleRouteServices>();
        var result = await sut.GetRouteInfo(addresses);

        // Assert
        result.ShouldNotBeNull();
        result.Routes.ShouldBe(expectedRoutes.routes);
        result.Message.ShouldBe(expectedMessage);
    }
}
