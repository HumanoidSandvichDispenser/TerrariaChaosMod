#nullable enable

using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content;

public class ChaosVotingSystem : ModSystem
{
    private int _tickCounter = 0;
    private int _votingDuration = 600;
    private int _voteNumberOffset = 4;

    // all buffs in the mod
    private HashSet<int> _buffPool = new();

    // all buffs that can be voted on
    private List<int> _votingPool = new();

    // buffs not in the voting pool but can be selected through the random
    // choice
    private HashSet<int> _randomizerPool = new();

    // buffs that have been selected to be applied, used in case the player is
    // dead when the buff is triggered
    public Queue<int> buffsToApply = new();

    // history of applied buffs, used for meta effects that replay previous
    // buffs
    public Stack<int> buffHistory = new();

    private int[] _votes = new int[4];

    public override void OnModLoad()
    {
        //InitializeBuffs();
        //SetupVoting();
    }

    private void InitializeBuffs()
    {
        if (_buffPool.Count > 0)
        {
            return;
        }

        _buffPool = new HashSet<int>
        {
            //ModContent.BuffType<Buffs.FakeLagBuff>(),
            //ModContent.BuffType<Buffs.BoostBrakingBuff>(),
            //ModContent.BuffType<Buffs.EatDaPooPooBuff>(),
            //ModContent.BuffType<Buffs.SpaceProgramBuff>(),
            //ModContent.BuffType<Buffs.KillPlayerBuff>(),
            //ModContent.BuffType<Buffs.ReforgeCurrentItemBuff>(),
            //ModContent.BuffType<Buffs.SidewaysGravityBuff>(),
            //ModContent.BuffType<Buffs.AllCritBuff>(),
            //ModContent.BuffType<Buffs.RagsToRichesBuff>(),
            //ModContent.BuffType<Buffs.RecallBuff>(),
            //ModContent.BuffType<Buffs.HelenKellerBuff>(),
            //ModContent.BuffType<Buffs.BlindPlaythroughBuff>(),
            //ModContent.BuffType<Buffs.NoIFramesBuff>(),
            //ModContent.BuffType<Buffs.PowerPlayBuff>(),
            //ModContent.BuffType<Buffs.ExtremeSniperScopeBuff>(),
        };
    }

    public override void PostUpdateWorld()
    {
        // only tick if world is active

        if (Main.gameMenu || !Main.LocalPlayer.active)
        {
            return;
        }

        _tickCounter++;

        if (_tickCounter >= _votingDuration)
        {
            _tickCounter = 0;
            //TriggerBuff();
            //SetupVoting();
        }
    }

    private void SetupVoting()
    {
        // pick 3 random buffs to be in the voting pool
        // put all other buffs in the randomizer pool

        _votingPool.Clear();
        _randomizerPool.Clear();

        _randomizerPool.UnionWith(_buffPool);

        var rand = new System.Random();

        _votingPool = Enumerable
            .Range(0, _buffPool.Count)
            .OrderBy(x => rand.Next())
            .Take(0)
            .Select(x => _buffPool.ElementAt(x))
            .ToList();

        //string buffNames = string.Join("\n", _votingPool.Select(x => GetBuffInstance(x).DisplayName));

        //Terraria.Chat.ChatHelper.BroadcastChatMessage(
        //    Terraria.Localization.NetworkText.FromLiteral("New chaos vote started!"),
        //    Microsoft.Xna.Framework.Color.LimeGreen);
        //Terraria.Chat.ChatHelper.BroadcastChatMessage(
        //    Terraria.Localization.NetworkText.FromLiteral(buffNames),
        //    Microsoft.Xna.Framework.Color.LimeGreen);

        foreach (var buff in _votingPool)
        {
            _randomizerPool.Remove(buff);
        }

        for (int i = 0; i < _votes.Length; i++)
        {
            _votes[i] = 0;
        }

        if (_voteNumberOffset == 0)
        {
            _voteNumberOffset = 4;
        }
        else
        {
            _voteNumberOffset = 0;
        }
    }

    public void CastVote(int voteNumber, string? voter = null)
    {
        // vote offset so stream delay does not affect voting
        voteNumber -= _voteNumberOffset;

        if (voteNumber < 1 || voteNumber > 4)
        {
            return;
        }

        _votes[voteNumber - 1]++;
    }

    //private Buffs.BaseChaosBuff GetBuffInstance(int buffType)
    //{
    //    if (!_buffPool.Contains(buffType))
    //    {
    //        throw new System.Exception("Buff type not in buff pool");
    //    }

    //    return (Buffs.BaseChaosBuff)ModContent.GetModBuff(buffType);
    //}

    private void TriggerBuff()
    {
        // weighted random selection based on votes

        var random = new System.Random();

        int pickRandomizer()
        {
            int index = random.Next(0, _randomizerPool.Count);
            return _randomizerPool.ElementAt(index);
        }

        int totalVotes = _votes.Sum();

        if (totalVotes == 0)
        {
            buffsToApply.Enqueue(pickRandomizer());
            return;
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
            buffsToApply.Enqueue(pickRandomizer());
        }
        else
        {
            buffsToApply.Enqueue(_votingPool[i]);
        }
    }
}
