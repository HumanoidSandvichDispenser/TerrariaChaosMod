namespace TerrariaChaosMod.Content.Effects.ItemEffects;

public class LargeItemsEffect : Effect
{
    public override bool Update(Terraria.Player player)
    {
        foreach (var item in player.inventory)
        {
            if (!item.IsAir)
            {
                item.scale = 4f;
            }
        }
        return base.Update(player);
    }
}
