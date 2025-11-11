using System;
using System.Threading.Tasks;
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

    public event EventHandler<CommonReadyArgs> OnJoinedChannel;

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
        _client.OnJoinedChannel += Client_OnJoinedChannel;
    }

    ~TwitchChatReader()
    {
        Disconnect();
    }

    public bool Connect(string channelName = null)
    {
        _client.Initialize(_credentials, channelName);
        return _client.Connect();
    }

    public async Task<bool> ConnectAsync(string channelName)
    {
        // twitch client does not support async connect
        // so we just call the sync version in a task
        // and wait for it to complete

        return await Task.Run(() => Connect(channelName));
    }

    public void Disconnect()
    {
        if (_client.IsConnected)
        {
            _client.Disconnect();
        }
    }

    public void JoinChannel(string channelName)
    {
        if (!_client.IsConnected)
        {
            Connect(channelName);
        }
        else
        {
            LeaveAllChannels();

            _client.JoinChannel(channelName);
        }
    }

    public void LeaveAllChannels()
    {
        if (_client.IsConnected)
        {
            foreach (var channel in _client.JoinedChannels)
            {
                _client.LeaveChannel(channel);
            }
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
        };

        OnConnected?.Invoke(this, args);
    }

    private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
    {
        var args = new CommonReadyArgs
        {
            ChannelName = e.Channel
        };

        OnJoinedChannel?.Invoke(this, args);
    }
}
