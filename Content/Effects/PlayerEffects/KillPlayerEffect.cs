using Terraria;
using Terraria.DataStructures;

namespace TerrariaChaosMod.Content.Effects.PlayerEffects;

public class KillPlayerEffect : Effect
{
    public override string Name => "Kill Player";

    public override int Duration => Seconds(5);

    public override void ApplyEffect(Player player)
    {
        base.ApplyEffect(player);

        Main.NewText("You have been cursed to die in 5 seconds!", 255, 0, 0);

        After(Seconds(5), () => {
            player.KillMe(PlayerDeathReason.ByCustomReason($"{player.name} was killed by chaos!"), 9999, 0);
        });
    }
}
