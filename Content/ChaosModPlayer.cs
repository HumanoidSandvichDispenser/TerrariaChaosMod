using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
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
        if (_chaosTimer % _chaosInterval == 0)
        {
            _chaosTimer = 0;
            var effect = new Effects.ItemEffects.EatDaPooPooEffect();
            activeEffects.Add(effect);
            effect.ResetTime();
            effect.ApplyEffect(Player);
        }
        _chaosTimer++;

        foreach (var effect in activeEffects)
        {
            effect.PostUpdate(Player);
        }

        //var chaosSystem = ModContent.GetInstance<ChaosVotingSystem>();
        //if (!Player.HasBuff(Terraria.ID.BuffID.Wisp))
        //{
        //    Player.AddBuff(Terraria.ID.BuffID.Wisp, 500);
        //}

        //while (chaosSystem.buffsToApply.Count > 0)
        //{
        //    int buffId = chaosSystem.buffsToApply.Dequeue();
        //    Player.AddBuff(buffId, 600);
        //    chaosSystem.buffHistory.Push(buffId);

        //    var buff = (Buffs.BaseChaosBuff)ModContent.GetModBuff(buffId);
        //    var name = buff.DisplayName;

        //    Main.NewText($"Chaos effect triggered: {name}!", Microsoft.Xna.Framework.Color.Cyan);
        //}
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
