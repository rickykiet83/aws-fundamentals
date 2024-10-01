using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using SqsPublisher;

var sqsClient = new AmazonSQSClient();

var customerCreatedEvent = new CustomerCreatedEvent
{
    Id = Guid.NewGuid(),
    FullName = "John Doe",
    Email = "kiet@gmail.com",
    DOB = new DateTime(1983, 10, 26),
    GitHubUsername = "rickykiet83"
};

var queueUrlResponse = await sqsClient.GetQueueUrlAsync("customers");

var sendMessageRequest = new SendMessageRequest
{
    QueueUrl = queueUrlResponse.QueueUrl,
    MessageBody = JsonSerializer.Serialize(customerCreatedEvent),
    MessageAttributes = new Dictionary<string, MessageAttributeValue>
    {
        {
            "MessageType", new MessageAttributeValue
            {
                DataType = "String",
                StringValue = nameof(CustomerCreatedEvent)
            }
        }
    },
    DelaySeconds = 5 // delay for 5 seconds before message is available for consumers
};

var response = await sqsClient.SendMessageAsync(sendMessageRequest);

Console.WriteLine();