using Terraria;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content.Effects.VisualEffects;

public class FakeLagEffect : Effect
{
    public override void ApplyEffect(Player player)
    {
        ModContent.GetInstance<ChaosVisualSystem>().ShouldSkipFrames = true;
        base.ApplyEffect(player);
    }

    public override void CleanUp(Player player)
    {
        ModContent.GetInstance<ChaosVisualSystem>().ShouldSkipFrames = false;
        base.CleanUp(player);
    }
}
