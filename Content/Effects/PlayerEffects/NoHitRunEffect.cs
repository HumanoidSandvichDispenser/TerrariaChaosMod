using Terraria;

namespace TerrariaChaosMod.Content.Effects.PlayerEffects;

public class NoHitRunEffect : Effect
{
    public override void PostUpdate(Player player)
    {
        player.GetModPlayer<EffectPlayer>().IsNoHitRunActive = true;
        base.PostUpdate(player);
    }
}
