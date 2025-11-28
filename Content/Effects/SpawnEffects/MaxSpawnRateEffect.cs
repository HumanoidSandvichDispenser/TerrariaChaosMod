using Terraria;

namespace TerrariaChaosMod.Content.Effects.SpawnEffects;

public class MaxSpawnRateEffect : Effect
{
    public override double Weight => HardmodeWeight(base.Weight / 8, base.Weight);

    public virtual int SpawnRateMultiplier => 256;

    public virtual int MaxSpawnsMultiplier => 256;

    public override void PostUpdate(Player player)
    {
        var effectPlayer = player.GetModPlayer<Effects.PlayerEffects.EffectPlayer>();
        effectPlayer.SpawnRateMultiplier = SpawnRateMultiplier;
        effectPlayer.MaxSpawnsMultiplier = MaxSpawnsMultiplier;

        base.PostUpdate(player);
    }
}
