using System.Net.Http.Json;

namespace GoogleRouteService;

public class GoogleRouteServices
{
		public async Task<DiscoveredRoutes> GetRouteInfo(Addresses addresses, string apiUrl, string apiKey)
		{
			HttpClient httpClient = new();
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
				discoveredRoutes.Message = $"Error: {response.StatusCode} - {response.ReasonPhrase}";
			}

			var responseBody = await response.Content.ReadAsStringAsync();
			var routes = System.Text.Json.JsonSerializer.Deserialize<Routes>(responseBody);
			if (routes is null)
			{
				discoveredRoutes.Message = "Failed to deserialize response to Routes.";
			}

			if (routes?.routes is null)
			{
				discoveredRoutes.Message = "Failed to deserialize response to Routes or routes array is null.";
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
	public required int DistanceMeters = distanceMeters;
	public required string Duration = duration;

	// Convert meters to miles
	public double DistanceInMiles => Math.Round(distanceMeters / 1609.34, 2);
}
public record RouteRequest(Origin origin, Destination destination, string travelMode
, string routingPreference, bool computeAlternativeRoutes, Routemodifiers routeModifiers
, string languageCode, string units);

public record Origin(string Address);

public record Destination(string Address);

public record Routemodifiers(bool avoidTolls, bool avoidHighways, bool avoidFerries);