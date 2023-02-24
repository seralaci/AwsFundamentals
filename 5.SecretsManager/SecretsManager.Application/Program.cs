
using System.Text.Json;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

var secretsManagerClient = new AmazonSecretsManagerClient();

var listSecretVersionsRequest = new ListSecretVersionIdsRequest
{
    SecretId = "ApiKey",
    IncludeDeprecated = true
};

var versionResponse = await secretsManagerClient.ListSecretVersionIdsAsync(listSecretVersionsRequest);

var request = new GetSecretValueRequest
{
    SecretId = "ApiKey",
    VersionStage = "AWSPREVIOUS", // AWSCURRENT
    //VersionId = "3b8c3da9-5442-47de-8fb1-d8eae3cd49a7"
};

var response = await secretsManagerClient.GetSecretValueAsync(request);

Console.WriteLine(response.SecretString);

var describeSecretRequest = new DescribeSecretRequest
{
    SecretId = "ApiKey"
};

var describeResponse = await secretsManagerClient.DescribeSecretAsync(describeSecretRequest);

Console.WriteLine(JsonSerializer.Serialize(describeResponse, new JsonSerializerOptions { WriteIndented = true}));
