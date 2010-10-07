﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NTimeline
{
	/// <summary>
	/// A time period represents a segment between to time elements on the timeline.
	/// If there isn't a second time element, that means that the period is endless.
	/// </summary>
	public class TimePeriod
	{
		#region Fields
		private readonly TimeElement _timeElementFrom;
		private readonly TimeElement _timeElementUntil;
		private IList<ITimeSource> _listTimeSources = new List<ITimeSource>();
		private ReadOnlyCollection<ITimeSource> _listTimeSourcesView; 
		#endregion

		#region Properties
		public TimeElement From
		{
			get { return _timeElementFrom; }
		}

		public TimeElement Until
		{
			get { return _timeElementUntil; }
		}

		public ReadOnlyCollection<ITimeSource> TimeSourcesView
		{
			get { return _listTimeSourcesView ?? (_listTimeSourcesView = new ReadOnlyCollection<ITimeSource>(_listTimeSources)); }
		}

		public IList<ITimeSource> TimeSources
		{
			set
			{
				_listTimeSources = value;
			}
		}
		#endregion

		#region Constructors
		public TimePeriod(TimeElement timeElementFrom)
		{
			if(timeElementFrom == null) throw new ArgumentNullException("timeElementFrom");
			_timeElementFrom = timeElementFrom;
		}

		public TimePeriod(TimeElement timeElementFrom, TimeElement timeElementUntil) : this(timeElementFrom)
		{
			if(timeElementFrom == null) throw new ArgumentNullException("timeElementFrom");
			if(timeElementUntil == null) throw new ArgumentNullException("timeElementUntil");
			if(timeElementFrom == timeElementUntil && !(timeElementUntil.IsFrom && timeElementUntil.IsUntil))
				throw new Exception("Invalid state.");

			_timeElementUntil = timeElementUntil;
		}
		#endregion

		#region Publics
		/// <summary>
		/// Returns a Duration which represents the period between the two time elements.
		/// </summary>
		/// <returns>Duration</returns>
		public Duration GetPeriod()
		{
			if(this.From == this.Until && !(this.From.IsFrom && this.From.IsUntil))
				throw new Exception("Invalid state.");

			// Special case: From and until date are the same
			if(this.From == this.Until) return new Duration(this.From.Date, this.Until.Date);

			DateTime dtFrom = this.From.GetPeriodDate(true);

			Duration duration;
			if(this.Until == null)
			{
				duration = new Duration(dtFrom);
			}
			else
			{
				DateTime dtUntil = this.Until.GetPeriodDate(false);
				duration = new Duration(dtFrom, dtUntil);
			}

			return duration;
		}
		#endregion
	}
}
