using Terraria;
using Terraria.ID;

namespace TerrariaChaosMod.Content.Buffs;

public class SpaceProgramBuff : BaseChaosBuff
{
    public override void Update(Player player, ref int buffIndex)
    {
        int minecartId = MountID.MeowmereMinecart;
        player.mount.SetMount(minecartId, player);

        if (player.buffTime[buffIndex] != BaseChaosBuff.ONE_TIME_APPLY_TICK)
        {
            return;
        }

        player.velocity.Y = -Player.jumpSpeed;
        player.velocity.X = player.direction * 128f;
        player.jump = Player.jumpHeight;

        player.velocity *= 128f;
    }
}
