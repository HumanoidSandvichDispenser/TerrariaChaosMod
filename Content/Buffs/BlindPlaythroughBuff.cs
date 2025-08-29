using Terraria;

namespace TerrariaChaosMod.Content.Buffs;

public class BlindPlaythroughBuff : BaseChaosBuff
{
    public override void Update(Terraria.Player player, ref int buffIndex)
    {
        Main.BlackFadeIn = 255;
    }
}
