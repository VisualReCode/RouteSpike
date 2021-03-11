using System;
using BenchmarkDotNet.Running;

namespace RouteSpikeBenchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<RouteBenchmarks>();
        }
    }
}
