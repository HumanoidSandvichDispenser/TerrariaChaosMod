using System.Text.Json;

namespace TerrariaChaosMod.Content;

public struct TwitchVoteData
{
    public record VoteOption(int Number, string Name, float Proportion);

    public VoteOption[] Options { get; set; }

    public bool NewVoteStarted { get; set; }

    public TwitchVoteData(VoteOption[] options, bool newVoteStarted)
    {
        Options = options;
        NewVoteStarted = newVoteStarted;
    }
}
