using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace TerrariaChaosMod.Content.Effects.NPCEffects;

public class RegenerateBossHealthEffect : Effect
{
    public override double Weight
    {
        get
        {
            // increase weight if any boss NPC is active
            if (Main.CurrentFrameFlags.AnyActiveBossNPC)
            {
                return 16.0 * base.Weight;
            }
            return base.Weight;
        }
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
