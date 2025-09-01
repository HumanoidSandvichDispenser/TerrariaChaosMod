using Terraria;

namespace TerrariaChaosMod.Content.Effects.PlayerEffects;

public class MovementSpeed5xEffect : Effect
{
    public override bool Update(Player player)
    {
        player.moveSpeed *= 5f;
        player.runAcceleration *= 5f;
        player.maxRunSpeed *= 5f;
        player.accRunSpeed *= 5f;
        return base.Update(player);
    }
}
