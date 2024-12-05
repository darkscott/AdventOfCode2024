namespace AOC.Tests;

using AOC;

[TestClass]
public sealed class Day4Tests
{
    static string[] input = [
                    "MMMSXXMASM",
                    "MSAMXMSMSA",
                    "AMXSXMAAMM",
                    "MSAMASMSMX",
                    "XMASAMXAMM",
                    "XXAMMXXAMA",
                    "SMSMSASXSS",
                    "SAXAMASAAA",
                    "MAMMMXMMMM",
                    "MXMXAXMASX"
        ];

    //[TestMethod]
    //public void CountXmas()
    //{
    //    Day4.SearchTable st = new(Day4.StringArrayToCharArray(input), input[0].Length);
    //    int count = 0;

    //    foreach (var pos in st.Positions())
    //    {
    //        if (st[pos] == 'X')
    //        {
    //            foreach (var dir in Day4.Direction.AllDirections)
    //            {
    //                if (Day4.CheckXmas(st, pos, pos, dir))
    //                {
    //                    count++;
    //                }
    //            }
    //        }
    //    }

    //    Assert.AreEqual(18, count);
    //}

    //[TestMethod]
    //public void CountX_MAS()
    //{
    //    Day4.SearchTable st = new(Day4.StringArrayToCharArray(input), input[0].Length);
    //    int count = 0;

    //    foreach (var pos in st.Positions())
    //    {
    //        if (st[pos] == 'A')
    //        {
    //            if (Day4.CheckX_MAS(st, pos))
    //            {
    //                count++;
    //            }
    //        }
    //    }

    //    Assert.AreEqual(9, count);
    //}
}
