using System;

using EWeibel.NTimeline.Core;

using NUnit.Framework;

namespace EWeibel.NTimeline.Test.Core
{
	[TestFixture]
	public class TimeElementTest
	{
		[Test]
		public void TestTimeElement_As_FromDate()
		{
			// Arrange
			DateTime dtDate = DateTime.Now;

			// Act
			TimeElement element = new TimeElement(dtDate, true);

			// Assert
			Assert.AreEqual(dtDate, element.Date);
			Assert.IsTrue(element.IsFrom);
			Assert.IsFalse(element.IsUntil);
		}

		[Test]
		public void TestTimeElement_As_UntilDate()
		{
			// Arrange
			DateTime dtDate = DateTime.Now;

			// Act
			TimeElement element = new TimeElement(dtDate, false);

			// Assert
			Assert.AreEqual(dtDate, element.Date);
			Assert.IsFalse(element.IsFrom);
			Assert.IsTrue(element.IsUntil);
		}

		[Test]
		public void TestFromPeriodDate()
		{
			// Arrange
			DateTime dtDate = DateTime.Now;
			TimeElement element = new TimeElement(dtDate, true);

			// Act
			DateTime dtFromDatePeriod = element.FromPeriodDate;

			// Assert
			Assert.AreEqual(dtDate, dtFromDatePeriod);
		}

		[Test]
		public void TestFromPeriodDate_When_Date_Is_UntilDate()
		{
			// Arrange
			DateTime dtDate = DateTime.Now;
			TimeElement element = new TimeElement(dtDate, false);

			// Act
			DateTime dtFromDatePeriod = element.FromPeriodDate;

			// Assert
			Assert.AreEqual(dtDate.AddDays(1), dtFromDatePeriod);
		}

		[Test]
		public void TestUntilPeriodDate()
		{
			// Arrange
			DateTime dtDate = DateTime.Now;
			TimeElement element = new TimeElement(dtDate, false);

			// Act
			DateTime dtUntilDatePeriod = element.UntilPeriodDate;

			// Assert
			Assert.AreEqual(dtDate, dtUntilDatePeriod);
		}

		[Test]
		public void TestUntilPeriodDate_When_Date_Is_FromDate()
		{
			// Arrange
			DateTime dtDate = DateTime.Now;
			TimeElement element = new TimeElement(dtDate, true);

			// Act
			DateTime dtUntilDatePeriod = element.UntilPeriodDate;

			// Assert
			Assert.AreEqual(dtDate.AddDays(-1), dtUntilDatePeriod);
		}
	}
}
