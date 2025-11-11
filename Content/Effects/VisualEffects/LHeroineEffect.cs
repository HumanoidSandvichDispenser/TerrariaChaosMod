using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Effects;

namespace TerrariaChaosMod.Content.Effects.VisualEffects;

public class LHeroineEffect : ShaderEffect
{
    public override string ShaderKey => "TerrariaChaosMod:LHeroine";

    public override void ApplyEffect(Player player)
    {
        var sound = TerrariaChaosMod.LHeroine;
        SoundEngine.PlaySound(sound, player.position);
        base.ApplyEffect(player);
    }

    public override bool Update(Player player)
    {
        const int beatLength = 18;

        // skip 4 bars (16 beats)
        int intro = Duration - beatLength * 4 * 4;
        int outro = Duration - beatLength * 4 * 19;
        if (TimeLeft > intro || TimeLeft < outro)
        {
            return base.Update(player);
        }

        int phase = (TimeLeft % beatLength);
        float normalized = phase / (float)beatLength;
        Filters.Scene[ShaderKey]
            .GetShader()
            .UseOpacity(normalized);

        return base.Update(player);
    }
}
