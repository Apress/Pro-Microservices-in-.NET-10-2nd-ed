
namespace GrpcDistance.Services;

public class gRpcDistanceService(GoogleRouteServices googleRouteService, IConfiguration config) : DistanceInfoSvc.DistanceInfoSvcBase
{

	public override async Task<DistanceInformation> GetDistance(GrpcDistanceMicroservice.Addresses request, ServerCallContext context)
	{
		var apiUrl = config["googleRoutesApi:apiUrl"]
				   ?? throw new InvalidOperationException("URL key, googleRouteApiUrl, not found.");
		var apiKey = config["googleRoutesApi:apiKey"]
			?? throw new InvalidOperationException("API key, googleRouteApiKey, not found in user secrets.");

		var addresses = new Addresses(request.OriginAddress, request.DestinationAddress);

		var distanceResponse = await googleRouteService.GetRouteInfo(addresses, apiUrl, apiKey);

		var response = new DistanceInformation()
		{
			OriginAddress = request.OriginAddress,
			DestinationAddress = request.DestinationAddress,
			Message = distanceResponse.Message
		};

		foreach (var routeInfo in distanceResponse.Routes)
		{
			response.Routes.Add(new GrpcDistanceMicroservice.Route() { DistanceMeters = routeInfo.DistanceMeters, Duration = routeInfo.Duration });
		}

		return response;
	}
}
