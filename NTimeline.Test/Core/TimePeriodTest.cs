using System;
using Moq;
using NTimeline.Core;
using NTimeline.Helpers;
using NUnit.Framework;

namespace NTimeline.Test.Core
{
	[TestFixture]
	public class TimePeriodTest
	{
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestTimePeriod_With_No_From_And_Duration()
		{
			// Act
			new TimePeriod(null,null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestTimePeriod_With_No_From_And_Until_And_Duration()
		{
			// Act
			new TimePeriod(null, null, null);
		}

		[Test]
		public void TestTimePeriod_With_FromDate()
		{
			// Arrange
			DateTime dtFrom = DateTime.Now;
			TimeElement elementFrom = new TimeElement(dtFrom, true);
			var duration = new Mock<Duration>(DateTime.Now);

			// Act
			TimePeriod period = new TimePeriod(elementFrom, duration.Object);

			// Assert
			Assert.AreEqual(elementFrom, period.From);
			Assert.IsNull(period.Until);
		}

		[Test]
		public void TestTimePeriod_With_FromDate_And_UntilDate()
		{
			// Arrange
			DateTime dtFrom = DateTime.Now;
			TimeElement elementFrom = new TimeElement(dtFrom, true);
			DateTime dtUntil = dtFrom.AddMonths(1);
			TimeElement elementUntil = new TimeElement(dtUntil, false);
			var duration = new Mock<Duration>(DateTime.Now);

			// Act
			TimePeriod period = new TimePeriod(elementFrom, elementUntil, duration.Object);

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
			var duration = new Mock<Duration>(DateTime.Now);

			// Act
			new TimePeriod(null, elementUntil, duration.Object);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestTimePeriod_With_FromDate_And_Null_As_UntilDate()
		{
			// Arrange
			DateTime dtFrom = DateTime.Now;
			TimeElement elementFrom = new TimeElement(dtFrom, true);
			var duration = new Mock<Duration>(DateTime.Now);

			// Act
			new TimePeriod(elementFrom, null, duration.Object);
		}

		[Test]
		public void TestTimePeriod_With_FromDate_And_Same_UntilDate()
		{
			// Arrange
			DateTime dtFrom = DateTime.Now;
			TimeElement elementFrom = new TimeElement(dtFrom, true) { IsUntil = true };
			var duration = new Mock<Duration>(DateTime.Now);

			// Act
			TimePeriod period = new TimePeriod(elementFrom, elementFrom, duration.Object);

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
			var duration = new Mock<Duration>(DateTime.Now);

			// Act
			new TimePeriod(elementFrom, elementFrom, duration.Object);
		}

		[Test]
		[ExpectedException(typeof(Exception))]
		public void TestTimePeriod_With_Invalid_FromDate_And_Same_UntilDate()
		{
			// Arrange
			DateTime dtFrom = DateTime.Now;
			TimeElement elementFrom = new TimeElement(dtFrom, false) { IsUntil = true };
			var duration = new Mock<Duration>(DateTime.Now);

			// Act
			new TimePeriod(elementFrom, elementFrom, duration.Object);
		}

		[Test]
		public void TestDuration()
		{
			// Arrange
			DateTime dtFrom = DateTime.Now;
			TimeElement elementFrom = new TimeElement(dtFrom, true);
			DateTime dtUntil = dtFrom.AddMonths(1);
			TimeElement elementUntil = new TimeElement(dtUntil, false);
			var duration = new Mock<Duration>(DateTime.Now);
			TimePeriod timePeriod = new TimePeriod(elementFrom, elementUntil, duration.Object);

			// Act
			Duration duration1 = timePeriod.Duration;

			// Assert
			Assert.IsNotNull(duration1);
			Assert.AreEqual(duration.Object, duration1);
		}
	}
}
