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

    /// <summary>
    /// Current time left of the effect in ticks.
    /// </summary>
    public int TimeLeft { get; set; }

    public int CurrentTick { get; set; }

    private Dictionary<int, Action> _scheduledActions = new();

    public virtual bool Conditions()
    {
        return true;
    }

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
        //_scheduledActions[tick] = action;
        _scheduledActions.Add(tick, action);
        Main.NewText($"Scheduled action at tick {tick} for effect {Name}", 255, 255, 0);
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
}
