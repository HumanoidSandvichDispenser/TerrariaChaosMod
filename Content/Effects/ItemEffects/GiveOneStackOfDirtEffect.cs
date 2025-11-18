namespace TerrariaChaosMod.Content.Effects.ItemEffects;

public class GiveOneStackOfDirtEffect : BaseGiveItemEffect
{
    public override int ItemType => Terraria.ID.ItemID.DirtBlock;

    public override int ItemStack => 999;
}
