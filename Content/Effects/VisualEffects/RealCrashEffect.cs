using Terraria;

namespace TerrariaChaosMod.Content.Effects.VisualEffects;

public class RealCrashEffect : FakeCrashEffect
{
    public override void ApplyEffect(Player player)
    {
        SetResponding(false);
        System.Threading.Thread.Sleep(5000);

        // instead of crashing, just disconnect the player
        Main.menuMode = 10;
        WorldGen.SaveAndQuit();
        // show a message in the main menu
        Main.statusText = "Disconnected due to an unexpected error.";

        base.ApplyEffect(player);
        SetResponding(true);
    }

    public override bool Update(Player player)
    {
        base.Update(player);
        return true;
    }
}
