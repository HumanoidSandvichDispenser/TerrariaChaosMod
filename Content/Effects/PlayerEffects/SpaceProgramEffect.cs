using System;
using Terraria;
using Terraria.ID;

namespace TerrariaChaosMod.Content.Effects.PlayerEffects;

public class SpaceProgramEffect : Effect
{
    // space is actually 35% from surface to world top but we leave some margin
    protected double SpaceHeight => Main.worldSurface * 16 * 0.25 - 256;

    private bool _reachedSpace = false;

    private float? _previousPosition = null;

    private float _initialY;

    private float _maxVelocity;

    private int _maxVelocityTick;

    private float _dy, _dy1, _dy2, _g;

    private float GetVelocity(int tick)
    {
        if (tick < _maxVelocityTick)
        {
            return 4 * _g * tick;
        }
        return _maxVelocity - _g * (tick - _maxVelocityTick);
    }

    private float GetY(int tick)
    {
        if (tick < _maxVelocityTick)
        {
            return (4 * _g * tick * tick) / 2;
        }

        tick = tick - _maxVelocityTick;
        return _dy1 + _maxVelocity * tick - _g * tick * tick / 2;
    }

    public override void ApplyEffect(Player player)
    {
        _initialY = player.position.Y;
        _dy = (float)Math.Abs(SpaceHeight - player.position.Y);
        _dy1 = _dy / 5;
        _dy2 = 4 * _dy / 5;
        _g = player.gravity;
        _maxVelocityTick = (int)Math.Sqrt(_dy1 / (2 * _g));
        _maxVelocity = (float)Math.Sqrt(8 * _g * _dy1);

        _reachedSpace = false;
        player.velocity.Y = -GetVelocity(CurrentTick);
    }

    public override bool Update(Player player)
    {
        if (_reachedSpace)
        {
            return true;
        }

        return base.Update(player);
    }

    public override void PostUpdate(Player player)
    {
        if (!_reachedSpace)
        {
            if (_previousPosition is not null)
            {
                //float actualVelocity = (player.position.Y - _previousPosition.Value);

                // the actual velocity often differs from the expected velocity
                // which makes the kinematics calculation inaccurate
                // so if the difference is too large, we correct it

                //if (Math.Abs(actualVelocity - player.velocity.Y) > 5f)
                //{
                //    // move player to expected position
                //    player.position.Y -= actualVelocity;
                //    player.position.Y += player.velocity.Y;
                //}

                float expectedY = _initialY - GetY(CurrentTick);

                if (Math.Abs(player.position.Y - expectedY) > 4f)
                {
                    player.position.Y = expectedY;
                }
            }
            player.velocity.Y = -GetVelocity(CurrentTick);
            player.AddBuff(BuffID.Shimmer, TimeLeft);
        }

        if (player.position.Y < SpaceHeight + 256)
        {
            _reachedSpace = true;
        }

        _previousPosition = player.position.Y;
        base.PostUpdate(player);
    }

    public override bool ShouldApplyNow(Player player)
    {
        // only apply if below space, not dead, and lock not already acquired
        return AcquireLock() && player.position.Y > SpaceHeight && !player.dead;
    }

    public override void CleanUp(Player player)
    {
        ReleaseLock();
        base.CleanUp(player);
    }
}
