using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Countdown.NumbersRound
{
    public static class Program
    {
        public static void Main()
        {
            var solver = new Solver(new ConsoleLogger<Solver>());

            var solutions = solver.GetPossibleSolutions(100, new List<int> { 1, 2, 3, 4, 5, 6 });
        }

        private class ConsoleLogger<T> : ILogger<T>
        {
            public IDisposable BeginScope<TState>(TState state) => throw new NotImplementedException();
            public bool IsEnabled(LogLevel logLevel) => true;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                Console.WriteLine(formatter(state, exception));
            }
        }
    }
}
