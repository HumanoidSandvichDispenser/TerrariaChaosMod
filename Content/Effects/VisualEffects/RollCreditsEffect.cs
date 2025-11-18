using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content.Effects.VisualEffects;

public class RollCreditsEffect : Effect
{
    public override int Duration => Seconds(130);

    public override void ApplyEffect(Player player)
    {
        AcquireLock(true);
        SkyManager.Instance.Activate("TerrariaChaosMod:CreditsRoll", player.Center);
        SkyManager.Instance["TerrariaChaosMod:CreditsRoll"].Opacity = 1f;

        base.ApplyEffect(player);
    }

    public override void CleanUp(Player player)
    {
        ReleaseLock();
        SkyManager.Instance.Deactivate("TerrariaChaosMod:CreditsRoll");
        base.CleanUp(player);
    }
}
