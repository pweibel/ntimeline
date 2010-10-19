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
			get
			{
				if(this.From == this.Until && !(this.From.IsFrom && this.From.IsUntil)) throw new Exception("Invalid state.");

				// Special case: From and until date are the same
				if(this.From == this.Until) return new Duration(this.From.Date, this.Until.Date);

				return this.Until == null ? new Duration(this.FromPeriodDate) : new Duration(this.FromPeriodDate, this.UntilPeriodDate);
			}
		}

		/// <summary>
		/// Returns the relevant from date for the period.
		/// </summary>
		/// <returns>Correct from Date for the period</returns>
		private DateTime FromPeriodDate
		{
			get
			{
				DateTime dtPeriodDate = this.From.Date;

				if (this.From.IsUntil) dtPeriodDate = dtPeriodDate.AddDays(1);

				return dtPeriodDate;
			}
		}

		/// <summary>
		/// Returns the relevant until date for the period.
		/// </summary>
		/// <returns>Correct until Date for the period</returns>
		private DateTime UntilPeriodDate
		{
			get
			{
				DateTime dtPeriodDate = this.Until.Date;

				if (this.Until.IsFrom) dtPeriodDate = dtPeriodDate.AddDays(-1);

				return dtPeriodDate;
			}
		}
		#endregion

		#region Constructors
		public TimePeriod(TimeElement timeElementFrom)
		{
			if(timeElementFrom == null) throw new ArgumentNullException("timeElementFrom");

			this.timeElementFrom = timeElementFrom;
		}

		public TimePeriod(TimeElement timeElementFrom, TimeElement timeElementUntil) : this(timeElementFrom)
		{
			if(timeElementFrom == null) throw new ArgumentNullException("timeElementFrom");
			if(timeElementUntil == null) throw new ArgumentNullException("timeElementUntil");
			if(timeElementFrom == timeElementUntil && !(timeElementUntil.IsFrom && timeElementUntil.IsUntil)) throw new Exception("Invalid state.");

			this.timeElementUntil = timeElementUntil;
		}
		#endregion
	}
}
