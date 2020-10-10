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
        private List<List<Operation>> _solutions = new List<List<Operation>>();
        private bool _isSolved = false;

        public Solver2(int target, List<int> availableNums)
        {
            _target = target;
            _availableNums = availableNums;
        }

        public int Solve()
        {
            if (_isSolved)
                throw new InvalidOperationException("Already solved.");

            CheckNumbersForSolution(_availableNums.Select(n => (double)n).ToList(), _target, new List<Operation>());
            DedupeSolutions();
            _isSolved = true;

            return _solutions.Count;
        }

        private void CheckNumbersForSolution(List<double> numbers, double target, List<Operation> previousOperations)
        {
            if (numbers.Count == 1 || numbers.Contains(target))
                return;

            var combination = new NumberCombination(numbers);

            if (_previouslyCheckedCombinations.TryGetValue(combination, out int opCount) && opCount <= previousOperations.Count)
                return;

            _previouslyCheckedCombinations[combination] = previousOperations.Count;

            var allPairs = new Variations<double>(numbers, 2, GenerateOption.WithoutRepetition);

            foreach(var pair in allPairs)
            {
                if (pair[0] < pair[1])
                    continue;

                foreach(var @operator in Operator.All)
                {
                    double newNum = @operator.Evaluate(pair[0], pair[1]);

                    if (newNum == target)
                    {
                        var solutionOperationsList = new List<Operation>(previousOperations);
                        solutionOperationsList.Add(new Operation(@operator, pair[0], pair[1]));
                        _solutions.Add(solutionOperationsList);
                        continue;
                    }

                    if (newNum % 1 != 0 || newNum < 0 || newNum == pair[0] || newNum == pair[1])
                        continue;

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
            _solutions = dedupedSolutions.OrderBy(d => d.Count).ToList();
        }
    }
}
