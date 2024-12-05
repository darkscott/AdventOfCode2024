namespace AOC.Tests;

using AOC;

[TestClass]
public sealed class Day2Tests
{
    [TestMethod]
    [DataRow("7 6 4 2 1", true)]
    [DataRow("1 2 7 8 9", false)]
    [DataRow("9 7 6 2 1", false)]
    [DataRow("1 3 2 4 5", false)]
    [DataRow("8 6 4 4 1", false)]
    [DataRow("1 3 6 7 9", true)]
    public void TestMethod1(string report, bool safe)
    {
        List<int> nums = new(8);
        Day2.ReportToNums(report, nums);
        Assert.AreEqual(safe, Day2.IsSafe(nums));
    }
}
