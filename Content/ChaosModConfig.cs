using System.Collections.Generic;
using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using TerrariaChaosMod.Content.Achievements;

namespace TerrariaChaosMod.Content;

public class ChaosModConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ServerSide;

    [DefaultValue(false)]
    public bool EnableTwitchVoting;

    [DefaultValue("")]
    public string TwitchChannelName;

    [DefaultValue(true)]
    public bool EnableProbabilisticVoting;

    [DefaultListValue("")]
    public List<string> DisabledEffects = new List<string>();

    public bool AnnounceNewVoteInChat = true;

    /// <summary>
    /// If true, one vote in chat will count as multiple votes for testing purposes.
    /// In addition, votes for other effects will be randomly generated periodically.
    /// </summary>
    public bool SimulatedVotesForTesting = false;

    public override void OnChanged()
    {
        base.OnChanged();
        var chaosSystem = ModContent.GetInstance<ChaosEffectsSystem>();
        chaosSystem?.LoadMasterPool();

        var achieve = ModContent.GetInstance<MaldingIntensifiesAchievement>();
        if (achieve is not null)
        {
            achieve.DisabledEffectsCondition.Value = DisabledEffects.Count;
        }
    }
}
