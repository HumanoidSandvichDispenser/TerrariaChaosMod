using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.ModLoader;
using TerrariaChaosMod.Content.Effects;

namespace TerrariaChaosMod.Content;

public partial class ChaosEffectsSystem : ModSystem
{
    private int _tickCounter = 25 * 60;

    public float Progress => _tickCounter / (float)_votingDuration;

    private int _votingDuration = 30 * 60;

    public float DurationMultiplier { get; set; } = 1f;

    // all effects defined in the mod
    private HashSet<Effect> _allEffects;

    private HashSet<Effect> _effectPool;

    private Dictionary<string, Effect> _effectDictionary = new();

    /// <summary>
    /// All enabled effects in the mod.
    /// </summary>
    public IReadOnlySet<Effect> EffectPool => _effectPool;

    public IReadOnlySet<Effect> DisabledEffects =>  _allEffects
            .Where(eff => eff is not null && !_effectPool.Contains(eff))
            .ToHashSet();

    public IReadOnlyDictionary<string, Effect> EffectDictionary => _effectDictionary;

    public Stack<Effect> EffectHistory { get; } = new();

    public Queue<Effect> EffectsToApply { get; } = new();

    public IEffectProvider CurrentEffectProvider { get; private set; }

    public TwitchVoteEffectProvider TwitchVoteEffectProvider { get; private set; }

    public RandomEffectProvider RandomEffectProvider { get; private set; }

    public bool IsEnabled { get; set; }

    public ChaosEffectsSystem()
    {
        TwitchVoteEffectProvider = new();
        RandomEffectProvider = new();

        CurrentEffectProvider = TwitchVoteEffectProvider;
    }

    private void LoadAllEffects()
    {
        _allEffects = new HashSet<Effects.Effect>();

        var effectType = typeof(Effects.Effect);
        var assembly = effectType.Assembly;
        var effectTypes = assembly.GetTypes()
            .Where(t => t.IsSubclassOf(effectType) && !t.IsAbstract);

        foreach (var type in effectTypes)
        {
            var method = typeof(ModContent)
                .GetMethod(nameof(ModContent.GetInstance))
                .MakeGenericMethod(type);

            var defaultInstance = method.Invoke(null, null) as Effects.Effect;
            _allEffects.Add(defaultInstance);
        }
    }

    private void PopulateEffectDictionary()
    {
        _effectDictionary.Clear();
        foreach (var effect in _effectPool)
        {
            var names = new HashSet<string>();
            string displayName = effect.DisplayName
                .ToString()
                .Replace(" ", "");
            names.Add(effect.Name);
            names.Add(displayName);

            // also add names with/without "Effect" suffix
            foreach (var name in names.ToList())
            {
                const string suffix = "Effect";
                if (name.EndsWith(suffix))
                {
                    names.Add(name.Substring(0, name.Length - suffix.Length));
                }
                else
                {
                    names.Add(name + suffix);
                }
            }

            // add all lowercase variants
            foreach (var name in names.ToList())
            {
                names.Add(name.ToLowerInvariant());
            }

            // finally add all names to dictionary to point to this effect
            foreach (var name in names.ToList())
            {
                if (!_effectDictionary.ContainsKey(name))
                {
                    _effectDictionary[name] = effect;
                }
            }
        }
    }

    /// <summary>
    /// Loads the master pool of chaos effects, removing any that are disabled
    /// in config. Also initializes the effect providers with the new pool.
    /// </summary>
    public void LoadMasterPool()
    {
        _effectPool = new HashSet<Effect>(_allEffects);

        // ensure no elements are null
        _effectPool.RemoveWhere(effect => effect is null);

        // disable effects if listed in config
        var config = ModContent.GetInstance<ChaosModConfig>();
        IEnumerable<string> disabledEffects = config.DisabledEffects
            .Select(s => s.Trim())
            .Where(s => !string.IsNullOrEmpty(s));

        // remove disabled effects from pool
        int removedCount = _effectPool
            .RemoveWhere(eff => disabledEffects.Any(d => eff.Matches(d)));

        PopulateEffectDictionary();

        RandomEffectProvider.ReinitializePool(_effectPool);
        TwitchVoteEffectProvider.ReinitializePool(_effectPool);

        DisplayLoadStatus();
    }

    private void DisplayLoadStatus()
    {
        var disabledEffects = DisabledEffects
            .Select(eff => eff.DisplayName.ToString());
        StringBuilder sb = new();
        sb.Append("Loaded effect pool ");
        sb.AppendFormat("with {0} effects enabled, ", _effectPool.Count);
        sb.AppendFormat("{0} effects disabled.", disabledEffects.Count());
        TerrariaChaosMod.ChatLogger.Info(sb.ToString());
        sb.Clear();

        sb.Append("List of disabled effects:\n");
        sb.AppendJoin(", ", disabledEffects);
        TerrariaChaosMod.ChatLogger.Info(sb.ToString());
    }

    public override void OnModLoad()
    {
        LoadAllEffects();
        LoadMasterPool();
    }

    public override void Unload()
    {
        TwitchVoteEffectProvider?.DisconnectTwitch();
        _ = TwitchVoteEffectProvider?.StopWebSocket();
    }

    public override void OnWorldLoad()
    {
        ModContent.GetInstance<Achievements.MaldingIntensifiesAchievement>()
            .DisabledEffectsCondition.Value = DisabledEffects.Count;
    }

    public void Startup()
    {
        DisplayLoadStatus();
        _tickCounter = 0;

        var config = ModContent.GetInstance<ChaosModConfig>();
        string channelName = config.TwitchChannelName.Trim();
        bool enableVoting = config.EnableTwitchVoting;

        if (enableVoting)
        {
            TerrariaChaosMod.ChatLogger.Info("Starting Twitch voting system...");
            if (!string.IsNullOrEmpty(channelName))
            {
                TwitchVoteEffectProvider.ConnectTwitch(channelName);
                TwitchVoteEffectProvider.StartWebSocket();
            }
            else
            {
                TerrariaChaosMod.ChatLogger.Warn("Twitch channel name is not set in config. Use /connect to connect to a channel and /ws to start the WebSocket server.");
            }
        }

    }

    public override void OnWorldUnload()
    {
        if (!Terraria.Main.dedServ)
        {
            Terraria.Main.LocalPlayer.GetModPlayer<ChaosModPlayer>()
                .CleanUpAllEffects();
        }
        TwitchVoteEffectProvider.DisconnectTwitch();
        _ = TwitchVoteEffectProvider.StopWebSocket();
    }

    public override void PreUpdateWorld()
    {
        if (!IsEnabled || !CurrentEffectProvider.IsReady)
        {
            return;
        }

        if (_tickCounter % 60 == 0)
        {
            TwitchVoteEffectProvider?.BroadcastTally();
        }

        if (_tickCounter >= _votingDuration)
        {
            _tickCounter = 0;
            if (CurrentEffectProvider.CanProvide)
            {
                var effect = CurrentEffectProvider.GetEffect().Clone() as Effect;
                EffectsToApply.Enqueue(effect);
            }
            CurrentEffectProvider.ReinitializePool(_effectPool);
        }

        _tickCounter += (int)DurationMultiplier;
    }

    public void ApplyEffect(Effect effect)
    {
        EffectsToApply.Enqueue(effect);
    }
}
