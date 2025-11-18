namespace TerrariaChaosMod.Content.Effects.ItemEffects;

public class GiveOneTorchEffect : BaseGiveItemEffect
{
    public override int ItemType => Terraria.ID.ItemID.Torch;

    public override int ItemStack => 1;
}
