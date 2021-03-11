using System;
using System.Collections.Generic;
using System.Linq;

namespace RouteTesterGen
{
    public class RouteTesterGenerator
    {
        private Route _root;
        private HashSet<string> _statics;
        
        public RouteTesterGenerator(IEnumerable<string> routes)
        {
            _root = new Route();
            var statics = routes.Where(r => !r.Contains('{'));
            _statics = new HashSet<string>(statics, StringComparer.OrdinalIgnoreCase);

            foreach (var route in routes.Where(r => r.Contains('{')))
            {
                var parts = route.ToLowerInvariant()
                    .Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
                
                _root.Add(parts);
            }
        }

        public string Generate()
        {
            var code = new IndentedStringBuilder();
            code.AppendLine("using System;");
            code.AppendLine("using System.Collections.Generic;");
            code.AppendLine();
            code.AppendLine("namespace RouteSpike");
            using (code.OpenBrace())
            {
                code.AppendLine("public class RouteTester");
                using (code.OpenBrace())
                {
                    WriteHashSet(code);
                    code.AppendLine();
                    WriteIsMatch(code);
                }
            }

            return code.ToString();
        }

        private void WriteIsMatch(IndentedStringBuilder code)
        {
            code.AppendLine("public bool IsMatch(string path)");
            using (code.OpenBrace())
            {
                code.AppendLine("if (path == string.Empty) return false;");
                if (_statics.Count > 0)
                {
                    code.AppendLine("if (_statics.Contains(path)) return true;");
                }

                code.AppendLine("Span<char> span = stackalloc char[path.Length];");
                code.AppendLine("MemoryExtensions.ToLowerInvariant(path, span);");
                code.AppendLine("if (span[0] == '/' && span.Length > 1)");
                using (code.OpenBrace())
                {
                    code.AppendLine("span = span.Slice(1);");
                }
            }
        }

        private void WriteHashSet(IndentedStringBuilder code)
        {
            if (_statics.Count == 0) return;

            code.AppendLine("private readonly HashSet<string> _statics = new(StringComparer.OrdinalIgnoreCase)");
            using (code.OpenBrace())
            {
                foreach (var @static in _statics)
                {
                    code.AppendLine($@"""{@static}"",");
                }
            }
        }
    }
}
