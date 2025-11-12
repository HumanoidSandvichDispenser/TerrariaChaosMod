using Terraria;

namespace TerrariaChaosMod.Content.Effects.SpawnEffects;

public class IncreasedSpawnRateEffect : Effect
{
    public virtual int SpawnRateMultiplier => 32;

    public virtual int MaxSpawnsMultiplier => 10;

    public override void PostUpdate(Player player)
    {
        var effectPlayer = player.GetModPlayer<Effects.PlayerEffects.EffectPlayer>();
        effectPlayer.SpawnRateMultiplier = SpawnRateMultiplier;
        effectPlayer.MaxSpawnsMultiplier = MaxSpawnsMultiplier;

        base.PostUpdate(player);
    }
}
