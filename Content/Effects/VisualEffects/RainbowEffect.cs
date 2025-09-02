using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;

namespace TerrariaChaosMod.Content.Effects.VisualEffects;

public class RainbowEffect : ShaderEffect
{
    public override string ShaderKey => "TerrariaChaosMod:Rainbow";

    public override bool Update(Player player)
    {
        Filters.Scene["TerrariaChaosMod:Rainbow"]
            .GetShader()
            .UseColor(GetRainbowColor(TimeLeft / 60f));
        return base.Update(player);
    }

    private static Color GetRainbowColor(float time, float speed = 1f)
    {
        float r = (float)(Math.Sin(speed * time + 0f) * 0.5 + 0.5);
        float g = (float)(Math.Sin(speed * time + 2f) * 0.5 + 0.5);
        float b = (float)(Math.Sin(speed * time + 4f) * 0.5 + 0.5);

        return new Color(r, g, b);
    }
}
