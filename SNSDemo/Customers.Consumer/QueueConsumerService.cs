using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Customers.Consumer.Messages;
using MediatR;
using Microsoft.Extensions.Options;

namespace Customers.Consumer;

public class QueueConsumerService(IAmazonSQS sqs, IOptions<QueueSettings> queueSettings, IMediator mediator, ILogger<QueueConsumerService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var queueUrlResponse = await sqs.GetQueueUrlAsync(queueSettings.Value.Name, stoppingToken);
        var request = new ReceiveMessageRequest
        {
            QueueUrl = queueUrlResponse.QueueUrl,
            MessageSystemAttributeNames = ["All"],
            MessageAttributeNames = ["All"],
            MaxNumberOfMessages = 1,
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            var response = await sqs.ReceiveMessageAsync(request, stoppingToken);
            foreach (var message in response.Messages)
            {
                var messageType = message.MessageAttributes["MessageType"].StringValue;
                
                var type = Type.GetType($"Customers.Consumer.Messages.{messageType}");
                if (type == null)
                {
                    logger.LogWarning($"Unknown message type: {messageType}");
                    continue;
                }
                
                var typedMessage = (ISqsMessage)JsonSerializer.Deserialize(message.Body, type)!;
                try
                {
                    await mediator.Send(typedMessage, stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing message");
                    continue;
                }
                
                // Process the message
                await sqs.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle, stoppingToken);
            }
            
            await Task.Delay(1000, stoppingToken);
        }
    }
}