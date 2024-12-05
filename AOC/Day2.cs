using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC;
internal class Day2
{
    public static bool IsSafe(List<int> nums, bool reverse = false, bool supportDampening = false)
    {
        int last = -1;
        int sign = 0;
        int errorCount = 0;
        int start = reverse ? nums.Count - 1 : 0;
        int end = reverse ? -1 : nums.Count;
        int step = reverse ? -1 : 1;

        last = nums[start];

        for (int i = start + step; i != end; i += step)
        {
            if (errorCount > 1) { break; }

            int n = nums[i];

            int diff = n - last;
            if (sign == 0)
            {
                sign = Math.Sign(diff);
            }
            else if (sign != Math.Sign(diff))
            {
                errorCount++;
                continue;
            }

            int absDiff = Math.Abs(diff);
            if (absDiff < 1 || absDiff > 3)
            {
                errorCount++;
                continue;
            }

            last = n;
        }

        return errorCount < (supportDampening ? 2 : 1);
    }

    public static void ReportToNums(string report, List<int> nums)
    {
        foreach (var num in report.AsSpan().Split(' '))
        {
            nums.Add(int.Parse(report[num]));
        }
    }

    public static void Run()
    {
        string[] lines = InputManager.GetInputLines(2, "input_part1.txt");
        int safeCount = 0;
        int totalCount = 0;
        int unsafeCount = 0;
        bool supportDampening = false;

        List<int> nums = new(8);

        foreach (var line in lines)
        {
            totalCount++;
            nums.Clear();

            foreach (var num in line.AsSpan().Split(' '))
            {
                nums.Add(int.Parse(line[num]));
            }

            if (IsSafe(nums, supportDampening: supportDampening) || 
               (supportDampening && IsSafe(nums, true, supportDampening)))
            {
                safeCount++;
            }
            else
            {
                unsafeCount++;
            }
        }

        Console.WriteLine(safeCount);
        Console.WriteLine(unsafeCount);
        Console.WriteLine(totalCount);

    }
}
