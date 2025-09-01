using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content.Commands;

public class ConnectToChannelCommand : ModCommand
{
    public override CommandType Type => CommandType.Chat;

    public override string Command => "connect";

    public override string Usage => "/connect <channel name>";

    public override string Description => "Connect to a Twitch channel to read chat messages from.";

    public override void Action(CommandCaller caller, string input, string[] args)
    {
        if (args.Length != 1)
        {
            caller.Reply("Expected channel name as argument.", Color.Red);
            return;
        }

        string channelName = args[0];
        var chaosSystem = ModContent.GetInstance<ChaosEffectsSystem>();
        caller.Reply($"Attempting to connect to channel \"{channelName}\"...", Color.SkyBlue);
        chaosSystem.TwitchVoteEffectProvider.Connect(channelName);
    }
}
