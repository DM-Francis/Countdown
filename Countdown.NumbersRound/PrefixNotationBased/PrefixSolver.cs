using Combinatorics.Collections;
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
            var expSolutions = GenerateAndTestExpressions(availableNums, exp => exp.Evaluate() == target);

            var stringSolutions = expSolutions.Select(exp => $"{string.Join(',', exp.RawExpression)} = {exp.Evaluate()}").ToList();

            return new SolveResult { ClosestDiff = 0, Solutions = stringSolutions };
        }

        private List<PrefixExpression> GenerateAndTestExpressions(List<int> availableNums, Func<PrefixExpression,bool> expressionFilter)
        {
            var expressionList = new List<PrefixExpression>();

            for (int n = 1; n <= availableNums.Count; n++)
            {
                var variations = new Variations<int>(availableNums, n, GenerateOption.WithoutRepetition);
                foreach (var variation in variations)
                {
                    foreach (var baseExp in GetPossibleTrees(n))
                    {
                        var availableNumStack = new Stack<int>(variation);
                        var newExpression = baseExp.RawExpression.ToList();
                        for (int i = 0; i < newExpression.Count; i++)
                        {
                            if (IsNumber(newExpression[i]))
                            {
                                newExpression[i] = availableNumStack.Pop().ToString();
                            }
                        }

                        var pfExp = new PrefixExpression(newExpression);
                        if (expressionFilter(pfExp))
                        {
                            expressionList.Add(pfExp);
                        }
                    }
                }
            }

            return expressionList;
        }

        private static bool IsNumber(string s)
        {
            return s == "1";
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
