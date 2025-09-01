using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace TerrariaChaosMod.Content.UI;

public class EffectsUISystem : ModSystem
{
    private EffectsUI _effectsUI;

    public UserInterface Ui { get; private set; }

    private GameTime _lastUpdateGameTime;

    public override void Load()
    {
        if (!Main.dedServ)
        {
            Ui = new UserInterface();

            _effectsUI = new EffectsUI();
            _effectsUI.Activate();
        }
    }

    public override void UpdateUI(GameTime gameTime)
    {
        _lastUpdateGameTime = gameTime;
        if (Ui?.CurrentState is not null)
        {
            Ui.Update(gameTime);
        }
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
        if (mouseTextIndex != -1)
        {
            layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                "TerrariaChaosMod: Ui",
                delegate
                {
                    if ( _lastUpdateGameTime != null && Ui?.CurrentState != null)
                    {
                        Ui.Draw(Main.spriteBatch, _lastUpdateGameTime);
                    }
                    return true;
                },
                InterfaceScaleType.UI));
        }
    }

    public void Show()
    {
        Ui?.SetState(_effectsUI);
    }

    public void Hide()
    {
        Ui?.SetState(null);
    }

    public override void OnWorldLoad()
    {
        Show();
    }

    public override void OnWorldUnload()
    {
        Hide();
    }

    public override void Unload()
    {
        _effectsUI = null;
    }
}
