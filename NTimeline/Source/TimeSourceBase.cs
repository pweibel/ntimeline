using System.Collections.Generic;

using NTimeline.Core;
using NTimeline.Helpers;

namespace NTimeline.Source
{
	public abstract class TimeSourceBase : ITimeSource
	{
		#region Properties
		public Timeline Timeline
		{
			get; set;
		}
		#endregion

		#region Abstracts
		public abstract IList<TimeElement> CreateTimeElements();

		public abstract bool IsValid(Duration duration);
		#endregion
	}
}
