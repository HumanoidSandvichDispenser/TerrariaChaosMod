using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TerrariaChaosMod.Integration;

public class WebSocketServer
{
    private readonly HttpListener _listener = new();
    private readonly ConcurrentDictionary<Guid, WebSocket> _clients = new();
    private readonly CancellationTokenSource _cts = new();

    public bool IsRunning => _listener.IsListening;

    /// <summary>
    /// Initializes a new instance of the <see cref="WebSocketServer"/> class.
    /// </summary>
    /// <param name="uriPrefix">
    /// The URI prefix to listen on (e.g., "http://localhost:8080/ws/").
    /// </param>
    public WebSocketServer(string uriPrefix)
    {
        _listener.Prefixes.Add(uriPrefix);
    }

    private void HandlePreflight(HttpListenerResponse response)
    {
        response.AddHeader("Access-Control-Allow-Origin", "*");
        response.AddHeader("Access-Control-Allow-Headers", "Content-Type");
        response.AddHeader("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
        response.StatusCode = 204;
    }

    public async Task StartAsync()
    {
        _listener.Start();
        Console.WriteLine("WebSocket server started.");

        while (!_cts.Token.IsCancellationRequested)
        {
            var context = await _listener.GetContextAsync();

            if (context.Request.HttpMethod == "OPTIONS")
            {
                HandlePreflight(context.Response);
                context.Response.Close();
                continue;
            }

            if (context.Request.IsWebSocketRequest)
            {
                _ = HandleClientAsync(context); // fire-and-forget
            }
            else
            {
                context.Response.StatusCode = 400;
                context.Response.Close();
            }
        }
    }

    public async Task StopAsync()
    {
        _cts.Cancel();

        // close all client connections
        foreach (var (id, socket) in _clients)
        {
            try
            {
                await socket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "Server shutting down",
                    CancellationToken.None);
            }
            catch
            {

            }
        }

        _cts.Dispose();
        _listener.Close();
    }

    private async Task HandleClientAsync(HttpListenerContext context)
    {
        WebSocket webSocket = null!;
        Guid id = Guid.NewGuid();

        try
        {
            var wsContext = await context.AcceptWebSocketAsync(null);
            webSocket = wsContext.WebSocket;
            _clients.TryAdd(id, webSocket);

            Console.WriteLine($"Client {id} connected.");

            var buffer = new byte[1024];
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(buffer, _cts.Token);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Client error: {ex.Message}");
        }
        finally
        {
            if (webSocket != null)
            {
                _clients.TryRemove(id, out _);
            }

            try
            {
                await webSocket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "Closing",
                    CancellationToken.None);
            }
            catch
            {

            }
            Console.WriteLine($"Client {id} disconnected.");
        }
    }

    public async Task BroadcastAsync(string data)
    {
        byte[] message = Encoding.UTF8.GetBytes(data);

        foreach (var (id, socket) in _clients)
        {
            if (socket.State != WebSocketState.Open)
            {
                continue;
            }

            try
            {
                await socket.SendAsync(message, WebSocketMessageType.Text, true, _cts.Token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending to {id}: {ex.Message}");
            }
        }
    }
}
