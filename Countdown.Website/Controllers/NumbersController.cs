using Countdown.NumbersRound.Solve;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Countdown.Website.Controllers
{
    [Route("api/v1/numbers")]
    [ApiController]
    public class NumbersController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("solve", Name = "numbers-solve")]
        public ActionResult<SolveResult> Solve(int target, List<int> chosenNums)
        {
            if (chosenNums.Count == 0 || chosenNums.Count > 8)
            {
                return BadRequest("Must provide between 1 and 8 numbers");
            }

            var solver = new Solver2(target, chosenNums);
            return solver.Solve();
        }
    }
}