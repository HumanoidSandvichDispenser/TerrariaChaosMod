using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrariaChaosMod.Content.NPCs;

public class DogTamerNPC : ModNPC
{
    private int _shootTimer;

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = 25;

        NPCID.Sets.ExtraFramesCount[Type] = 9;
        NPCID.Sets.AttackFrameCount[Type] = 4;
        NPCID.Sets.DangerDetectRange[Type] = 700;
        NPCID.Sets.AttackType[Type] = 1;
        NPCID.Sets.AttackTime[Type] = 5;
        NPCID.Sets.AttackAverageChance[Type] = 2;
    }

    public override void SetDefaults()
    {
        NPC.width = 18;
        NPC.height = 40;
        NPC.damage = 0;
        NPC.defense = short.MaxValue;
        NPC.lifeMax = short.MaxValue;
        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath1;
        NPC.knockBackResist = 0.5f;
        NPC.aiStyle = NPCAIStyleID.Fighter;
        AIType = NPCID.PirateDeadeye;

        NPC.GivenName = "God Gamer the Dog Tamer";
        NPC.friendly = false;
        NPC.townNPC = false;
    }

    public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
    {
        SetHealth();
    }

    public static void SetHealthOfAll()
    {
        for (int i = 0; i < Main.maxNPCs; i++)
        {
            NPC npc = Main.npc[i];
            if (npc.active && npc.type == ModContent.NPCType<NPCs.DogTamerNPC>())
            {
                ((DogTamerNPC)npc.ModNPC).SetHealth();
            }
        }
    }

    public void SetHealth()
    {
        int totalPlayerHealth = 0;
        int totalPlayerDefense = 0;

        for (int i = 0; i < Main.maxPlayers; i++)
        {
            Player player = Main.player[i];
            if (player.active)
            {
                totalPlayerHealth += player.statLifeMax2;
                totalPlayerDefense += player.statDefense;
            }
        }

        NPC.lifeMax = totalPlayerHealth * 50;
        NPC.life = totalPlayerHealth * 50;
        NPC.defense = totalPlayerDefense * 5;

        NPC.netUpdate = true;
    }

    public override void AI()
    {
        Player target = Main.player[NPC.target];

        NPC.TargetClosest(true);

        _shootTimer--;

        if (_shootTimer <= 0)
        {
            if (target.active && !target.dead)
            {
                if (target.Center.X < NPC.Center.X)
                {
                    NPC.direction = -1;
                    NPC.spriteDirection = -1;
                }
                else
                {
                    NPC.direction = 1;
                    NPC.spriteDirection = 1;
                }

                Vector2 displacement = target.Center - NPC.Center;
                float distance = displacement.Length();
                Vector2 direction = displacement
                    .SafeNormalize(Vector2.UnitX) * 10f;

                if (distance < 300f)
                {
                    Shoot(direction, ProjectileID.BulletDeadeye, 30, 8);
                }
                else if (distance < 700f)
                {
                    Shoot(direction, ProjectileID.Fireball, 400, 120);
                }
            }
        }
    }

    private void Shoot(Vector2 direction, int projectileId, int damage, int timer)
    {
        int index = Projectile.NewProjectile(
            NPC.GetSource_FromAI(),
            NPC.Center,
            direction,
            projectileId,
            Damage: 400,
            KnockBack: 1f,
            Owner: Main.myPlayer
        );

        Projectile projectile = Main.projectile[index];
        projectile.npcProj = true;
        projectile.friendly = false;
        projectile.hostile = true;
        projectile.usesLocalNPCImmunity = true;
        projectile.localNPCImmunity[NPC.whoAmI] = 10;

        _shootTimer = timer;
    }

    public override void FindFrame(int frameHeight)
    {
        // Determine if walking
        bool walking = Math.Abs(NPC.velocity.X) > 0.2f;

        if (NPC.IsABestiaryIconDummy)
        {
            // Bestiary idle animation
            NPC.frameCounter++;
            if (NPC.frameCounter >= 6)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y >= frameHeight * 17)
                    NPC.frame.Y = 0;
            }
            return;
        }

        // Attack frame logic: Fighter NPCs set "NPC.localAI[0]" when attacking
        bool attacking = NPC.localAI[0] > 0f;

        if (attacking)
        {
            // play attack animation frames 18–24
            NPC.frameCounter++;
            if (NPC.frameCounter >= 5)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y++;

                if (NPC.frame.Y < 18 * frameHeight)
                    NPC.frame.Y = 18 * frameHeight;

                if (NPC.frame.Y > 24 * frameHeight)
                    NPC.frame.Y = 18 * frameHeight; // loop attack frames
            }

            return;
        }

        // Walking animation 0–17
        if (walking)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= 5)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;

                if (NPC.frame.Y < 0)
                    NPC.frame.Y = 0;

                if (NPC.frame.Y >= frameHeight * 18)
                    NPC.frame.Y = 0; // loop 0–17
            }
        }
        else
        {
            // idle frame
            NPC.frameCounter = 0;
            NPC.frame.Y = 0;
        }
    }

    // always save
    public override bool NeedSaving() => true;
}
