using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Countdown.NumbersRound.Solve
{
    internal sealed class Operator
    {
        public static readonly Operator Add = new Operator(ExpressionType.Add);
        public static readonly Operator Subtract = new Operator(ExpressionType.Subtract);
        public static readonly Operator Multiply = new Operator(ExpressionType.Multiply);
        public static readonly Operator Divide = new Operator(ExpressionType.Divide);

        public static IReadOnlyCollection<Operator> All = new ReadOnlyCollection<Operator>(new[] { Add, Subtract, Multiply, Divide });

        public ExpressionType Type { get; }
        private readonly Func<double, double, double> _delegate;

        private Operator(ExpressionType type)
        {
            _delegate = type switch
            {
                ExpressionType.Add => (a, b) => a + b,
                ExpressionType.Subtract => (a, b) => a - b,
                ExpressionType.Multiply => (a, b) => a * b,
                ExpressionType.Divide => (a, b) => a / b,
                _ => throw new NotSupportedException()
            };

            Type = type;
        }

        public double Evaluate(double a, double b) => _delegate(a, b);

        public override string ToString() => Type.ToString();

        public string ToString(double a, double b)
        {
            string withBrackets = Type switch
            {
                ExpressionType.Add => Expression.Add(Expression.Constant(a), Expression.Constant(b)).ToString(),
                ExpressionType.Subtract => Expression.Subtract(Expression.Constant(a), Expression.Constant(b)).ToString(),
                ExpressionType.Multiply => Expression.Multiply(Expression.Constant(a), Expression.Constant(b)).ToString(),
                ExpressionType.Divide => Expression.Divide(Expression.Constant(a), Expression.Constant(b)).ToString(),
                _ => throw new NotSupportedException(),
            };

            return withBrackets.Substring(1, withBrackets.Length - 2);
        }
    }
}
