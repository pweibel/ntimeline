using System;

using EWeibel.NTimeline.Helpers;

using NUnit.Framework;

namespace EWeibel.NTimeline.Test.Helpers
{
	[TestFixture]
	public class DurationTest
	{
		#region Tests
		[Test]
		public void TestDuration_With_Only_A_From_Date()
		{
			// Arrange
			DateTime dtFrom = DateTime.Now;
			
			// Act
			Duration duration = new Duration(dtFrom);

			// Assert
			Assert.AreEqual(dtFrom, duration.From);
			Assert.IsNull(duration.Until);
			Assert.IsFalse(duration.IsDuration);
		}

		[Test]
		public void TestDuration_With_A_From_Date_And_An_Until_Date()
		{
			// Arrange
			DateTime dtFrom = DateTime.Now;
			DateTime dtUntil = dtFrom.AddMonths(1);

			// Act
			Duration duration = new Duration(dtFrom, dtUntil);

			// Assert
			Assert.AreEqual(dtFrom, duration.From);
			Assert.AreEqual(dtUntil, duration.Until);
			Assert.IsTrue(duration.IsDuration);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestDuration_With_A_From_Date_And_An_Invalid_Until_Date()
		{
			// Arrange
			DateTime dtFrom = DateTime.Now;
			DateTime dtUntil = dtFrom.AddMonths(-1);

			// Act
			new Duration(dtFrom, dtUntil);
		}
		#endregion
	}
}
