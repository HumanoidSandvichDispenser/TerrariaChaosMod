using Terraria;

namespace TerrariaChaosMod.Content.Effects.ItemEffects;

public class ShuffleInventoryEffect : Effect
{
    public override EffectSide Side => EffectSide.Client;

    public override void ApplyEffect(Player player)
    {
        Item[] items = player.inventory;
        const int n = 50;
        for (int i = 0; i < n; i++)
        {
            int j = Main.rand.Next(i, n);
            Item temp = items[i];
            items[i] = items[j];
            items[j] = temp;
        }
        base.ApplyEffect(player);
    }
}
