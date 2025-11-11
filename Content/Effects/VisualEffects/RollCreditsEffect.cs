using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content.Effects.VisualEffects;

public class RollCreditsEffect : Effect
{
    public override int Duration => Seconds(60);

    public override void ApplyEffect(Player player)
    {
        SkyManager.Instance.Activate("TerrariaChaosMod:CreditsRoll", player.Center);
        SkyManager.Instance["TerrariaChaosMod:CreditsRoll"].Opacity = 1f;
        //Main.musicFade[Main.curMusic] = 1f;
        ChaosVolumeSystem volumeSystem = ModContent.GetInstance<ChaosVolumeSystem>();
        volumeSystem.ForceCreditsMusic = true;

        base.ApplyEffect(player);
    }

    public override bool Update(Player player)
    {
        //int credits = MusicID.Credits;
        //if (Main.curMusic != credits)
        //{
        //    Main.musicFade[Main.curMusic] = 1f;
        //}
        //Main.curMusic = credits;
        //Main.musicFade[credits] = 0f;

        return base.Update(player);
    }

    public override void CleanUp(Player player)
    {
        SkyManager.Instance.Deactivate("TerrariaChaosMod:CreditsRoll");
        ChaosVolumeSystem volumeSystem = ModContent.GetInstance<ChaosVolumeSystem>();
        volumeSystem.ForceCreditsMusic = false;
        base.CleanUp(player);
    }
}
