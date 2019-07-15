using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Countdown.NumbersRound.PrefixNotationBased
{
    public sealed class PrefixSolver : ISolver
    {
        private readonly Dictionary<int, List<PrefixExpression>> _expressionCache = new Dictionary<int, List<PrefixExpression>>();

        public SolveResult GetPossibleSolutions(int target, List<int> availableNums)
        {
            var expList = GenerateExpressions(5, availableNums);
            var solutions = new List<string>();
            
            foreach(var exp in expList)
            {
                solutions.Add($"{string.Join(',', exp.RawExpression)} = {exp.Evaluate()}");
            }

            return new SolveResult { ClosestDiff = 0, Solutions = solutions };
        }

        private List<PrefixExpression> GenerateExpressions(int N, List<int> availableNums)
        {
            var expressionList = new List<PrefixExpression>();

            for (int i = 1; i <= N; i++)
            {
                expressionList.AddRange(GetPossibleTrees(i));
            }

            return expressionList;
        }

        private IEnumerable<PrefixExpression> GetPossibleTrees(int N)
        {
            if (_expressionCache.ContainsKey(N))
            {
                _expressionCache.TryGetValue(N, out List<PrefixExpression> cacheResult);

                return cacheResult;
            }

            var resultList = new List<PrefixExpression>();

            if (N == 1)
            {
                resultList.Add(new PrefixExpression("1"));
                resultList.Add(new PrefixExpression("2"));
                resultList.Add(new PrefixExpression("3"));
                resultList.Add(new PrefixExpression("4"));
                resultList.Add(new PrefixExpression("5"));
                resultList.Add(new PrefixExpression("6"));
            }
            else
            {
                for (int x = 1; x < N; x++)
                {
                    foreach (var leftTree in GetPossibleTrees(x))
                    {
                        foreach (var rightTree in GetPossibleTrees(N - x))
                        {
                            var possibleExpressions = GetCombinedExpressions(leftTree, rightTree);
                            resultList.AddRange(possibleExpressions);
                        }
                    }
                }
            }

            _expressionCache.Add(N, resultList);

            return resultList;
        }

        private List<PrefixExpression> GetCombinedExpressions(PrefixExpression leftTree, PrefixExpression rightTree)
        {
            var outlist = new List<PrefixExpression>();

            foreach (var operation in PrefixExpression.Operations)
            {
                var input = new List<string>() { operation };
                input = input.Concat(leftTree.RawExpression).Concat(rightTree.RawExpression).ToList();
                outlist.Add(new PrefixExpression(input));
            }

            return outlist;
        }
    }
}
