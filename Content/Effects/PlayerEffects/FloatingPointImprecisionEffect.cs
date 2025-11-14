using System;
using Terraria;

namespace TerrariaChaosMod.Content.Effects.PlayerEffects;

public class FloatingPointImprecisionEffect : Effect
{
    private static float TruncateFloat(float value, int eBits, int mBits)
    {
        if (eBits <= 0 || eBits > 8 || mBits < 0 || mBits > 23)
            throw new ArgumentOutOfRangeException();

        // Interpret bits
        uint bits = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
        uint sign = bits >> 31;
        int exponent = (int)((bits >> 23) & 0xFF);
        uint mantissa = bits & 0x7FFFFF;

        // Handle zero, NaN, inf
        if (exponent == 0xFF)
            return value;

        // Original and new biases
        int oldBias = 127;
        int newBias = (1 << (eBits - 1)) - 1;

        // Rebias exponent
        int newExp = exponent - oldBias + newBias;

        // Clamp new exponent range
        int maxExp = (1 << eBits) - 2; // reserve all-ones for inf/NaN
        if (newExp <= 0)
        {
            // Underflow to subnormal or zero
            newExp = 0;
        }
        else if (newExp > maxExp)
        {
            // Overflow to infinity
            newExp = maxExp + 1;
            mantissa = 0;
        }

        // Truncate mantissa (keep high bits)
        if (mBits < 23)
        {
            int shift = 23 - mBits;
            mantissa = (mantissa >> shift) << shift;
        }

        // Re-expand exponent back to 8-bit range
        int restoredExp = newExp - newBias + oldBias;
        if (restoredExp <= 0) restoredExp = 0;
        if (restoredExp >= 0xFF) restoredExp = 0xFF;

        // Reassemble float
        uint newBits = (sign << 31) | ((uint)restoredExp << 23) | mantissa;
        return BitConverter.ToSingle(BitConverter.GetBytes(newBits), 0);
    }

    public override void PostUpdate(Player player)
    {
        // only leave 4 bits of mantissa for position and velocity values
        player.position.X = TruncateFloat(player.position.X, 7, 15);
        player.position.Y = TruncateFloat(player.position.Y, 7, 15);
        player.velocity.X = TruncateFloat(player.velocity.X, 7, 8);
        player.velocity.Y = TruncateFloat(player.velocity.Y, 7, 8);
        base.PostUpdate(player);
    }
}
