using Terraria;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content.Effects.NPCEffects;

public class KillOneDogTamerEffect : Effect
{
    public override EffectSide Side => EffectSide.Server;

    public override void ApplyEffect(Player player)
    {
        for (int i = 0; i < Main.maxNPCs; i++)
        {
            NPC npc = Main.npc[i];
            if (npc.active && npc.type == ModContent.NPCType<NPCs.DogTamerNPC>())
            {
                npc.StrikeInstantKill();
                npc.netUpdate = true;
                break;
            }
        }
    }
}
