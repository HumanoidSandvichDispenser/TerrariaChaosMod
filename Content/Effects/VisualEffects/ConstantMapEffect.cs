using Terraria;

namespace TerrariaChaosMod.Content.Effects.VisualEffects;

public class ConstantMapEffect : Effect
{
    public override void ApplyEffect(Player player)
    {
        Main.NewText("Sounds like you need help finding your way...", 50, 150, 255);
    }

    public override void PostUpdate(Terraria.Player player)
    {
        if (Main.rand.NextBool(120))
        {
            player.TryOpeningFullscreenMap();
        }
        base.PostUpdate(player);
    }
}
