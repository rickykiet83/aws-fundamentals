using Customers.Api.Contracts.Messages;
using Customers.Api.Domain;

namespace Customers.Api.Mapping;

public static class DomainToMessageMapper
{
    public static CustomerCreatedMessage ToCustomerCreatedMessage(this Customer customer)
    {
        return new CustomerCreatedMessage
        {
            Id = customer.Id,
            FullName = customer.FullName,
            Email = customer.Email,
            GitHubUsername = customer.GitHubUsername,
            DOB = customer.Dob
        };
    }
    
    public static CustomerUpdatedMessage ToCustomerUpdatedMessage(this Customer customer)
    {
        return new CustomerUpdatedMessage
        {
            Id = customer.Id,
            FullName = customer.FullName,
            Email = customer.Email,
            GitHubUsername = customer.GitHubUsername,
            DOB = customer.Dob
        };
    }
}