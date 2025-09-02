using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;

namespace TerrariaChaosMod.Content.Effects.VisualEffects;

public abstract class ShaderEffect : Effect
{
    public abstract string ShaderKey { get; }

    public override void ApplyEffect(Player player)
    {
        //Filters.Scene[ShaderKey]?.Activate();
        Filters.Scene.Activate(ShaderKey);
    }

    public override void CleanUp(Player player)
    {
        Filters.Scene.Deactivate(ShaderKey);
    }
}
