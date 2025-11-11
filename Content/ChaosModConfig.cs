using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace TerrariaChaosMod.Content;

public class ChaosModConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;

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
    public bool EnableProbabilisticVoting = true;
}
