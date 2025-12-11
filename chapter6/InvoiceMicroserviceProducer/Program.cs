
var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

//Used for competing consumers
await channel.QueueDeclareAsync(queue: "invoice-service", durable: false,
	exclusive: false, autoDelete: false, arguments: null);

//Used for pubsub
//await channel.ExchangeDeclareAsync(exchange: "invoice-service", ExchangeType.Fanout);

var exit = false;
while (!exit)
{
	Console.WriteLine("Press 'q' to exit or any other key to create an invoice.");
	var key = Console.ReadKey(true).Key;
	if (key == ConsoleKey.Q)
	{
		exit = true;
		continue;
	}

	var newInvoiceNumber = new Random().Next(10000, 99999);
	Console.WriteLine($"Created invoice with number:{newInvoiceNumber}");

	InvoiceCreated invoiceCreated = new()
	{
		InvoiceNumber = newInvoiceNumber,
		InvoiceData = new InvoiceToCreate()
		{
			CustomerNumber = 12345,
			InvoiceItems =
			[
				new InvoiceItems
				{
					Description = "Item 1",
					Price = 100.0,
					ActualMileage = 50.0,
					BaseRate = 10.0,
					IsOversized = false,
					IsRefrigerated = false,
					IsHazardousMaterial = false
				},
				new InvoiceItems
				{
					Description = "Item 2",
					Price = 200.0,
					ActualMileage = 75.0,
					BaseRate = 15.0,
					IsOversized = true,
					IsRefrigerated = false,
					IsHazardousMaterial = true
				}
			]
		}
	};

	var message = JsonSerializer.Serialize(invoiceCreated);
	var body = Encoding.UTF8.GetBytes(message);

	//for competing consumers
	await channel.BasicPublishAsync(exchange: string.Empty,
		routingKey: "invoice-service", body: body);

	//for pubsub
	//await channel.BasicPublishAsync(exchange: "invoice-service",
	//	routingKey: string.Empty, body: body);
}