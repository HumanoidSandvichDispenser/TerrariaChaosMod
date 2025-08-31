using Terraria;
using Terraria.ID;

namespace TerrariaChaosMod.Content.Effects.PlayerEffects;

public class TemporaryMediumcoreEffect : Effect
{
    public override void PostUpdate(Player player)
    {
        player.GetModPlayer<EffectPlayer>().TemporaryMediumcore = true;
        base.PostUpdate(player);
    }

    public override void CleanUp(Player player)
    {
        base.CleanUp(player);
    }
}
