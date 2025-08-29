using Terraria;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content.Buffs;

public class FakeLagBuff : BaseChaosBuff
{
    public override void SetStaticDefaults()
    {
        Main.debuff[Type] = true; // Mark as a debuff
        Main.buffNoSave[Type] = true; // Do not save when exiting
        Main.buffNoTimeDisplay[Type] = false; // Show time remaining
    }

    public override void Update(Player player, ref int buffIndex)
    {
        var chaosModPlayer = player.GetModPlayer<ChaosModPlayer>();

        chaosModPlayer.shouldLag = true;
    }
}
