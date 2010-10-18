using NTimeline.Context;
using NTimeline.Core;

namespace NTimeline.Visitor
{
	public interface ITimelineVisitor
	{
		IContext Context { get; set; }
		void Visit(TimePeriod period);
	}
}
