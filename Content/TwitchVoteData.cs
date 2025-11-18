using System.Text.Json;

namespace TerrariaChaosMod.Content;

public struct TwitchVoteData
{
    public record VoteOption(int Number, string Name, float Proportion);

    public int TotalVotes { get; set; }

    public VoteOption[] Options { get; set; }

    public bool NewVoteStarted { get; set; }

    public TwitchVoteData(int totalVotes, VoteOption[] options, bool newVoteStarted)
    {
        TotalVotes = totalVotes;
        Options = options;
        NewVoteStarted = newVoteStarted;
    }
}
