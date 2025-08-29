using Terraria;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content.Buffs;

public abstract class BaseChaosBuff : ModBuff
{
    public const int ONE_TIME_APPLY_TICK = 598;

    public override void SetStaticDefaults()
    {
        Main.debuff[Type] = true;
        Main.buffNoSave[Type] = true;
        Main.buffNoTimeDisplay[Type] = false;
    }

    public virtual void Initialize()
    {

    }

    public virtual void Cleanup()
    {

    }

    public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
    {
        rare = -13;
    }

    protected bool IsOneTimeApplyTick(int buffTime) => buffTime == ONE_TIME_APPLY_TICK;
}
