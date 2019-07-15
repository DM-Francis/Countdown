using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Countdown.NumbersRound.PrefixNotationBased
{
    public sealed class PrefixExpression
    {
        public static List<string> Operations = new List<string> { "+", "-", "/", "*" };

        private Stack<string> _stack;
        private int _numsSinceTopOperation;
        private int _operationsInStack;

        public IReadOnlyList<string> RawExpression { get; }

        public PrefixExpression(List<string> expression)
        {
            RawExpression = expression;
            if (!IsValid()) throw new FormatException();
        }

        public PrefixExpression(string constant)
        {
            RawExpression = new List<string> { constant };
        }

        public float Evaluate()
        {
            var tokenStack = new Stack<string>();
            foreach(var token in RawExpression.Reverse())
            {
                float num;
                if (float.TryParse(token, out num))
                {
                    tokenStack.Push(num.ToString());
                }
                else
                {
                    float num1 = float.Parse(tokenStack.Pop());
                    float num2 = float.Parse(tokenStack.Pop());

                    switch (token)
                    {
                        case "+":
                            tokenStack.Push((num1 + num2).ToString());
                            break;
                        case "-":
                            tokenStack.Push((num1 - num2).ToString());
                            break;
                        case "/":
                            tokenStack.Push((num1 / num2).ToString());
                            break;
                        case "*":
                            tokenStack.Push((num1 * num2).ToString());
                            break;
                        default:
                            throw new InvalidOperationException($"Expected a string representing an operation, but instead got {token}.");
                    }
                }
            }

            if (tokenStack.Count != 1)
            {
                throw new InvalidOperationException($"Expected only 1 remaining number in the stack, but there were {tokenStack.Count}");
            }

            return float.Parse(tokenStack.Pop());
        }

        private bool IsNumber(string s)
        {
            return float.TryParse(s, out _);
        }

        private bool IsOperation(string s)
        {
            return Operations.Contains(s);
        }

        private bool IsValid()
        {
            if (RawExpression.Count % 2 == 0) return false;

            string first = RawExpression[0];
            if (RawExpression.Count == 1)
            {
                return IsNumber(first);
            }
            else
            {
                string last = RawExpression.Last();
                return IsOperation(first) && IsNumber(last);
            }
        }
    }
}
