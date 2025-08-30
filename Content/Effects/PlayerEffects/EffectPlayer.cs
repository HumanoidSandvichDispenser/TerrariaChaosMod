using Terraria;
using Terraria.ModLoader;

namespace TerrariaChaosMod.Content.Effects.PlayerEffects;

public class EffectPlayer : ModPlayer
{
    public int HealValueScale = 1;

    public float Zoom = -1;

    public override void ResetEffects()
    {
        HealValueScale = 1;
        Zoom = -1;
    }

    public override void GetHealLife(Item item, bool quickHeal, ref int healValue)
    {
        healValue = (int)(healValue * HealValueScale);
    }

    public override void ModifyZoom(ref float zoom)
    {
        zoom = Zoom;
    }
}
