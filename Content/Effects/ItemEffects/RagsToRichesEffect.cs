using Terraria;
using Terraria.ID;
using Terraria.DataStructures;

namespace TerrariaChaosMod.Content.Effects.ItemEffects;

public class RagsToRichesEffect : Effect
{
    protected void ReplaceWith(Player player, ref Item item, int itemId)
    {
        int stack = item.stack;
        item.TurnToAir();

        player.QuickSpawnItem(
            new EntitySource_Misc(Name),
            itemId,
            stack);
    }

    protected virtual int GetNewItemId(int oldItemId)
    {
        switch (oldItemId)
        {
            case ItemID.CopperCoin:
            case ItemID.SilverCoin:
            case ItemID.GoldCoin:
                return oldItemId + 1;
            default:
                return -1;
        }
    }

    public override void ApplyEffect(Player player)
    {
        for (int i = 0; i < player.inventory.Length; i++)
        {
            int newItemId = GetNewItemId(player.inventory[i].type);
            if (newItemId != -1)
            {
                ReplaceWith(player, ref player.inventory[i], newItemId);
            }
        }

        base.ApplyEffect(player);
    }
}
