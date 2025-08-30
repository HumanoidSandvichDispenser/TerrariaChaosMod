using Terraria;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content.Effects;

public abstract class BuffEffect<T> : Effect, IBuffEffect where T : ModBuff
{
    private int _buffId;

    public virtual int BuffId => _buffId;

    public BuffEffect()
    {
        _buffId = ModContent.BuffType<T>();
    }

    public override void PostUpdate(Player player)
    {
        int buffIndex = player.FindBuffIndex(_buffId);
        if (buffIndex < 0)
        {
            player.AddBuff(_buffId, TimeLeft);
        }
        else
        {
            player.buffTime[buffIndex] = TimeLeft;
        }

        base.PostUpdate(player);
    }
}
