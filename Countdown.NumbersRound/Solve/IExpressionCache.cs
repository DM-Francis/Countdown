using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Countdown.NumbersRound.Solve
{
    public interface IExpressionCache
    {
        bool TryGetValue(int N, [NotNullWhen(true)] out IEnumerable<Expression>? expressions);
        void Add(int N, IEnumerable<Expression> expressions);
    }
}