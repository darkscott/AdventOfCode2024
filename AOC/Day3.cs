using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC;

internal struct MulInstruction
{
    public int Left { get; set; }
    public int Right { get; set; }
}

internal partial class Day3
{
    public static Regex MulRegex = MulRegexGenerated();

    public static List<MulInstruction> CollectMulInstructions(string input, bool ignoreDoRanges = false)
    {
        const int DO_LEN = 4, DONT_LEN = 7, MUL_LEN = 4;
        Span<Range> ranges = stackalloc Range[2];
        var span = input.AsSpan();
        bool inDoRange = true;
        List<MulInstruction> instructions = new(1024);

        foreach (var match in MulRegex.EnumerateMatches(span))
        {
            if (match.Length == DO_LEN) { inDoRange = true; continue; }
            if (match.Length == DONT_LEN) { inDoRange = false; continue; }

            if (inDoRange || ignoreDoRanges)
            {
                Range r = new(match.Index + MUL_LEN, match.Index + match.Length - 1); // trim off mul( ... )
                var numPairSpan = span[r];
                Debug.Assert(numPairSpan.Split(ranges, ',') == 2);
                instructions.Add(new MulInstruction { Left = int.Parse(numPairSpan[ranges[0]]), Right = int.Parse(numPairSpan[ranges[1]]) });
            }
        }

        return instructions;

    }

    public static void Run()
    {
        string input = InputManager.GetInput(3, "input_part1.txt");
        var instructions = CollectMulInstructions(input, true);

        Console.WriteLine(instructions.Count);
        Console.WriteLine(instructions.Sum(x => x.Left * x.Right));

    }

    [GeneratedRegex(@"(do\(\)|don't\(\)|mul\(\d{1,3},\d{1,3}\))")]
    private static partial Regex MulRegexGenerated();
}
