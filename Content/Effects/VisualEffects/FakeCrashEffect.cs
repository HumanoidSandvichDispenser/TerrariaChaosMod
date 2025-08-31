using Terraria;

namespace TerrariaChaosMod.Content.Effects.VisualEffects;

public class FakeCrashEffect : Effect
{
    private bool _hasSecondFreeze = false;

    protected void SetResponding(bool isResponding)
    {
        const string SUFFIX = " (Not Responding)";
        var window = Main.instance.Window;

        if (isResponding && window.Title.EndsWith(SUFFIX))
        {
            int len = window.Title.Length - SUFFIX.Length;
            window.Title = window.Title.Substring(0, len);
        }
        else if (!isResponding && !window.Title.EndsWith(SUFFIX))
        {
            window.Title += SUFFIX;
        }
    }

    public override void ApplyEffect(Player player)
    {
        SetResponding(false);

        // freeze for 5 seconds, and another short freeze after 30 ticks
        _hasSecondFreeze = false;
        System.Threading.Thread.Sleep(5000);
        After(30,
            () => {
                System.Threading.Thread.Sleep(1000);
                _hasSecondFreeze = true;
            });

        base.ApplyEffect(player);
    }

    public override bool Update(Player player)
    {
        // end the effect after the second freeze
        base.Update(player);
        return _hasSecondFreeze;
    }

    public override void CleanUp(Player player)
    {
        SetResponding(true);
        base.CleanUp(player);
    }
}
