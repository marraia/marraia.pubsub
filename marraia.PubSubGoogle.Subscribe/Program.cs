using Google.Apis.Auth.OAuth2;
using Google.Cloud.PubSub.V1;
using Grpc.Auth;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace marraia.PubSubGoogle.Subscribe
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var credential = GoogleCredential.FromFile(@"C:\_fbfm\credentials.json");
            var createSettings = new SubscriberClient.ClientCreationSettings(credentials: credential.ToChannelCredentials());

            var acknowledge = true;
            var subscriptionName = SubscriptionName.FromProjectSubscription("marraiademo", "marraia-sub");
            var subscriber = await SubscriberClient.CreateAsync(subscriptionName, clientCreationSettings: createSettings);
            // SubscriberClient runs your message handle function on multiple
            // threads to maximize throughput.
            int messageCount = 0;
            Task startTask = subscriber.StartAsync((PubsubMessage message, CancellationToken cancel) =>
            {
                string text = System.Text.Encoding.UTF8.GetString(message.Data.ToArray());
                Console.WriteLine($"Message {message.MessageId}: {text}");
                Interlocked.Increment(ref messageCount);
                return Task.FromResult(acknowledge ? SubscriberClient.Reply.Ack : SubscriberClient.Reply.Nack);
            });
            // Run for 5 seconds.
            await Task.Delay(5000);
            //await subscriber.StopAsync(CancellationToken.None);
            // Lets make sure that the start task finished successfully after the call to stop.
            await startTask;
        }
    }
}
