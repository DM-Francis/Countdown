using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Combinatorics.Collections;
using Countdown.NumbersRound.Solve;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Countdown.Store
{
    class Program
    {
        static void Main(string[] args)
        {
            var availableLarge = new List<int> { 25, 50, 75, 100 };
            var availableSmall = new List<int> { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10 };

            var availableAll = new List<int>(availableLarge);
            availableAll.AddRange(availableSmall);

            var allTargets = Enumerable.Range(100, 900);

            var combs = new Combinations<int>(availableAll, 6, GenerateOption.WithoutRepetition);

            var allPossibleGames = from target in allTargets
                                   from nums in combs
                                   select (target, nums);

            if (File.Exists("solutions.txt")) File.Delete("solutions.txt");
            var outputStream = File.OpenWrite("solutions.txt");
            using var writer = new StreamWriter(outputStream);

            int count = 0;
            var solver = new Solver(new NullLogger<Solver>(), new DictionaryDelegateCache());
            foreach (var (target, nums) in allPossibleGames)
            {
                var result = solver.GetPossibleSolutions(target, new List<int>(nums));
                writer.WriteLine(JsonConvert.SerializeObject(result));

                if (count++ % 10 == 0)
                {
                    writer.Flush();
                }

                if (count % 20 == 0) break;

                Console.WriteLine(count);
            }
        }
    }
}
