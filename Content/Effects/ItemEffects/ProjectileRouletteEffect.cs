using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace TerrariaChaosMod.Content.Effects.ItemEffects;

public class ProjectileRouletteEffect : Effect
{
    private static HashSet<int> _excludedProjectiles;

    static ProjectileRouletteEffect()
    {
        _excludedProjectiles = new HashSet<int>
        {
            ProjectileID.DryadsWardCircle,
        };
    }

    public static int NextProjectile()
    {
        int projectile;
        do
        {
            projectile = Main.rand.Next(1, ProjectileID.Count);
        }
        while (_excludedProjectiles.Contains(projectile));
        return projectile;
    }

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
