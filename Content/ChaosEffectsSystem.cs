using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;
using TerrariaChaosMod.Content.Effects;
using ItemEffects = TerrariaChaosMod.Content.Effects.ItemEffects;
using PlayerEffects = TerrariaChaosMod.Content.Effects.PlayerEffects;
using VisualEffects = TerrariaChaosMod.Content.Effects.VisualEffects;

namespace TerrariaChaosMod.Content;

public partial class ChaosEffectsSystem : ModSystem
{
    private int _tickCounter = 25 * 60;

    private int _votingDuration = 30 * 60;

    private HashSet<Effect> _effectPool;

    private Dictionary<string, Effect> _effectDictionary = new();

    public IReadOnlySet<Effect> EffectPool => _effectPool;

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

    public override void OnModLoad()
    {
        _effectPool = new HashSet<Effects.Effect>()
        {
            ModContent.GetInstance<ItemEffects.ReforgeEffect>(),
            ModContent.GetInstance<ItemEffects.LargeItemsEffect>(),
            ModContent.GetInstance<ItemEffects.EatDaPooPooEffect>(),
            ModContent.GetInstance<ItemEffects.SwitchToRandomLoadoutEffect>(),
            ModContent.GetInstance<ItemEffects.RagsToRichesEffect>(),
            ModContent.GetInstance<ItemEffects.RichesToRagsEffect>(),

            ModContent.GetInstance<PlayerEffects.AllCritsEffect>(),
            ModContent.GetInstance<PlayerEffects.PowerPlayEffect>(),
            ModContent.GetInstance<PlayerEffects.KillPlayerEffect>(),
            ModContent.GetInstance<PlayerEffects.BoostBrakingEffect>(),
            ModContent.GetInstance<PlayerEffects.SpaceProgramEffect>(),
            ModContent.GetInstance<PlayerEffects.SidewaysGravityEffect>(),
            ModContent.GetInstance<PlayerEffects.ExtremeSniperScopeEffect>(),
            ModContent.GetInstance<PlayerEffects.MovementSpeed5xEffect>(),
            ModContent.GetInstance<PlayerEffects.SlowAccelerationEffect>(),
            ModContent.GetInstance<PlayerEffects.IgnitePlayerEffect>(),
            //ModContent.GetInstance<PlayerEffects.AFKEffect>(),
            ModContent.GetInstance<PlayerEffects.TemporaryMediumcoreEffect>(),
            ModContent.GetInstance<PlayerEffects.NoIFramesEffect>(),

            ModContent.GetInstance<VisualEffects.FakeCrashEffect>(),
            ModContent.GetInstance<VisualEffects.ConstantMapEffect>(),
            ModContent.GetInstance<VisualEffects.HelenKellerEffect>(),
            //ModContent.GetInstance<VisualEffects.RollCreditsEffect>(),
            ModContent.GetInstance<VisualEffects.FakeLagEffect>(),
            ModContent.GetInstance<VisualEffects.InvertColorsEffect>(),
            ModContent.GetInstance<VisualEffects.RainbowEffect>(),
            ModContent.GetInstance<VisualEffects.LHeroineEffect>(),
        };

        // ensure no elements are null
        _effectPool = new HashSet<Effect>(_effectPool.Where(e => e != null));

        foreach (var effect in _effectPool)
        {
            _effectDictionary[effect.Name] = effect;
        }

        RandomEffectProvider.ReinitializePool(_effectPool);
        TwitchVoteEffectProvider.ReinitializePool(_effectPool);
    }

    public override void Unload()
    {
        TwitchVoteEffectProvider?.Disconnect();
    }

    public override void OnWorldLoad()
    {
        _tickCounter = 25 * 60;
    }

    public override void OnWorldUnload()
    {
        TwitchVoteEffectProvider.Disconnect();
    }

    public override void PreUpdateWorld()
    {
        if (!IsEnabled || !CurrentEffectProvider.IsReady)
        {
            return;
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

        _tickCounter++;
    }

    public void ApplyEffect(Effect effect)
    {
        EffectsToApply.Enqueue(effect);
    }
}
