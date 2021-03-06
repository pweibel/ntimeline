﻿using System;
using System.Collections.Generic;
using Moq;
using NTimeline.Core;
using NTimeline.Helpers;
using NTimeline.Source;
using NUnit.Framework;

namespace NTimeline.Test.Core
{
	[TestFixture]
	public class TimelineTest
	{
		[Test]
		public void TestAddTimeSource()
		{
			// Arrange
			var source = new Mock<ITimeSource>();
			Timeline timeline = new Timeline();

			// Act
			timeline.AddTimeSource(source.Object);

			// Assert
			// No exception should be throw
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestAddTimeSource_With_Null()
		{
			// Arrange
			Timeline timeline = new Timeline();

			// Act
			timeline.AddTimeSource(null);
		}

		[Test]
		public void TestRemoveTimeSource()
		{
			// Arrange
			Timeline timeline = new Timeline();
			var source = new Mock<ITimeSource>();
			timeline.AddTimeSource(source.Object);

			// Act
			timeline.RemoveTimeSource(source.Object);

			// Assert
			// No exception should be throw
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestRemoveTimeSource_With_Null()
		{
			// Arrange
			Timeline timeline = new Timeline();

			// Act
			timeline.RemoveTimeSource(null);
		}

		[Test]
		public void TestBuild()
		{
			// Arrange
			Timeline timeline = new Timeline();
			DateTime dtFrom = new DateTime(2010, 1, 1);
			var source = new Mock<ITimeSource>();
			source.Setup(x => x.CreateTimeElements()).Returns(new List<TimeElement> { new TimeElement(dtFrom, true) });
			source.Setup(x => x.IsValid(It.IsAny<Duration>())).Returns(true);
			timeline.AddTimeSource(source.Object);

			// Act
			timeline.Build();

			// Assert
			Assert.AreEqual(dtFrom, timeline.TimePeriods[0].From.Date);
			Assert.IsNull(timeline.TimePeriods[0].Until);
		}

		[Test]
		public void TestTimePeriods()
		{
			// Arrange
			Timeline timeline = new Timeline();
			DateTime dtFrom = new DateTime(2010, 1, 1);
			var source = new Mock<ITimeSource>();
			source.Setup(x => x.CreateTimeElements()).Returns(new List<TimeElement> { new TimeElement(dtFrom, true) });
			source.Setup(x => x.IsValid(It.IsAny<Duration>())).Returns(true);
			timeline.AddTimeSource(source.Object);
			timeline.Build();

			// Act
			IList<TimePeriod> periods = timeline.TimePeriods;

			// Assert
			Assert.AreEqual(1, periods.Count);
			Assert.AreEqual(dtFrom, periods[0].From.Date);
			Assert.IsNull(periods[0].Until);
		}

		[Test]
		public void TestGetTimePeriods_With_Date_In_Period()
		{
			// Arrange
			Timeline timeline = new Timeline();
			DateTime dtFrom = new DateTime(2010, 1, 1);
			var source = new Mock<ITimeSource>();
			source.Setup(x => x.CreateTimeElements()).Returns(new List<TimeElement> { new TimeElement(dtFrom, true) });
			source.Setup(x => x.IsValid(It.IsAny<Duration>())).Returns(true);
			timeline.AddTimeSource(source.Object);
			timeline.Build();

			// Act
			IList<TimePeriod> periods = timeline.GetTimePeriods(dtFrom);

			// Assert
			Assert.AreEqual(1, periods.Count);
			Assert.AreEqual(dtFrom, periods[0].From.Date);
			Assert.IsNull(periods[0].Until);
		}

		[Test]
		public void TestGetTimePeriods_With_Date_Before_Period()
		{
			// Arrange
			Timeline timeline = new Timeline();
			DateTime dtFrom = new DateTime(2010, 1, 1);
			var source = new Mock<ITimeSource>();
			source.Setup(x => x.CreateTimeElements()).Returns(new List<TimeElement> { new TimeElement(dtFrom, true) });
			source.Setup(x => x.IsValid(It.IsAny<Duration>())).Returns(true);
			timeline.AddTimeSource(source.Object);
			timeline.Build();

			// Act
			IList<TimePeriod> periods = timeline.GetTimePeriods(dtFrom.AddDays(-5));

			// Assert
			Assert.AreEqual(0, periods.Count);
		}
	}
}
