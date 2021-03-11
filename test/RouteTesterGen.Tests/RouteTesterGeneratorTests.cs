using System;
using Xunit;

namespace RouteTesterGen.Tests
{
    public class RouteTesterGeneratorTests
    {
        [Fact]
        public void GeneratesCorrectCode()
        {
            var routes = new[]
            {
                "/posts",
                "/posts/{id}"
            };
            var target = new RouteTesterGenerator(routes);
            var actual = target.Generate();
            
            Assert.Equal(Expected, actual);
        }

        private const string Expected = @"using System;
using System.Collections.Generic;

namespace RouteSpike
{
    public class RouteTester
    {
        private readonly HashSet<string> _statics = new(StringComparer.OrdinalIgnoreCase)
        {
            ""/posts"",
        }

        public bool IsMatch(string path)
        {
            if (path == string.Empty) return false;
            if (_statics.Contains(path)) return true;
            Span<char> span = stackalloc char[path.Length];
            MemoryExtensions.ToLowerInvariant(path, span);
            if (span[0] == '/' && span.Length > 1)
            {
                span = span.Slice(1);
            }
        }
    }
}
";
    }
}
