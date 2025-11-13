using Terraria;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content.Effects.MetaEffects;

public class ComboMetaEffect : Effect
{
    public override int Duration => Seconds(10);

    public override void ApplyEffect(Player player)
    {
        var system = ModContent.GetInstance<ChaosEffectsSystem>();
        system.DurationMultiplier += 8;
        base.ApplyEffect(player);
    }

    public override void CleanUp(Player player)
    {
        var system = ModContent.GetInstance<ChaosEffectsSystem>();
        system.DurationMultiplier -= 8;
        base.CleanUp(player);
    }
}
