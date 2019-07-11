using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Countdown.NumbersRound.PolishNotationBased
{
    public sealed class PolishExpression
    {
        private List<string> _expression;

        private Stack<string> _stack;
        private string _lastPushedOperation = "";
        private int _numsSinceOperation = 0;

        public PolishExpression(List<string> expression)
        {
            _expression = expression;
            if (!IsValid()) throw new FormatException();
        }

        public float Evaluate()
        {
            _stack = new Stack<string>();
            _lastPushedOperation = "";

            foreach(string s in _expression)
            {
                _stack.Push(s);
                if (IsOperation(s))
                {
                    _lastPushedOperation = s;
                    _numsSinceOperation = 0;
                }
                else if (IsNumber(s))
                {
                    _numsSinceOperation++;
                    if (_numsSinceOperation == 2)
                    {
                        EvaluateTopOperation();
                    }
                }
                else
                {
                    throw new InvalidOperationException($"Expected either a number or an operation, got {s}");
                }
            }

            float.TryParse(_stack.Pop(), out float result);
            return result;
        }

        private void EvaluateTopOperation()
        {
            float.TryParse(_stack.Pop(), out float num2);
            float.TryParse(_stack.Pop(), out float num1);

            string operation = _stack.Pop();
            if (!IsOperation(operation)) throw new InvalidOperationException($"Expected a string representing an operation, but instead got {operation}.");

            switch (operation)
            {
                case "+":
                    _stack.Push((num1 + num2).ToString());
                    break;
                case "-":
                    _stack.Push((num1 - num2).ToString());
                    break;
                case "/":
                    _stack.Push((num1 / num2).ToString());
                    break;
                case "*":
                    _stack.Push((num1 * num2).ToString());
                    break;
                default:
                    throw new InvalidOperationException($"Expected a string representing an operation, but instead got {operation}.");
            }
        }

        private bool IsNumber(string s)
        {
            return float.TryParse(s, out _);
        }

        private bool IsOperation(string s)
        {
            return s == "+" || s == "-" || s == "/" || s == "*";
        }

        private bool IsValid()
        {
            string first = _expression[0];
            if (first == "+" ||first == "-" || first == "/" || first == "*")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
