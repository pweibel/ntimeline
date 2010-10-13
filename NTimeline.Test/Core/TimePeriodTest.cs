using System;
using System.Collections.Generic;

using EWeibel.NTimeline.Core;
using EWeibel.NTimeline.Helpers;
using EWeibel.NTimeline.Source;

using Moq;

using NUnit.Framework;

namespace EWeibel.NTimeline.Test.Core
{
	[TestFixture]
	public class TimePeriodTest
	{
		[Test]
		public void TestTimePeriod_With_Only_FromDate()
		{
			// Arrange
			DateTime dtFrom = DateTime.Now;
			TimeElement elementFrom = new TimeElement(dtFrom, true);

			// Act
			TimePeriod period = new TimePeriod(elementFrom);

			// Assert
			Assert.AreEqual(elementFrom, period.From);
			Assert.IsNull(period.Until);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestTimePeriod_With_Null_As_FromDate()
		{
			// Act
			new TimePeriod(null);
		}

		[Test]
		public void TestTimePeriod_With_FromDate_And_UntilDate()
		{
			// Arrange
			DateTime dtFrom = DateTime.Now;
			TimeElement elementFrom = new TimeElement(dtFrom, true);
			DateTime dtUntil = dtFrom.AddMonths(1);
			TimeElement elementUntil = new TimeElement(dtUntil, false);

			// Act
			TimePeriod period = new TimePeriod(elementFrom, elementUntil);

			// Assert
			Assert.AreEqual(elementFrom, period.From);
			Assert.AreEqual(elementUntil, period.Until);
		}
		
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestTimePeriod_With_Null_As_FromDate_And_UntilDate()
		{
			// Arrange
			DateTime dtUntil = DateTime.Now;
			TimeElement elementUntil = new TimeElement(dtUntil, false);

			// Act
			new TimePeriod(null, elementUntil);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestTimePeriod_With_FromDate_And_Null_As_UntilDate()
		{
			// Arrange
			DateTime dtFrom = DateTime.Now;
			TimeElement elementFrom = new TimeElement(dtFrom, true);

			// Act
			new TimePeriod(elementFrom, null);
		}

		[Test]
		public void TestTimePeriod_With_FromDate_And_Same_UntilDate()
		{
			// Arrange
			DateTime dtFrom = DateTime.Now;
			TimeElement elementFrom = new TimeElement(dtFrom, true);
			elementFrom.IsUntil = true;

			// Act
			TimePeriod period = new TimePeriod(elementFrom, elementFrom);

			// Assert
			Assert.AreEqual(elementFrom, period.From);
			Assert.AreEqual(elementFrom, period.Until);
		}

		[Test]
		[ExpectedException(typeof(Exception))]
		public void TestTimePeriod_With_FromDate_And_Same_Invalid_UntilDate()
		{
			// Arrange
			DateTime dtFrom = DateTime.Now;
			TimeElement elementFrom = new TimeElement(dtFrom, true);

			// Act
			new TimePeriod(elementFrom, elementFrom);
		}

		[Test]
		[ExpectedException(typeof(Exception))]
		public void TestTimePeriod_With_Invalid_FromDate_And_Same_UntilDate()
		{
			// Arrange
			DateTime dtFrom = DateTime.Now;
			TimeElement elementFrom = new TimeElement(dtFrom, false);
			elementFrom.IsUntil = true;

			// Act
			new TimePeriod(elementFrom, elementFrom);
		}

		[Test]
		public void TestTimeSources()
		{
			// Arrange
			var timeSourceMock1 = new Mock<ITimeSource>();
			var timeSourceMock2 = new Mock<ITimeSource>();
			IList<ITimeSource> listTimeSource = new List<ITimeSource> { timeSourceMock1.Object, timeSourceMock2.Object }; 
			TimePeriod period = new TimePeriod(new TimeElement(DateTime.Now, true));

			// Act
			period.TimeSources = listTimeSource;

			// Assert
			Assert.IsNotNull(period.TimeSourcesView);
			Assert.AreEqual(2, period.TimeSourcesView.Count);
		}

		[Test]
		public void TestDuration()
		{
			// Arrange
			DateTime dtFrom = DateTime.Now;
			TimeElement elementFrom = new TimeElement(dtFrom, true);
			DateTime dtUntil = dtFrom.AddMonths(1);
			TimeElement elementUntil = new TimeElement(dtUntil, false);
			TimePeriod timePeriod = new TimePeriod(elementFrom, elementUntil);

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
			DateTime dtFrom = DateTime.Now;
			TimeElement elementFrom = new TimeElement(dtFrom, true);
			elementFrom.IsUntil = true;
			TimePeriod timePeriod = new TimePeriod(elementFrom, elementFrom);

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
			DateTime dtFrom = DateTime.Now;
			TimeElement elementFrom = new TimeElement(dtFrom, true);
			TimePeriod timePeriod = new TimePeriod(elementFrom, elementFrom);

			// Act
			Duration duration = timePeriod.Duration;
		}

		[Test]
		public void TestDuration_With_Only_From()
		{
			// Arrange
			DateTime dtFrom = DateTime.Now;
			TimeElement elementFrom = new TimeElement(dtFrom, true);
			TimePeriod timePeriod = new TimePeriod(elementFrom);

			// Act
			Duration duration = timePeriod.Duration;

			// Assert
			Assert.IsNotNull(duration);
			Assert.IsFalse(duration.IsDuration);
			Assert.AreEqual(dtFrom, duration.From);
		}
	}
}
