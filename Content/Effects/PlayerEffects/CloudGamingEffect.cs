using System.Collections.Generic;
using Terraria;

namespace TerrariaChaosMod.Content.Effects.PlayerEffects;

public sealed class CloudGamingEffect : Effect
{
    /*
    private void SetForceControls(Player player, bool value)
    {
        player.GetModPlayer<EffectPlayer>()
            .ForceControlsFromQueue = value;
    }

    public override bool Update(Player player)
    {
        var effectPlayer = player.GetModPlayer<EffectPlayer>();
        int secondsLeft = TimeLeft / 60;

        if (secondsLeft % 2 == 0)
        {
            effectPlayer.ForceControlsFromQueue = true;
            SetForceControls(player, true);
        }
        else
        {
            // if we just switched, clear the queue
            if (effectPlayer.ForceControlsFromQueue)
            {
                effectPlayer.ForceControlsFromQueue = false;
                effectPlayer.ControlQueue.Clear();
            }

            var state = effectPlayer.GetMovementState();
            effectPlayer.ControlQueue.Enqueue(state);
        }

        return base.Update(player);
    }
    */

    public override void ApplyEffect(Player player)
    {
        var effectPlayer = player.GetModPlayer<EffectPlayer>();
        effectPlayer.EnqueueControls = true;
        After(Seconds(1), () => effectPlayer.DequeueControls = true);

        base.ApplyEffect(player);
    }

    public override void CleanUp(Player player)
    {
        var effectPlayer = player.GetModPlayer<EffectPlayer>();
        effectPlayer.EnqueueControls = effectPlayer.DequeueControls = false;
        effectPlayer.ControlQueue.Clear();

        base.CleanUp(player);
    }
}
