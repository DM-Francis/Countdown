using Countdown.NumbersRound.Expressions;
using Countdown.NumbersRound.Solve;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Countdown.Website.Caching
{
    public class DelegateMemoryCache : IDelegateCache
    {
        private readonly IMemoryCache _memoryCache;

        public DelegateMemoryCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void Add(int N, List<DelegateExpressionPair> delegateExpressions)
        {
            _memoryCache.Set(N, delegateExpressions);
        }

        public bool TryGetValue(int N, out List<DelegateExpressionPair> delegateExpressions)
        {
            return _memoryCache.TryGetValue(N, out delegateExpressions);
        }
    }
}
