using System.IO;
using System.Linq;
using System.Text.Json;
using Terraria;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content.Commands;

public class ExportEffectsCommand : ModCommand
{
    public override CommandType Type => CommandType.Chat;

    public override string Command => "exporteffects";

    public override string Usage => "/exporteffects";

    public override string Description => "Exports the list of effects to a JSON file";

    public override void Action(CommandCaller caller, string input, string[] args)
    {
        var system = ModContent.GetInstance<ChaosEffectsSystem>();
        var exports = Path.Combine(Main.SavePath, "ModExports");
        var savefile = Path.Combine(exports, "effects.json");

        var metadata = system.AllEffects.Select(eff => eff.GetMetadata());
        string json = JsonSerializer.Serialize(
            metadata.ToList(),
            new JsonSerializerOptions { WriteIndented = true });

        if (!Directory.Exists(exports))
        {
            Directory.CreateDirectory(exports);
        }
            
        File.WriteAllText(savefile, json);

        caller.Reply($"Exported all effect metadata to {savefile}.");
    }
}
