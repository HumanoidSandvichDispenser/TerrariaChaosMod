using Terraria;

namespace TerrariaChaosMod.Content.Effects.PlayerEffects;

public class PacifistRunEffect : Effect
{
    public override void PostUpdate(Player player)
    {
        player.GetModPlayer<EffectPlayer>().IsPacifistRunActive = true;
        base.PostUpdate(player);
    }
}
