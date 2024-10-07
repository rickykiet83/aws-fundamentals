using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;

namespace Customers.Api.Messaging;

public class SqsMessenger(IAmazonSQS sqs, IOptions<QueueSettings> queueSettings) : ISqsMessenger
{
    private string? _queueUrl;
    
    public async Task<SendMessageResponse> SendMessageAsync<T>(T message)
    {
        var queueUrl = await GetQueueUrlAsync<T>();
        
        var sendMessageRequest = new SendMessageRequest
        {
            QueueUrl = queueUrl,
            MessageBody = JsonSerializer.Serialize(message),
            MessageAttributes = new Dictionary<string, MessageAttributeValue>
            {
                {
                    "MessageType", new MessageAttributeValue
                    {
                        DataType = "String",
                        StringValue = typeof(T).Name
                    }
                }
            }
        };
        
        return await sqs.SendMessageAsync(sendMessageRequest);
    }
    
    private async Task<string> GetQueueUrlAsync<T>()
    {
        if (_queueUrl is not null) return _queueUrl;
        
        var response = await sqs.GetQueueUrlAsync(queueSettings.Value.Name);
        _queueUrl = response.QueueUrl;
        
        return _queueUrl;
    }
}