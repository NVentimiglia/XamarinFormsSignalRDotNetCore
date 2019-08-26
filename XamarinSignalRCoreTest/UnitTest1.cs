using NUnit.Framework;
using System.Threading.Tasks;
using XamarinSignalRClient;

namespace Tests
{
    public class Tests
    {
        [Test]
        public async Task Test1()
        {
            string path = "ws://localhost:5009/signal/xamhub";

            var client = new XamHubClient(path);
            await client.Connect(true);
            Assert.IsTrue(client.IsConnected);

            var client2 = new XamHubClient(path);
            await client2.Connect(true);
            Assert.IsTrue(client2.IsConnected);

            int counter = 0;
            client.OnMessage += (s) =>
            {
                counter++;
            };
            client2.OnMessage += (s) =>
            {
                counter++;
            };
            
            await client.SendAll("Hello");
            await client.SendOthers("Hello");
            await client2.SendAll("Hello");
            await client2.SendOthers("Hello");

            await Task.Delay(1000);
            Assert.IsTrue(counter == 6);

            await client.DisposeAsync();
            Assert.IsTrue(!client.IsConnected);

            await client2.DisposeAsync();
            Assert.IsTrue(!client2.IsConnected);
        }
    }
}