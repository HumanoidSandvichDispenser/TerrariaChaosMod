using Terraria.ID;

namespace TerrariaChaosMod.Content.Effects.PlayerEffects;

public class PowerPlayEffect : Effect
{
    public override bool Update(Terraria.Player player)
    {
        if (TimeLeft % 15 == 0)
        {
            player.Heal(1);
        }

        player.AddBuff(BuffID.WellFed3, TimeLeft);
        player.AddBuff(BuffID.Regeneration, TimeLeft);
        player.AddBuff(BuffID.ManaRegeneration, TimeLeft);
        player.AddBuff(BuffID.Ironskin, TimeLeft);
        player.AddBuff(BuffID.Wrath, TimeLeft);
        player.AddBuff(BuffID.Rage, TimeLeft);
        player.AddBuff(BuffID.Endurance, TimeLeft);
        player.AddBuff(BuffID.Swiftness, TimeLeft);
        player.AddBuff(BuffID.Thorns, TimeLeft);

        return base.Update(player);
    }
}
