using Terraria;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content.Buffs;

public class HelenKellerBuff : BaseChaosBuff
{
    public override void Update(Player player, ref int buffIndex)
    {
        var volumeSystem = ModContent.GetInstance<ChaosVolumeSystem>();
        int buffTime = player.buffTime[buffIndex];

        if (buffTime > 1 && buffTime < BaseChaosBuff.ONE_TIME_APPLY_TICK)
        {
            if (!volumeSystem.IsVolumeModified())
            {
                volumeSystem.SaveVolumeSettings();
                Main.musicVolume = 0f;
                Main.ambientVolume = 0f;
                Main.soundVolume = 0f;
            }

            Main.BlackFadeIn = 255;
            Lighting.GlobalBrightness = 0f;
        }
        else
        {
            Lighting.GlobalBrightness = 1f;
            volumeSystem.RestoreVolumeSettings();
        }
    }
}
