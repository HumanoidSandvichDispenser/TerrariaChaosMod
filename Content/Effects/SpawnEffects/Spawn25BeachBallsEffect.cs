using Microsoft.Xna.Framework;
using Terraria;

namespace TerrariaChaosMod.Content.Effects.SpawnEffects;

public class Spawn25BeachBallsEffect : Effect
{
    public override EffectSide Side => EffectSide.Server;

    private int _count = 0;

    private int[] _ids;

    public override void ApplyEffect(Player player)
    {
        _ids = new int[25];
        base.ApplyEffect(player);
    }

    public override void PostUpdate(Player player)
    {
        for (int i = 0; i < 5 && _count < 25; i++)
        {
            // spawn a beach ball above the player
            Vector2 dir = -Vector2.UnitY * 16f;
            float angleDeviation = Main.rand.NextFloatDirection() * MathHelper.PiOver4;

            Projectile.NewProjectile(
                null,
                player.Center,
                dir.RotatedBy(angleDeviation),
                Terraria.ID.ProjectileID.BeachBall,
                0,
                500,
                Main.myPlayer);
            _count++;
        }
    }

    public override void CleanUp(Player player)
    {
        foreach (int id in _ids)
        {
            // remove projectile if beach ball
            if (Main.projectile[id].type == Terraria.ID.ProjectileID.BeachBall)
            {
                Main.projectile[id].Kill();
            }
        }
    }
}
