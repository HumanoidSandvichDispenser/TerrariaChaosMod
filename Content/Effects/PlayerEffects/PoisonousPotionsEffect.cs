using Terraria;

namespace TerrariaChaosMod.Content.Effects.PlayerEffects;

public class PoisonousPotionsEffect : Effect
{
    public override void PostUpdate(Player player)
    {
        player.GetModPlayer<EffectPlayer>().HealValueScale = -1;
        base.PostUpdate(player);
    }
}
