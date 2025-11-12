using Terraria;
using Terraria.ID;

namespace TerrariaChaosMod.Content.Effects.NPCEffects;

public class TeleportAllNpcsEffect : Effect
{
    public override void ApplyEffect(Player player)
    {
        // Teleport all NPCs to the player's position
        foreach (var npc in Main.npc)
        {
            if (npc.active && npc.type != NPCID.TargetDummy)
            {
                npc.position = player.Center;
            }
        }
    }
}
