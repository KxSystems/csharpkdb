using BenchmarkDotNet.Running;

namespace kx.Benchmark.Test
{
    static class Program
    {
        static void Main(string[] args)
        {
            var switcher = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly);
            switcher.Run(args);
        }
    }
}
