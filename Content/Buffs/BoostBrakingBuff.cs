using Microsoft.Xna.Framework;
using Terraria;

namespace TerrariaChaosMod.Content.Buffs;

public class BoostBrakingBuff : BaseChaosBuff
{
    public override void Update(Player player, ref int buffIndex)
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
            //player.velocity *= 2f;

            player.velocity += -input * 2;
        }
    }
}
