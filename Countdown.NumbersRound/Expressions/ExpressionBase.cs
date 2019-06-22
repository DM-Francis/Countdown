using System;
using System.Collections.Generic;
using System.Text;

namespace Countdown.NumbersRound.Expressions
{
    internal abstract class ExpressionBase
    {
        public ExpressionBase Data { get; set; }
        public ExpressionBase Left { get; set; }
        public ExpressionBase Right { get; set; }

        public abstract float Value { get; }
    }
}
