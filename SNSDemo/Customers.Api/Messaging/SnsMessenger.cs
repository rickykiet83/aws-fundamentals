using System.Text.Json;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.Extensions.Options;

namespace Customers.Api.Messaging;

public class SnsMessenger(IAmazonSimpleNotificationService sns, IOptions<TopicSettings> topicSettings) : ISnsMessenger
{
    private string? _topicArn;
    
    public async Task<PublishResponse> PublishedMessageAsync<T>(T message)
    {
        var topicArn = await GetTopicArnAsync();
        
        var sendMessageRequest = new PublishRequest
        {
            TopicArn = topicArn,
            Message = JsonSerializer.Serialize(message),
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
        
        return await sns.PublishAsync(sendMessageRequest);
    }
    
    private async Task<string> GetTopicArnAsync()
    {
        if (_topicArn is not null) return _topicArn;
        
        var queueUrlResponse = await sns.FindTopicAsync(topicSettings.Value.Name);
        _topicArn = queueUrlResponse.TopicArn;
        
        return _topicArn;
    }
}