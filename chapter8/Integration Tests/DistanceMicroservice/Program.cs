
using GoogleRouteService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddScoped<IGoogleRouteServices, GoogleRouteServices>();

GoogleRouteSettings settings = new GoogleRouteSettings();
builder.Configuration.Bind("googleRoutesApi", settings);
builder.Services.AddSingleton(settings);

var app = builder.Build();

IConfiguration config = app.Configuration;

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.UseSwagger();
	app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My microservice for map information."));
}

app.UseHttpsRedirection();

app.MapPost("/getdistanceinfo", (Addresses addresses, IGoogleRouteServices googleRouteService) => 
	googleRouteService.GetRouteInfo(addresses)
)
.WithName("GetDistanceInfo");

app.Run();

// For testing:
public partial class Program { }
