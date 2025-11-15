using System.Collections.Generic;
using Terraria;

namespace TerrariaChaosMod.Content.Effects.ItemEffects;

public sealed class BubbleSortInventoryEffect : SortInventoryEffect
{
    protected override IEnumerable<SortIteration> SortRoutine(Player player)
    {
        Item[] items = player.inventory;

        int swaps = 0;

        for (int i = 0; i < TOTAL_ITEMS - 1; i++)
        {
            _sortedItems = i;
            for (int j = 0; j < TOTAL_ITEMS - i - 1; j++)
            {
                if (!CompareItem(items[j], items[j + 1]))
                {
                    Item temp = items[j];
                    items[j] = items[j + 1];
                    items[j + 1] = temp;
                    swaps++;

                    yield return new(j + 1, j);
                }
                else
                {
                    yield return new(j, j);
                }
            }

            if (swaps == 0)
            {
                // array is sorted
                break;
            }
        }
    }
}
