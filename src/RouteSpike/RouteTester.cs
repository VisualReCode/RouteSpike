using System;
using System.Collections.Generic;

namespace RouteSpike
{
    public class RouteTester
    {
        private readonly HashSet<string> _staticPaths = new(StringComparer.OrdinalIgnoreCase)
        {
            "/posts",
            "/posts/new",
            "/people",
        };
        
        public bool IsMatch(string path)
        {
            if (path == string.Empty) return false;
            if (_staticPaths.Contains(path)) return true;

            Span<char> span = stackalloc char[path.Length];

            MemoryExtensions.ToLowerInvariant(path, span);

            if (span[0] == '/' && span.Length > 1)
            {
                span = span.Slice(1);
            }

            switch (span[0])
            {
                case 'p':
                    span = span.Slice(1);
                    if (span.Length == 0) return false;
                    switch (span[0])
                    {
                        case 'o':
                        {
                            var rest = ToNextSlash(ref span);
                            if (span.Length == 0) return false;
                            if (rest.SequenceEqual("osts"))
                            {
                                ToNextSlash(ref span);
                                if (span.Length == 0)
                                {
                                    return true;
                                }

                                rest = ToNextSlash(ref span);
                                if (rest.SequenceEqual("edit"))
                                {
                                    return span.Length == 0;
                                }

                                return false;
                            }
                            break;
                        }
                        case 'e':
                        {
                            var rest = ToNextSlash(ref span);
                            if (span.Length == 0) return false;
                            if (rest.SequenceEqual("eople"))
                            {
                                ToNextSlash(ref span);
                                if (span.Length == 0)
                                {
                                    return true;
                                }
                                return false;
                            }
                            return false;
                        }
                    }
                    return false;
                case 'f':
                    {
                        var rest = ToNextSlash(ref span);
                        if (rest.SequenceEqual("iles"))
                        {
                            return span.Length > 0;
                        }

                        return false;
                    }
                default:
                    return false;
            }
        }
        
        /*
            "posts/{id}"
            "posts/{id}/edit",
            "people/{id}",
            "files/{*path}"
         */

        private static Span<char> ToNextSlash(ref Span<char> span)
        {
            var index = span.IndexOf('/');
            if (index > 0)
            {
                var ret = span.Slice(0, index);
                span = span.Slice(index + 1);
                return ret;
            }
            else
            {
                var ret = span;
                span = Array.Empty<char>().AsSpan();
                return ret;
            }
        }
    }
}
