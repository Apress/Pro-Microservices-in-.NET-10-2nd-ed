
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddHttpClient();

builder.Services.AddScoped<IGoogleRouteServices, GoogleRouteServices>();

GoogleRouteSettings settings = new GoogleRouteSettings();
builder.Configuration.Bind("googleRoutesApi", settings);
builder.Services.AddSingleton(settings);

var app = builder.Build();

app.MapGrpcService<gRpcDistanceService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
