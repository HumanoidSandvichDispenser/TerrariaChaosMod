using Terraria;
using Terraria.Achievements;
using Terraria.GameContent.Achievements;

namespace TerrariaChaosMod.Content.Achievements;

public class MaldingIntensifiesAchievement : ChaosModAchievement
{
    public CustomIntCondition DisabledEffectsCondition { get; private set; }

    public override void SetStaticDefaults()
    {
        Achievement.SetCategory(AchievementCategory.None);

        DisabledEffectsCondition = AddIntCondition("DisabledEffects", 5);

        base.SetStaticDefaults();
    }
}
