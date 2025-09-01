using Terraria;

namespace TerrariaChaosMod.Content.Effects.PlayerEffects;

public class SlowAccelerationEffect : Effect
{
    public override bool Update(Player player)
    {
        player.runAcceleration *= 0.2f;
        return base.Update(player);
    }
}
