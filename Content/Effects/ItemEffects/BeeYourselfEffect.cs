using Terraria;

namespace TerrariaChaosMod.Content.Effects.ItemEffects;

public class BeeYourselfEffect : Effect
{
    public override bool ShouldApplyNow(Player player)
    {
        return player.GetModPlayer<PlayerEffects.EffectPlayer>()
            .BeeYourself
            .Acquire();
    }

    public override void CleanUp(Player player)
    {
        player.GetModPlayer<PlayerEffects.EffectPlayer>()
            .BeeYourself
            .Release();
        base.CleanUp(player);
    }
}
