using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace GoogleRouteService;

public interface IGoogleRouteServices
{
    Task<DiscoveredRoutes> GetRouteInfo(Addresses addresses);
}

public class GoogleRouteServices(GoogleRouteSettings settings, IHttpClientFactory httpClientFactory) : IGoogleRouteServices
{
    public async Task<DiscoveredRoutes> GetRouteInfo(Addresses addresses)
    {
        var apiUrl = settings.ApiUrl
            ?? throw new InvalidOperationException("googleRoutesApi:ApiUrl not found in config.");
        var apiKey = settings.ApiKey
            ?? throw new InvalidOperationException("googleRoutesApi:ApiKey not found in config");

        var httpClient = httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Add("X-Goog-Api-Key", apiKey);
        httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");
        httpClient.DefaultRequestHeaders.Add("X-Goog-FieldMask", "routes.duration,routes.distanceMeters");

        var routeRequest = new RouteRequest(
              new Origin(addresses.OriginAddress)
            , new Destination(addresses.DestinationAddress)
            , travelMode: "DRIVE", routingPreference: "TRAFFIC_AWARE", computeAlternativeRoutes: true
            , new Routemodifiers(avoidTolls: false, avoidHighways: false, avoidFerries: false)
            , languageCode: "en-US", units: "UNITS_UNSPECIFIED");

        HttpResponseMessage response = await httpClient.PostAsJsonAsync(apiUrl, routeRequest);

        var discoveredRoutes = new DiscoveredRoutes(
            addresses.OriginAddress, addresses.DestinationAddress, string.Empty, Array.Empty<Route>());

        if (!response.IsSuccessStatusCode)
        {
            discoveredRoutes.Message = $"Error: {(int)response.StatusCode} - {response.ReasonPhrase}";
            return discoveredRoutes;
        }

        var responseBody = await response.Content.ReadAsStringAsync();
        Routes? routes;
        try
        {
            routes = JsonSerializer.Deserialize<Routes>(responseBody);
        }
        catch
        {
            discoveredRoutes.Message = "Error deserializing response to Routes.";
            return discoveredRoutes;
        }
        if (routes is null)
        {
            discoveredRoutes.Message = "Failed to deserialize response to Routes.";
            return discoveredRoutes;
        }

        if (routes?.routes is null)
        {
            discoveredRoutes.Message = "Failed to deserialize response to Routes or routes array is null.";
            return discoveredRoutes;
        }

        discoveredRoutes.Routes = routes?.routes ?? Array.Empty<Route>();
        discoveredRoutes.Message = $"Number of routes found: {discoveredRoutes.Routes.Length}";

        return discoveredRoutes;
    }
}


public record Addresses(string OriginAddress, string DestinationAddress);

public class DiscoveredRoutes(string OriginAddress, string DestinationAddress, string Message, Route[] Routes)
{
	public string OriginAddress { get; } = OriginAddress;
	public string DestinationAddress { get; } = DestinationAddress;
	public string Message { get; set; } = Message;
	public Route[] Routes { get; set; } = Routes;
}

public record Routes(Route[] routes);

/// <param name="distanceMeters"> Distance in meters. </param>
/// <param name="duration"> Duration in seconds. </param>
public record Route(int distanceMeters, string duration)
{
	// Convert meters to miles
	public double DistanceInMiles => Math.Round(distanceMeters / 1609.34, 2);
}
public record RouteRequest(Origin origin, Destination destination, string travelMode
, string routingPreference, bool computeAlternativeRoutes, Routemodifiers routeModifiers
, string languageCode, string units);

public record Origin(string Address);

public record Destination(string Address);

public record Routemodifiers(bool avoidTolls, bool avoidHighways, bool avoidFerries);