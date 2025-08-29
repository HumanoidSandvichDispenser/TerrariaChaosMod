using Terraria.ModLoader;

namespace TerrariaChaosMod.Content.Buffs;

public class LargeItemsBuff : BaseChaosBuff
{
    public override void Update(Terraria.Player player, ref int buffIndex)
    {
        if (player.buffTime[buffIndex] != ONE_TIME_APPLY_TICK)
        {
            return;
        }

        player.HeldItem.scale = 3f;
    }
}
