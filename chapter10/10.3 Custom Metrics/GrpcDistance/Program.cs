
var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults(meters: [GoogleRouteServices.DISTANCE_METER_NAME], sources: [GoogleRouteServices.ACTIVITY_SOURCE_NAME]);

builder.Services.AddGrpc();

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapGrpcService<gRpcDistanceService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
