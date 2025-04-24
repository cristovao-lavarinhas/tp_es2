using System;
using System.Collections.Generic;
using esii.Models;

namespace esii.stratagies
{
    public class LoginContext
    {
        private readonly Dictionary<string, ILoginStrategy> _strategies;

        public LoginContext(Dictionary<string, ILoginStrategy> strategies)
        {
            _strategies = strategies;
        }

        public ILoginStrategy? GetStrategy(string method)
        {
            _strategies.TryGetValue(method.ToLower(), out var strategy);
            return strategy;
        }
    }
}