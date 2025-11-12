using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace TerrariaChaosMod.Content.Effects.NPCEffects;

public class RegenerateBossHealthEffect : Effect
{
    public override bool ShouldIncludeInPool(ICollection<Effect> pool)
    {
        return Main.CurrentFrameFlags.AnyActiveBossNPC;
    }

    public override void ApplyEffect(Player player)
    {
        foreach (NPC npc in Main.npc)
        {
            // regenerate boss health to maximum

            if (npc.active && npc.boss)
            {
                npc.life = npc.lifeMax;
                npc.HealEffect(npc.lifeMax);
                npc.netUpdate = true;
            }
        }
    }
}
