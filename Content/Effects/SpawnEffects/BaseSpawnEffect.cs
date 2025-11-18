using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace TerrariaChaosMod.Content.Effects.SpawnEffects;

public class BaseSpawnEffect : Effect
{
    protected int _npcIndex = -1;
    private Func<int> _npcIdFunc;

    public override EffectSide Side => EffectSide.Server;

    public BaseSpawnEffect(int npcId)
    {
        _npcIdFunc = () => npcId;
    }

    public BaseSpawnEffect(Func<int> npcIdFunc)
    {
        _npcIdFunc = npcIdFunc;
    }

    public override void ApplyEffect(Player player)
    {
        _npcIndex = SpawnNPCAtPlayer(_npcIdFunc(), player);
        base.ApplyEffect(player);
    }

    public int SpawnNPCAtPlayer(int npcId, Player player)
    {
        Vector2 spawnPosition = player.Center + new Vector2(0, -64);
        int x = (int)spawnPosition.X;
        int y = (int)spawnPosition.Y;

        return NPC.NewNPC(null, x, y, npcId, 0, 0f, 0f, 0f, 0f, 255);
    }
}
