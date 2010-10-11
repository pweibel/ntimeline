using System;

using NTimeline.Core;

using NUnit.Framework;

namespace NTimeline.Test.Core
{
	[TestFixture]
	public class TimeElementTest
	{
		[Test]
		public void TestTimeElement()
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
	}
}
