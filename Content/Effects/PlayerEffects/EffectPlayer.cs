using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TerrariaChaosMod.Content.Effects.PlayerEffects;

public sealed class EffectPlayer : ModPlayer
{
    public int HealValueScale = 1;

    public float Zoom = -1;

    public Vector2 GravityDirection = Vector2.Zero;
    public bool DoGravity = false;

    public bool TemporaryMediumcore = false;

    public bool EveryChestIsTrapped = false;

    public bool IsNoHitRunActive = false;

    public bool IsPacifistRunActive = false;

    public bool IsHollowKnightMovementActive = false;

    public Queue<MovementState> ControlQueue = new();

    public bool EnqueueControls = false;

    public bool DequeueControls = false;

    public struct MovementState
    {
        public bool ControlLeft;
        public bool ControlRight;
        public bool ControlUp;
        public bool ControlDown;
        public bool ControlUseItem;
        public bool ControlHook;
        public bool ControlMount;
        public bool ControlJump;
    }

    public MovementState GetMovementState()
    {
        return new MovementState
        {
            ControlLeft = Player.controlLeft,
            ControlRight = Player.controlRight,
            ControlUp = Player.controlUp,
            ControlDown = Player.controlDown,
            ControlUseItem = Player.controlUseItem,
            ControlHook = Player.controlHook,
            ControlMount = Player.controlMount,
            ControlJump = Player.controlJump
        };
    }

    public override void ResetEffects()
    {
        HealValueScale = 1;
        Zoom = -1;
        DoGravity = false;
        TemporaryMediumcore = false;
        EveryChestIsTrapped = true;
        IsNoHitRunActive = false;
    }

    public override void GetHealLife(Item item, bool quickHeal, ref int healValue)
    {
        healValue = (int)(healValue * HealValueScale);
    }

    public override void ModifyZoom(ref float zoom)
    {
        zoom = Zoom;
    }

    public override void PreUpdate()
    {
        if (DoGravity)
        {
            // apply sideways gravity, ensuring velocity in each component does not
            // exceed terminal velocity
            float terminalVelocity = Player.maxFallSpeed;
            float newX = Player.velocity.X + GravityDirection.X;
            float newY = Player.velocity.Y + GravityDirection.Y;

            // can exceed terminal velocity if already exceeding it, but gravity won't
            // make it exceed terminal velocity if it isn't already
            if (System.Math.Abs(newX) < terminalVelocity)
            {
                Player.velocity.X = newX;
            }
            else if (System.Math.Abs(Player.velocity.X) < terminalVelocity)
            {
                Player.velocity.X = terminalVelocity * System.Math.Sign(newX);
            }

            if (System.Math.Abs(newY) < terminalVelocity)
            {
                Player.velocity.Y = newY;
            }
            else if (System.Math.Abs(Player.velocity.Y) < terminalVelocity)
            {
                Player.velocity.Y = terminalVelocity * System.Math.Sign(newY);
            }
        }
    }

    private void ModifyIncomingDamage(ref Player.HurtModifiers modifiers)
    {
        if (IsNoHitRunActive)
        {
            modifiers.SourceDamage.Flat = Player.statLifeMax2;
        }
    }

    private void ModifyOutgoingDamage(ref NPC.HitModifiers modifiers)
    {
        if (IsPacifistRunActive)
        {
            modifiers.SourceDamage.Scale(0);
        }
    }

    public override void ModifyHurt(ref Player.HurtModifiers modifiers)
    {
        ModifyIncomingDamage(ref modifiers);
    }

    public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
    {
        ModifyIncomingDamage(ref modifiers);
    }

    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        ModifyOutgoingDamage(ref modifiers);
    }

    public override void Kill(
        double damage,
        int hitDirection,
        bool pvp,
        Terraria.DataStructures.PlayerDeathReason damageSource)
    {
        if (TemporaryMediumcore)
        {
            // drop all items

            Player.DropItems();
            Player.DropCoins();
        }
    }

    public override void SetControls()
    {
        if (EnqueueControls)
        {
            MovementState state = GetMovementState();
            ControlQueue.Enqueue(state);
        }

        if (DequeueControls)
        {
            MovementState state;
            if (ControlQueue.TryDequeue(out state))
            {
                Player.controlLeft = state.ControlLeft;
                Player.controlRight = state.ControlRight;
                Player.controlUp = state.ControlUp;
                Player.controlDown = state.ControlDown;
                Player.controlUseItem = state.ControlUseItem;
                Player.controlHook = state.ControlHook;
                Player.controlMount = state.ControlMount;
                Player.controlJump = state.ControlJump;
            }
            else
            {
                // no input, so disable all controls
                Player.controlLeft = false;
                Player.controlRight = false;
                Player.controlUp = false;
                Player.controlDown = false;
                Player.controlUseItem = false;
                Player.controlHook = false;
                Player.controlMount = false;
                Player.controlJump = false;
            }
        }

        base.SetControls();
    }
}
