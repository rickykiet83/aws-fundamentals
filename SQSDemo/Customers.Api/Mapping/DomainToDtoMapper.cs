﻿using Customers.Api.Contracts.Data;
using Customers.Api.Domain;

namespace Customers.Api.Mapping;

public static class DomainToDtoMapper
{
    public static CustomerDto ToCustomerDto(this Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.Id,
            Email = customer.Email,
            GitHubUsername = customer.GitHubUsername,
            FullName = customer.FullName,
            Dob = customer.Dob
        };
    }
}
