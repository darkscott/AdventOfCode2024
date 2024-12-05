// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet;
using AOC;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;

// Day1.Run();
//Day2.Run();


//DoPart1();
//DoPart2();

// Benchmark it
BenchmarkDotNet.Configs.ManualConfig config = new();
config.AddLogger(BenchmarkDotNet.Loggers.ConsoleLogger.Default);
config.AddExporter(BenchmarkDotNet.Exporters.DefaultExporters.AsciiDoc);
config.AddDiagnoser(BenchmarkDotNet.Diagnosers.MemoryDiagnoser.Default);
config.AddColumnProvider(BenchmarkDotNet.Columns.DefaultColumnProviders.Instance);

BenchmarkRunner.Run<Day4>(config);

void DoPart1()
{
    Day4 d4 = new();
    var count = d4.Part1();
    var count2 = d4.Part1Opt();

    var count4 = d4.Part1P();
    var count3 = d4.Part1POpt();

    Console.WriteLine(count == count2);
    Console.WriteLine(count == count3);
    Console.WriteLine(count == count4);

    Console.WriteLine(count == 2517);
    Console.WriteLine(count);
    Console.WriteLine(count2);
    Console.WriteLine(count3);
    Console.WriteLine(count4);
}

void DoPart2()
{
    Day4 d4 = new();
    var count = d4.Part2();
    var count2 = d4.Part2Opt();

    Console.WriteLine(count == 1960);
    Console.WriteLine(count == count2);
    Console.WriteLine(count);
    Console.WriteLine(count2);
}