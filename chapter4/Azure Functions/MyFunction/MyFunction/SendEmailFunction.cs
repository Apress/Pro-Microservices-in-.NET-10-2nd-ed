using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Text.Json;

namespace MyFunction;

public class SendEmailFunction(ILogger<SendEmailFunction> logger, IConfiguration configuration)
{
    private readonly SmtpClient smtpClient = new SmtpClient(configuration["EmailSettings:MailServer"]);
	string emailAddress = configuration["EmailSettings:RecipientAddress"] ?? throw new ArgumentNullException("EmailSettings:RecipiientAddress not set");

	[Function(nameof(SendEmailFunction))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
	{
		string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
		var data = JsonSerializer.Deserialize<EmailModel>(requestBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

		if (data is null)
		{
			return new BadRequestObjectResult("Invalid request payload.");
		}

		logger.LogInformation($"Sending email to {emailAddress} with subject: {data.Subject}");
		var mailMessage = new MailMessage(emailAddress, emailAddress, data.Subject, data.Body);
		smtpClient.Send(mailMessage);

		return new OkObjectResult("Welcome to Azure Functions!");
    }
}

public record EmailModel(string Subject, string Body);
