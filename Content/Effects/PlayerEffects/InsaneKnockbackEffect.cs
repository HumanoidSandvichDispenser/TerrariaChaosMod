using Terraria;

namespace TerrariaChaosMod.Content.Effects.PlayerEffects;

public class InsaneKnockbackEffect : Effect
{
    public override bool Update(Player player)
    {
        player.GetModPlayer<EffectPlayer>().InsaneKnockback = true;
        return base.Update(player);
    }

    public override void PostUpdate(Player player)
    {
        player.GetModPlayer<EffectPlayer>().InsaneKnockback = true;
        base.PostUpdate(player);
    }
}
