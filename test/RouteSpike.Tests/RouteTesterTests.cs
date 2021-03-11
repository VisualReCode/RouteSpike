using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RouteSpike.Tests
{
    public class RouteTesterTests
    {
        [Theory]
        [MemberData(nameof(Paths))]
        public void MatchesRoutes(string path)
        {
            Assert.True(new RouteTester().IsMatch(path));
        }

        public static IEnumerable<object[]> Paths()
        {
            return PathStrings.Select(v => new object[] {v});
        }

        private static readonly string[] PathStrings =
        {
            "/Posts",
            "/posts",
            "/Posts/42",
            "/pOSTS/42",
            "/posts/42/edit",
            "/posts/new",
            "/people",
            "/people/23",
            "/files/foo/bar/quux.pdf"
        };
    }
}
