using Terraria;
using Terraria.ID;

namespace TerrariaChaosMod.Content.Effects.PlayerEffects;

public class TemporaryMediumcoreEffect : Effect
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
