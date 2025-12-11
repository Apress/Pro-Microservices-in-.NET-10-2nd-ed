
var factory = new ConnectionFactory { HostName = "localhost" };
//replace HostName with your RabbitMQ server address if not running locally
using var connection = await factory.CreateConnectionAsync();
using (var channel = await connection.CreateChannelAsync())
{
	await channel.QueueDeclareAsync(queue: "invoice-service", durable: false,
		exclusive: false, autoDelete: false, arguments: null);

	//await channel.ExchangeDeclareAsync(exchange: "invoice-service", ExchangeType.Fanout);

	//QueueDeclareOk queueDeclareResult = await channel.QueueDeclareAsync();
	//string queueName = queueDeclareResult.QueueName;
	//await channel.QueueBindAsync(queue: queueName, exchange: "invoice-service", routingKey: string.Empty);

	Console.WriteLine($" [*] Waiting for messages. - {DateTime.Now}");

	var consumer = new AsyncEventingBasicConsumer(channel);
	consumer.ReceivedAsync += (model, ea) =>
	{
		var body = ea.Body.ToArray();
		var message = JsonSerializer.Deserialize<InvoiceCreated>(body);
		Console.WriteLine($" [x] Received invoice number: " +
			$"{message?.InvoiceNumber}");
		return Task.CompletedTask;
	};

	//for competing consumers
	await channel.BasicConsumeAsync("invoice-service",
		autoAck: true, consumer: consumer);

	//for pubsub
	//await channel.BasicConsumeAsync(queueName,
	//	autoAck: true, consumer: consumer);


	Console.WriteLine(" Press [enter] to exit.");
	Console.ReadLine();
};