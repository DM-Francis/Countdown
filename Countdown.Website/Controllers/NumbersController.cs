using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Countdown.NumbersRound;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Countdown.Website.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NumbersController : ControllerBase
    {
        private readonly ISolver _solver;

        public NumbersController(ISolver solver)
        {
            _solver = solver;
        }

        [HttpPost]
        [Route("solve", Name = "numbers-solve")]
        public async Task<SolveResult> SolveAsync(int target, List<int> chosenNums)
        {
            return await Task.Run(() => _solver.GetPossibleSolutions(target, chosenNums)).ConfigureAwait(false);
        }
    }
}