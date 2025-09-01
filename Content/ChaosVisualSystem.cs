using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content;

public class ChaosVisualSystem : ModSystem
{
    public bool ShouldSkipFrames { get; set; } = false;

    private int _skipCount = 0;

    public override void OnWorldLoad()
    {
        _skipCount = 0;

        On_Main.DoDraw += On_Main_DoDraw;

        base.OnWorldLoad();
    }

    private void On_Main_DoDraw(On_Main.orig_DoDraw orig, Main main, GameTime gameTime)
    {
        if (!ShouldSkipFrames || _skipCount++ >= 4)
        {
            _skipCount = 0;

            orig(main, gameTime);
        }
    }

    public override void OnWorldUnload()
    {
        On_Main.DoDraw -= On_Main_DoDraw;
        base.OnWorldUnload();
    }
}
