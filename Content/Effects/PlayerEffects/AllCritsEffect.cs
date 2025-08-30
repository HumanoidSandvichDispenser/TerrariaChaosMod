using Terraria;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content.Effects.PlayerEffects;

public class AllCritsEffect : Effect
{
    public override bool Update(Player player)
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
        return base.Update(player);
    }
}
