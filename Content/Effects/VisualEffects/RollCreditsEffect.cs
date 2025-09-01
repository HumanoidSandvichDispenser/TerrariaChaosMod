using Terraria;
using Terraria.Graphics.Effects;

namespace TerrariaChaosMod.Content.Effects.VisualEffects;

public class RollCreditsEffect : Effect
{
    public override void ApplyEffect(Player player)
    {
        SkyManager.Instance.Activate("MoonLord", player.Center);

        base.ApplyEffect(player);
    }

    public override bool Update(Player player)
    {
        return base.Update(player);
    }
}
