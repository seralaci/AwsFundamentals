using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using SqsPublisher;

var sqsClient = new AmazonSQSClient();

var customer = new CustomerCreated
{
    Id = Guid.NewGuid(),
    Email = "john@doe.com",
    FullName = "John Doe",
    GitHubUsername = "johndoe",
    DateOfBirth = new DateOnly(1991, 12, 22)
};

var queueUrlResponse = await sqsClient.GetQueueUrlAsync("customers");

var sendMessageRequest = new SendMessageRequest
{
  QueueUrl  = queueUrlResponse.QueueUrl,
  MessageBody = JsonSerializer.Serialize(customer),
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

var response = await sqsClient.SendMessageAsync(sendMessageRequest);

Console.WriteLine();