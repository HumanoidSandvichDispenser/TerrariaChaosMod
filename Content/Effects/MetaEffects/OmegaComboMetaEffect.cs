using Terraria;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content.Effects.MetaEffects;

public class OmegaComboMetaEffect : Effect
{
    public override int Duration => Seconds(10);

    public override double Weight => HardmodeWeight(base.Weight / 8, base.Weight);

    public override void ApplyEffect(Player player)
    {
        var system = ModContent.GetInstance<ChaosEffectsSystem>();
        system.DurationMultiplier += 32;
        base.ApplyEffect(player);
    }

    public override void CleanUp(Player player)
    {
        var system = ModContent.GetInstance<ChaosEffectsSystem>();
        system.DurationMultiplier -= 32;
        base.CleanUp(player);
    }
}
