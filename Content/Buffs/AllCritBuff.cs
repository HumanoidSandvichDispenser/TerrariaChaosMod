using Terraria.ModLoader;

namespace TerrariaChaosMod.Content.Buffs;

public class AllCritBuff : BaseChaosBuff
{
    public override void Update(Terraria.Player player, ref int buffIndex)
    {
        player.GetCritChance(DamageClass.Magic) = 100;
        player.GetCritChance(DamageClass.Melee) = 100;
        player.GetCritChance(DamageClass.MeleeNoSpeed) = 100;
        player.GetCritChance(DamageClass.Ranged) = 100;
        player.GetCritChance(DamageClass.Summon) = 100;
        player.GetCritChance(DamageClass.SummonMeleeSpeed) = 100;
        player.GetCritChance(DamageClass.Default) = 100;
        player.GetCritChance(DamageClass.Generic) = 100;
        player.GetCritChance(DamageClass.Throwing) = 100;
    }
}
