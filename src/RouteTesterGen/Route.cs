using System;
using System.Collections.Generic;

namespace RouteTesterGen
{
    internal class Route
    {
        private readonly Dictionary<string, Route> _routes = new();
        
        public bool IsEnd { get; private set; }
        public bool IsWildcard { get; private set; }

        public void Add(Span<string> parts)
        {
            if (parts.Length == 0)
            {
                IsEnd = true;
                return;
            }
            
            var part = parts[0];

            if (part[0] == '{')
            {
                if (part.Length > 1 && part[1] == '*')
                {
                    IsWildcard = true;
                    return;
                }

                part = "*";
            }
            
            if (!_routes.TryGetValue(part, out var route))
            {
                route = new Route();
            }
            route.Add(parts.Slice(1));
        }
    }
}