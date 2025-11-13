using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content.Effects.MetaEffects;

public class CheatCodeVotingEffect : Effect
{
    private HashSet<Effect> _requestedEffects;

    public override bool ShouldIncludeInPool(ICollection<Effect> pool)
    {
        // Twitch integration must be enabled
        var system = ModContent.GetInstance<ChaosEffectsSystem>();
        return system.TwitchVoteEffectProvider?.IsReady ?? false;
    }

    public override void ApplyEffect(Player player)
    {
        _requestedEffects = new HashSet<Effect>();

        var system = ModContent.GetInstance<ChaosEffectsSystem>();
        system.TwitchVoteEffectProvider.OnChatMessageReceived += OnChatMessageReceived;

        Terraria.Main.NewText("Cheat Code Voting Effect Active! Type effect names in chat to request them!", Microsoft.Xna.Framework.Color.LimeGreen);

        base.ApplyEffect(player);
    }

    public override void CleanUp(Player player)
    {
        var system = ModContent.GetInstance<ChaosEffectsSystem>();
        system.TwitchVoteEffectProvider.OnChatMessageReceived -= OnChatMessageReceived;

        foreach (var effectName in _requestedEffects)
        {
            system.EffectsToApply.Enqueue(effectName);
        }

        base.CleanUp(player);
    }

    private void OnChatMessageReceived(object sender, Integration.CommonMessageArgs e)
    {
        var system = ModContent.GetInstance<ChaosEffectsSystem>();

        // lookup effect by name
        if (system.EffectDictionary.ContainsKey(e.Message))
        {
            var effect = system.EffectDictionary[e.Message];

            if (effect is CheatCodeVotingEffect)
            {
                // prevent requesting this effect again
                return;
            }

            _requestedEffects.Add(effect.Clone() as Effect);
        }
    }
}
