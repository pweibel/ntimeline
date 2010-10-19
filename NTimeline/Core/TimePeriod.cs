using System;
using System.Collections.Generic;

using NTimeline.Helpers;
using NTimeline.Source;

namespace NTimeline.Core
{
	/// <summary>
	/// A time period represents a segment between to time elements on the timeline.
	/// If there isn't a second time element, that means that the period is endless.
	/// </summary>
	public class TimePeriod
	{
		#region Fields
		private readonly TimeElement timeElementFrom;
		private readonly TimeElement timeElementUntil;
	    private readonly Duration duration;
		private IList<ITimeSource> listTimeSources = new List<ITimeSource>();
		#endregion

		#region Properties
		public TimeElement From
		{
			get { return timeElementFrom; }
		}

		public TimeElement Until
		{
			get { return timeElementUntil; }
		}

		public IList<ITimeSource> TimeSources
		{
		    get { return this.listTimeSources; }
		    set { this.listTimeSources = value; }
		}

		/// <summary>
		/// Returns a Duration which represents the period between the two time elements.
		/// </summary>
		/// <returns>Duration</returns>
		public Duration Duration
		{
			get { return duration; }
		}
		#endregion

		#region Constructors
		public TimePeriod(TimeElement timeElementFrom, Duration duration)
		{
			if(timeElementFrom == null) throw new ArgumentNullException("timeElementFrom");
		    if(duration == null) throw new ArgumentNullException("duration");

		    this.timeElementFrom = timeElementFrom;
		    this.duration = duration;
		}

		public TimePeriod(TimeElement timeElementFrom, TimeElement timeElementUntil, Duration duration) : this(timeElementFrom, duration)
		{
			if(timeElementFrom == null) throw new ArgumentNullException("timeElementFrom");
			if(timeElementUntil == null) throw new ArgumentNullException("timeElementUntil");
		    if(duration == null) throw new ArgumentNullException("duration");
		    if(timeElementFrom == timeElementUntil && !(timeElementUntil.IsFrom && timeElementUntil.IsUntil)) throw new Exception("Invalid state.");

			this.timeElementUntil = timeElementUntil;
		}
		#endregion
	}
}
