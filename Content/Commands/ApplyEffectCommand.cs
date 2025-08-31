using Microsoft.Xna.Framework;

using Terraria.ModLoader;

namespace TerrariaChaosMod.Content.Commands;

public class ApplyEffectCommand : ModCommand
{
    public override CommandType Type => CommandType.Chat;

    public override string Command => "applyeffect";

    public override string Description => "Applies an effect to yourself, randomly if no effect name is given.";

    public override bool IsCaseSensitive => true;

    public override void Action(CommandCaller caller, string input, string[] args)
    {
        var player = caller.Player;
        var effectSystem = ModContent.GetInstance<ChaosEffectsSystem>();

        if (args.Length == 0)
        {
            var effect = effectSystem.RandomEffectProvider
                .GetEffect()
                .Clone() as Effects.Effect;

            caller.Reply($"Applied {effect.Name}", Color.Teal);
            effectSystem.EffectsToApply.Enqueue(effect);
        }
        else
        {
            string effectName = args[0];
            var dict = effectSystem.EffectDictionary;

            if (dict.TryGetValue(effectName, out var foundEffect))
            {
                var effect = foundEffect.Clone() as Effects.Effect;
                effectSystem.EffectsToApply.Enqueue(effect);
                caller.Reply($"Applied {effect.Name}", Color.Teal);
            }
            else
            {
                caller.Reply($"Effect \"{effectName}\" not found", Color.Red);
            }
        }
    }
}
