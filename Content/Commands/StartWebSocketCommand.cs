using Terraria.ModLoader;

namespace TerrariaChaosMod.Content.Commands;

public class StartWebSocketCommand : ModCommand
{
    public override CommandType Type => CommandType.Chat;

    public override string Command => "ws";

    public override string Usage => "/ws [start|stop|restart]";

    public override string Description => "Starts the Twitch voting tally WebSocket";

    public override void Action(CommandCaller caller, string input, string[] args)
    {
        var system = ModContent.GetInstance<ChaosEffectsSystem>();
        caller.Reply("Starting WebSocket server.");

        switch (args.Length > 0 ? args[0].ToLower() : default)
        {
            case "start":
                caller.Reply("Starting WebSocket server.");
                system.TwitchVoteEffectProvider?.StartWebSocket();
                break;
            case "stop":
                caller.Reply("Stopping WebSocket server.");
                system.TwitchVoteEffectProvider?.StopWebSocket();
                break;
            case "restart":
            default:
                caller.Reply("Restarting WebSocket server.");
                system.TwitchVoteEffectProvider?.RestartWebSocket();
                break;
        }
    }
}
