using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
		private readonly Timeline timeline;
		private readonly TimeElement timeElementFrom;
		private readonly TimeElement timeElementUntil;
		private IList<ITimeSource> listTimeSources = new List<ITimeSource>();
		private ReadOnlyCollection<ITimeSource> listTimeSourcesView; 
		#endregion

		#region Properties
		public Timeline Timeline
		{
			get { return timeline; }
		}
		
		public TimeElement From
		{
			get { return timeElementFrom; }
		}

		public TimeElement Until
		{
			get { return timeElementUntil; }
		}

		public ReadOnlyCollection<ITimeSource> TimeSourcesView
		{
			get { return this.listTimeSourcesView ?? (this.listTimeSourcesView = new ReadOnlyCollection<ITimeSource>(this.listTimeSources)); }
		}

		public IList<ITimeSource> TimeSources
		{
			set
			{
				this.listTimeSources = value;
			}
		}

		/// <summary>
		/// Returns a Duration which represents the period between the two time elements.
		/// </summary>
		/// <returns>Duration</returns>
		public Duration Duration
		{
			get
			{
				if(this.From == this.Until && !(this.From.IsFrom && this.From.IsUntil)) throw new Exception("Invalid state.");

				// Special case: From and until date are the same
				if(this.From == this.Until) return new Duration(this.From.Date, this.Until.Date);

				return this.Until == null ? new Duration(this.From.FromPeriodDate) : new Duration(this.From.FromPeriodDate, this.Until.UntilPeriodDate);
			}
		}
		#endregion

		#region Constructors
		public TimePeriod(Timeline timeline, TimeElement timeElementFrom)
		{
			if(timeline == null) throw new ArgumentNullException("timeline");
			if(timeElementFrom == null) throw new ArgumentNullException("timeElementFrom");

			this.timeline = timeline;
			this.timeElementFrom = timeElementFrom;
		}

		public TimePeriod(Timeline timeline, TimeElement timeElementFrom, TimeElement timeElementUntil) : this(timeline, timeElementFrom)
		{
			if(timeline == null) throw new ArgumentNullException("timeline");
			if(timeElementFrom == null) throw new ArgumentNullException("timeElementFrom");
			if(timeElementUntil == null) throw new ArgumentNullException("timeElementUntil");
			if(timeElementFrom == timeElementUntil && !(timeElementUntil.IsFrom && timeElementUntil.IsUntil)) throw new Exception("Invalid state.");

			this.timeElementUntil = timeElementUntil;
		}
		#endregion
	}
}
