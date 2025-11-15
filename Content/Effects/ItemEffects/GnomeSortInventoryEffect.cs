using System.Collections.Generic;
using Terraria;

namespace TerrariaChaosMod.Content.Effects.ItemEffects;

public sealed class GnomeSortInventoryEffect : SortInventoryEffect
{
    protected override IEnumerable<SortIteration> SortRoutine(Player player)
    {
        Item[] items = player.inventory;

        int i = 0;
        while (i < TOTAL_ITEMS)
        {
            if (i == 0 || CompareItem(items[i - 1], items[i]))
            {
                yield return new(i, i);
                i++;
            }
            else
            {
                yield return new(i, i - 1);
                Item temp = items[i - 1];
                items[i - 1] = items[i];
                items[i] = temp;
                i--;
            }
        }
    }
}
