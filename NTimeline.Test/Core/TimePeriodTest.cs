using System;
using System.Collections.Generic;
using Moq;
using NTimeline.Core;
using NTimeline.Helpers;
using NTimeline.Source;
using NUnit.Framework;

namespace NTimeline.Test.Core
{
	[TestFixture]
	public class TimePeriodTest
	{
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestTimePeriod_With_No_Timeline_And_From()
		{
			// Act
			new TimePeriod(null, null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestTimePeriod_With_No_Timeline_And_From_And_Until()
		{
			// Act
			new TimePeriod(null, null, null);
		}

		[Test]
		public void TestTimePeriod_With_Timeline_And_FromDate()
		{
			// Arrange
			var timeline = new Mock<Timeline>(null);
			DateTime dtFrom = DateTime.Now;
			TimeElement elementFrom = new TimeElement(dtFrom, true);

			// Act
			TimePeriod period = new TimePeriod(timeline.Object, elementFrom);

			// Assert
			Assert.AreEqual(timeline.Object, period.Timeline);
			Assert.AreEqual(elementFrom, period.From);
			Assert.IsNull(period.Until);
		}

		[Test]
		public void TestTimePeriod_With_Only_FromDate()
		{
			// Arrange
			var timeline = new Mock<Timeline>(null);
			DateTime dtFrom = DateTime.Now;
			TimeElement elementFrom = new TimeElement(dtFrom, true);

			// Act
			TimePeriod period = new TimePeriod(timeline.Object, elementFrom);

			// Assert
			Assert.AreEqual(elementFrom, period.From);
			Assert.IsNull(period.Until);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestTimePeriod_With_Null_As_FromDate()
		{
			// Arrange
			var timeline = new Mock<Timeline>(null);

			// Act
			new TimePeriod(timeline.Object, null);
		}

		[Test]
		public void TestTimePeriod_With_FromDate_And_UntilDate()
		{
			// Arrange
			var timeline = new Mock<Timeline>(null);
			DateTime dtFrom = DateTime.Now;
			TimeElement elementFrom = new TimeElement(dtFrom, true);
			DateTime dtUntil = dtFrom.AddMonths(1);
			TimeElement elementUntil = new TimeElement(dtUntil, false);

			// Act
			TimePeriod period = new TimePeriod(timeline.Object, elementFrom, elementUntil);

			// Assert
			Assert.AreEqual(elementFrom, period.From);
			Assert.AreEqual(elementUntil, period.Until);
		}
		
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestTimePeriod_With_Null_As_FromDate_And_UntilDate()
		{
			// Arrange
			var timeline = new Mock<Timeline>(null);
			DateTime dtUntil = DateTime.Now;
			TimeElement elementUntil = new TimeElement(dtUntil, false);

			// Act
			new TimePeriod(timeline.Object, null, elementUntil);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestTimePeriod_With_FromDate_And_Null_As_UntilDate()
		{
			// Arrange
			var timeline = new Mock<Timeline>(null);
			DateTime dtFrom = DateTime.Now;
			TimeElement elementFrom = new TimeElement(dtFrom, true);

			// Act
			new TimePeriod(timeline.Object, elementFrom, null);
		}

		[Test]
		public void TestTimePeriod_With_FromDate_And_Same_UntilDate()
		{
			// Arrange
			var timeline = new Mock<Timeline>(null);
			DateTime dtFrom = DateTime.Now;
			TimeElement elementFrom = new TimeElement(dtFrom, true) { IsUntil = true };

			// Act
			TimePeriod period = new TimePeriod(timeline.Object, elementFrom, elementFrom);

			// Assert
			Assert.AreEqual(elementFrom, period.From);
			Assert.AreEqual(elementFrom, period.Until);
		}

		[Test]
		[ExpectedException(typeof(Exception))]
		public void TestTimePeriod_With_FromDate_And_Same_Invalid_UntilDate()
		{
			// Arrange
			var timeline = new Mock<Timeline>(null);
			DateTime dtFrom = DateTime.Now;
			TimeElement elementFrom = new TimeElement(dtFrom, true);

			// Act
			new TimePeriod(timeline.Object, elementFrom, elementFrom);
		}

		[Test]
		[ExpectedException(typeof(Exception))]
		public void TestTimePeriod_With_Invalid_FromDate_And_Same_UntilDate()
		{
			// Arrange
			var timeline = new Mock<Timeline>(null);
			DateTime dtFrom = DateTime.Now;
			TimeElement elementFrom = new TimeElement(dtFrom, false) { IsUntil = true };

			// Act
			new TimePeriod(timeline.Object, elementFrom, elementFrom);
		}

		[Test]
		public void TestTimeSources()
		{
			// Arrange
			var timeline = new Mock<Timeline>(null);
			var timeSourceMock1 = new Mock<ITimeSource>();
			var timeSourceMock2 = new Mock<ITimeSource>();
			IList<ITimeSource> listTimeSource = new List<ITimeSource> { timeSourceMock1.Object, timeSourceMock2.Object }; 
			TimePeriod period = new TimePeriod(timeline.Object, new TimeElement(DateTime.Now, true)) { TimeSources = listTimeSource };

			// Act

			// Assert
			Assert.IsNotNull(period.TimeSourcesView);
			Assert.AreEqual(2, period.TimeSourcesView.Count);
		}

		[Test]
		public void TestDuration()
		{
			// Arrange
			var timeline = new Mock<Timeline>(null);
			DateTime dtFrom = DateTime.Now;
			TimeElement elementFrom = new TimeElement(dtFrom, true);
			DateTime dtUntil = dtFrom.AddMonths(1);
			TimeElement elementUntil = new TimeElement(dtUntil, false);
			TimePeriod timePeriod = new TimePeriod(timeline.Object, elementFrom, elementUntil);

			// Act
			Duration duration = timePeriod.Duration;

			// Assert
			Assert.IsNotNull(duration);
			Assert.IsTrue(duration.IsDuration);
			Assert.AreEqual(dtFrom, duration.From);
			Assert.AreEqual(dtUntil, duration.Until);
		}

		[Test]
		public void TestDuration_With_Identical_From_And_Until()
		{
			// Arrange
			var timeline = new Mock<Timeline>(null);
			DateTime dtFrom = DateTime.Now;
			TimeElement elementFrom = new TimeElement(dtFrom, true) { IsUntil = true };
			TimePeriod timePeriod = new TimePeriod(timeline.Object, elementFrom, elementFrom);

			// Act
			Duration duration = timePeriod.Duration;

			// Assert
			Assert.IsNotNull(duration);
			Assert.IsTrue(duration.IsDuration);
			Assert.AreEqual(dtFrom, duration.From);
			Assert.AreEqual(dtFrom, duration.Until);
		}

		[Test]
		[ExpectedException(typeof(Exception))]
		public void TestDuration_With_Invalid_State()
		{
			// Arrange
			var timeline = new Mock<Timeline>(null);
			DateTime dtFrom = DateTime.Now;
			TimeElement elementFrom = new TimeElement(dtFrom, true);
			TimePeriod timePeriod = new TimePeriod(timeline.Object, elementFrom, elementFrom);

			// Act
			var duration = timePeriod.Duration;

			// Assert
			Assert.Fail("Duration should throw an exception and the return value here should be null.", duration);
		}

		[Test]
		public void TestDuration_With_Only_From()
		{
			// Arrange
			var timeline = new Mock<Timeline>(null);
			DateTime dtFrom = DateTime.Now;
			TimeElement elementFrom = new TimeElement(dtFrom, true);
			TimePeriod timePeriod = new TimePeriod(timeline.Object, elementFrom);

			// Act
			Duration duration = timePeriod.Duration;

			// Assert
			Assert.IsNotNull(duration);
			Assert.IsFalse(duration.IsDuration);
			Assert.AreEqual(dtFrom, duration.From);
		}
	}
}
