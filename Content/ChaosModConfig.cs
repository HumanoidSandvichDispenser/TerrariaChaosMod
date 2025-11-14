using System.Collections.Generic;
using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

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

    public override void OnChanged()
    {
        base.OnChanged();
        var chaosSystem = ModContent.GetInstance<ChaosEffectsSystem>();
        chaosSystem?.LoadMasterPool();
    }
}
