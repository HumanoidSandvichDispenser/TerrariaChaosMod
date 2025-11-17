using System.Collections.Generic;
using Terraria;

namespace TerrariaChaosMod.Content.Effects.ItemEffects;

public sealed class InsertionSortInventoryEffect : SortInventoryEffect
{
    protected override IEnumerable<SortIteration> SortRoutine(Player player)
    {
        Item[] items = player.inventory;

        for (int i = 1; i < TOTAL_ITEMS; i++)
        {
            _sortedItems = i;

            int j = i - 1;
            bool keyInserted = false;

            while (j >= 0 && !CompareItem(items[j], items[j + 1]))
            {
                Item temp = items[j];
                items[j] = items[j + 1];
                items[j + 1] = temp;
                keyInserted = true;

                yield return new(j, j + 1);
                j--;
            }

            // if no swaps were made, still yield
            if (!keyInserted)
            {
                yield return new(i, i);
            }
        }
    }
}
