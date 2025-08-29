using Terraria;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content;

public class ChaosVolumeSystem : ModSystem
{
    private float _previousMusicVolume = -1;
    private float _previousSoundVolume = -1;
    private float _previousAmbientVolume = -1;

    public void SaveVolumeSettings()
    {
        _previousMusicVolume = Main.musicVolume;
        _previousSoundVolume = Main.soundVolume;
        _previousAmbientVolume = Main.ambientVolume;
    }

    public void RestoreVolumeSettings()
    {
        if (_previousMusicVolume >= 0)
        {
            Main.musicVolume = _previousMusicVolume;
            Main.soundVolume = _previousSoundVolume;
            Main.ambientVolume = _previousAmbientVolume;
            _previousMusicVolume = -1;
            _previousSoundVolume = -1;
            _previousAmbientVolume = -1;
        }
    }

    public bool IsVolumeModified()
    {
        return _previousMusicVolume >= 0;
    }

    public override void OnWorldUnload()
    {
        RestoreVolumeSettings();
        base.OnWorldUnload();
    }

    public override void OnModUnload()
    {
        RestoreVolumeSettings();
        base.OnModUnload();
    }
}
