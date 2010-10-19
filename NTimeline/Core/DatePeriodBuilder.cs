using System;
using System.Collections.Generic;
using NTimeline.Helpers;

namespace NTimeline.Core
{
    public class DatePeriodBuilder : ITimePeriodBuilder
    {
        #region Publics
        /// <summary>
        /// Returns a list of time periodes.
        /// </summary>
        /// <param name="timeElements">The time Elements from the timeline</param>
        /// <returns>List with all time periods. If there are no entries on the timeline, then an empty list will be returned</returns>
        public virtual IList<TimePeriod> BuildTimePeriods(SortedList<DateTime, TimeElement> timeElements)
        {
            IList<TimePeriod> listTimePeriod = new List<TimePeriod>();

            if(timeElements.Values.Count == 0) return listTimePeriod;

            for (int i = 0; i < timeElements.Values.Count; i++)
            {
                // If the current time element is a From and a Until date, then a one day period has to be created.
                if (timeElements.Values[i].IsFrom && timeElements.Values[i].IsUntil)
                {
                    // Create time period for one day
                    TimePeriod timePeriodOneDay = CreateTimePeriod(timeElements.Values[i], timeElements.Values[i]);
                    if (timePeriodOneDay != null && !listTimePeriod.Contains(timePeriodOneDay)) listTimePeriod.Add(timePeriodOneDay);
                }

                TimeElement timeElementFrom = timeElements.Values[i];

                // The second element will only be set, if there exists another one.
                TimeElement timeElementUntil = i < timeElements.Count - 1 ? timeElements.Values[i + 1] : null;

                // if the second time element follows directly after the first time element, there is nothing to do.
                if (timeElementUntil != null && timeElementFrom.Date.AddDays(1) == timeElementUntil.Date && timeElementFrom.IsUntil && timeElementUntil.IsFrom) continue;

                // Create time period
                TimePeriod timePeriod = CreateTimePeriod(timeElementFrom, timeElementUntil);
                if (timePeriod != null && !listTimePeriod.Contains(timePeriod)) listTimePeriod.Add(timePeriod);
            }

            return listTimePeriod;
        }
        #endregion

        #region Protecteds
        /// <summary>
        /// Creates a time period and determin all relevant time sources.
        /// </summary>
        /// <param name="timeElementFrom">from time element</param>
        /// <param name="timeElementUntil">until time element. Could be also empty.</param>
        /// <returns>Created time period or null.</returns>
        protected virtual TimePeriod CreateTimePeriod(TimeElement timeElementFrom, TimeElement timeElementUntil)
        {
            if (timeElementFrom == null) throw new ArgumentNullException("timeElementFrom");

            Duration duration = BuildDuration(timeElementFrom, timeElementUntil);

            TimePeriod timePeriod = timeElementUntil == null ? new TimePeriod(timeElementFrom, duration) : new TimePeriod(timeElementFrom, timeElementUntil, duration);

            return timePeriod;
        }

        /// <summary>
        /// Creates a duration with the given time elements
        /// </summary>
        /// <param name="from">Begin time element of the duration</param>
        /// <param name="until">End time elemnt of the duration. Could be null</param>
        /// <returns>Duration between the two time elements</returns>
        protected virtual Duration BuildDuration(TimeElement from, TimeElement until)
        {
            if(from == null) throw new ArgumentNullException("from");

            // Special case: From and until date are the same
            if(from == until) return new Duration(from.Date, until.Date);

            DateTime dtFrom = from.IsUntil ? from.Date.AddDays(1) : from.Date;
            DateTime? dtUntil = null;
            if(until != null) dtUntil = until.IsFrom ? until.Date.AddDays(-1) : until.Date;

            return until == null ? new Duration(dtFrom) : new Duration(dtFrom, dtUntil);
        }
        #endregion
    }
}
