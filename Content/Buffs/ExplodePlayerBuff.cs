using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;

namespace TerrariaChaosMod.Content.Buffs;

public class ExplodePlayerBuff : BaseChaosBuff
{
    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();
        Main.buffNoTimeDisplay[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        if (player.buffTime[buffIndex] != BaseChaosBuff.ONE_TIME_APPLY_TICK - 60 * 5)
        {
            return;
        }
    }
}
