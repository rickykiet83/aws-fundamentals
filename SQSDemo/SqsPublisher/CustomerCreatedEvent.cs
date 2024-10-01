namespace SqsPublisher;

public class CustomerCreatedEvent
{
    public required Guid Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string GitHubUsername { get; set; }
    public required DateTime DOB { get; set; }
}