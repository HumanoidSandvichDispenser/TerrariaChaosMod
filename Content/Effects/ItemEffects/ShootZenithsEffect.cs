using Terraria;

namespace TerrariaChaosMod.Content.Effects.ItemEffects;

public class ShootZenithsEffect : Effect
{
    public override void ApplyEffect(Player player)
    {
        AcquireLock();
        base.ApplyEffect(player);
    }

    public override void CleanUp(Player player)
    {
        ReleaseLock();
        base.CleanUp(player);
    }
}
