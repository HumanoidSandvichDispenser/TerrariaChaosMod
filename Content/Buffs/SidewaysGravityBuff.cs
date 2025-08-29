using Microsoft.Xna.Framework;

namespace TerrariaChaosMod.Content.Buffs;

public class SidewaysGravityBuff : BaseChaosBuff
{
    public override void Update(Terraria.Player player, ref int buffIndex)
    {
        if (player.buffTime[buffIndex] > 1)
        {
            int mod = player.buffTime[buffIndex] % 240;

            player.GetModPlayer<ChaosModPlayer>().manualGravity =
                -Vector2.UnitX * player.gravity;

            if (mod < 120)
            {
                player.GetModPlayer<ChaosModPlayer>().manualGravity *= -1f;
            }
        }
        else
        {
            player.GetModPlayer<ChaosModPlayer>().manualGravity = Vector2.Zero;
        }
    }
}
