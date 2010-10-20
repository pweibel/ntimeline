using System;
using System.Collections.Generic;
using NTimeline.Core;
using NTimeline.Helpers;
using NTimeline.Source;
using NTimeline.Visitor;
using NUnit.Framework;

namespace NTimeline.Test.Visitor
{
    [TestFixture]
    public class TimelineVisitorTest
    {
        [Test]
        public void TestAccept()
        {
            // Arrange
            Timeline timeline = new Timeline();
            TestSource source = new TestSource(timeline);
            timeline.Generate();
            ConsoleVisitor visitor = new ConsoleVisitor();

            // Act
            timeline.Accept(visitor);

            // Assert
            Assert.AreEqual(2, timeline.BuildTimePeriods().Count);
        }

        #region Private Class
        private class ConsoleVisitor : ITimelineVisitor
        {
            public void Visit(TimePeriod period)
            {
                Console.WriteLine("Visit period {0}", period);
            }
        }

        private class TestSource : TimeSourceBase
        {
            public TestSource(Timeline timeline) : base(timeline)
            {
            }

            public override IList<TimeElement> CreateTimeElements()
            {
                return new List<TimeElement>
                           {
                               new TimeElement(new DateTime(2010, 1, 1), true),
                               new TimeElement(new DateTime(2010, 3, 1), true)
                           };
            }

            public override bool IsValid(Duration duration)
            {
                return true;
            }
        }
        #endregion
    }
}
