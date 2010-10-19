using System;

using NTimeline.Core;

using NUnit.Framework;

namespace NTimeline.Test.Core
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
	}
}
