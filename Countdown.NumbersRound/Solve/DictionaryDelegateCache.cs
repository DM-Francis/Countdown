using Countdown.NumbersRound.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Countdown.NumbersRound.Solve
{
    public class DictionaryDelegateCache : IDelegateCache
    {
        private readonly Dictionary<int, List<DelegateExpressionPair>> _cache = new Dictionary<int, List<DelegateExpressionPair>>();

        public void Add(int N, List<DelegateExpressionPair> delegateExpressions)
        {
            _cache[N] = delegateExpressions;
        }

        public bool TryGetValue(int N, out List<DelegateExpressionPair> delegateExpressions)
        {
            return _cache.TryGetValue(N, out delegateExpressions);
        }
    }
}
