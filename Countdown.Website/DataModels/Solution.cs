using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Countdown.Website.DataModels
{
    public class Solution
    {
        public int SolutionId { get; set; }
        public string Value { get; set; }

        public Problem Problem { get; set; }
    }
}
