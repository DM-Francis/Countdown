namespace Countdown.NumbersRound.Solve
{
    internal class Operation
    {
        public Operation(Operator @operator, double a, double b)
        {
            Operator = @operator;
            A = a;
            B = b;
            Result = Operator.Evaluate(A, B);
        }

        public Operator Operator { get; }
        public double A { get; }
        public double B { get; }
        public double Result { get; }

        public override string ToString()
        {
            double result = Operator.Evaluate(A, B);
            return $"{Operator.ToString(A, B)} = {result}";
        }
    }
}
