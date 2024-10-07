// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using SnsPublisher;

var customer = new CustomerCreatedMessage
{
    Id = Guid.NewGuid(),
    Email = "kietpham.dev@gmail.com",
    FullName = "Kiet Pham",
    GitHubUsername = "rickykiet83",
    DOB = new DateTime(1983,10, 26)
};

var snsClient = new AmazonSimpleNotificationServiceClient();

var topicArn = await snsClient.FindTopicAsync("customers");

var publishRequest = new PublishRequest
{
    TopicArn = topicArn.TopicArn,
    Message = JsonSerializer.Serialize(customer),
    MessageAttributes = new Dictionary<string, MessageAttributeValue>
    {
        {
            "MessageType", new MessageAttributeValue
            {
                DataType = "String",
                StringValue = nameof(CustomerCreatedMessage)
            }
        }
    }
};

var response = await snsClient.PublishAsync(publishRequest);

