using Terraria;

namespace TerrariaChaosMod.Content.Buffs;

public abstract class BaseChaosEffect : IEffect
{
    public abstract void Apply(Player player);
}
