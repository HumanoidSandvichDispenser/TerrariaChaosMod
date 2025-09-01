using System.Collections.Generic;
using System.Linq;
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

    private int[] _votes = new int[3];

    public int VoteNumberOffset { get; private set; } = 0;

    private Integration.TwitchChatReader _chatReader;

    public TwitchVoteEffectProvider()
    {
        _chatReader = new Integration.TwitchChatReader();
        _chatReader.OnMessageReceived += ChatReader_OnMessageReceived;
        _chatReader.OnConnected += ChatReader_OnConnected;
    }

    public void Connect(string channel)
    {
        if (_chatReader.IsConnected)
        {
            _chatReader.Disconnect();
        }
        IsReady = _chatReader.Connect(channel);
    }

    public void Disconnect()
    {
        if (_chatReader.IsConnected)
        {
            _chatReader.Disconnect();
        }
        IsReady = false;
        CanProvide = false;
    }

    private void ChatReader_OnMessageReceived(object sender, CommonMessageArgs e)
    {
        if (!IsReady)
        {
            return;
        }

        Terraria.Main.NewText($"Message from {e.Username}: {e.Message}");
        if (int.TryParse(e.Message, out int voteNumber))
        {
            if (voteNumber >= 1 + VoteNumberOffset && voteNumber <= 4 + VoteNumberOffset)
            {
                Terraria.Main.NewText($"{e.Username} voted for option {voteNumber}");
                TallyVote(voteNumber);
            }
        }
    }

    private void ChatReader_OnConnected(object sender, CommonReadyArgs e)
    {
        // notify that we have connected to chat

        Terraria.Main.NewText($"Connected to channel {e.ChannelName}");
    }

    public void ReinitializePool(IReadOnlySet<Effects.Effect> effectPool)
    {
        // when initializing the pool, take 3 random effects from the effect
        // pool and store them for voting

        _effectsList = effectPool;
        SetupVotingPool();
        CanProvide = true;
    }

    private void SetupVotingPool()
    {
        var crash = ModContent.GetInstance<RealCrashEffect>();

        _randomPool.Clear();
        _randomPool.UnionWith(_effectsList);
        _randomPool.Add(crash);

        var rand = new System.Random();

        const int TAKE_N = 3;

        _votingPool = Enumerable
            .Range(0, _effectsList.Count)
            .OrderBy(x => rand.Next())
            .Take(TAKE_N)
            .Select(x => _effectsList.ElementAt(x))
            .ToArray();

        foreach (var effect in _votingPool)
        {
            _randomPool.Remove(effect);
        }

        // real crash can be voted for, but not randomly selected
        _randomPool.Remove(crash);

        for (int i = 0; i < _votes.Length; i++)
        {
            _votes[i] = 0;
        }

        VoteNumberOffset = (VoteNumberOffset == 0) ? 4 : 0;

        string text = "New chaos vote started!\n";
        text += string.Join("\n", _votingPool.Select((e, i) => $"{i + 1 + VoteNumberOffset}. {e.DisplayName}"));
        text += $"\n{4 + VoteNumberOffset}. Random Effect";
        Terraria.Main.NewText(text);
    }

    private void TallyVote(int voteNumber)
    {
        int index = voteNumber - VoteNumberOffset - 1;

        if (index < 0 || index >= _votingPool.Length)
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
