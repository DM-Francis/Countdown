using Combinatorics.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Countdown.NumbersRound.Solve
{
    public class Solver2
    {
        private readonly Dictionary<NumberCombination, int> _previouslyCheckedCombinations = new Dictionary<NumberCombination, int>();
        private readonly int _target;
        private readonly List<int> _availableNums;
        private List<Solution> _solutions = new List<Solution>();
        private bool _isSolved = false;
        private int _closestDiff;

        public Solver2(int target, List<int> availableNums)
        {
            _target = target;
            _availableNums = availableNums;
            _closestDiff = target;
        }

        public SolveResult Solve()
        {
            if (_isSolved)
                throw new InvalidOperationException("Already solved.");


            var availableNumbers = _availableNums.Select(n => (double)n).ToList();
            var solutionStrings = new List<string>();

            if (availableNumbers.Contains(_target))
            {
                _closestDiff = 0;
                solutionStrings.Add($"{_target} = {_target}");
            }

            CheckNumbersForSolution(availableNumbers, _target, new List<Operation>());
            DedupeSolutions();
            _isSolved = true;

            foreach(var solution in _solutions)
            {
                solutionStrings.Add(solution.ToString());
            }

            return new SolveResult(_closestDiff, solutionStrings);
        }

        private void CheckNumbersForSolution(List<double> numbers, double target, List<Operation> previousOperations)
        {
            if (numbers.Count == 1)
                return;

            var combination = new NumberCombination(numbers);

            if (_previouslyCheckedCombinations.TryGetValue(combination, out int opCount) && opCount <= previousOperations.Count)
                return;

            _previouslyCheckedCombinations[combination] = previousOperations.Count;

            foreach(var pair in new Variations<double>(numbers, 2, GenerateOption.WithoutRepetition))
            {
                if (pair[0] < pair[1])
                    continue;

                foreach(var @operator in Operator.All)
                {
                    double newNum = @operator.Evaluate(pair[0], pair[1]);

                    if (newNum % 1 != 0 || newNum < 0 || newNum == pair[0] || newNum == pair[1])
                        continue;

                    double newDiff = Math.Abs(newNum - target);
                    if (newDiff <= _closestDiff)
                    {
                        if (newDiff < _closestDiff)
                        {
                            _solutions.Clear();
                            _closestDiff = (int)newDiff;
                        }

                        var solutionOperationsList = new List<Operation>(previousOperations);
                        solutionOperationsList.Add(new Operation(@operator, pair[0], pair[1]));
                        _solutions.Add(new Solution(solutionOperationsList));
                        continue;
                    }

                    var newOperationsList = new List<Operation>(previousOperations);
                    newOperationsList.Add(new Operation(@operator, pair[0], pair[1]));

                    var newNumbers = new List<double>(numbers);
                    newNumbers.Remove(pair[0]);
                    newNumbers.Remove(pair[1]);
                    newNumbers.Add(newNum);

                    CheckNumbersForSolution(newNumbers, target, newOperationsList);
                }
            }
        }

        private void DedupeSolutions()
        {
            var dedupedSolutions = _solutions.Distinct(new SolutionComparer());
            _solutions = dedupedSolutions
                .OrderBy(s => s.Operations.Count)
                .ThenBy(s => s.Operations.Last().Result)
                .ToList();
        }
    }
}
