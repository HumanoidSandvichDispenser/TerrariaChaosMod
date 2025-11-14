using Terraria;
using Terraria.ID;

namespace TerrariaChaosMod.Content.Effects.PlayerEffects;

public class ManaForHealthEffect : Effect
{
    public override void PostUpdate(Player player)
    {
        // if we are below 20% max mana, convert 20% of max health to mana

        if (player.statMana < player.statManaMax2 * 0.2f)
        {
            int healthToConvert = (int)(player.statLifeMax2 * 0.2f);
            if (player.statLife > healthToConvert)
            {
                int manaToGain = (int)(player.statManaMax2 * 0.2f);
                player.statLife -= healthToConvert;
                player.statMana += manaToGain;
                if (player.statMana > player.statManaMax2)
                {
                    manaToGain -= player.statMana - player.statManaMax2;
                    player.statMana = player.statManaMax2;
                }
                player.ManaEffect(manaToGain);

                // network sync
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    NetMessage.SendData(MessageID.PlayerLifeMana, -1, -1, null, player.whoAmI);
                }
            }
        }

        base.PostUpdate(player);
    }
}
