using Terraria;

namespace TerrariaChaosMod.Content.Effects.ItemEffects;

public class SwitchToRandomLoadoutEffect : Effect
{
    public override void ApplyEffect(Player player)
    {
        int index;
        do
        {
            index = Main.rand.Next(player.Loadouts.Length);
        }
        while (index == player.CurrentLoadoutIndex);

        player.TrySwitchingLoadout(index);

        base.ApplyEffect(player);
    }
}
