using System;
using System.Collections.Generic;
using NTimeline.Core;
using NUnit.Framework;

namespace NTimeline.Test.Core
{
    [TestFixture]
    public class DatePeriodBuilderTest
    {
        [Test]
        public void TestBuildTimePeriods()
        {
            // Arrange
            SortedList<DateTime, TimeElement> sortedList = new SortedList<DateTime, TimeElement>();
            DateTime dtFrom = new DateTime(2010, 1, 1);
            TimeElement elementFrom = new TimeElement(dtFrom, true);
            sortedList.Add(dtFrom, elementFrom);
            DatePeriodBuilder builder = new DatePeriodBuilder();

            // Act
            IList<TimePeriod> periods = builder.BuildTimePeriods(sortedList);

            // Assert
            Assert.AreEqual(1, periods.Count);
            Assert.AreEqual(elementFrom, periods[0].From);
            Assert.IsNull(periods[0].Until);
        }
    }
}
