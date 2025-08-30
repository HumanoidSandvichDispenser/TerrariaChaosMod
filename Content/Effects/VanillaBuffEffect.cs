using Terraria;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content.Effects;

public abstract class VanillaBuffEffect : Effect, IBuffEffect
{
    private int _buffId;

    public virtual int BuffId => _buffId;

    public VanillaBuffEffect(int buffId)
    {
        _buffId = buffId;
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
