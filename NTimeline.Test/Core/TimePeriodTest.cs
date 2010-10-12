using System;

using NTimeline.Core;

using NUnit.Framework;

namespace NTimeline.Test.Core
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
	}
}
