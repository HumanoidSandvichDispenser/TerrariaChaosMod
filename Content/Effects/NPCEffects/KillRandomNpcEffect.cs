using System.Linq;
using Terraria;

namespace TerrariaChaosMod.Content.Effects.NPCEffects;

public class KillRandomNpcEffect : Effect
{
    public override EffectSide Side => EffectSide.Server;

    public override void ApplyEffect(Player player)
    {
        var npcs = Main.npc
            .Where(npc => npc.active && npc.lifeMax > 0)
            .ToList();
        int npcIndex = Main.rand.Next(npcs.Count);
        npcs[npcIndex].StrikeInstantKill();
        npcs[npcIndex].netUpdate = true;
        base.ApplyEffect(player);
    }
}
