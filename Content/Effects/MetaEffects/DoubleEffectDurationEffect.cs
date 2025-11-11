using Terraria;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content.Effects.MetaEffects;

public class DoubleEffectDurationEffect : Effect
{
    public override void ApplyEffect(Player player)
    {
        var system = ModContent.GetInstance<ChaosEffectsSystem>();
        system.DurationMultiplier = 2;
        base.ApplyEffect(player);
    }

    public override void CleanUp(Player player)
    {
        var system = ModContent.GetInstance<ChaosEffectsSystem>();
        system.DurationMultiplier = 1;
        base.CleanUp(player);
    }
}
