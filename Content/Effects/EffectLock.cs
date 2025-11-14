using System.Collections.Generic;

namespace TerrariaChaosMod.Content.Effects;

public sealed class EffectLock
{
    private static Dictionary<System.Type, EffectLock> _semaphores = new();

    public static EffectLock Of(System.Type type)
    {
        if (!_semaphores.TryGetValue(type, out var sem))
        {
            sem = new EffectLock();
            _semaphores[type] = sem;
        }
        return _semaphores[type];
    }

    public static EffectLock Of<TEffect>()
    {
        var type = typeof(TEffect);
        return Of(type);
    }

    public int Value { get; private set; }

    public bool IsAcquired => Value > 0;

    /// <summary>
    /// Attempts to acquire the lock.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the lock was successfully acquired; otherwise, <c>false</c>.
    /// </returns>
    public bool Acquire(bool force = false)
    {
        if (!IsAcquired || force)
        {
            Value++;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Releases the lock.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the lock is released more times than it was acquired.
    /// </exception>
    public void Release()
    {
        if (--Value < 0)
        {
            string msg = "Semaphore released more times than it was acquired.";
            throw new System.InvalidOperationException(msg);
        }
    }
}
