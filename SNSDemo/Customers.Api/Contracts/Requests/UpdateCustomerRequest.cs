﻿using Microsoft.AspNetCore.Mvc;

namespace Customers.Api.Contracts.Requests;

public class UpdateCustomerRequest
{
    [FromRoute(Name = "id")] public Guid Id { get; set; }

    [FromBody] public CustomerRequest Customer { get; set; } = default!;
}
