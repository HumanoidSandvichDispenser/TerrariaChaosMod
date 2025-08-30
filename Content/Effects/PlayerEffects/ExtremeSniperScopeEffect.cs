using Terraria;

namespace TerrariaChaosMod.Content.Effects.PlayerEffects;

public class ExtremeSniperScopeEffect : Effect
{
    public override bool Update(Player player)
    {
        player.GetModPlayer<EffectPlayer>().Zoom = 2;
        return base.Update(player);
    }
}
