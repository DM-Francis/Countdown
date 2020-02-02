using Countdown.NumbersRound.Solve;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Countdown.Website.Controllers
{
    [Route("api/v1/numbers")]
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
        public IActionResult Solve(int target, List<int> chosenNums)
        {
            if (chosenNums.Count == 0 || chosenNums.Count > 6)
            {
                return BadRequest("Must provide between 1 and 6 numbers");
            }

            var solveResult =  _solver.GetPossibleSolutions(target, chosenNums);
            return Ok(solveResult);
        }
    }
}