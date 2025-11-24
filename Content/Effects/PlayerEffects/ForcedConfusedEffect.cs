using Terraria;
using Terraria.ID;

namespace TerrariaChaosMod.Content.Effects.PlayerEffects;

public class ForcedConfusedEffect : Effect
{
    public override bool Update(Player player)
    {
        player.AddBuff(BuffID.Confused, Duration);
        return base.Update(player);
    }
}
