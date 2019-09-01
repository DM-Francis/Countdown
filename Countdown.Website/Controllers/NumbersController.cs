using Countdown.NumbersRound.Solve;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<IActionResult> SolveAsync(int target, List<int> chosenNums)
        {
            if (chosenNums.Count == 0 || chosenNums.Count > 6)
            {
                return BadRequest();
            }

            var solveResult = await Task.Run(() => _solver.GetPossibleSolutions(target, chosenNums));
            return Ok(solveResult);
        }
    }
}