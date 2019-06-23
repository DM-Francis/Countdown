using Combinatorics.Collections;
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

    }
}
