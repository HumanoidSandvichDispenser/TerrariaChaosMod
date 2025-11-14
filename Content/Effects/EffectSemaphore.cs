namespace TerrariaChaosMod.Content.Effects;

public sealed class EffectSemaphore : Effect
{
    public int Value { get; private set; }

    public bool IsAcquired => Value > 0;

    /// <summary>
    /// Attempts to acquire the semaphore.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the semaphore was successfully acquired; otherwise, <c>false</c>.
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
    /// Releases the semaphore.
    /// </summary>
    public void Release()
    {
        if (Value > 0)
        {
            Value--;
        }
    }
}
