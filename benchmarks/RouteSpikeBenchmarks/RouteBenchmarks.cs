using BenchmarkDotNet.Attributes;
using RouteSpike;

namespace RouteSpikeBenchmarks
{
    [MemoryDiagnoser]
    public class RouteBenchmarks
    {
        private static readonly BlazorRoutes BlazorRoutes = new BlazorRoutes(new[]
        {
            "posts/{id}",
            "posts/{id}/edit",
            "people/{id}",
            "files/{*path}",
            "/posts",
            "/posts/new",
            "/people",
        });

        private static readonly RouteTester RouteTester = new();
        
        [Benchmark(Baseline = true)]
        public bool Existing()
        {
            var target = BlazorRoutes;
            var a = target.Contains("/posts/42/edit");
            var b = target.Contains("/people/23");
            var c = target.Contains("/posts");
            var d = target.Contains("/files/foo/bar/quux.pdf");
            return a && b && c && d;
        }
        
        [Benchmark]
        public bool New()
        {
            var target = RouteTester;
            var a = target.IsMatch("/posts/42/edit");
            var b = target.IsMatch("/people/23");
            var c = target.IsMatch("/posts");
            var d = target.IsMatch("/files/foo/bar/quux.pdf");
            return a && b && c && d;
        }
    }
}