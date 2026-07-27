using System.Collections.Concurrent;

namespace CricInfo.API.Memory;

public static class MatchBallStore
{
    public static ConcurrentDictionary<int, List<string>> FirstInningsBalls
        = new();

    public static ConcurrentDictionary<int, List<string>> SecondInningsBalls
        = new();
}