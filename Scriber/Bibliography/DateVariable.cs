using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Scriber.Bibliography
{
    public struct DateVariable : IDateVariable, IEquatable<DateVariable>
    {
        public DateVariable(DateTime date)
            : this(date, date, false)
        {
        }
        public DateVariable(DateTime from, DateTime to)
            : this(from, to, false)
        {
        }
        public DateVariable(IDateVariable from, IDateVariable to)
            : this(from, to, false)
        {
        }
        public DateVariable(DateTime from, DateTime to, bool isCirca)
        {
            // done
            YearFrom = from.Year;
            YearTo = to.Year;
            SeasonFrom = GetSeason(from);
            SeasonTo = GetSeason(to);
            MonthFrom = from.Month;
            MonthTo = to.Month;
            DayFrom = from.Day;
            DayTo = to.Day;
            IsApproximate = isCirca;
        }
        public DateVariable(IDateVariable from, IDateVariable to, bool isCirca)
            : this(from.YearFrom, from.SeasonFrom, from.MonthFrom, from.DayFrom, to.YearTo, to.SeasonTo, to.MonthTo, to.DayTo, isCirca)
        {
        }
        public DateVariable(int year, Season? season, int? month, int? day, bool isCirca)
            : this(year, season, month, day, year, season, month, day, isCirca)
        {
        }
        public DateVariable(int yearFrom, Season? seasonFrom, int? monthFrom, int? dayFrom, int yearTo, Season? seasonTo, int? monthTo, int? dayTo, bool isCirca)
        {
            // done
            YearFrom = yearFrom;
            YearTo = yearTo;
            SeasonFrom = seasonFrom;
            SeasonTo = seasonTo;
            MonthFrom = monthFrom;
            MonthTo = monthTo;
            DayFrom = dayFrom;
            DayTo = dayTo;
            IsApproximate = isCirca;
        }
        private static Season GetSeason(DateTime date)
        {
            if (date < new DateTime(date.Year, 3, 21))
            {
                return Season.Winter;
            }
            else if (date < new DateTime(date.Year, 6, 21))
            {
                return Season.Spring;
            }
            else if (date < new DateTime(date.Year, 9, 21))
            {
                return Season.Summer;
            }
            else if (date < new DateTime(date.Year, 12, 21))
            {
                return Season.Autumn;
            }
            else
            {
                return Season.Winter;
            }
        }

        public int YearFrom { get; }
        public int YearTo { get; }

        public Season? SeasonFrom { get; }
        public Season? SeasonTo { get; }

        public int? MonthFrom { get; }
        public int? MonthTo { get; }

        public int? DayFrom { get; }
        public int? DayTo { get; }

        public bool IsApproximate { get; }

        public static bool TryParse(string value, out DateVariable variable)
        {
            // number?
            if (TryParseDate(value, out variable))
            {
                return true;
            }

            return TrySplitDate(value, ",", out variable)
                || TrySplitDate(value, "&", out variable)
                || TrySplitDate(value, "--", out variable);
        }

        public static bool TryParse(string year, string? month, out DateVariable variable)
        {
            if (!int.TryParse(year, out var yearValue))
            {
                variable = new DateVariable();
                return false;
            }
            else
            {
                int? monthValue = null;
                if (int.TryParse(month, out var parsedMonthValue))
                {
                    monthValue = parsedMonthValue;
                }
                variable = new DateVariable(yearValue, null, monthValue, null, false);
                return true;
            }
        }

        private static bool TryParseDate(string text, out DateVariable value)
        {
            if (TryParseDateExact(text, "yyyy-MM-dd", out var full))
            {
                value = new DateVariable(full);
                return true;
            }
            else if (TryParseDateExact(text, "yyyy-MM", out var month))
            {
                value = new DateVariable(month.Year, null, month.Month, null, false);
                return true;
            }
            else if (TryParseDateExact(text, "yyyy", out var year))
            {
                value = new DateVariable(year.Year, null, null, null, false);
                return true;
            }
            value = new DateVariable();
            return false;
        }

        private static bool TryParseDateExact(string text, string format, out DateTime value)
        {
            return DateTime.TryParseExact(text, format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out value);
        }

        private static bool TrySplitDate(string text, string separators, out DateVariable variable)
        {
            // init
            DateVariable? result = null;

            // split
            var parts = text
                .Split(separators, StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            // 2 parts?
            if (parts.Length == 2)
            {
                // done
                if (TryParseDate(parts[0], out var n1) && TryParseDate(parts[1], out var n2))
                {
                    result = new DateVariable(n1, n2);
                }
            }

            variable = result ?? new DateVariable();

            // done
            return result != null;
        }

        public override bool Equals(object? obj)
        {
            if (obj is DateVariable date)
            {
                return Equals(date);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(YearFrom, YearTo, SeasonFrom, SeasonTo, MonthFrom, MonthTo, DayFrom, DayTo) + IsApproximate.GetHashCode();
        }

        public bool Equals([AllowNull] DateVariable other)
        {
            if (other == null)
            {
                return false;
            }

            return YearFrom == other.YearFrom
                && YearTo == other.YearTo
                && SeasonFrom == other.SeasonFrom
                && SeasonTo == other.SeasonTo
                && MonthFrom == other.MonthFrom
                && MonthTo == other.MonthTo
                && DayFrom == other.DayFrom
                && DayTo == other.DayTo
                && IsApproximate && other.IsApproximate;
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            sb.Append(YearFrom);

            if (MonthFrom.HasValue)
            {
                sb.Append('/');
                sb.Append(MonthFrom.Value);
            }
            if (DayFrom.HasValue)
            {
                sb.Append('/');
                sb.Append(DayFrom.Value);
            }
            if (YearFrom != YearTo)
            {
                sb.Append('-');
                sb.Append(YearTo);

                if (MonthTo.HasValue)
                {
                    sb.Append('/');
                    sb.Append(MonthTo.Value);
                }
                if (DayTo.HasValue)
                {
                    sb.Append('/');
                    sb.Append(DayTo.Value);
                }
            }

            return sb.ToString();
        }

        public static bool operator ==(DateVariable left, DateVariable right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DateVariable left, DateVariable right)
        {
            return !(left == right);
        }
    }
}