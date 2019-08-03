using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Countdown.Website.DataModels
{
    public class Problem
    {
        public int ProblemId { get; set; }
        public int Target { get; set; }
        public List<int> AvailableNumbers { get; set; }
        public int ClosestDiff { get; internal set; }

        public ICollection<Solution> Solutions { get; set; }
    }
}
