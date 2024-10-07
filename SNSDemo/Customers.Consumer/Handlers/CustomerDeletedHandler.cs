using Customers.Consumer.Messages;
using MediatR;

namespace Customers.Consumer.Handlers;

public class CustomerDeletedHandler(ILogger<CustomerCreatedHandler> logger) : IRequestHandler<CustomerDeletedMessage>
{
    public async Task Handle(CustomerDeletedMessage request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Customer deleted: {Id})", request.Id.ToString());   
    }
}