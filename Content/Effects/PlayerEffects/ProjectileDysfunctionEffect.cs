using Terraria;

namespace TerrariaChaosMod.Content.Effects.PlayerEffects;

public class ProjectileDysfunctionEffect : Effect
{
    public override bool ShouldApplyNow(Player player)
    {
        return player.GetModPlayer<PlayerEffects.EffectPlayer>()
            .ProjectileDysfunction
            .Acquire();
    }

    public override void CleanUp(Player player)
    {
        player.GetModPlayer<PlayerEffects.EffectPlayer>()
            .ProjectileDysfunction
            .Release();
        base.CleanUp(player);
    }
}
