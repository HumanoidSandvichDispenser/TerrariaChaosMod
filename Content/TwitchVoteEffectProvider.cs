using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Terraria.ModLoader;
using TerrariaChaosMod.Content.Effects.VisualEffects;
using TerrariaChaosMod.Integration;

namespace TerrariaChaosMod.Content;

public class TwitchVoteEffectProvider : IEffectProvider
{
    public bool IsReady { get; private set; } = false;

    public bool CanProvide { get; private set; } = false;

    private IReadOnlySet<Effects.Effect> _effectsList;

    private HashSet<Effects.Effect> _randomPool = new HashSet<Effects.Effect>();

    private Effects.Effect[] _votingPool = new Effects.Effect[3];

    public IReadOnlyList<Effects.Effect> VotingPool => _votingPool;

    private int[] _votes = new int[4];

    public int VoteNumberOffset { get; private set; } = 0;

    private Integration.TwitchChatReader _chatReader;

    public string CurrentChannel => _chatReader?.CurrentChannel;

    // for displaying vote results via websocket
    private WebSocketServer _wsServer;

    public event System.EventHandler<Integration.CommonMessageArgs> OnChatMessageReceived;

    public TwitchVoteEffectProvider()
    {

    }

    ~TwitchVoteEffectProvider()
    {
        DisconnectTwitch();
        _ = StopWebSocket();
    }

    public void ConnectTwitch(string channelName)
    {
        if (_chatReader is null)
        {
            _chatReader = new Integration.TwitchChatReader();
            _chatReader.OnMessageReceived += ChatReader_OnMessageReceived;
            _chatReader.OnConnected += ChatReader_OnConnected;
            _chatReader.OnJoinedChannel += ChatReader_OnConnected;
        }

        _chatReader.JoinChannel(channelName);
        IsReady = _chatReader.IsConnected;
    }

    public void DisconnectTwitch()
    {
        IsReady = false;
        CanProvide = false;

        if (_chatReader is not null)
        {
            _chatReader.LeaveAllChannels();
            _chatReader.Disconnect();
            _chatReader.OnMessageReceived -= ChatReader_OnMessageReceived;
            _chatReader.OnConnected -= ChatReader_OnConnected;
            _chatReader.OnJoinedChannel -= ChatReader_OnConnected;
        }

        _chatReader = null;
    }

    public void StartWebSocket()
    {
        const string URI = "http://localhost:9091/ws/";
        if (_wsServer is null)
        {
            _wsServer = new(URI);
        }

        if (_wsServer.IsRunning)
        {
            return;
        }

        TerrariaChaosMod.ChatLogger.Info($"Starting WebSocket server at {URI}");

        _ = _wsServer.StartAsync();
    }

    public async System.Threading.Tasks.Task StopWebSocket()
    {
        if (_wsServer?.IsRunning ?? false)
        {
            await _wsServer.StopAsync();
            TerrariaChaosMod.ChatLogger.Info($"Stopped WebSocket server");
        }
        _wsServer = null;
    }

    public async System.Threading.Tasks.Task RestartWebSocket()
    {
        await StopWebSocket();
        StartWebSocket();
    }

    public async void BroadcastTally(bool voteStarted = false)
    {
        if (!IsReady || _wsServer is null)
        {
            return;
        }

        int sum = _votes.Sum();
        List<TwitchVoteData.VoteOption> options = new();

        int i;
        for (i = 0; i < _votingPool.Length; i++)
        {
            string effectName = _votingPool[i].DisplayName.Value;
            int votes = _votes[i];

            float proportion = sum > 0 ? (float)votes / sum : 0;
            int number = i + 1 + VoteNumberOffset;
            options.Add(new(number, effectName, proportion));
        }

        // add random option
        float randomProportion = sum > 0 ? (float)_votes[i] / sum : 1;
        options.Add(new(i + 1 + VoteNumberOffset, "Random Effect", randomProportion));

        TwitchVoteData data = new(sum, options.ToArray(), voteStarted);

        _ = _wsServer.BroadcastAsync(JsonSerializer.Serialize(data));
    }

