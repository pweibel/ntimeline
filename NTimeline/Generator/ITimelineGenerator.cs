using EWeibel.NTimeline.Context;

namespace EWeibel.NTimeline.Generator
{
	public interface ITimelineGenerator
	{
		ITimeContext Context { get; }
	}
}
