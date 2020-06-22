using System;

namespace Scriber.Bibliography
{
    public struct DateVariable : IDateVariable
    {
        public DateVariable(DateTime date)
            : this(date, date, false)
        {
        }
        public DateVariable(DateTime from, DateTime to)
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
    }
}