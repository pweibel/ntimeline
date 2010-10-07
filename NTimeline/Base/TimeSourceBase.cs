using System;
using System.Collections.Generic;

namespace NTimeline.Base
{
	public abstract class TimeSourceBase : ITimeSource
	{
		#region Properties
		public Timeline Timeline
		{
			get; private set;
		}
		#endregion

		#region Constructors
		protected TimeSourceBase(Timeline timeline)
		{
			if(timeline == null) throw new ArgumentNullException("timeline");

			this.Timeline = timeline;
			this.Timeline.AddTimeSource(this);
		}
		#endregion

		#region Abstracts
		public abstract IList<TimeElement> CreateTimeElements();

		public abstract bool IsValid(Duration duration);
		#endregion
	}
}
