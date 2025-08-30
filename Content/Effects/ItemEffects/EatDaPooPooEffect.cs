using Terraria;
using Terraria.ID;

namespace TerrariaChaosMod.Content.Effects.ItemEffects;

public class EatDaPooPooEffect : Effect
{
    public override void ApplyEffect(Player player)
    {
        const int LAST_INVENTORY_SLOT = 49;

        for (int i = 0; i < LAST_INVENTORY_SLOT; i++)
        {
            if (player.inventory[i].type == ItemID.None ||
                player.inventory[i] is null ||
                player.inventory[i].IsAir)
            {
                player.inventory[i] = new Item(ItemID.PoopBlock);
            }
        }

        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item16, player.position);
    }
}
