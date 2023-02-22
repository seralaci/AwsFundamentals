using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;

namespace Customers.Api.Messaging;

public class SqsMessenger : ISqsMessenger
{
    private readonly IAmazonSQS _sqs;
    private readonly IOptions<QueueSettings> _queueSettings;
    private string? _queueUrl;

    public SqsMessenger(IAmazonSQS sqs, IOptions<QueueSettings> queueSettings)
    {
        _sqs = sqs;
        _queueSettings = queueSettings;
    }

    public async Task<SendMessageResponse> SendMessageAsync<T>(T message, CancellationToken cancellationToken = default)
    {
        var queueUrl = await GetQueueUrlAsync(cancellationToken);

        var sendMessageRequest = new SendMessageRequest
        {
            QueueUrl = queueUrl,
            MessageBody = JsonSerializer.Serialize(message),
            MessageAttributes = new Dictionary<string, MessageAttributeValue>
            {
                {
                    "MessageType", new MessageAttributeValue
                    {
                        DataType = "String",
                        StringValue = typeof(T).Name
                    }
                }
            }
        };

        return await _sqs.SendMessageAsync(sendMessageRequest, cancellationToken);
    }

    private async ValueTask<string> GetQueueUrlAsync(CancellationToken cancellationToken = default)
    {
        if (_queueUrl is not null)
        {
            return _queueUrl;
            
        }
        
        var responseUrlResponse = await _sqs.GetQueueUrlAsync(_queueSettings.Value.Name, cancellationToken);
        _queueUrl = responseUrlResponse.QueueUrl;
        return _queueUrl;
    }
}