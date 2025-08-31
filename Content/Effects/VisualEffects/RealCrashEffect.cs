using Terraria;

namespace TerrariaChaosMod.Content.Effects.VisualEffects;

public class RealCrashEffect : FakeCrashEffect
{
    private bool _hasCrashed = false;

    public override void ApplyEffect(Player player)
    {
        SetResponding(false);
        WorldGen.saveAndPlay();

        _hasCrashed = false;
        After(61,
            () => {
                // exit the game after a second delay
                Main.WeGameRequireExitGame();
                _hasCrashed = true;
            });

        base.ApplyEffect(player);
    }

    public override bool Update(Player player)
    {
        base.Update(player);
        return _hasCrashed;
    }
}
