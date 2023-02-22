﻿using Amazon.SQS;
using Amazon.SQS.Model;

var queueName = args.Length == 1
    ? args[0]
    : "customers";

var cancellationTokenSource = new CancellationTokenSource();
var sqsClient = new AmazonSQSClient();

var queueUrlResponse = await sqsClient.GetQueueUrlAsync(queueName);

Console.WriteLine($"Receive messages from {queueUrlResponse.QueueUrl}");

var receiveMessageRequest = new ReceiveMessageRequest
{
    QueueUrl = queueUrlResponse.QueueUrl,
    AttributeNames = new List<string>{ "All" },
    MessageAttributeNames = new List<string>{ "MessageType" } // All can also be used here
};

while (!cancellationTokenSource.IsCancellationRequested)
{
    var response = await sqsClient.ReceiveMessageAsync(receiveMessageRequest, cancellationTokenSource.Token);

    foreach (var message in response.Messages)
    {
        Console.WriteLine($"Message Id: {message.MessageId}");
        Console.WriteLine($"Message Body: {message.Body}");

        await sqsClient.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle);
    }
    
    await Task.Delay(3000);
}