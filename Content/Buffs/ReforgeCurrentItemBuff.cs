using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.UI;

namespace TerrariaChaosMod.Content.Buffs;

public class ReforgeCurrentItemBuff : BaseChaosBuff
{
    public override void Update(Player player, ref int buffIndex)
    {
        if (player.buffTime[buffIndex] != BaseChaosBuff.ONE_TIME_APPLY_TICK)
        {
            return;
        }

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
    }
}
