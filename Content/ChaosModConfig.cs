using System.Collections.Generic;
using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace TerrariaChaosMod.Content;

public class ChaosModConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ServerSide;

    [Label("Enable Twitch Voting")]
    [Tooltip("Allows viewers to vote for effects via chat messages.")]
    [DefaultValue(false)]
    public bool EnableTwitchVoting;

    [Label("Twitch Channel Name")]
    [Tooltip("The name of the Twitch channel to connect to for voting.")]
    [DefaultValue("")]
    public string TwitchChannelName;

    [Label("Enable Probabilistic Voting")]
    [Tooltip("Enables probabilistic voting where effects with more votes have a higher chance of being selected. If off, enables deterministic voting where the effect with the most votes is always selected.")]
    [DefaultValue(true)]
    public bool EnableProbabilisticVoting;

    [Label("Disabled Effects")]
    [Tooltip("Separate multiple effect names with commas. Effects listed here will not be applied during gameplay.")]
    [DefaultListValue("")]
    public List<string> DisabledEffects = new List<string>();

    [Label("Announce new vote through in-game chat")]
    [Tooltip("Sends a message in the in-game chat displaying the new vote options when a new Twitch vote starts.")]
    public bool AnnounceNewVoteInChat = true;

    public override void OnChanged()
    {
        base.OnChanged();
        var chaosSystem = ModContent.GetInstance<ChaosEffectsSystem>();
        chaosSystem?.LoadMasterPool();
    }
}
