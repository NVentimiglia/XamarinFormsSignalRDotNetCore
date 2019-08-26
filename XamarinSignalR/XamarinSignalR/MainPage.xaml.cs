using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinSignalRClient;

namespace XamarinSignalR
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                string path = "ws://localhost:5009/signal/xamhub";

                output.Text = "Starting Test";

                var client = new XamHubClient(path);
                await client.Connect(true);
                Assert(client.IsConnected, "client1 did not connect");

                var client2 = new XamHubClient(path);
                await client2.Connect(true);
                Assert(client2.IsConnected, "client2 did not connect");

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
                Assert(counter == 6, "client did get messages " + counter);

                await client.DisposeAsync();
                Assert(!client.IsConnected, "client1 did not disconnect");

                await client2.DisposeAsync();
                Assert(!client2.IsConnected, "client2 did not disconnect");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                output.Text = ex.Message;
                output.TextColor = Color.Red;
            }
        }

        public void Assert(bool condition, string message)
        {
            if (condition)
            {
                return;
            }

            output.Text = message;
            throw new Exception(message);
        }
    }
}
