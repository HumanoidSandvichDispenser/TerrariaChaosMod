using Terraria;

namespace TerrariaChaosMod.Content.Effects.ItemEffects;

public abstract class BaseGiveItemEffect : Effect
{
    public sealed override EffectSide Side => EffectSide.Client;

    public abstract int ItemType { get; }

    public abstract int ItemStack { get; }

    public override void ApplyEffect(Player player)
    {
        player.QuickSpawnItem(null, ItemType, ItemStack);
        base.ApplyEffect(player);
    }
}
