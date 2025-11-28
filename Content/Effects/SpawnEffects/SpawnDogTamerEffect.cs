using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content.Effects.SpawnEffects;

public class SpawnDogTamerEffect : BaseSpawnEffect
{
    private static Func<int> getId = () => ModContent.NPCType<Content.NPCs.DogTamerNPC>();

    public override double Weight
    {
        get
        {
            double factor = 0.0;

            // increase weight if skeletron has been defeated
            if (Terraria.NPC.downedBoss3)
            {
                factor += 0.25;
            }

            // increase weight if hardmode
            if (Terraria.Main.hardMode)
            {
                factor += 0.5;
            }

            // increase weight if plantera has been defeated
            if (Terraria.NPC.downedPlantBoss)
            {
                factor += 0.25;
            }

            // if expert/master mode, double the factor
            if (Terraria.Main.expertMode)
            {
                factor *= 2.0;
            }

            return factor * base.Weight;
        }
    }

    public SpawnDogTamerEffect() : base(getId)
    {

    }
}
