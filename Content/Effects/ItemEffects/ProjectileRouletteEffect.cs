using Terraria;

namespace TerrariaChaosMod.Content.Effects.ItemEffects;

public class ProjectileRouletteEffect : Effect
{
    public override void ApplyEffect(Player player)
    {
        AcquireLock(true);
        base.ApplyEffect(player);
    }

    public override void CleanUp(Player player)
    {
        ReleaseLock();
        base.CleanUp(player);
    }
}
