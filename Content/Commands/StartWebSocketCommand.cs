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
        string cmd = args.Length > 0 ? args[0].ToLower() : "info";

        if (cmd == "start")
        {
            system.TwitchVoteEffectProvider?.StartWebSocket();
        }
        else if (cmd == "stop")
        {
            system.TwitchVoteEffectProvider?.StopWebSocket();
        }
        else if (cmd == "restart")
        {
            system.TwitchVoteEffectProvider?.RestartWebSocket();
        }
        else
        {
            caller.Reply("Usage: " + Usage);
        }

        //switch (cmd)
        //{
        //    case "start":
        //        system.TwitchVoteEffectProvider?.StartWebSocket();
        //        break;
        //    case "stop":
        //        system.TwitchVoteEffectProvider?.StopWebSocket();
        //        break;
        //    case "restart":
        //        system.TwitchVoteEffectProvider?.RestartWebSocket();
        //        break;
        //}
    }
}
