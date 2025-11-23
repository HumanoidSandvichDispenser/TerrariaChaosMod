using Terraria;
using Terraria.DataStructures;

namespace TerrariaChaosMod.Content.Effects.PlayerEffects;

public class KillPlayerEffect : Effect
{
    public override int Duration => Seconds(5);

    public override void ApplyEffect(Player player)
    {
        base.ApplyEffect(player);

        Main.NewText("You have been cursed by the Dog Tamer!", 255, 0, 0);

        After(Seconds(5), () => {
            player.KillMe(PlayerDeathReason.ByCustomReason($"{player.name} was smitten!"), 9999, 0);
        });
    }
}
