using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Countdown.NumbersRound.PrefixNotationBased
{
    public sealed class PrefixExpression
    {
        public static List<string> Operations = new List<string> { "+", "-", "/", "*" };

        public IReadOnlyList<string> RawExpression { get; }

        public PrefixExpression(List<string> expression)
        {
            RawExpression = expression;
            //if (!IsValid()) throw new FormatException();
        }

        public PrefixExpression(string constant)
        {
            RawExpression = new List<string> { constant };
        }

        public float Evaluate()
        {
            var tokenStack = new Stack<float>();
            foreach(var token in RawExpression.Reverse())
            {
                if (float.TryParse(token, out float num))
                {
                    tokenStack.Push(num);
                }
                else
                {
                    float num1 = tokenStack.Pop();
                    float num2 = tokenStack.Pop();

                    if (!OperationIsValid(token, num1, num2))
                    {
                        return float.NaN;
                    }

                    switch (token)
                    {
                        case "+":
                            tokenStack.Push(num1 + num2);
                            break;
                        case "-":
                            tokenStack.Push(num1 - num2);
                            break;
                        case "/":
                            tokenStack.Push(num1 / num2);
                            break;
                        case "*":
                            tokenStack.Push(num1 * num2);
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

            return tokenStack.Pop();
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

        private bool OperationIsValid(string operation, float num1, float num2)
        {
            if (num1 < num2)
            {
                return false;
            }

            if (operation == "*" && (num1 == 1 || num2 == 1))
            {
                return false;
            }
            else if (operation == "/")
            {
                if (num2 == 1)
                {
                    return false;
                }
                else if (num1 % num2 != 0)
                {
                    return false;
                }
                else if (num2 * num2 == num1)
                {
                    return false;
                }
            }
            else if (operation == "-")
            {
                if (num1 == num2)
                {
                    return false;
                }
                else if (num1 == 2 * num2)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
