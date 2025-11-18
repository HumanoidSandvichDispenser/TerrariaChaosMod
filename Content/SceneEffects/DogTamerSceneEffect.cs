using Terraria;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content.SceneEffects;

public class DogTamerSceneEffect : ModSceneEffect
{
    public override bool IsSceneEffectActive(Player player)
    {
        // if a Dog Tamer NPC is present and the player is nearby, activate
        // the scene effect
        for (int i = 0; i < Main.maxNPCs; i++)
        {
            NPC npc = Main.npc[i];
            if (npc.active && npc.ModNPC is Content.NPCs.DogTamerNPC)
            {
                return npc.Distance(player.Center) < 1000f;
            }
        }
        return false;
    }

    public override int Music => TerrariaChaosMod.ForsenTheme;

    public override SceneEffectPriority Priority => SceneEffectPriority.BossMedium;
}
