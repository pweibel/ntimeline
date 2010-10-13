using System.Collections.Generic;

using EWeibel.NTimeline.Core;
using EWeibel.NTimeline.Helpers;

namespace EWeibel.NTimeline.Source
{
	/// <summary>
	/// A time source provides data for the timeline.
	/// </summary>
	public interface ITimeSource
	{
		/// <summary>
		/// Timeline where this source is registered.
		/// </summary>
		Timeline Timeline { get; }

		/// <summary>
		/// Creates a list of time elements.
		/// </summary>
		/// <returns>List of time elements. If there are no elements available, the list will be empty.</returns>
		IList<TimeElement> CreateTimeElements();

		/// <summary>
		/// The timeline calls this method to check, if the time source has valid elements during the period.
		/// </summary>
		/// <param name="duration">Period</param>
		/// <returns>Returns true if the time source is valid during the period.</returns>
		bool IsValid(Duration duration);
	}
}
