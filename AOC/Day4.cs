using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

namespace AOC;
public class Day4
{
    const int BATCHSIZE = 32;
    static char[] XMAS = ['X', 'M', 'A', 'S'];

    public readonly ref struct Position
    {
        public readonly int X; // I wrote this
        public readonly int Y; // Copilot wrote the rest

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public readonly Position Up => new(X, Y - 1);
        public readonly Position Down => new(X, Y + 1);
        public readonly Position Left => new(X - 1, Y);
        public readonly Position Right => new(X + 1, Y);
        public readonly Position UpLeft => new(X - 1, Y - 1);
        public readonly Position UpRight => new(X + 1, Y - 1);
        public readonly Position DownLeft => new(X - 1, Y + 1);
        public readonly Position DownRight => new(X + 1, Y + 1);

        public override int GetHashCode()
        {
            return (Y << 8) | X; // Except this, I typed "Y <<" and Copilot wrote the rest
        }
    }

    public struct Direction
    {
        public int X; // I wrote this
        public int Y; // Copilot wrote the rest

        public Direction(int x, int y)
        {
            X = x; Y = y;
        }

        public static Direction Up => new(0, -1);
        public static Direction Down => new(0, 1);
        public static Direction Left => new(-1, 0);
        public static Direction Right => new(1, 0);
        public static Direction UpLeft => new(-1, -1);
        public static Direction UpRight => new(1, -1);
        public static Direction DownLeft => new(-1, 1);
        public static Direction DownRight => new(1, 1);

        public static Direction[] AllDirections = new Direction[] { Up, Down, Left, Right, UpLeft, UpRight, DownLeft, DownRight };

        public static Position operator +(Position pos, Direction dir)
        {
            return new Position(pos.X + dir.X, pos.Y + dir.Y);
        }

        public static Position operator -(Position pos, Direction dir)
        {
            return new Position(pos.X - dir.X, pos.Y - dir.Y);
        }

        public static Direction operator -(Direction dir)
        {
            return new Direction(-dir.X, -dir.Y);
        }

        public static bool operator ==(Direction left, Direction right)
        {
            return left.X == right.X && left.Y == right.Y;
        }

        public static bool operator !=(Direction left, Direction right)
        {
            return left.X != right.X || left.Y != right.Y;
        }
    }

    public class SearchTable
    {
        public byte[] Table { get; }
        public int Width { get; }
        public int Height { get; }

        public SearchTable(byte[] table, int width)
        {
            Table = table;
            Width = width;
            Height = table.Length / width;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public bool InBounds(Position pos)
        {
            return !(pos.X < 0 || pos.X >= Width || pos.Y < 0 || pos.Y >= Height);
        }

        public byte this[Position pos]
        {
            get => InBounds(pos) ? Table[pos.Y * Width + pos.X] : (byte)0;
        }

        public byte this[int index]
        {
            get => Table[index];
        }
    }

    public static bool CheckX_MAS(SearchTable st, Position pos)
    {
        if (!(st.InBounds(pos.UpLeft) &&
            st.InBounds(pos.DownRight) &&
            st.InBounds(pos.UpRight) &&
            st.InBounds(pos.DownLeft)))
        {
            return false;
        }

        byte upLeft = st[pos.UpLeft];
        byte upRight = st[pos.UpRight];
        byte downLeft = st[pos.DownLeft];
        byte downRight = st[pos.DownRight];

        bool cross1 = (upLeft == 'M' && downRight == 'S') || (upLeft == 'S' && downRight == 'M');
        bool cross2 = (upRight == 'M' && downLeft == 'S') || (upRight == 'S' && downLeft == 'M');

        return cross1 && cross2;
    }

    const byte MS = 'M' ^ 'S';

    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    public static bool CheckX_MASOpt(SearchTable st, Position pos)
    {
        if ((st[pos.UpLeft] ^ st[pos.DownRight]) != MS) return false;
        if ((st[pos.UpRight] ^ st[pos.DownLeft]) != MS) return false;
        return true;
    }

    public static bool CheckXmas(SearchTable st, Position start, Position pos, Direction direction, int depth = 0)
    {
        if (depth == XMAS.Length)
        {
            return true; // st.MarkFound(start, pos);
        }
        if (!st.InBounds(pos))
        {
            return false;
        }
        if (st[pos] != XMAS[depth])
        {
            return false;
        }
        return CheckXmas(st, start, pos + direction, direction, depth + 1);
    }

    static Position FromIndex(int index, SearchTable st)
    {
        return new(index % st.Width, index / st.Width);
    }

    static Day4()
    {
        lines = InputManager.GetInputLines(4, "input_part1.txt");
        bytes = StringArrayToCharArray(lines);
    }

    static string[] lines;
    static byte[] bytes;
    static SearchTable st;


