using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content.Buffs;

public class EatDaPooPooBuff : BaseChaosBuff
{
    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();
        Main.buffNoTimeDisplay[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        if (player.buffTime[buffIndex] != BaseChaosBuff.ONE_TIME_APPLY_TICK)
        {
            return;
        }

        // if inventory has any free slots, add poo item to it
        const int LAST_INVENTORY_SLOT = 49;

        for (int i = 0; i <= LAST_INVENTORY_SLOT; i++)
        {
            if (player.inventory[i].type == ItemID.None ||
                player.inventory[i] is null)
            {
                int poopBlock = ItemID.PoopBlock;
                player.inventory[i] = new Item(poopBlock);
            }
        }
    }
}
