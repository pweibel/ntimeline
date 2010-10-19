using System;
using System.Collections.Generic;

namespace NTimeline.Core
{
    public interface ITimePeriodBuilder
    {
        IList<TimePeriod> BuildTimePeriods(SortedList<DateTime, TimeElement> timeElements);
    }
}