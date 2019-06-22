using Combinatorics.Collections;
using Countdown.NumbersRound.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Countdown.NumbersRound
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            // Objectives:
            //  - Get all possible combinations of n numbers, for n in [1,2,3,4,5,6]
            //  - Get all possible combinations of the 4 operations. +, -, /, *
            //  - Get all possibilities for placing brackets around n numbers. - Hardest.

            // PrintNumberAndOperationVariations();

            ExpressionTesting.TestExpression();

            // Brackets

            // For this we will need to come up with a data structure to store these.
            // We can potentially use the C# expression library to evaluate these expressions once created.
        }

        private static void PrintNumberAndOperationVariations()
        {
            // Numbers
            var all3Variations = CreateNumberVariations(3);

            Console.WriteLine(all3Variations.Count);

            foreach (var variant in all3Variations)
            {
                Console.WriteLine(string.Join(',', variant));
            }

            // Operations
            var operator3Variations = CreateOperationVariations(3);

            foreach (var variant in operator3Variations)
            {
                Console.WriteLine(string.Join(',', variant));
            }
        }

        private static Variations<int> CreateNumberVariations(int n)
        {
            if (n < 1 || n > 6)
            {
                throw new ArgumentOutOfRangeException(nameof(n), n, $"{nameof(n)} must be between 1 and 6.");
            }

            var domain = new List<int> { 1, 2, 3, 4, 5, 6 };

            return new Variations<int>(domain, n, GenerateOption.WithRepetition);
        }

        private static Variations<OperationType> CreateOperationVariations(int n)
        {
            var domain = new List<OperationType> { OperationType.Addition, OperationType.Subtraction, OperationType.Multiplication, OperationType.Division };

            return new Variations<OperationType>(domain, n, GenerateOption.WithRepetition);
        }
    }
}
