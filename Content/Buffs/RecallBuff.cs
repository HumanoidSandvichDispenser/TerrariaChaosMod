using Microsoft.Xna.Framework;

namespace TerrariaChaosMod.Content.Buffs;

public class RecallBuff : BaseChaosBuff
{
    public override void Update(Terraria.Player player, ref int buffIndex)
    {
        if (player.buffTime[buffIndex] == BaseChaosBuff.ONE_TIME_APPLY_TICK)
        {
            Vector2 spawn = new Vector2(player.SpawnX, player.SpawnY) * 16;
            player.Teleport(spawn);
        }
    }
}
