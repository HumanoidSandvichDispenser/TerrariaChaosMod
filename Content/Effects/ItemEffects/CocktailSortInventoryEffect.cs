using System.Collections.Generic;
using Terraria;

namespace TerrariaChaosMod.Content.Effects.ItemEffects;

public sealed class CocktailSortInventoryEffect : SortInventoryEffect
{
    protected override IEnumerable<SortIteration> SortRoutine(Player player)
    {
        Item[] items = player.inventory;

        bool swapped = false;

        int start = 0;
        int end = TOTAL_ITEMS - 1;

        while (start < end)
        {
            swapped = false;

            // forward pass
            for (int i = start; i < end; i++)
            {
                if (!CompareItem(items[i], items[i + 1]))
                {
                    Item temp = items[i];
                    items[i] = items[i + 1];
                    items[i + 1] = temp;
                    swapped = true;
                    yield return new(i + 1, i);
                }
                else
                {
                    yield return new(i, i + 1);
                }
            }

            if (!swapped)
            {
                yield break;
            }

            _sortedItems = end--;

            swapped = false;

            for (int i = end - 1; i >= start; i--)
            {
                if (!CompareItem(items[i], items[i + 1]))
                {
                    Item temp = items[i];
                    items[i] = items[i + 1];
                    items[i + 1] = temp;
                    swapped = true;
                    yield return new(i, i + 1);
                }
                else
                {
                    yield return new(i + 1, i);
                }
            }

            if (!swapped)
            {
                yield break;
            }

            start++;

            _sortedItems = TOTAL_ITEMS - start;
        }
    }
}
