using System.Linq;
using System.Collections.Generic;

namespace TerrariaChaosMod.Content;

public class RandomEffectProvider : IEffectProvider
{
    public bool IsReady => true;

    public bool CanProvide => true;

    private HashSet<Effects.Effect> _effectPool;

    public void ReinitializePool(IReadOnlySet<Effects.Effect> effectPool)
    {
        _effectPool = effectPool.ToHashSet();
        _effectPool.RemoveWhere(effect => !effect.ShouldIncludeInPool(_effectPool));
    }

    public Effects.Effect GetEffect()
    {
        var rand = new System.Random();
        int index = rand.Next(0, _effectPool.Count);
        return _effectPool.ElementAt(index);
    }
}
