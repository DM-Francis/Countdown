using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Countdown.NumbersRound.Solve
{
    internal class NumberCombination : IReadOnlyCollection<double>, IEquatable<NumberCombination?>
    {
        private readonly List<double> _numbers;

        public NumberCombination(IEnumerable<double> numbers)
        {
            _numbers = new List<double>(numbers.OrderBy(n => n));
        }

        public int Count => _numbers.Count;

        public override bool Equals(object? obj) => Equals(obj as NumberCombination);

        public bool Equals(NumberCombination? other)
        {
            if (other is null)
                return false;

            return _numbers.SequenceEqual(other._numbers);
        }

        public override int GetHashCode()
        {
            int hashCode = 1348472664;

            foreach(double value in _numbers)
            {
                hashCode ^= value.GetHashCode();
            }

            return hashCode;
        }

        public IEnumerator<double> GetEnumerator() => ((IEnumerable<double>)_numbers).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_numbers).GetEnumerator();

        public static bool operator ==(NumberCombination left, NumberCombination right) => EqualityComparer<NumberCombination>.Default.Equals(left, right);
        public static bool operator !=(NumberCombination left, NumberCombination right) => !(left == right);
    }
}
