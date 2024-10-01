using Customers.Api.Contracts.Requests;
using Customers.Api.Contracts.Responses;
using Customers.Api.Mapping;
using Customers.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Customers.Api.Endpoints;

public static class CustomerEndpoints
{
    public static RouteGroupBuilder MapCustomerEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes
            .MapGroup("/api/customers")
            .WithParameterValidation()
            .WithTags("Customers");

        group.MapPost("/", async (ICustomerService service, CustomerRequest request) =>
            {
                var customer = request.ToCustomer();
                await service.CreateAsync(customer);
                var customerResponse = customer.ToCustomerResponse();
                return TypedResults.CreatedAtRoute(customerResponse, "Get", new { customerResponse.Id });
            }).WithSummary("Creates a new customer")
            .WithDescription("Creates a new customer with the specified details")
            .Produces<CustomerResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            ;

        group.MapGet("/{id:guid}", async (ICustomerService service, Guid id) =>
            {
                var customer = await service.GetAsync(id);
                if (customer is null)
                    return Results.NotFound();

                var customerResponse = customer.ToCustomerResponse();
                return Results.Ok(customerResponse);
            }).WithName("Get")
            .WithDescription("Gets the customer that has the specified id")
            .Produces<CustomerResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            ;

        group.MapGet("/", async (
                ICustomerService service) =>
            {
                var customers = await service.GetAllAsync();
                var customersResponse = customers.ToCustomersResponse();
                return Results.Ok(customersResponse);
            })
            .WithSummary("Gets all customers")
            .WithDescription("Gets all customers")
            .Produces<IEnumerable<CustomerResponse>>(StatusCodes.Status200OK)
            ;

        group.MapPut("/{id:guid}", async (
                [FromRoute]Guid id, 
                [FromServices]ICustomerService service, 
                [FromBody]UpdateCustomerRequest request) =>
            {
                request.Id = id;
                var existingCustomer = await service.GetAsync(id);

                if (existingCustomer is null)
                    return Results.NotFound();

                var customer = request.ToCustomer();
                await service.UpdateAsync(customer);

                var customerResponse = customer.ToCustomerResponse();
                return Results.Ok(customerResponse);
            }).WithSummary("Updates a customer")
            .WithDescription("Updates a customer")
            .Produces<CustomerResponse>(StatusCodes.Status200OK);
        
        group.MapDelete("/{id:guid}", async (Guid id, ICustomerService service) =>
            {
                await service.DeleteAsync(id);
                return TypedResults.NoContent();
            })
            .WithSummary("Deletes a user by id")
            .WithDescription("Deletes the user that has the specified id")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            ;

        return group;
    }
}