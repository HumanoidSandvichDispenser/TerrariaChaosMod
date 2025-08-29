using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;

namespace TerrariaChaosMod.Content.Buffs;

public class KillPlayerBuff : BaseChaosBuff
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

        var reason = NetworkText.FromKey("Mods.TerrariaChaosMod.DeathReasons.KillPlayerBuff");
        player.KillMe(PlayerDeathReason.ByCustomReason(reason), 9999, 0);
    }
}
