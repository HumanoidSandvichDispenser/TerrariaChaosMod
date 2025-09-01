using Terraria.ID;

namespace TerrariaChaosMod.Content.Effects.ItemEffects;

public class RichesToRagsEffect : RagsToRichesEffect
{
    protected override int GetNewItemId(int oldItemId)
    {
        switch (oldItemId)
        {
            case ItemID.SilverCoin:
            case ItemID.GoldCoin:
            case ItemID.PlatinumCoin:
                return ItemID.CopperCoin;
            default:
                return -1;
        }
    }
}
