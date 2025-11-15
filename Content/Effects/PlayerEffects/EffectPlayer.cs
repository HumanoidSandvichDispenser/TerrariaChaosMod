using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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

    public bool AfkMode = false;

    public float SpawnRateMultiplier = 1f;

    public float MaxSpawnsMultiplier = 1f;

    public bool InsaneKnockback = false;

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
        AfkMode = false;
        SpawnRateMultiplier = 1f;
        MaxSpawnsMultiplier = 1f;
        InsaneKnockback = false;
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
            Terraria.Main.NewText("No-Hit Run Failed!", 255, 0, 0);
            modifiers.SourceDamage.Flat += Player.statLifeMax2;
            modifiers.FinalDamage.Flat += Player.statLifeMax2;
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

    public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (InsaneKnockback)
        {
            // push the player in the opposite direction with increased force
            int hitDirection = hit.HitDirection;
            float knockback = hit.Knockback;
            Player.velocity.X -= hitDirection * knockback * 3f;
        }
    }

    public override void ModifyWeaponKnockback(Item item, ref StatModifier knockback)
    {
        if (InsaneKnockback)
        {
            knockback += 512f;
            knockback.Flat += 512f;
        }
    }

    public override void Kill(
        double damage,
        int hitDirection,
        bool pvp,
        Terraria.DataStructures.PlayerDeathReason damageSource)
    {
        if (EffectLock.Of<TemporaryMediumcoreEffect>().IsAcquired)
        {
            // drop all items

            Player.DropItems();
            Player.DropCoins();
        }
    }

    public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        if (EffectLock.Of<ItemEffects.ProjectileRouletteEffect>().IsAcquired)
        {
            type = ItemEffects.ProjectileRouletteEffect.NextProjectile();
        }

        if (EffectLock.Of<ProjectileDysfunctionEffect>().IsAcquired)
        {
            // randomize velocity direction and magnitude
            float speed = velocity.Length();
            float randomAngle = Main.rand.NextFloat(0, MathHelper.TwoPi);
            float randomSpeed = Main.rand.NextFloat(0.1f * speed, 1.5f * speed);
            velocity = new Vector2(
                randomSpeed * (float)System.Math.Cos(randomAngle),
                randomSpeed * (float)System.Math.Sin(randomAngle));
        }

        if (EffectLock.Of<ItemEffects.BeeYourselfEffect>().IsAcquired)
        {
            // change projectile type to bee projectile
            type = ProjectileID.Bee;
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

        if (AfkMode)
        {
            // disable all controls
            Player.controlLeft = false;
            Player.controlRight = false;
            Player.controlUp = false;
            Player.controlDown = false;
            Player.controlUseItem = false;
            Player.controlUseTile = false;
            Player.controlHook = false;
            Player.controlMount = false;
            Player.controlJump = false;
            Player.controlQuickHeal = false;
            Player.controlQuickMana = false;
        }

        base.SetControls();
    }
}

public static class PlayerExtensions
{
    public static EffectPlayer GetEffectPlayer(this Player player)
    {
        return player.GetModPlayer<EffectPlayer>();
    }
}
