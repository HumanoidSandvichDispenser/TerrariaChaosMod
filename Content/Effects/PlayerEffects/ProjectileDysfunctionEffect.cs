using Terraria;

namespace TerrariaChaosMod.Content.Effects.PlayerEffects;

public class ProjectileDysfunctionEffect : Effect
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
