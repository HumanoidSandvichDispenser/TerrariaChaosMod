using Terraria;

namespace TerrariaChaosMod.Content.Effects.NPCEffects;

public class ButtbotEffect : Effect
{
    public override void ApplyEffect(Player player)
    {
        ChaosModGlobalNPC.ButtbotSemaphore++;
        base.ApplyEffect(player);
    }

    public override void CleanUp(Player player)
    {
        ChaosModGlobalNPC.ButtbotSemaphore--;
        base.CleanUp(player);
    }
}
