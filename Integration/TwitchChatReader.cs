using System;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace TerrariaChaosMod.Integration;

public class TwitchChatReader
{
    private readonly TwitchClient _client;

    private readonly ConnectionCredentials _credentials;

    public bool IsConnected => _client.IsConnected;

    public event EventHandler<CommonMessageArgs> OnMessageReceived;

    public event EventHandler<CommonReadyArgs> OnConnected;

    public TwitchChatReader()
    {
        _client = new();

        const string USERNAME = "justinfan777";

        const string PASS = "";

        _credentials = new(USERNAME, PASS);

        var clientOptions = new ClientOptions
        {
            MessagesAllowedInPeriod = 750,
            ThrottlingPeriod = TimeSpan.FromSeconds(30)
        };

        WebSocketClient wsClient = new(clientOptions);
        _client = new TwitchClient(wsClient);

        _client.OnConnected += Client_OnConnected;
        _client.OnMessageReceived += Client_OnMessageReceived;
    }

    ~TwitchChatReader()
    {
        Disconnect();
    }

    public bool Connect(string channelName)
    {
        _client.Initialize(_credentials, channelName);
        return _client.Connect();
    }

    public void Disconnect()
    {
        if (_client.IsConnected)
        {
            _client.Disconnect();
        }
    }

    private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
    {
        var args = new CommonMessageArgs
        {
            Username = e.ChatMessage.Username,
            Message = e.ChatMessage.Message,
            IsPrivileged = e.ChatMessage.IsModerator ||
                e.ChatMessage.IsBroadcaster ||
                e.ChatMessage.IsVip
        };

        OnMessageReceived?.Invoke(this, args);
    }

    private void Client_OnConnected(object sender, OnConnectedArgs e)
    {
        var args = new CommonReadyArgs
        {
            ChannelName = e.AutoJoinChannel
        };

        OnConnected?.Invoke(this, args);
    }
}
