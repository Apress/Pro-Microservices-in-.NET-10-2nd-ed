namespace DistanceMicroservice.Tests.Fixtures;

public static class FakeHttpClientFactory
{
    public static HttpMessageHandler MakeFakeHttpClientFactory(this IFixture ioc, HttpResponseMessage message)
    {
        var fakeHandler = new FakeHttpMessageHandler(message);
        var fakeClient = new HttpClient(fakeHandler)
        {
            BaseAddress = new Uri("https://fake.api/")
        };
        var mockFactory = ioc.Freeze<IHttpClientFactory>();
        mockFactory.CreateClient().Returns(fakeClient);
        return fakeHandler;
    }
}
