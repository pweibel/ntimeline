using NTimeline.Core;

namespace NTimeline.Visitor
{
	public interface ITimelineVisitor
	{
		void Visit(TimePeriod period);
	}
}
