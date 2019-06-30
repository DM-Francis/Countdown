using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Countdown.NumbersRound;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Countdown.Website.Pages
{
    public class NumbersRoundModel : PageModel
    {
        private readonly Random _rng = new Random();
        private readonly ISolver _solver;

        [BindProperty]
        public int Target { get; set; }

        [BindProperty]
        public List<int> ChosenNums { get; set; } = new List<int>();

        public List<string> Solutions { get; set; }

        public int DefaultNumListSize { get; } = 6;
        public List<int> AvailableLarge { get; } = new List<int> { 25, 50, 75, 100 };
        public List<int> AvailableSmall { get; } = new List<int> { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10 };

        public NumbersRoundModel(ISolver solver)
        {
            _solver = solver;
        }

        public void OnGet()
        {
            GenerateTarget();
        }

        public async Task<IActionResult> OnPostAsync(int target, List<int> chosenNums)
        {
            Solutions = await Task.Run(() => _solver.GetPossibleSolutions(target, chosenNums)).ConfigureAwait(false);
            return Page();
        }

        private void GenerateTarget()
        {
            Target = _rng.Next(101, 999);
        }
    }
}
