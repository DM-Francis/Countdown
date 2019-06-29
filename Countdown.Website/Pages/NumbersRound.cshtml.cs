using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Countdown.Website.Pages
{
    public class NumbersRoundModel : PageModel
    {
        private readonly Random _rng = new Random();

        [BindProperty]
        public int Target { get; set; }

        [BindProperty]
        public List<int> ChosenNums { get; set; } = new List<int>();

        public int DefaultNumListSize { get; } = 6;
        public List<int> AvailableLarge { get; } = new List<int> { 25, 50, 75, 100 };
        public List<int> AvailableSmall { get; } = new List<int> { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10 };

        public void OnGet()
        {
            GenerateTarget();
        }

        public IActionResult OnPost(int target, List<int> chosenNums)
        {
            return Page();
        }

        private void GenerateTarget()
        {
            Target = _rng.Next(101, 999);
        }
    }
}
