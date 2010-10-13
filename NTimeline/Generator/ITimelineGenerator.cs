using NTimeline.Context;

namespace NTimeline.Generator
{
	public interface ITimelineGenerator
	{
		ITimeContext Context { get; }
	}
}
