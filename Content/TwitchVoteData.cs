using System.Text.Json;

namespace TerrariaChaosMod.Content;

public struct TwitchVoteData
{
    public record VoteOption(string Name, float Proportion);

    public VoteOption[] Options { get; set; }

    public TwitchVoteData(VoteOption[] options)
    {
        Options = options;
    }
}
