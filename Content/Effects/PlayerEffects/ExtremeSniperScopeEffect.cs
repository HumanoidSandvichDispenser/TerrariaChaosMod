using Terraria;

namespace TerrariaChaosMod.Content.Effects.PlayerEffects;

public class ExtremeSniperScopeEffect : Effect
{
    public override void PostUpdate(Player player)
    {
        player.GetModPlayer<EffectPlayer>().Zoom = 2;
        base.PostUpdate(player);
    }
}
