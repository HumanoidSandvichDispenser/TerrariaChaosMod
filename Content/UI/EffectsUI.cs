using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;

namespace TerrariaChaosMod.Content.UI;

public class EffectsUI : UIState
{
    private UIText _text;

    public override void OnInitialize()
    {
        //UIPanel panel = new UIPanel();

        _text = new UIText("Effects UI\nlol");
        //panel.Append(text);
        _text.Width.Set(512f, 0f);
        _text.Height.Set(256f, 0f);
        _text.HAlign = 0f;
        _text.VAlign = 0.5f;
        _text.Left.Set(16f, 0f);
        _text.TextOriginX = 0f;
        _text.TextOriginY = 0.5f;
        Append(_text);
    }

    // update text with active effects
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (Main.playerInventory)
        {
            _text.SetText("");
            return;
        }

        // get active effects from player
        var chaosPlayer = Main.LocalPlayer.GetModPlayer<ChaosModPlayer>();
        var activeEffects = chaosPlayer.activeEffects;
        string text;
        string icon = $"[i:{ItemID.RainbowCursor}]";

        // update text
        if (activeEffects.Count == 0)
        {
            text = $"{icon} No active effects";
            _text.TextColor = Color.DimGray;
        }
        else
        {
            var effectNameMap = activeEffects
                .Where(eff => eff.DisplayName.Value != string.Empty)
                .Select(eff => $"{eff.DisplayName} ({eff.TimeLeft / 60}s)");
            text = string.Join("\n", effectNameMap);
            _text.TextColor = Color.White;
        }
        _text.SetText(text);
    }
}
