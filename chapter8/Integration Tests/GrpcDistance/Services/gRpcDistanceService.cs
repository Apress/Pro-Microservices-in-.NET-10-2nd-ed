
namespace GrpcDistance.Services;

public class gRpcDistanceService(IGoogleRouteServices googleRouteService) : DistanceInfoSvc.DistanceInfoSvcBase
{

	public override async Task<DistanceInformation> GetDistance(GrpcDistanceMicroservice.Addresses request, ServerCallContext context)
	{
		var addresses = new Addresses(request.OriginAddress, request.DestinationAddress);

		var distanceResponse = await googleRouteService.GetRouteInfo(addresses);

		var response = new DistanceInformation()
		{
			OriginAddress = request.OriginAddress,
			DestinationAddress = request.DestinationAddress,
			Message = distanceResponse.Message
		};

		foreach (var routeInfo in distanceResponse.Routes)
		{
			response.Routes.Add(new GrpcDistanceMicroservice.Route() { DistanceMeters = routeInfo.distanceMeters, Duration = routeInfo.duration });
		}

		return response;
	}
}
