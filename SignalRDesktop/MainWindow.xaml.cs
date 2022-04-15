using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SignalRDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HubConnection connection;
        public MainWindow()
        {
            InitializeComponent();

            connection = new HubConnectionBuilder()
                //.WithUrl("https://localhost:44345/chatHub")
                .WithUrl("https://localhost:7303/chatHub")
                //.WithUrl("http://localhost:5000/chatHub", options =>
                .WithUrl("http://localhost:5139/chatHub", options =>
                {
                    options.AccessTokenProvider = async () =>
                    {
                        // Get and return the access token.
                        return await Task.FromResult<string>("123");
                    };
                })
                //.WithUrl("http://localhost:5139/chatHub")
                //.WithUrl("http://localhost:5139/notificationHub")
                .ConfigureLogging(logging =>
                {
                    // Log to your custom provider
                    //logging.AddProvider(new MyCustomLoggingProvider());
                    //logging.AddDebug();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                 // .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(30) }) yields the default behavior.
                 //.WithAutomaticReconnect()
                 .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.Zero, TimeSpan.FromSeconds(10) })
                .AddJsonProtocol(options =>
                {
                    options.PayloadSerializerOptions.PropertyNamingPolicy = null;
                })
                .Build();


            connection.Reconnecting += error =>
            {
                Debug.Assert(connection.State == HubConnectionState.Reconnecting);

                // Notify users the connection was lost and the client is reconnecting.
                // Start queuing or dropping messages.

                Debug.Assert(connection.State == HubConnectionState.Connected);

                return Task.CompletedTask;
            };


            connection.Closed += async error =>
            {
                Debug.Assert(connection.State == HubConnectionState.Disconnected);

                // Notify users the connection has been closed or manually try to restart the connection.
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
                await Task.CompletedTask;
            };


            // Call client methods from hub
            connection.On<string>("online", (msg) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    txtInfo.Text += msg + "\r\n";
                });
            });

            connection.On<string, string>("ReceiveMessage", (user, msg) =>
             {
                 this.Dispatcher.Invoke(() =>
                 {
                     txtMsg.Text += $"{user}:{msg} \r\n";
                 });
             });

            connection.On<string>("Login2", (msg) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    txtInfo.Text += msg + "\r\n";
                });
            });

            connection.On<string>("Notify", (msg) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    txtInfo.Text += msg + "\r\n";
                });
            });

            connection.StartAsync();
        }

        private void btnIn_Click(object sender, RoutedEventArgs e)
        {
            string title = $"监工{new Random().Next(1, 99999)}号";
            Title = title;

            // Call hub methods from client
            connection.InvokeAsync("Login", title);
            btnSend.IsEnabled = true;
        }

        private void btnOut_Click(object sender, RoutedEventArgs e)
        {
            connection.InvokeAsync("SignOut", Title);
            connection.StopAsync();
            //connection.DisposeAsync();
            this.Close();
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (txtSend.Text == "") return;

            connection.InvokeAsync("SendMessage", Title, txtSend.Text);
        }


        public static async Task<bool> ConnectWithRetryAsync(HubConnection connection, CancellationToken token)
        {
            // Keep trying to until we can start or the token is canceled.
            while (true)
            {
                try
                {
                    await connection.StartAsync(token);
                    Debug.Assert(connection.State == HubConnectionState.Connected);
                    return true;
                }
                catch when (token.IsCancellationRequested)
                {
                    return false;
                }
                catch
                {
                    // Failed to connect, trying again in 5000 ms.
                    Debug.Assert(connection.State == HubConnectionState.Disconnected);
                    await Task.Delay(5000);
                }
            }
        }
    }


    public class RandomRetryPolicy : IRetryPolicy
    {
        private readonly Random _random = new Random();

        public TimeSpan? NextRetryDelay(RetryContext retryContext)
        {
            // If we've been reconnecting for less than 60 seconds so far,
            // wait between 0 and 10 seconds before the next reconnect attempt.
            if (retryContext.ElapsedTime < TimeSpan.FromSeconds(60))
            {
                return TimeSpan.FromSeconds(_random.NextDouble() * 10);
            }
            else
            {
                // If we've been reconnecting for more than 60 seconds so far, stop reconnecting.
                return null;
            }
        }
    }
}
