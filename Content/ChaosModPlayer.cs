using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TerrariaChaosMod.Content;

public class ChaosModPlayer : ModPlayer
{
    public bool shouldLag = false;
    public Vector2 manualGravity = Vector2.Zero;
    public float previousSoundVolume = -1;
    public float previousAmbientVolume = -1;
    public float previousMusicVolume = -1;

    public Vector2 lastPositionLag = Vector2.Zero;

    private int _chaosTimer = 0;
    private int _chaosInterval = 600;
    private int _frameCounter = 0;
    private int _updateCounter = 0;
    private PlayerDrawSet? _drawInfo = null;

    public override void ResetEffects()
    {
        shouldLag = false;
        manualGravity = Vector2.Zero;

        if (previousSoundVolume >= 0)
        {
            Main.musicVolume = previousMusicVolume;
            Main.soundVolume = previousSoundVolume;
            Main.ambientVolume = previousAmbientVolume;
        }
    }

    public override void PreUpdate()
    {
        if (manualGravity.LengthSquared() > 0.01f)
        {
            Player.velocity += manualGravity;
        }
    }

    public override void PostUpdate()
    {
        var chaosSystem = ModContent.GetInstance<ChaosVotingSystem>();

        while (chaosSystem.buffsToApply.Count > 0)
        {
            int buffId = chaosSystem.buffsToApply.Dequeue();
            Player.AddBuff(buffId, 600);
            chaosSystem.buffHistory.Push(buffId);

            var buff = (Buffs.BaseChaosBuff)ModContent.GetModBuff(buffId);
            var name = buff.DisplayName;

            Main.NewText($"Chaos effect triggered: {name}!", Microsoft.Xna.Framework.Color.Cyan);
        }
    }

    public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
    {
        if (!shouldLag)
        {
            _drawInfo = null;
            return;
        }

        _frameCounter++;

        if (_frameCounter % 15 == 0)
        {
            _drawInfo = drawInfo;
        }
        else if (_drawInfo.HasValue)
        {
            drawInfo = _drawInfo.Value;
        }

        if (_frameCounter % 3600 == 0)
        {
            _frameCounter = 0;
        }
    }
}
