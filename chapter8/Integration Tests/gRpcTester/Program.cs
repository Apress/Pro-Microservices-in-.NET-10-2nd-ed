using Grpc.Net.Client;
using GrpcDistanceMicroserviceClient;

// The port number must match the port of the gRPC server.
using var channel = GrpcChannel.ForAddress("https://localhost:7005");
var client = new DistanceInfoSvc.DistanceInfoSvcClient(channel);

var address = new Addresses() {
	OriginAddress = "101 S. Main St, Chicago, IL",
	DestinationAddress = "123 S. Main St., Los Angeles, CA"
};

Console.WriteLine("Getting distance");

var reply = await client.GetDistanceAsync(address);

Console.WriteLine("Message: " + reply.Message);

var routeCount = 1;
foreach(var r in reply.Routes)
{
	var miles = Math.Round(r.DistanceMeters / 1609.34, 2);
	Console.WriteLine($"Route {routeCount} distance in meters: {r.DistanceMeters.ToString("N0")}; Miles {miles.ToString("N2")}");
	routeCount++;
}

Console.WriteLine();
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
