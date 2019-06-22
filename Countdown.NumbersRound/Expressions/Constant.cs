using System;
using System.Collections.Generic;
using System.Text;

namespace Countdown.NumbersRound.Expressions
{
    internal class Constant : ExpressionBase
    {
        public override float Value
        {
            get => Data.Value;
        }
    }
}
