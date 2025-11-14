using Terraria;

namespace TerrariaChaosMod.Content.Effects.NPCEffects;

public class ButtbotEffect : Effect
{
    public override int Duration => Seconds(60) * 3;

    public override void ApplyEffect(Player player)
    {
        ChaosModGlobalNPC.Buttbot.Acquire(true);
        base.ApplyEffect(player);
    }

    public override void CleanUp(Player player)
    {
        ChaosModGlobalNPC.Buttbot.Release();
        base.CleanUp(player);
    }
}
