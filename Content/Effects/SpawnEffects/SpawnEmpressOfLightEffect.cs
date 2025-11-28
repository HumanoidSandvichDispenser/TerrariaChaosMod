using Terraria.ID;

namespace TerrariaChaosMod.Content.Effects.SpawnEffects;

public class SpawnEmpressOfLightEffect : BaseSpawnEffect
{
    public override double Weight => HardmodeWeight(base.Weight / 8, base.Weight);

    public SpawnEmpressOfLightEffect() : base(NPCID.HallowBoss)
    {

    }
}
