
using GoogleRouteService;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults(meters: [GoogleRouteServices.DISTANCE_METER_NAME], sources: [GoogleRouteServices.ACTIVITY_SOURCE_NAME]);

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<GoogleRouteServices>();

var app = builder.Build();

app.MapDefaultEndpoints();

IConfiguration config = app.Configuration;

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.UseSwagger();
	app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My microservice for map information."));
}

app.UseHttpsRedirection();

app.MapPost("/getdistanceinfo", (Addresses addresses, GoogleRouteServices googleRouteService) =>
{
	var apiUrl = config["googleRoutesApi:apiUrl"]
			   ?? throw new InvalidOperationException("URL key, googleRouteApiUrl, not found.");
	var apiKey = config["googleRoutesApi:apiKey"]
		?? throw new InvalidOperationException("API key, googleRouteApiKey, not found in user secrets.");

	var response = googleRouteService.GetRouteInfo(addresses, apiUrl, apiKey);
	return response;
})
.WithName("GetDistanceInfo");

app.Run();
