using Microsoft.Xna.Framework;
using Terraria;

namespace TerrariaChaosMod.Content.Effects;

public partial class SlingshotEffect : Effect
{
    private Vector2 _accumulatedVelocity;

    private float _speed => (float)System.Math.Round(_accumulatedVelocity.Length());

    public override int Duration => Seconds(10);

    public override string StatusText
    {
        get => $"{DisplayName.Value} ({TimeLeft / 60}s, Speed: {_speed} u/s)";
    }

    public override bool Update(Player player)
    {
        _accumulatedVelocity += player.velocity;
        player.velocity = Vector2.Zero;
        return base.Update(player);
    }

    public override void CleanUp(Player player)
    {
        player.velocity += _accumulatedVelocity;
    }
}
