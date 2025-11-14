using Terraria;

namespace TerrariaChaosMod.Content.Effects.PlayerEffects;

public class AFKEffect : Effect
{
    public override int Duration => Seconds(10);

    public override void PostUpdate(Player player)
    {
        player.GetModPlayer<EffectPlayer>().AfkMode = true;
        base.PostUpdate(player);
    }
}
