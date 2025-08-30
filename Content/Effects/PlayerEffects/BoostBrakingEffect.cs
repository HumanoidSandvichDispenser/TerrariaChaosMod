using Microsoft.Xna.Framework;
using Terraria;

namespace TerrariaChaosMod.Content.Effects.PlayerEffects;

public class BoostBrakingEffect : Effect
{
    public override bool Update(Player player)
    {
        Vector2 input = Vector2.Zero;

        if (player.controlLeft)
        {
            input.X -= 1;
        }
        if (player.controlRight)
        {
            input.X += 1;
        }

        if (input.X != 0 && Vector2.Dot(player.velocity, input) < 0)
        {
            player.velocity += -input * 2;
        }

        return base.Update(player);
    }
}
