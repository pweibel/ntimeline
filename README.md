# NTimeline

## What is NTimeline?
NTimeline is a library to build a timeline which has several source which deliver points of time or durations.

## Where can I use NTimeline?
NTimeline is a library which is designed to be uses in your business layer.

## What does NTimeline?
Here some of the key operations of NTimeline:
* Collect points of time
* Collect durations
* Sort all collected data
* Get time slices from the created timeline

## What are typical use cases for NTimeline?
* Calculate rates upon time slices
* Calculate a timeline upon points of time or time periods given by time sources

## How to create a timeline?
It's quite easy. First you have to create a time source. NTimeline gives you an abstract base class called TimeSourceBase which implements the interface ITimeSource:
{code:c#}
public class TestSource : TimeSourceBase
{
    public override IList<TimeElement> CreateTimeElements()
    {
        return new List<TimeElement> {
            new TimeElement(new DateTime(2010, 1, 1), true),
            new TimeElement(new DateTime(2010, 3, 1), true) };
    }

    public override bool IsValid(Duration duration)
    {
        // Dummy implementation
        return true;
    }
}
{code:c#}

Now you can already build a timeline:
{code:c#}
TestSource source = new TestSource();
Timeline timeline = new Timeline();
timeline.AddTimeSource(source);
timeline.Build();
{code:c#}

## How to walk through a timeline?
That is also quite easy. First you have create your visitor. NTimeline provide the interface ITimelineVisitor, so your class has to implement this interface.
{code:c#}
public class ConsoleVisitor : ITimelineVisitor
{
    public void Visit(TimePeriod period)
    {
        Console.WriteLine("Visit period {0}", period);
    }
}
{code:c#}
Now you have to call the following method on your timeline instance:
{code:c#}
ConsoleVisitor visitor = new ConsoleVisitor();
timeline.VisitTimePeriods(visitor);
{code:c#}
