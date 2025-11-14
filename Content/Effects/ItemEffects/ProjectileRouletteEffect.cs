using Terraria;

namespace TerrariaChaosMod.Content.Effects.ItemEffects;

public class ProjectileRouletteEffect : Effect
{
    public override bool ShouldApplyNow(Player player)
    {
        return player.GetModPlayer<PlayerEffects.EffectPlayer>()
            .ProjectileRoulette
            .Acquire();
    }

    public override void CleanUp(Player player)
    {
        player.GetModPlayer<PlayerEffects.EffectPlayer>()
            .ProjectileRoulette
            .Release();
        base.CleanUp(player);
    }
}
