using Terraria.ID;
using Terraria.DataStructures;

namespace TerrariaChaosMod.Content.Buffs;

public class RagsToRichesBuff : BaseChaosBuff
{
    public override void Update(Terraria.Player player, ref int buffIndex)
    {
        if (player.buffTime[buffIndex] != BaseChaosBuff.ONE_TIME_APPLY_TICK)
        {
            return;
        }

        // loop through inventory and replace all copper coins with platinum coins

        for (int i = 0; i < player.inventory.Length; i++)
        {
            if (player.inventory[i].type == ItemID.CopperCoin)
            {
                int stack = player.inventory[i].stack;
                player.inventory[i].TurnToAir();

                // spawn platinum coins
                player.QuickSpawnItem(
                    new EntitySource_Buff(player, Type, buffIndex),
                    ItemID.PlatinumCoin,
                    stack);
            }
        }
    }
}
