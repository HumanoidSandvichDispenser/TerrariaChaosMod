namespace TerrariaChaosMod.Content.Buffs;

public class NoIFramesBuff : EffectBuff
{
    public override void Update(Terraria.Player player, ref int buffIndex)
    {
        player.immune = false;
        player.immuneTime = 0;
    }
}
