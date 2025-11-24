using Terraria;

namespace TerrariaChaosMod.Content.Effects.PlayerEffects;

public class InvertCurrentVelocityEffect : Effect
{
    public override void ApplyEffect(Player player)
    {
        player.velocity = -player.velocity;
        base.ApplyEffect(player);
    }
}
