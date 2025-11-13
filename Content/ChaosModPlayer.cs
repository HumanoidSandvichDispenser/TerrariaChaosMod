using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TerrariaChaosMod.Content;

public class ChaosModPlayer : ModPlayer
{
    public HashSet<Effects.Effect> activeEffects = new();

    private Queue<Effects.Effect> _waitingEffects = new();

    public override void ResetEffects()
    {

    }

    private void ApplyEffect(Effects.Effect effect)
    {
        activeEffects.Add(effect);
        effect.ResetTime();
        effect.ApplyEffect(Player);

        // notify player with icon and text
        string icon = $"[i:{ItemID.RainbowCursor}]";
        var color = Color.MediumPurple;
        Main.NewText($"{icon} New Effect: {effect.DisplayName}", color);
    }

    public override void PreUpdate()
    {
        var effectsSystem = ModContent.GetInstance<ChaosEffectsSystem>();

        while (effectsSystem.EffectsToApply.TryDequeue(out var effect))
        {
            if (!effect.ShouldApplyNow(Player))
            {
                _waitingEffects.Enqueue(effect);
                continue;
            }

            ApplyEffect(effect);
        }

        // re-enqueue waiting effects for next tick
        while (_waitingEffects.Count > 0)
        {
            effectsSystem.EffectsToApply.Enqueue(_waitingEffects.Dequeue());
        }

        foreach (var effect in activeEffects)
        {
            effect.CurrentTick++;
            if (effect.Update(Player))
            {
                effect.CleanUp(Player);
                activeEffects.Remove(effect);
            }
        }
    }

    public override void PostUpdate()
    {
        foreach (var effect in activeEffects)
        {
            effect.PostUpdate(Player);
        }
    }

    public override void OnEnterWorld()
    {
        var effectsSystem = ModContent.GetInstance<ChaosEffectsSystem>();
        effectsSystem.Startup();
    }
}
