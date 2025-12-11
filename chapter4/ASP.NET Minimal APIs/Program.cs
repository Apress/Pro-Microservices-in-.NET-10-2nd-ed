var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyDbContext")));

builder.Services.AddTransient<IOrderRepository, OrderRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi("/swagger/v1/swagger.json");
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapGet("/", () => new { message = "Welcome to Pro Microservices in .NET 10" });

app.MapGet("/order", (IOrderRepository orderRepository) => orderRepository.GetAllOrders());
app.MapGet("/order/{id:int}", (IOrderRepository orderRepository, int? id) =>
{
    var order = orderRepository.GetOrderById(id ?? 0);
    if (order == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(order);
});
app.MapPost("/order", (IOrderRepository orderRepository, [FromBody] Order order) =>
{
    order.Id = 0;
    orderRepository.SaveOrder(order);
    return Results.Created($"/order/{order.Id}", order);
});
app.MapPut("/order/{id}", (IOrderRepository orderRepository, int id, [FromBody] Order order) =>
{
    order.Id = id;
    orderRepository.SaveOrder(order);
    return order;
});
app.MapDelete("/order/{id}", (IOrderRepository orderRepository, int id) =>
{
    orderRepository.DeleteOrder(id);
    return Results.Ok();
});

app.MapHealthChecks("/health");
app.Map("/error", () => Results.Problem());
app.Map("/{*url}", () => Results.NotFound(new { message = "Not Found", status = 404 }));

app.Run();
