using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content.Effects.SpawnEffects;

public class SpawnDogTamerEffect : BaseSpawnEffect
{
    private static Func<int> getId = () => ModContent.NPCType<Content.NPCs.DogTamerNPC>();

    public override bool ShouldIncludeInPool(ICollection<Effect> pool)
    {
        // only enable if skeletron has been defeated or hardmode
        return Terraria.NPC.downedBoss3 || Terraria.Main.hardMode;
    }

    public SpawnDogTamerEffect() : base(getId)
    {

    }
}
