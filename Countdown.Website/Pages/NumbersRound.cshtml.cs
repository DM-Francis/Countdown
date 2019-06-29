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

        public int Target { get; set; }
        public int SmallChoices { get; set; } = 20;

        public List<int> AvailableLarge { get; set; } = new List<int> { 25, 50, 75, 100 };
        public List<int> AvailableSmall { get; set; } = new List<int> { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10 };

        public void OnGet()
        {
            RefreshTarget();
            ShuffleNumbers();
        }

        private void RefreshTarget()
        {
            Target = _rng.Next(101, 999);
        }

        private void ShuffleNumbers()
        {
            AvailableLarge = AvailableLarge.OrderBy(_ => Guid.NewGuid()).ToList();
            AvailableSmall = AvailableSmall.OrderBy(_ => Guid.NewGuid()).ToList();
        }
    }
}
