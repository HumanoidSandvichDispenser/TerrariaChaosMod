using Terraria;
using Terraria.ID;

namespace TerrariaChaosMod.Content.Buffs;

public class NoMountBuff : BaseChaosBuff
{
    public override void Update(Player player, ref int buffIndex)
    {
        player.mount.SetMount(MountID.None, player);
    }
}
