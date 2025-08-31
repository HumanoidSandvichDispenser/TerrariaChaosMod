using System.Collections.Generic;

namespace TerrariaChaosMod.Content;

public interface IEffectProvider
{
    public bool IsReady { get; }

    public void ReinitializePool(IReadOnlySet<Effects.Effect> effectPool);

    public Effects.Effect GetEffect();
}
