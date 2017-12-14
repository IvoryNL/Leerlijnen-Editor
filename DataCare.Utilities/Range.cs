// -----------------------------------------------------------------------
// <copyright file="Range.cs" company="DataCare BV">
// </copyright>
// -----------------------------------------------------------------------

namespace DataCare.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    public static class Range
    {
        public static Range<T> Create<T>(T firstBound, T secondBound) where T : IComparable<T>
        {
            return new Range<T>(firstBound, secondBound);
        }
    }

    /// <summary>
    /// Simple class representing a range of numbers
    /// </summary>
    public class Range<T> where T : IComparable<T>
    {
        public T LowerBound { get; private set; }
        public T UpperBound { get; private set; }

        public Range(T firstBound, T secondBound)
        {
            var comparison = firstBound.CompareTo(secondBound);
            if (comparison <= 0)
            {
                LowerBound = firstBound;
                UpperBound = secondBound;
            }
            else
            {
                UpperBound = firstBound;
                LowerBound = secondBound;
            }
        }

        public bool Contains(T value)
        {
            return LowerBound.CompareTo(value) <= 0 && UpperBound.CompareTo(value) >= 0;
        }

        public bool Contains(Range<T> range)
        {
            Contract.Requires(range != null);

            return LowerBound.CompareTo(range.LowerBound) <= 0 && UpperBound.CompareTo(range.UpperBound) >= 0;
        }

        public bool IsContainedBy(Range<T> range)
        {
            Contract.Requires(range != null);

            return range.Contains(this);
        }

        public bool Overlaps(Range<T> range)
        {
            Contract.Requires(range != null);

            return Contains(range.LowerBound) || Contains(range.UpperBound) || IsContainedBy(range);
        }

        public IEnumerable<T> Step(Func<T, T> next)
        {
            for (T current = LowerBound; current.CompareTo(UpperBound) <= 0; current = next(current))
            {
                yield return current;
            }
        }

        public bool Between(Range<T> range)
        {
            Contract.Requires(range != null);

            return this.LowerBound.CompareTo(range.LowerBound) >= 0 && this.UpperBound.CompareTo(range.UpperBound) <= 0;
        }

        /// <summary>
        /// Wordt de gegeven periode geheel afgedekt door de periodes
        /// </summary>
        /// <param name="periods"></param>
        /// <param name="periodToCheck"></param>
        /// <returns></returns>
        public static bool FullRange(
            Range<DateTime> periodToCheck,
            List<Range<DateTime>> periods)
        {
            return GetFirstFreeRange(periodToCheck, periods) == null;
        }

        /// <summary>
        /// Als de gegeven periode geheel wordt afgedekt door de periodes geef dan null terug,
        /// zoniet geef dan de eerste vrije periode terug.
        /// </summary>
        /// <param name="periodToCheck"></param>
        /// <param name="periods"></param>
        /// <returns></returns>
        public static Range<DateTime> GetFirstFreeRange(
            Range<DateTime> periodToCheck,
            List<Range<DateTime>> periods)
        {
            if (!periods.Any())
            {
                return periodToCheck;
            }

            var periodsThatApply = periods.Where(p => periodToCheck.Overlaps(p)).ToList();
            if (!periodsThatApply.Any())
            {
                return periodToCheck;
            }

            periodsThatApply.Sort(new CompareDateTimeRange());
            if (periodsThatApply.First().LowerBound.Date > periodToCheck.LowerBound.Date)
            {
                return new Range<DateTime>(periodToCheck.LowerBound.Date, periodsThatApply.First().LowerBound.AddDays(-1).Date);
            }

            var currentPeriod = periodsThatApply.First();
            foreach (var period in periodsThatApply.Skip(1))
            {
                if (currentPeriod.UpperBound.AddDays(1).Date == period.LowerBound.Date)
                {
                    currentPeriod = period;
                }
                else
                {
                    return new Range<DateTime>(currentPeriod.UpperBound.AddDays(1).Date, period.LowerBound.AddDays(-1).Date);
                }
            }

            if (periodsThatApply.Last().UpperBound.Date < periodToCheck.UpperBound.Date)
            {
                return new Range<DateTime>(periodsThatApply.Last().UpperBound.AddDays(1).Date, periodToCheck.UpperBound.Date);
            }

            // Gehele periode afgedekt.
            return null;
        }

        private class CompareDateTimeRange : IComparer<Range<DateTime>>
        {
            // Sorteer op volgorde van LowerBound.
            public int Compare(Range<DateTime> x, Range<DateTime> y)
            {
                if (y.LowerBound > x.LowerBound)
                {
                    return -1;
                }

                if (y.LowerBound < x.LowerBound)
                {
                    return 1;
                }

                return 0;
            }
        }
    }
}