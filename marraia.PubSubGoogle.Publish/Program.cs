using Google.Apis.Auth.OAuth2;
using Google.Cloud.PubSub.V1;
using Grpc.Auth;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace marraia.PubSubGoogle.Publish
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var credential = GoogleCredential.FromFile(@"C:\_fbfm\credentials.json");
            var createSettings = new PublisherClient.ClientCreationSettings(credentials: credential.ToChannelCredentials());

            var topic = TopicName.FromProjectTopic("marraiademo", "marraia");
            var publisher = await PublisherClient.CreateAsync(topic, clientCreationSettings: createSettings);

            int publishedMessageCount = 0;
            var message = "Fernando Mendes";
            try
            {
                string publishMessage = await publisher.PublishAsync(message);
                Console.WriteLine($"Published message {publishMessage}");
                Interlocked.Increment(ref publishedMessageCount);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"An error ocurred when publishing message {message}: {exception.Message}");
            }
        }
    }
}
