using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace TerrariaChaosMod.Content.Effects;

public abstract class Effect : ModType, ILocalizedModType, ICloneable
{
    public string LocalizationCategory => "Effects";

    public virtual LocalizedText DisplayName
    {
        get => this.GetLocalization(nameof(DisplayName), PrettyPrintName);
    }

    /// <summary>
    /// Duration of the effect in ticks.
    /// </summary>
    public virtual int Duration => Seconds(30);

    public virtual string StatusText => $"{DisplayName} ({TimeLeft / 60}s)";

    /// <summary>
    /// Current time left of the effect in ticks.
    /// </summary>
    public int TimeLeft { get; set; }

    public int CurrentTick { get; set; }

    private Dictionary<int, Action> _scheduledActions = new();

    /// <summary>
    /// Condition to check whether or not the effect can be added to the effect
    /// pool.
    /// </summary>
    public virtual bool ShouldIncludeInPool(ICollection<Effect> pool) => true;

    /// <summary>
    /// Condition to check whether or not the effect should be applied now.
    /// </summary>
    public virtual bool ShouldApplyNow(Player player) => true;

    public Effect()
    {

    }

    public override void Load()
    {
        this.GetLocalization(nameof(DisplayName), PrettyPrintName);
    }

    public void ResetTime()
    {
        TimeLeft = Duration;
    }

    /// <summary>
    /// Called when the effect is applied to the player, before updates.
    /// </summary>
    public virtual void ApplyEffect(Player player)
    {

    }

    /// <summary>
    /// Called every tick to update the effect on the player.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the effect has ended or should be removed.
    /// </returns>
    public virtual bool Update(Player player)
    {
        TimeLeft--;

        if (_scheduledActions.TryGetValue(TimeLeft, out var action))
        {
            action.Invoke();
            _scheduledActions.Remove(TimeLeft);
        }

        return TimeLeft <= 0;
    }

    public virtual void PostUpdate(Player player)
    {

    }

    public virtual void CleanUp(Player player)
    {
        _scheduledActions.Clear();
    }

    protected void After(int tick, Action action)
    {
        OnTick(TimeLeft - tick, action);
    }

    protected void OnTick(int tick, Action action)
    {
        _scheduledActions.Add(tick, action);
        string log = $"Scheduled action at tick {tick} for effect {Name}";
        TerrariaChaosMod.ChatLogger.Debug(log);
    }

    protected static int Seconds(float seconds)
    {
        return (int)(seconds * 60);
    }

    protected void OnTickProgress(float progress, Action action)
    {
        progress = Math.Clamp(progress, 0f, 1f);
        int tick = (int)(Duration * progress);
        OnTick(tick, action);
    }

    public static string GetLocalizedName(Type type)
    {
        return $"Mods.{nameof(TerrariaChaosMod)}.Effects.{type.Name}";
    }

    protected sealed override void Register()
    {
        ModTypeLookup<Effect>.Register(this);
    }

    public object Clone()
    {
        var clone = MemberwiseClone() as Effect;
        clone._scheduledActions = new();
        return clone;
    }

    public bool Matches(string name)
    {
        // match by Name or DisplayName, with or without "Effect" suffix,
        // case-insensitive

        const string suffix = "Effect";
        string displayName = DisplayName.ToString();
        string nameToCheck = name.EndsWith(suffix, StringComparison.OrdinalIgnoreCase)
            ? name.Substring(0, name.Length - suffix.Length)
            : name + suffix;
        string displayNameToCheck = displayName.EndsWith(suffix, StringComparison.OrdinalIgnoreCase)
            ? displayName.Substring(0, displayName.Length - suffix.Length)
            : displayName + suffix;

        var namesToMatch = new HashSet<string>()
        {
            Name.ToLower().Replace(" ", ""),
            nameToCheck.ToLower().Replace(" ", ""),
            DisplayName.ToString().ToLower().Replace(" ", ""),
            displayNameToCheck.ToLower().Replace(" ", "")
        };

        return namesToMatch.Contains(name.ToLower());
    }

    /// <summary>
    /// Attempts to acquire the effect lock.
    /// </summary>
    /// <param name="force">
    /// If set to <c>true</c>, forces the acquisition of the lock, which treats
    /// the lock as a semaphore. This is useful for effects that can stack.
    /// </param>
    protected bool AcquireLock(bool force = false)
    {
        return EffectLock.Of(GetType()).Acquire(force);
    }

    /// <summary>
    /// Releases the effect lock.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the lock is released more times than it was acquired.
    /// </exception>
    protected void ReleaseLock()
    {
        EffectLock.Of(GetType()).Release();
    }
}
