using Terraria;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content.Buffs;

public abstract class EffectBuff : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.debuff[Type] = true;
        Main.buffNoSave[Type] = true;
    }

    public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
    {
        rare = -13;
    }
}
