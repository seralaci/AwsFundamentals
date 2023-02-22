namespace Customers.Api.Messaging;

public class TopicSettings
{
    public const string SectionName = "Topic";
    
    public required string Name { get; init; }
}