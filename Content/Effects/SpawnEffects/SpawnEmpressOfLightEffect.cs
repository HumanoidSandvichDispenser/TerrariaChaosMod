using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace TerrariaChaosMod.Content.Effects.SpawnEffects;

public class SpawnEmpressOfLightEffect : Effect
{
    public override void ApplyEffect(Player player)
    {
        Vector2 spawnPosition = player.Center + new Vector2(0, -512);

        var npcIndex = NPC.NewNPC(null, (int)spawnPosition.X, (int)spawnPosition.Y, NPCID.HallowBoss, 0, 0f, 0f, 0f, 0f, 255);
    }
}
