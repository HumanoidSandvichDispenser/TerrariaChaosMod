using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content;

public class ChaosVolumeSystem : ModSystem
{
    private float _previousMusicVolume = -1;
    private float _previousSoundVolume = -1;
    private float _previousAmbientVolume = -1;

    public bool ForceCreditsMusic { get; set; } = false;

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

    public void MuteVolume()
    {
        Main.musicVolume = 0;
        Main.soundVolume = 0;
        Main.ambientVolume = 0;
    }

    public override void PostUpdatePlayers()
    {
        if (ForceCreditsMusic)
        {
            if (Main.curMusic != MusicID.Credits)
            {
                Main.musicFade[Main.curMusic] = 0f;
            }
            Main.curMusic = MusicID.Credits;
            Main.musicFade[Main.curMusic] = 1f;
        }
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
