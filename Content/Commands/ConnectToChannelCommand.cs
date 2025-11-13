using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content.Commands;

public class ConnectToChannelCommand : ModCommand
{
    public override CommandType Type => CommandType.Chat;

    public override string Command => "connect";

    public override string Usage => "/connect [<CHANNEL_NAME>]";

    public override string Description => "Connect to a Twitch channel to read chat messages from.";

    public override void Action(CommandCaller caller, string input, string[] args)
    {
        var chaosSystem = ModContent.GetInstance<ChaosEffectsSystem>();

        if (args.Length != 1)
        {
            string channel = chaosSystem.TwitchVoteEffectProvider.CurrentChannel;
            caller.Reply($"Currently connected to channel: \"{channel}\"", Color.SkyBlue);
            return;
        }

        string channelName = args[0];
        caller.Reply($"Attempting to connect to channel \"{channelName}\"...", Color.SkyBlue);
        chaosSystem.TwitchVoteEffectProvider.ConnectTwitch(channelName);
    }
}
