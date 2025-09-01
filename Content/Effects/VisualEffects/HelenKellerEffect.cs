using Terraria;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content.Effects.VisualEffects;

public class HelenKellerEffect : Effect
{
    private readonly ChaosVolumeSystem _volumeSystem;

    public override int Duration => Seconds(15);

    public HelenKellerEffect()
    {
        _volumeSystem = ModContent.GetInstance<ChaosVolumeSystem>();
    }

    public override void ApplyEffect(Player player)
    {
        if (!_volumeSystem.IsVolumeModified())
        {
            _volumeSystem.SaveVolumeSettings();
            _volumeSystem.MuteVolume();
        }

        base.ApplyEffect(player);
    }

    public override bool Update(Player player)
    {
        if (!_volumeSystem.IsVolumeModified())
        {
            _volumeSystem.SaveVolumeSettings();
        }
        _volumeSystem.MuteVolume();

        Main.BlackFadeIn = 255;
        Lighting.GlobalBrightness = 0f;
        return base.Update(player);
    }

    public override void CleanUp(Player player)
    {
        _volumeSystem.RestoreVolumeSettings();
        Lighting.GlobalBrightness = 1f;
        base.CleanUp(player);
    }
}
