using System.Collections.Generic;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content.Effects;

public class EffectLoader : ILoadable
{
    internal static List<Effect> _effects = new List<Effect>();

    public static IReadOnlyList<Effect> Effects => _effects;

    internal static int Add(Effect effect)
    {
        int type = _effects.Count;
        _effects.Add(effect);
        return type;
    }

    public void Load(Mod mod)
    {

    }

    public void Unload()
    {

    }
}
