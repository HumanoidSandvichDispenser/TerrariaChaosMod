using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrariaChaosMod.Content.Effects;
using TerrariaChaosMod.Content.Effects.VisualEffects;

namespace TerrariaChaosMod.Content.SceneEffects;

public class EndCreditsSceneEffect : ModSceneEffect
{
    public override bool IsSceneEffectActive(Player player)
    {
        return EffectLock.Of<RollCreditsEffect>().IsAcquired;
    }

    public override int Music => MusicID.Credits;

    public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
}
