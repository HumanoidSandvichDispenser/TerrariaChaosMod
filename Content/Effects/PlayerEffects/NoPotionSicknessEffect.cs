using Terraria;
using Terraria.ID;

namespace TerrariaChaosMod.Content.Effects.PlayerEffects;

public class NoPotionSicknessEffect : Effect
{
    public override void PostUpdate(Player player)
    {
        int buffIndex = player.FindBuffIndex(BuffID.PotionSickness);
        if (buffIndex >= 0)
        {
            player.buffTime[buffIndex] = player.potionDelay = 0;
        }
        base.PostUpdate(player);
    }
}
