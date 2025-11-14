using Terraria;

namespace TerrariaChaosMod.Content.Effects.NPCEffects;

public class ButtbotEffect : Effect
{
    public override int Duration => Seconds(60) * 3;

    public override void ApplyEffect(Player player)
    {
        AcquireLock(true);
        base.ApplyEffect(player);
    }

    public override void CleanUp(Player player)
    {
        ReleaseLock();
        base.CleanUp(player);
    }
}
