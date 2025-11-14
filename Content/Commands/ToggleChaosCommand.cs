using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content.Commands;

public class ToggleChaosCommand : ModCommand
{
    public override CommandType Type => CommandType.Chat;

    public override string Command => "togglechaos";

    public override string Description => "Toggles the Chaos Effects System on or off.";

    public override void Action(CommandCaller caller, string input, string[] args)
    {
        var effectSystem = ModContent.GetInstance<ChaosEffectsSystem>();
        effectSystem.IsEnabled = !effectSystem.IsEnabled;
        string status = effectSystem.IsEnabled ? "enabled" : "disabled";
        var color = effectSystem.IsEnabled ? Color.Green : Color.Red;
        var text = $"Chaos effects have been {status}.";
        caller.Reply(ChaosModLogger.GetLogText(log4net.Core.Level.Info, text));
    }
}
