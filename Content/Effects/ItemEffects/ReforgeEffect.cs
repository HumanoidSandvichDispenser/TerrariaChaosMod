using Terraria;

namespace TerrariaChaosMod.Content.Effects.ItemEffects;

public class ReforgeEffect : Effect
{
    public override void ApplyEffect(Player player)
    {
        // reforge current item if applicable, otherwise pick best item
        Item item = player.HeldItem;
        if (item is null || item.IsAir)
        {
            item = player.GetBestPickaxe();
        }

        // play sound
        if (item is not null && !item.IsAir)
        {
            Terraria.Audio.SoundEngine.PlaySound(Terraria.ID.SoundID.Item37, player.position);
            item.Prefix(-1);

            PopupText.NewText(
                PopupTextContext.ItemReforge,
                item,
                item.stack,
                noStack: true);
        }

        base.ApplyEffect(player);
    }
}
