using System;
using System.Collections.Generic;
using System.Linq;

using NTimeline.Context;
using NTimeline.Helpers;
using NTimeline.Source;
using NTimeline.Visitor;

namespace NTimeline.Core
{
	public class Timeline
	{
		#region Fields
		private readonly IList<ITimeSource> listTimeSources = new List<ITimeSource>();
		private readonly SortedList<DateTime, TimeElement> listTimeElements = new SortedList<DateTime, TimeElement>();
		#endregion

		#region Properties
		public IContext Context { get; private set; }

		private IList<ITimeSource> TimeSources
		{
			get { return this.listTimeSources; }
		}

		private SortedList<DateTime, TimeElement> TimeElements
		{
			get { return this.listTimeElements;  }
		}
		#endregion

		#region Constructors
		public Timeline(IContext context)
		{
			this.Context = context;
		}
		#endregion
		
		#region Publics
		/// <summary>
		/// Adds a time source.
		/// </summary>
		/// <param name="timeSource">new time source</param>
		public void AddTimeSource(ITimeSource timeSource)
		{
			if(timeSource == null) throw new ArgumentNullException("timeSource");

			if(!this.TimeSources.Contains(timeSource))
			{
				this.TimeSources.Add(timeSource);
			}
		}

		/// <summary>
		/// Removes a time source from the timeline
		/// </summary>
		/// <param name="timeSource">Time source which has to be removed</param>
		public void RemoveTimeSource(ITimeSource timeSource)
		{
			if(timeSource == null) throw new ArgumentNullException("timeSource");

			this.TimeSources.Remove(timeSource);
		}

		/// <summary>
		/// Get all the time elements of every time source and add them to the timeline.
		/// </summary>
		public void Generate()
		{
			// Clear current time elements
			this.TimeElements.Clear();

			// Add all time elements of every time source
			foreach(ITimeSource timeSource in this.TimeSources)
			{
				IList<TimeElement> listTimeElement = timeSource.CreateTimeElements();
				if(listTimeElement == null) continue;
				foreach(TimeElement timeElement in listTimeElement)
				{
					AddOrCompleteTimeElement(timeElement);
				}
			}
		}

		/// <summary>
		/// Returns a list of time periodes.
		/// </summary>
		/// <returns>List with all time periods. If there are no entries on the timeline, then an empty list will be returned.</returns>
		public IList<TimePeriod> BuildTimePeriods()
		{
			IList<TimePeriod> listTimePeriod = new List<TimePeriod>();

			if(this.TimeElements.Values.Count == 0) return listTimePeriod;

			for(int i = 0; i < this.TimeElements.Values.Count; i++)
			{
				// If the current time element is a From and a Until date, then a one day period has to be created.
				if(this.TimeElements.Values[i].IsFrom && this.TimeElements.Values[i].IsUntil)
				{
					// Create time period for one day
					TimePeriod timePeriodOneDay = CreateTimePeriod(this.TimeElements.Values[i], this.TimeElements.Values[i]);
					if(timePeriodOneDay != null && !listTimePeriod.Contains(timePeriodOneDay)) listTimePeriod.Add(timePeriodOneDay);
				}

				TimeElement timeElementFrom = this.TimeElements.Values[i];

				// The second element will only be set, if there exists another one.
				TimeElement timeElementUntil = i < this.TimeElements.Count - 1 ? this.TimeElements.Values[i + 1] : null;

				// if the second time element follows directly after the first time element, there is nothing to do.
				if(timeElementUntil != null && timeElementFrom.Date.AddDays(1) == timeElementUntil.Date && timeElementFrom.IsUntil && timeElementUntil.IsFrom) continue;

				// Create time period
				TimePeriod timePeriod = CreateTimePeriod(timeElementFrom, timeElementUntil);
				if(timePeriod != null && !listTimePeriod.Contains(timePeriod)) listTimePeriod.Add(timePeriod);
			}

			return listTimePeriod;
		}

		/// <summary>
		/// Returns a list with time periods which are valid during the date
		/// </summary>
		/// <param name="dtDate">Date</param>
		/// <returns></returns>
		public IList<TimePeriod> BuildTimePeriods(DateTime dtDate)
		{
			return (from timePeriod in BuildTimePeriods()
			        let duration = timePeriod.Duration
			        where duration.From <= dtDate && (duration.Until == null || dtDate <= duration.Until)
			        select timePeriod).ToList();
		}

		/// <summary>
		/// Loops through all time periods of the timeline in chronological order.
		/// </summary>
		/// <param name="timelineVisitor">Visitor</param>
		public void Accept(ITimelineVisitor timelineVisitor)
		{
			if(timelineVisitor == null) throw new ArgumentNullException("timelineVisitor");

			foreach(TimePeriod period in BuildTimePeriods())
			{
				timelineVisitor.Visit(period);
			}
		}

		/// <summary>
		/// Loops through all time periods of the timeline in chronological order starting at the given Date.
		/// </summary>
		/// <param name="timelineVisitor">Visitor</param>
		/// <param name="dtDate">All valid and future periods will be visited</param>
		public void Accept(ITimelineVisitor timelineVisitor, DateTime dtDate)
		{
			if(timelineVisitor == null) throw new ArgumentNullException("timelineVisitor");

			foreach(TimePeriod period in BuildTimePeriods(dtDate))
			{
				timelineVisitor.Visit(period);
			}
		}
		#endregion

		#region Privates
		/// <summary>
		/// Add a time element to the timeline.
		/// </summary>
		/// <param name="timeElementNew">New time element.</param>
		private void AddOrCompleteTimeElement(TimeElement timeElementNew)
		{
			if(timeElementNew == null) throw new ArgumentNullException("timeElementNew");

			TimeElement timeElement;
			if(this.TimeElements.ContainsKey(timeElementNew.Date))
			{
				timeElement = this.TimeElements[timeElementNew.Date];

				// Set From
				if(timeElementNew.IsFrom) timeElement.IsFrom = true;

				// Set Until
				if(timeElementNew.IsUntil) timeElement.IsUntil = true;
			}
			else
			{
				this.TimeElements.Add(timeElementNew.Date, timeElementNew);
			}
		}

		/// <summary>
		/// Asks every time source if they are relevant during the duration.
		/// </summary>
		/// <param name="duration">Duration</param>
		/// <returns>List of all relevant time sources</returns>
		private IList<ITimeSource> DetermineTimeSources(Duration duration)
		{
			if(duration == null) throw new ArgumentNullException("duration");

			IList<ITimeSource> listTimeSource = new List<ITimeSource>();
			foreach(ITimeSource timeSource in this.TimeSources)
			{
				if(timeSource.IsValid(duration) && !listTimeSource.Contains(timeSource))
				{
					listTimeSource.Add(timeSource);
				}
			}

			return listTimeSource;
		}

		/// <summary>
		/// Creates a time period and determin all relevant time sources.
		/// </summary>
		/// <param name="timeElementFrom">from time element</param>
		/// <param name="timeElementUntil">until time element. Could be also empty.</param>
		/// <returns>Created time period or null.</returns>
		private TimePeriod CreateTimePeriod(TimeElement timeElementFrom, TimeElement timeElementUntil)
		{
			if(timeElementFrom == null) throw new ArgumentNullException("timeElementFrom");

			TimePeriod timePeriod = timeElementUntil == null ? new TimePeriod(this, timeElementFrom) : new TimePeriod(this, timeElementFrom, timeElementUntil);

			timePeriod.TimeSources = DetermineTimeSources(timePeriod.Duration);

			return timePeriod.TimeSourcesView.Count == 0 ? null : timePeriod;
		}
		#endregion
	}
}
