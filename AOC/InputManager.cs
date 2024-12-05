using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC;
internal class InputManager
{
    public static string GetInput(int day, string filename)
    {
        string path = Path.Combine(Environment.GetEnvironmentVariable("INPUT_FILES_ROOT"), $"Day{day}/{filename}");
        return File.ReadAllText(path);
    }

    public static string[] GetInputLines(int day, string filename)
    {
        string path = Path.Combine(Environment.GetEnvironmentVariable("INPUT_FILES_ROOT"), $"Day{day}/{filename}");
        return File.ReadAllLines(path);
    }
}