    [Benchmark]
    public int Part1()
    {
        int count = 0;
        st = new SearchTable(bytes, lines[0].Length);

        int index = 0;
        do
        {
            if (st[index] == 'X')
            {
                for (var i = 0; i < Day4.Direction.AllDirections.Length; i++)
                {
                    Position pos = FromIndex(index, st);
                    if (Day4.CheckXmas(st, pos, pos, Day4.Direction.AllDirections[i]))
                    {
                        Interlocked.Increment(ref count);
                    }
                }
            }
            index++;

        }
        while (index < st.Table.Length);

        return count;
    }

    [Benchmark]
    public int Part1Opt()
    {
        int count = 0;
        st = new SearchTable(bytes, lines[0].Length);

        int index = 0;
        do
        {
            if (st[index] == 'X')
            {
                for (var i = 0; i < Day4.Direction.AllDirections.Length; i++)
                {
                    Position pos = FromIndex(index, st);
                    if (Day4.CheckXmas(st, pos, pos + Day4.Direction.AllDirections[i], Day4.Direction.AllDirections[i], 1))
                    {
                        Interlocked.Increment(ref count);
                    }
                }
            }
            index++;

        }
        while (index < st.Table.Length);

        return count;
    }

    [Benchmark]
    public int Part1P()
    {
        int count = 0;
        st = new SearchTable(bytes, lines[0].Length);

        // Parallel.Foreach with partitioner
        var partitioner = Partitioner.Create(0, st.Width * st.Height - 1, st.Width * BATCHSIZE);

        Parallel.ForEach(partitioner, part =>
        {
            int start = part.Item1;
            int end = part.Item2;

            for (int p = start; p <= end; p++)
            {
                if (st[p] == 'X')
                {
                    for (var i = 0; i < Day4.Direction.AllDirections.Length; i++)
                    {
                        Position pos = FromIndex(p, st);
                        if (Day4.CheckXmas(st, pos, pos, Day4.Direction.AllDirections[i]))
                        {
                            Interlocked.Increment(ref count);
                        }
                    }
                }
            }
        });

        return count;
    }

    [Benchmark]
    public int Part1POpt()
    {
        int count = 0;
        st = new SearchTable(bytes, lines[0].Length);

        var partitioner = Partitioner.Create(0, st.Width * st.Height - 1, st.Width * BATCHSIZE);

        Parallel.ForEach(partitioner, part =>
        {
            int start = part.Item1;
            int end = part.Item2;

            for (int p = start; p <= end; p++)
            {
                if (st[p] == 'X')
                {
                    for (var i = 0; i < Day4.Direction.AllDirections.Length; i++)
                    {
                        Position pos = FromIndex(p, st);
                        if (Day4.CheckXmas(st, pos, pos + Day4.Direction.AllDirections[i], Day4.Direction.AllDirections[i], 1))
                        {
                            Interlocked.Increment(ref count);
                        }
                    }
                }
            }
        });

        return count;
    }

    [Benchmark]
    public int Part2()
    {
        int count = 0;
        st = new SearchTable(bytes, lines[0].Length);

        int index = st.Width;

        do
        {
            if (st[index] == 'A')
            {
                if (Day4.CheckX_MAS(st, FromIndex(index, st)))
                {
                    Interlocked.Increment(ref count);
                }
            }
            index++;
        }
        while (index < st.Table.Length - st.Width);

        return count;
    }

    static SearchValues<byte> SV_A = SearchValues.Create((byte)'A');

    [Benchmark]
    public int Part2Opt()
    {
        int count = 0;
        st = new SearchTable(bytes, lines[0].Length);

        ReadOnlySpan<byte> data = st.Table;
        data = data.Slice(st.Width, data.Length - st.Width);
        int offset = st.Width;

        var partitioner = Partitioner.Create(st.Width, st.Width * (st.Height - 1), st.Width * BATCHSIZE);

        Parallel.ForEach(partitioner, part =>
        {
            int start = part.Item1;
            int end = part.Item2;
            int row = start / st.Width;
            int rows = (end - start) / st.Width;

            for (int i = 0; i < rows; i++)
            {
                for (int p = start; p < start + st.Width; p++)
                {
                    if (st[p] == 'A')
                    {
                        if (Day4.CheckX_MASOpt(st, new Position(p - start, row + i)))
                        {
                            Interlocked.Increment(ref count);
                        }
                    }
                }
                start += st.Width;
            }
        });

        return count;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static byte[] StringArrayToCharArray(string[] input)
    {
        int width = input[0].Length;
        byte[] arr = new byte[input.Length * width];
        for (int i = 0; i < input.Length; i++)
        {
            Encoding.ASCII.GetBytes(input[i], arr.AsSpan(i * width));
        }
        return arr;
    }
}
