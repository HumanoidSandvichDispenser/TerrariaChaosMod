using Terraria;
using Terraria.ID;

namespace TerrariaChaosMod.Content.Effects.PlayerEffects;

public class SpaceProgramEffect : Effect
{
    public override void ApplyEffect(Player player)
    {
        player.velocity.Y = -Player.jumpSpeed;
        player.velocity.X = player.direction * 4f;
        player.jump = Player.jumpHeight;

        player.velocity *= 8f;
    }

    public override bool Update(Player player)
    {
        if (player.velocity.Y < 0)
        {
            player.AddBuff(BuffID.Shimmer, TimeLeft);
        }
        else
        {
            player.mount.SetMount(MountID.MeowmereMinecart, player);
        }

        return base.Update(player);
    }
}
