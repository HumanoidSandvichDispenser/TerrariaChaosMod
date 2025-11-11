using Terraria;

namespace TerrariaChaosMod.Content.Effects.PlayerEffects;

public class ButteryShoesEffect : Effect
{
    public override bool Update(Player player)
    {
        player.runSlowdown *= 0.1f;

        return base.Update(player);
    }
}
