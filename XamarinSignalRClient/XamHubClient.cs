using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace XamarinSignalRClient
{
    /// <summary>
    /// Api Service CLient
    /// </summary>
    public class XamHubClient
    {
        /// <summary>
        /// Signal Handler
        /// </summary>
        public event Action<XamHubMessage> OnMessage = delegate { };

        /// <summary>
        /// Is this connected to the server
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.State == HubConnectionState.Connected;
            }
        }

        HubConnection _connection;
        private bool _enabled;

        public XamHubClient(string path)
        {
            _connection = new HubConnectionBuilder()
                      .WithUrl(path)
                      .Build();

            _connection.On<XamHubMessage>("OnMessage", HandleMessage);
            _connection.Closed += _connection_Closed;

        }

        private Task _connection_Closed(Exception arg)
        {
            Console.WriteLine(arg.Message);

            if (_enabled)
            {
                return Connect(_enabled);
            }
            else
            {
                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// connect
        /// </summary>
        public async Task<bool> Connect(bool enableReconnect = false)
        {
            try
            {
                if (IsConnected)
                {
                    Console.WriteLine("Already connected or connecting");
                    return false;
                }

                _enabled = enableReconnect;

                await _connection.StartAsync();

                if (!IsConnected)
                {
                    Console.WriteLine("Failed to connect");
                }

                return IsConnected;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task DisposeAsync()
        {
            _enabled = false;

            try
            {
                if (_connection != null)
                {
                    await _connection.DisposeAsync();
                }
                _connection = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task SendAll(string message)
        {
            await _connection.SendAsync("SendAll", message);
        }

        public async Task SendOthers(string message)
        {
            await _connection.SendAsync("SendOthers", message);
        }

        public async Task SendTo(string message, string connectionId)
        {
            await _connection.SendAsync("SendTo", message, connectionId);
        }

        void HandleMessage(XamHubMessage signal)
        {
            OnMessage(signal);
        }
    }
}
