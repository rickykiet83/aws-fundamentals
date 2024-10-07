using Customers.Consumer.Messages;
using MediatR;

namespace Customers.Consumer.Handlers;

public class CustomerUpdatedHandler(ILogger<CustomerCreatedHandler> logger) : IRequestHandler<CustomerUpdatedMessage>
{
    public Task Handle(CustomerUpdatedMessage request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Customer updated: {FullName} ({Email})", request.FullName, request.Email);
        return Task.CompletedTask;
    }
}