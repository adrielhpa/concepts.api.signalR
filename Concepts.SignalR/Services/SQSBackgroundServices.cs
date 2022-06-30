using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Concepts.SignalR.Hubs;
using Concepts.SignalR.Models;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace Concepts.SQS.Services
{
    public class SQSService : BackgroundService
    {
        private readonly IHubContext<MessageHub> _messageHub;
        public SQSService(IHubContext<MessageHub> messageHub)
        {
            _messageHub = messageHub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Starting Background Service");
            var accessKey = Environment.GetEnvironmentVariable("AmazonAccessKey");
            var secretKey = Environment.GetEnvironmentVariable("AmazonSecretKey");
            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            var client = new AmazonSQSClient(credentials, RegionEndpoint.USEast1);

            while (!stoppingToken.IsCancellationRequested)
            {
                var request = new ReceiveMessageRequest()
                {
                    QueueUrl = "https://sqs.us-east-1.amazonaws.com/473698569966/conceptsQueue",
                };

                var response = await client.ReceiveMessageAsync(request);

                var message = response.Messages.OrderByDescending(x => x.MessageId).FirstOrDefault();
                if (message != null)
                {
                    var messageDesserialized = JsonSerializer.Deserialize<Response>(message.Body);

                    if (messageDesserialized?.UserData is not null)
                    {
                        switch (messageDesserialized?.RequestType)
                        {
                            case RequestType.POST:
                                await _messageHub.Clients.Group("Users").SendAsync("CreatedUser", messageDesserialized);
                                break;
                            case RequestType.PUT:
                                await _messageHub.Clients.Group("Users").SendAsync("UpdatedUser", messageDesserialized);
                                break;
                            case RequestType.DELETE:
                                await _messageHub.Clients.Group("Users").SendAsync("DeletedUser", messageDesserialized);
                                break;
                            default:
                                break;
                        }
                    }
                    else if (messageDesserialized?.ProductData is not null)
                    {
                        switch (messageDesserialized?.RequestType)
                        {
                            case RequestType.POST:
                                await _messageHub.Clients.Group("Products").SendAsync("CreatedProduct", messageDesserialized);
                                break;
                            case RequestType.PUT:
                                await _messageHub.Clients.Group("Products").SendAsync("UpdatedProduct", messageDesserialized);
                                break;
                            case RequestType.DELETE:
                                await _messageHub.Clients.Group("Products").SendAsync("DeletedProduct", messageDesserialized);
                                break;
                            default:
                                break;
                        }
                    }

                    
                    var deleteRequest = new DeleteMessageRequest()
                    {
                        QueueUrl = "https://sqs.us-east-1.amazonaws.com/473698569966/conceptsQueue",
                        ReceiptHandle = message.ReceiptHandle
                    };

                    await client.DeleteMessageAsync(deleteRequest);
                }
            }
        }
    }
}
