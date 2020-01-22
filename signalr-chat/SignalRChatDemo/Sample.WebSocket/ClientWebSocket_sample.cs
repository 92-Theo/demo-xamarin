using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;
using Websocket.Client;

namespace Sample.WebSocket
{
    internal class ClientWebSocket_sample
    {
        private static readonly ManualResetEvent ExitEvent = new ManualResetEvent(false);

        public static bool IsRunRead { get; set; } = false;
        public static bool IsRun { get; set; } = true;
        public static List<byte> Message = new List<byte>();

        public static async Task Run()
        {
            InitLogging();

            AppDomain.CurrentDomain.ProcessExit += CurrentDomainOnProcessExit;
            AssemblyLoadContext.Default.Unloading += DefaultOnUnloading;
            Console.CancelKeyPress += ConsoleOnCancelKeyPress;

            Console.WriteLine("|=======================|");
            Console.WriteLine("|    CLIEN WEBSOCKETT   |");
            Console.WriteLine("|=======================|");
            Console.WriteLine();

            Log.Debug("====================================");
            Log.Debug("              STARTING              ");
            Log.Debug("====================================");

            ClientWebSocket client = new ClientWebSocket();
            Thread recvThread = new Thread(new ParameterizedThreadStart(Read));
            Thread sendThread = new Thread(new ParameterizedThreadStart(Write));

            while (IsRun)
            {
                Log.Debug("====================================");
                Log.Debug("Retry");
                Log.Debug("====================================");
                // Connect
                while (client.State != WebSocketState.Open)
                {
                    if (!IsRun)
                        break;
                    try
                    {
                        Log.Information("Try ConnectAsync");
                        await client.ConnectAsync(new Uri(Program.URI), CancellationToken.None);
                    }
                    catch (Exception e)
                    {
                        Log.Error(e, "ConnectAsync");
                    }
                }

                // Receive
                if (client.State == WebSocketState.Open)
                {
                    if (!IsRun)
                        break;

                    IsRunRead = true;
                    sendThread.Start(client);
                    recvThread.Start(client);
                }
                Log.Debug("====================================");
                Log.Debug("Wait");
                Log.Debug("====================================");
                ExitEvent.WaitOne();
            }
            Log.Debug("====================================");
            Log.Debug("Exit");
            Log.Debug("====================================");


        }

        public static async void Write(object obj)
        {
            while (IsRunRead)
            {
                ClientWebSocket client = obj as ClientWebSocket;
                if (client == null)
                {
                    Log.Warning("Read client casting NULL");
                    IsRunRead = false;
                    break;
                }
                if (client.State != WebSocketState.Open)
                {
                    Log.Warning("Read Not Open");
                    IsRunRead = false;
                    break;
                }
                string msg = Console.ReadLine();
                if (msg == default)
                    continue;
                try
                {
                    await client.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(msg)), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                catch (InvalidDataException ide)
                {
                    Log.Error(ide, "Send");
                }
                catch (ObjectDisposedException ode)
                {
                    Log.Error(ode, "Send");
                }
            }
        }
        public static async void Read(object obj)
        {
            Message.Clear();
            byte[] buffer = new byte[4096];
            while (IsRunRead)
            {
                ClientWebSocket client = obj as ClientWebSocket;
                if (client == null)
                {
                    Log.Warning("Read client casting NULL");
                    IsRunRead = false;
                    break;
                }
                if (client.State != WebSocketState.Open)
                {
                    Log.Warning("Read Not Open");
                    IsRunRead = false;
                    break;
                }

                var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.Count > 0 && result.MessageType == WebSocketMessageType.Text)
                {
                    Message.AddRange(buffer);
                    buffer.Initialize();
                    if (result.EndOfMessage)
                    {
                        var msg = Encoding.UTF8.GetString(Message.ToArray());
                        Message.Clear();
                        Log.Information($"Received :{msg}");
                    }
                }

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    Log.Warning("Received Close Message");
                }
            }
        }

        private static void InitLogging()
        {
            var executingDir = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            var logPath = Path.Combine(executingDir, "logs", "verbose.log");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
                .WriteTo.ColoredConsole(LogEventLevel.Verbose,
                    outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] {Message} {NewLine}{Exception}")
                .CreateLogger();

        }

        private static void CurrentDomainOnProcessExit(object sender, EventArgs eventArgs)
        {
            Log.Warning("Exiting process");
            IsRun = false;
            IsRunRead = false;
            ExitEvent.Set();
        }
        private static void DefaultOnUnloading(AssemblyLoadContext assemblyLoadContext)
        {
            Log.Warning("Unloading process");
            IsRun = false;
            IsRunRead = false;
            ExitEvent.Set();
        }

        private static void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Log.Warning("Canceling process");
            IsRun = false;
            IsRunRead = false;
            e.Cancel = true;
            ExitEvent.Set();
        }
    }
}
