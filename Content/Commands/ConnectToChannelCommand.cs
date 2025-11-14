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
        string log;

        if (args.Length < 1)
        {
            string channel = chaosSystem.TwitchVoteEffectProvider.CurrentChannel;
            string status = $"Currently connected to Twitch channel: \"{channel}\"";
            log = ChaosModLogger.GetLogText(log4net.Core.Level.Info, status);
            caller.Reply(log);

            return;
        }

        string channelName = args[0];
        string text = $"Connecting to Twitch channel: \"{channelName}\"...";
        log = ChaosModLogger.GetLogText(log4net.Core.Level.Info, text);
        caller.Reply(log);

        chaosSystem.TwitchVoteEffectProvider.ConnectTwitch(channelName);
    }
}
