using System.Collections.Generic;
using Terraria;

namespace TerrariaChaosMod.Content.Effects.ItemEffects;

public class BubbleSortInventoryEffect : Effect
{
    private IEnumerator<object> _enumerator = null;

    public override bool ShouldApplyNow(Player player)
    {
        return AcquireLock();
    }

    public override void ApplyEffect(Player player)
    {
        _enumerator = BubbleSortRoutine(player).GetEnumerator();
        base.ApplyEffect(player);
    }

    public override void CleanUp(Player player)
    {
        ReleaseLock();
        base.CleanUp(player);
    }

    public override bool Update(Player player)
    {
        if (_enumerator is null)
        {
            return true;
        }

        return base.Update(player);
    }

    public override void PostUpdate(Player player)
    {
        if (_enumerator is not null)
        {
            // every tick, perform one pass of bubble sort on the player's
            // inventory
            if (!_enumerator.MoveNext())
            {
                _enumerator = null;
            }
        }
        base.PostUpdate(player);
    }

    private IEnumerable<object> BubbleSortRoutine(Player player)
    {
        Item[] items = player.inventory;
        int n = items.Length;

        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                string left = items[j]?.AffixName() ?? string.Empty;
                string right = items[j + 1]?.AffixName() ?? string.Empty;
                if (string.Compare(left, right) > 0)
                {
                    Item temp = items[j];
                    items[j] = items[j + 1];
                    items[j + 1] = temp;
                }

                // set selected item to show progress
                player.selectedItem = j;

                // yield to spread the sorting over multiple ticks
                yield return null;
            }
        }
    }
}
