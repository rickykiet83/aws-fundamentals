using Amazon.SimpleNotificationService.Model;

namespace Customers.Api.Messaging;

public interface ISnsMessenger
{
    Task<PublishResponse> PublishedMessageAsync<T>(T message);
}