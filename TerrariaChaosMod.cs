using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace TerrariaChaosMod;

// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
public class TerrariaChaosMod : Mod
{
    public static int LHeroine { get; private set; }

    public override void Load()
    {
        // This method is called when the mod is loaded.
        // You can use it to initialize any resources or settings your mod needs.

        if (!Main.dedServ)
        {
            //new ScreenShaderData(Mod.Assets.Request<Effect>(""))
            Filters.Scene["TerrariaChaosMod:InvertColors"] = new Filter(
                new ScreenShaderData("FilterInvert"),
                EffectPriority.High);
            Filters.Scene["TerrariaChaosMod:Rainbow"] = new Filter(
                new ScreenShaderData("FilterMiniTower")
                    .UseOpacity(0.5f),
                EffectPriority.High);
            Filters.Scene["TerrariaChaosMod:LHeroine"] = new Filter(
                new ScreenShaderData("FilterMiniTower")
                    .UseColor(0, 0.5f, 2)
                    .UseOpacity(0.0f),
                EffectPriority.High);
        }
    }
}
