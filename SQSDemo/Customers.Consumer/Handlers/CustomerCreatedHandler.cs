using Customers.Consumer.Messages;
using MediatR;

namespace Customers.Consumer.Handlers;

public class CustomerCreatedHandler(ILogger<CustomerCreatedHandler> logger) : IRequestHandler<CustomerCreatedMessage>
{
    public async Task Handle(CustomerCreatedMessage request, CancellationToken cancellationToken)
    {
        // throw new Exception("Something went wrong");
        logger.LogInformation("Customer created: {FullName} ({Email})", request.FullName, request.Email);
    }
}