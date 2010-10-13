using System;

using EWeibel.NTimeline.Core;
using EWeibel.NTimeline.Generator;

using Moq;

using NUnit.Framework;

namespace EWeibel.NTimeline.Test.Core
{
	[TestFixture]
	public class TimelineTest
	{
		[Test]
		public void TestTimeline()
		{
			// Arrange
			var generator = new Mock<ITimelineGenerator>();

			// Act
			Timeline timeline = new Timeline(generator.Object);

			// Assert
			Assert.AreEqual(generator.Object, timeline.TimelineGenerator);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestTimeline_With_Null_As_Parameter()
		{
			// Act
			new Timeline(null);
		}
	}
}
