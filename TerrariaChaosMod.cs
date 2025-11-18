using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.GameContent.Skies;

namespace TerrariaChaosMod;

// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
public class TerrariaChaosMod : Mod
{
    public static Terraria.Audio.SoundStyle LHeroine { get; private set; }

    public static int ForsenTheme { get; private set; }

    public static Terraria.Audio.SoundStyle SquareWave { get; private set; }

    public static Effect PaletteShaderEffect { get; private set; }

    public static ChaosModLogger ChatLogger { get; private set; } = new(null!);

    public override void Load()
    {
        // This method is called when the mod is loaded.
        // You can use it to initialize any resources or settings your mod needs.

        ChatLogger = new(this);

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
            Filters.Scene["TerrariaChaosMod:LHeroineInvert"] = new Filter(
                new ScreenShaderData("FilterInvert")
                    .UseOpacity(0.0f),
                EffectPriority.High);

            SkyManager.Instance["TerrariaChaosMod:CreditsRoll"] =
                new Content.Skies.ForcedCreditsRollSky();

            LHeroine = new("TerrariaChaosMod/Content/Music/heroine");

            SquareWave = new("TerrariaChaosMod/Content/Audio/tone");

            PaletteShaderEffect = Assets.Request<Effect>("Content/Shaders/PaletteShader").Value;

            string forsenThemePath = "Content/Audio/forsen/dont-doubt";
            MusicLoader.AddMusic(this, forsenThemePath);
            ForsenTheme = MusicLoader.GetMusicSlot(this, forsenThemePath);
        }
    }
}
