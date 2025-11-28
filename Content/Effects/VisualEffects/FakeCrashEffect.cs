using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace TerrariaChaosMod.Content.Effects.VisualEffects;

public class FakeCrashEffect : Effect
{
    private bool _hasLastFreeze = false;

    private string _statusText = "";

    public override string StatusText => _statusText;

    // higher in pre-hardmode since real crash is disabled in pre-hardmode
    public override double Weight => HardmodeWeight(2, 1);

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
        _hasLastFreeze = false;
        System.Threading.Thread.Sleep(200);
        After(30, () => System.Threading.Thread.Sleep(400));
        After(40, () => System.Threading.Thread.Sleep(400));
        After(60, () => { System.Threading.Thread.Sleep(5000); _hasLastFreeze = true; });
        After(61, () => _statusText = DisplayName.ToString());

        base.ApplyEffect(player);
    }

    public override bool Update(Player player)
    {
        return base.Update(player);
    }

    public override void CleanUp(Player player)
    {
        SetResponding(true);
        base.CleanUp(player);
    }
}
