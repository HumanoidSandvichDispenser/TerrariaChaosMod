using Terraria.ID;

namespace TerrariaChaosMod.Content.Effects.SpawnEffects;

public class SpawnQueenSlimeEffect : BaseSpawnEffect
{
    public override double Weight => HardmodeWeight(base.Weight / 8, base.Weight);

    public SpawnQueenSlimeEffect() : base(NPCID.QueenSlimeBoss)
    {

    }
}
