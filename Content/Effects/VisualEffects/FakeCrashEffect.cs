using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace TerrariaChaosMod.Content.Effects.VisualEffects;

public class FakeCrashEffect : Effect
{
    private bool _hasLastFreeze = false;

    protected bool _showName = true;

    public override LocalizedText DisplayName
    {
        get
        {
            if (!_showName)
            {
                return Language.GetText("Mods.TerrariaChaosMod.HiddenEffect");
            }
            return this.GetLocalization(nameof(DisplayName));
        }
    }

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
        _showName = false;
        SetResponding(false);

        // freeze for 5 seconds, and another short freeze after 30 ticks
        _hasLastFreeze = false;
        System.Threading.Thread.Sleep(200);
        After(30, () => System.Threading.Thread.Sleep(400));
        After(40, () => System.Threading.Thread.Sleep(400));
        After(60, () => { System.Threading.Thread.Sleep(5000); _hasLastFreeze = true; });

        base.ApplyEffect(player);
    }

    public override bool Update(Player player)
    {
        // end the effect after the second freeze
        base.Update(player);
        return _hasLastFreeze;
    }

    public override void CleanUp(Player player)
    {
        _showName = true;
        SetResponding(true);
        base.CleanUp(player);
    }
}
