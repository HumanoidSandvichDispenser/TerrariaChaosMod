using Terraria;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content;

public class ChaosModGlobalNPC : GlobalNPC
{
    /// <summary>
    /// Semaphore to indicate whether Buttbot effect is active.
    /// </summary>
    public static int ButtbotSemaphore = 0;

    public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
    {
        var effectPlayer = player.GetModPlayer<Effects.PlayerEffects.EffectPlayer>();
        spawnRate = (int)(spawnRate / effectPlayer.SpawnRateMultiplier);
        maxSpawns = (int)(maxSpawns * effectPlayer.MaxSpawnsMultiplier);

        base.EditSpawnRate(player, ref spawnRate, ref maxSpawns);
    }

    public override void GetChat(NPC npc, ref string chat)
    {
        if (ButtbotSemaphore > 0)
        {
            // replace pseudorandom words with "butt"
            // if hash of each character of word modulo 5 is 0

            var words = chat.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                int hash = 17;
                foreach (char c in words[i])
                {
                    hash = hash * 31 + (c * i);
                }
                if (hash % 5 == 0)
                {
                    words[i] = "butt";
                }
            }

            chat = string.Join(' ', words);
        }
    }
}
