using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace TerrariaChaosMod.Content.Effects.ItemEffects;

public abstract class SortInventoryEffect : Effect
{
    protected record struct SortIteration(int CurrentIndex, int SwappedIndex);

    protected int _sortedItems = 0;

    protected const int TOTAL_ITEMS = 50;

    protected IEnumerator<SortIteration> _enumerator = null;

    protected IEnumerator<SortIteration> _checker = null;

    public override string StatusText => $"{DisplayName} ({_sortedItems}/{TOTAL_ITEMS})";

    public sealed override int Duration => -1;

    public override bool ShouldApplyNow(Player player)
    {
        // don't use AcquireLock because all SortInventoryEffects must share
        // the same lock to prevent multiple sorts at once
        bool @lock = EffectLock.Of<SortInventoryEffect>().Acquire();
        return @lock;
    }

    public override void ApplyEffect(Player player)
    {
        _enumerator = SortRoutine(player).GetEnumerator();
        base.ApplyEffect(player);
    }

    public override void CleanUp(Player player)
    {
        EffectLock.Of<SortInventoryEffect>().Release();
        base.CleanUp(player);
    }

    public override bool Update(Player player)
    {
        if (_enumerator is null && _checker is null)
        {
            return true;
        }

        base.Update(player);
        return false;
    }

    public sealed override void PostUpdate(Player player)
    {
        if (_checker is not null)
        {
            if (_checker.MoveNext())
            {
                var iteration = _checker.Current;
                player.selectedItem = iteration.CurrentIndex;
                float key = GetItemKey(player.inventory[iteration.SwappedIndex]);
                PlaySound(key);
            }
            else
            {
                _checker = null;
            }
        }

        if (_enumerator is not null)
        {
            // every tick, perform one pass of bubble sort on the player's
            // inventory
            if (_enumerator.MoveNext())
            {
                // play sound effect for each comparison
                var iteration = _enumerator.Current;
                player.selectedItem = iteration.CurrentIndex;
                float key = GetItemKey(player.inventory[iteration.SwappedIndex]);
                PlaySound(key);
            }
            else
            {
                _sortedItems = TOTAL_ITEMS;
                _enumerator = null;
                _checker = Check(player).GetEnumerator();
            }
        }

        base.PostUpdate(player);
    }

    protected float GetItemKey(Item item)
    {
        string name = item?.AffixName() ?? string.Empty;

        // Empty strings map to 1
        if (string.IsNullOrEmpty(name))
        {
            return 1f;
        }

        // Convert each character to a value 0–35: 0-9 digits, 10-35 letters
        float key = 0f;
        float baseValue = 36f;
        float factor = 1f / baseValue;

        for (int i = 0; i < name.Length; i++)
        {
            char c = name[i];
            int val = 0;

            if (c >= '0' && c <= '9') val = c - '0';
            else if (c >= 'A' && c <= 'Z') val = c - 'A' + 10;
            else if (c >= 'a' && c <= 'z') val = c - 'a' + 10;
            else val = 0; // treat other chars as 0

            key += val * factor;
            factor /= baseValue; // next character contributes less
        }

        return System.Math.Clamp(key, 0, 1);
    }

    protected void PlaySound(float value)
    {
        float pitchOffset = value * 6 - 4;
        var sound = TerrariaChaosMod.SquareWave.WithPitchOffset(pitchOffset);
        SoundEngine.PlaySound(sound);
    }

    protected abstract IEnumerable<SortIteration> SortRoutine(Player player);

    private IEnumerable<SortIteration> Check(Player player)
    {
        for (int i = 0; i < TOTAL_ITEMS - 1; i++)
        {
            if (!CompareItem(player.inventory[i], player.inventory[i + 1]))
            {
                // something is out of order, restart the effect
                _enumerator = SortRoutine(player).GetEnumerator();

                string item1 = player.inventory[i]?.AffixName() ?? string.Empty;
                string item2 = player.inventory[i + 1]?.AffixName() ?? string.Empty;

                StringBuilder sb = new();
                sb.Append("Assertion failed: ");
                sb.AppendFormat("\"{0}\" < \"{1}\" returned false.", item1, item2);
                sb.AppendLine();
                sb.Append("FATAL ERROR: ");
                sb.AppendFormat("Sorting interrupted in {0}. Restarting...", DisplayName);
                Terraria.Main.NewText(sb.ToString(), Microsoft.Xna.Framework.Color.Red);
                yield break;
            }

            yield return new SortIteration(i, i);
        }
    }

    /// <summary>
    /// Compares two items by their affix names, treating null or empty affix names
    /// as greater than any non-empty affix name.
    /// </summary>
    /// <returns>
    /// <c>true</c> if <paramref name="item1"/> should come before <paramref name="item2"/>
    /// </returns>
    protected bool CompareItem(Item item1, Item item2)
    {
        string left = item1?.AffixName() ?? string.Empty;
        string right = item2?.AffixName() ?? string.Empty;

        if (string.IsNullOrEmpty(left) && string.IsNullOrEmpty(right))
            return true;
        if (string.IsNullOrEmpty(left))
            return false;
        if (string.IsNullOrEmpty(right))
            return true;
        return string.Compare(left, right) <= 0;
    }
}
