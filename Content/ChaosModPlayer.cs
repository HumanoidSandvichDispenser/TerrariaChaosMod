using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TerrariaChaosMod.Content;

public class ChaosModPlayer : ModPlayer
{
    public HashSet<Effects.Effect> activeEffects = new HashSet<Effects.Effect>();

    public bool shouldLag = false;
    public Vector2 manualGravity = Vector2.Zero;
    public float previousSoundVolume = -1;
    public float previousAmbientVolume = -1;
    public float previousMusicVolume = -1;
    public float zoomFactor = -1;
    public float healValueScale = 1;

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
        zoomFactor = -1;
        healValueScale = 1;

        if (previousSoundVolume >= 0)
        {
            Main.musicVolume = previousMusicVolume;
            Main.soundVolume = previousSoundVolume;
            Main.ambientVolume = previousAmbientVolume;
        }
    }

    public override void PreUpdate()
    {
        var effectsSystem = ModContent.GetInstance<ChaosEffectsSystem>();

        while (effectsSystem.EffectsToApply.TryDequeue(out var effect))
        {
            activeEffects.Add(effect);
            effect.ResetTime();
            effect.ApplyEffect(Player);

            // notify player with icon and text
            string icon = $"[i:{ItemID.RainbowCursor}]";
            var color = Color.MediumPurple;
            Main.NewText($"{icon} New Effect: {effect.DisplayName}", color);
        }

        foreach (var effect in activeEffects)
        {
            if (effect.Update(Player))
            {
                effect.CleanUp(Player);
                activeEffects.Remove(effect);
            }
        }
    }

    public override void PostUpdate()
    {
        /*
        if (_chaosTimer % _chaosInterval == 0)
        {
            _chaosTimer = 0;
            var effect = ModContent
                .GetInstance<Effects.VisualEffects.RealCrashEffect>()
                .Clone() as Effects.Effect;
            activeEffects.Add(effect);
            effect.ResetTime();
            effect.ApplyEffect(Player);
        }
        _chaosTimer++;
        */
        foreach (var effect in activeEffects)
        {
            effect.PostUpdate(Player);
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

    public override void ModifyZoom(ref float zoom)
    {
        if (zoomFactor > 0)
        {
            zoom = zoomFactor;
        }
    }

    public override void GetHealLife(Item item, bool quickHeal, ref int healValue)
    {
        healValue = (int)(healValue * healValueScale);
    }
}
