namespace Customers.Api.Contracts.Messages;

public class CustomerCreatedMessage
{
    public required Guid Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string GitHubUsername { get; set; }
    public required DateTime DOB { get; set; }
}

public class CustomerUpdatedMessage
{
    public required Guid Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string GitHubUsername { get; set; }
    public required DateTime DOB { get; set; }
}

public record CustomerDeletedMessage(Guid Id)
{
}