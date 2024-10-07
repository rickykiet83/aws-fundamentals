using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Customers.Api.Contracts.Messages;
using Microsoft.Extensions.Options;

namespace Customers.Consumer;

public class QueueConsumerService(IAmazonSQS sqs, IOptions<QueueSettings> queueSettings) : BackgroundService
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
                switch (messageType)
                {
                    case nameof(CustomerCreatedMessage):
                        break;
                    case nameof(CustomerUpdatedMessage):
                        break;
                    case nameof(CustomerDeletedMessage):
                        break;
                }
                // Process the message
                await sqs.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle, stoppingToken);
            }
        }
    }
}