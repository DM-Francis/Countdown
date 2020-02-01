using Countdown.NumbersRound.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Countdown.NumbersRound.Solve
{
    public interface IDelegateCache
    {
        bool TryGetValue(int N, out List<DelegateExpressionPair> delegateExpressions);
        void Add(int N, List<DelegateExpressionPair> delegateExpressions);
    }
}
