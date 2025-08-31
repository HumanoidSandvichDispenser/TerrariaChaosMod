using System.Collections.Generic;
using System.Linq;

namespace TerrariaChaosMod.Content;

public class TwitchVoteEffectProvider : IEffectProvider
{
    public bool IsReady { get; private set; } = false;

    private IReadOnlySet<Effects.Effect> _effectsList;

    private HashSet<Effects.Effect> _randomPool = new HashSet<Effects.Effect>();

    private Effects.Effect[] _votingPool = new Effects.Effect[3];

    private int[] _votes = new int[3];

    public int VoteNumberOffset { get; private set; } = 0;

    public void ReinitializePool(IReadOnlySet<Effects.Effect> effectPool)
    {
        // when initializing the pool, take 3 random effects from the effect
        // pool and store them for voting

        _effectsList = effectPool;
        SetupVotingPool();
        IsReady = true;
    }

    private void SetupVotingPool()
    {
        _randomPool.Clear();
        _randomPool.UnionWith(_effectsList);

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
