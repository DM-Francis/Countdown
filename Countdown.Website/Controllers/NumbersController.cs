using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Countdown.NumbersRound;
using Countdown.Website.DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Countdown.Website.Controllers
{
    [Route("api/numbers")]
    [ApiController]
    public class NumbersController : ControllerBase
    {
        private readonly ISolver _solver;
        private readonly CountdownContext _context;

        public NumbersController(ISolver solver, CountdownContext context)
        {
            _solver = solver;
            _context = context;
        }

        [HttpPost]
        [Route("solve", Name = "numbers-solve")]
        public async Task<SolveResult> SolveAsync(int target, List<int> chosenNums)
        {
            var solveTask = Task.Run(() => _solver.GetPossibleSolutions(target, chosenNums));
            var problem = new Problem { Target = target, AvailableNumbers = chosenNums };

            SolveResult result = await solveTask;
            var solutionsData = result.Solutions.Select(sol => new Solution { Problem = problem, Value = sol });

            problem.ClosestDiff = result.ClosestDiff;
            _context.Add(problem);
            _context.AddRange(solutionsData);

            await _context.SaveChangesAsync().ConfigureAwait(false);
            return result;
        }
    }
}