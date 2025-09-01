using Terraria;
using Microsoft.Xna.Framework;

namespace TerrariaChaosMod.Content.Effects.PlayerEffects;

public class SidewaysGravityEffect : Effect
{
    Vector2 gravityDirection = Vector2.Zero;

    public override void ApplyEffect(Terraria.Player player)
    {
        float x = Main.rand.NextFloat(0.5f, 1) * (Main.rand.NextBool() ? 1 : -1);
        float y = Main.rand.NextFloat(-0.125f, 0);
        gravityDirection = new Vector2(x, y);
        gravityDirection.Normalize();
        gravityDirection *= player.gravity;
        gravityDirection.Y -= player.gravity;

        base.ApplyEffect(player);
    }

    public override void PostUpdate(Terraria.Player player)
    {
        // apply sideways gravity, ensuring velocity in each component does not
        // exceed terminal velocity
        float terminalVelocity = player.maxFallSpeed;
        float newX = player.velocity.X + gravityDirection.X;
        float newY = player.velocity.Y + gravityDirection.Y;

        // can exceed terminal velocity if already exceeding it, but gravity won't
        // make it exceed terminal velocity if it isn't already
        if (System.Math.Abs(newX) < terminalVelocity)
        {
            player.velocity.X = newX;
        }
        else if (System.Math.Abs(player.velocity.X) < terminalVelocity)
        {
            player.velocity.X = terminalVelocity * System.Math.Sign(newX);
        }

        if (System.Math.Abs(newY) < terminalVelocity)
        {
            player.velocity.Y = newY;
        }
        else if (System.Math.Abs(player.velocity.Y) < terminalVelocity)
        {
            player.velocity.Y = terminalVelocity * System.Math.Sign(newY);
        }

        base.PostUpdate(player);
    }
}
