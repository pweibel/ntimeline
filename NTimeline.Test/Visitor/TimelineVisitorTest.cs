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
            TestSource source = new TestSource();
            Timeline timeline = new Timeline();
            timeline.AddTimeSource(source);
            timeline.Build();
            ConsoleVisitor visitor = new ConsoleVisitor();

            // Act
            timeline.Accept(visitor);

            // Assert
            Assert.AreEqual(2, timeline.TimePeriods.Count);
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
