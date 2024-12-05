using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC;
internal class Day1
{
    public static void Run()
    {
        string[] lines = InputManager.GetInputLines(1, "input_part1.txt");

        List<int> leftList = new(1024);
        List<int> rightList = new(1024);

        int[] rightCounts = new int[100000];

        Span<Range> ranges = stackalloc Range[2];

        foreach (var line in lines)
        {
            line.AsSpan().Split(ranges, ' ');
            leftList.Add(int.Parse(line[ranges[0]]));
            int right = int.Parse(line[ranges[1]]);
            rightCounts[right]++;
            rightList.Add(right);
        }

        leftList.Sort();
        rightList.Sort();

        int distSum = 0;
        long similarity = 0;

        for (int i = 0; i < leftList.Count; i++)
        {
            distSum += Math.Abs(rightList[i] - leftList[i]);
            similarity += leftList[i] * rightCounts[leftList[i]];
        }

        Console.WriteLine(distSum);
        Console.WriteLine(similarity);
    }
}
