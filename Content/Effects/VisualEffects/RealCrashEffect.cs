using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content.Effects.VisualEffects;

public class RealCrashEffect : FakeCrashEffect
{
    public override string StatusText => "";

    public override bool ShouldIncludeInPool(ICollection<Effect> pool)
    {
        // only include this effect if this is the voting pool,
        // so this effect can only be chosen by Twitch votes
        var chaosSystem = ModContent.GetInstance<ChaosEffectsSystem>();
        return pool == chaosSystem.TwitchVoteEffectProvider?.VotingPool;
    }

    private bool _hasCrashed = false;

    public override void ApplyEffect(Player player)
    {
        SetResponding(false);
        WorldGen.saveAndPlay();

        _hasCrashed = false;
        After(61,
            () => {
                // exit the game after a second delay
                Main.WeGameRequireExitGame();
                _hasCrashed = true;
            });

        base.ApplyEffect(player);
    }

    public override bool Update(Player player)
    {
        base.Update(player);
        return _hasCrashed;
    }
}
