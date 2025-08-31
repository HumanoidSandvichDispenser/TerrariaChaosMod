using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content.Effects.PlayerEffects;

public sealed class EffectPlayer : ModPlayer
{
    public int HealValueScale = 1;

    public float Zoom = -1;

    public Vector2 GravityDirection = Vector2.Zero;
    public bool DoGravity = false;

    public override void ResetEffects()
    {
        HealValueScale = 1;
        Zoom = -1;
        DoGravity = true;
    }

    public override void GetHealLife(Item item, bool quickHeal, ref int healValue)
    {
        healValue = (int)(healValue * HealValueScale);
    }

    public override void ModifyZoom(ref float zoom)
    {
        zoom = Zoom;
    }

    public override void PreUpdate()
    {
        if (DoGravity)
        {
            // apply sideways gravity, ensuring velocity in each component does not
            // exceed terminal velocity
            float terminalVelocity = Player.maxFallSpeed;
            float newX = Player.velocity.X + GravityDirection.X;
            float newY = Player.velocity.Y + GravityDirection.Y;

            // can exceed terminal velocity if already exceeding it, but gravity won't
            // make it exceed terminal velocity if it isn't already
            if (System.Math.Abs(newX) < terminalVelocity)
            {
                Player.velocity.X = newX;
            }
            else if (System.Math.Abs(Player.velocity.X) < terminalVelocity)
            {
                Player.velocity.X = terminalVelocity * System.Math.Sign(newX);
            }

            if (System.Math.Abs(newY) < terminalVelocity)
            {
                Player.velocity.Y = newY;
            }
            else if (System.Math.Abs(Player.velocity.Y) < terminalVelocity)
            {
                Player.velocity.Y = terminalVelocity * System.Math.Sign(newY);
            }
        }
    }
}
