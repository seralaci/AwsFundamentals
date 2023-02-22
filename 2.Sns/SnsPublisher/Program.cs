using System.Text.Json;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using SnsPublisher;

var customer = new CustomerCreated
{
    Id = Guid.NewGuid(),
    Email = "john@doe.com",
    FullName = "John Doe",
    GitHubUsername = "johndoe",
    DateOfBirth = new DateOnly(1991, 12, 22)
};

var snsClient = new AmazonSimpleNotificationServiceClient();

var topicResponse = await snsClient.FindTopicAsync("customers");

var publishRequest = new PublishRequest
{
    TopicArn = topicResponse.TopicArn,
    Message = JsonSerializer.Serialize(customer),
    MessageAttributes = new Dictionary<string, MessageAttributeValue>
    {
        {
            "MessageType", new MessageAttributeValue
            {
                DataType = "String",
                StringValue = nameof(CustomerCreated)
            }
        }
    }
};

var response = await snsClient.PublishAsync(publishRequest);

Console.WriteLine();