    private void ChatReader_OnMessageReceived(object sender, CommonMessageArgs e)
    {
        if (!IsReady)
        {
            return;
        }

        if (int.TryParse(e.Message, out int voteNumber))
        {
            if (voteNumber >= 1 + VoteNumberOffset && voteNumber <= 4 + VoteNumberOffset)
            {
                TallyVote(voteNumber);
            }
        }

        OnChatMessageReceived?.Invoke(this, e);
    }

    private void ChatReader_OnConnected(object sender, CommonReadyArgs e)
    {
        // notify that we have connected to chat

        //Terraria.Main.NewText($"Connected to channel {e.ChannelName}");
        if (string.IsNullOrEmpty(e.ChannelName))
        {
            TerrariaChaosMod.ChatLogger
                .Info("Connected to Twitch chat server.");
        }
        else
        {
            TerrariaChaosMod.ChatLogger
                .Info($"Joined Twitch channel: {e.ChannelName}");
        }
    }

    public void ReinitializePool(IReadOnlySet<Effects.Effect> effectPool)
    {
        // when initializing the pool, take 3 random effects from the effect
        // pool and store them for voting

        _effectsList = effectPool;
        SetupVotingPool();
        CanProvide = true;
        
        // broadcast tally to notify overlay of new vote
        BroadcastTally(true);

        // if config enables it, display voting options in-game chat
        if (ModContent.GetInstance<ChaosModConfig>().AnnounceNewVoteInChat)
        {
            StringBuilder sb = new("New chaos vote started!\n");
            sb.AppendJoin(
                "\n",
                _votingPool.Select((e, i) => $"{i + 1 + VoteNumberOffset}. {e.DisplayName}"));
            sb.Append($"\n{4 + VoteNumberOffset}. Random Effect");
            Terraria.Main.NewText(sb.ToString());
        }
    }

    private void SetupVotingPool()
    {
        _randomPool.Clear();
        _randomPool.UnionWith(_effectsList);
        _votingPool = new Effects.Effect[_votes.Length - 1];

        var rand = Terraria.Main.rand;

        const int TAKE_N = 3;

        while (_votingPool.Count(e => e is not null) < TAKE_N)
        {
            // pick a random effect from the effect list
            var effect = _effectsList.ElementAt(rand.Next(0, _effectsList.Count));

            // only add the effect if it is not already in the voting pool,
            // and it satisfies its condition for inclusion
            if (!_votingPool.Contains(effect) && effect.ShouldIncludeInPool(_votingPool))
            {
                _votingPool[_votingPool.Count(e => e is not null)] = effect;
            }
        }

        // remove all effects in the voting pool from the random pool
        foreach (var effect in _votingPool)
        {
            _randomPool.Remove(effect);
        }

        // remove all effects from random pool that do not satisfy its
        // condition for inclusion
        foreach (var effect in _effectsList)
        {
            if (!effect.ShouldIncludeInPool(_randomPool))
            {
                _randomPool.Remove(effect);
            }
        }

        // reset votes
        for (int i = 0; i < _votes.Length; i++)
        {
            _votes[i] = 0;
        }

        VoteNumberOffset = (VoteNumberOffset == 0) ? 4 : 0;
    }

    private void TallyVote(int voteNumber)
    {
        int index = voteNumber - VoteNumberOffset - 1;

        if (index < 0 || index >= _votes.Length)
        {
            return;
        }

        _votes[index]++;
    }

    public Effects.Effect GetEffect()
    {
        var random = new System.Random();

        Effects.Effect pickRandomizer()
        {
            int index = random.Next(0, _randomPool.Count);
            return _randomPool.ElementAt(index);
        }

        int totalVotes = _votes.Sum();

        if (totalVotes == 0)
        {
            return pickRandomizer();
        }

        int roll = random.Next(0, totalVotes);
        int cumulative = 0;
        int i;

        for (i = 0; i < _votes.Length; i++)
        {
            cumulative += _votes[i];
            if (roll < cumulative)
            {
                break;
            }
        }

        if (i == 3)
        {
            return pickRandomizer();
        }

        return _votingPool[i];
    }
}
