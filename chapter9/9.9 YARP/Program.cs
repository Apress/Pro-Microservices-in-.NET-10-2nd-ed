var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
   .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseHttpsRedirection(); // <-- you can still use ASP.NET middleware

app.MapReverseProxy();

app.Run();
