using System;
using System.Collections.Generic;
using System.Linq;

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
		private ITimePeriodBuilder timePeriodBuilder = new DatePeriodBuilder();
		#endregion

		#region Properties
		public ITimePeriodBuilder TimePeriodBuilder
		{ 
			get { return timePeriodBuilder; }
			set { timePeriodBuilder = value; }
		}

		private IList<ITimeSource> TimeSources
		{
			get { return listTimeSources; }
		}

		private SortedList<DateTime, TimeElement> TimeElements
		{
			get { return listTimeElements;  }
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
			if(this.TimePeriodBuilder == null) throw new InvalidOperationException("No TimePeriodBuilder available.");
			IList<TimePeriod> timePeriods = this.TimePeriodBuilder.BuildTimePeriods(this.TimeElements);
			
			IList<TimePeriod> result = new List<TimePeriod>();
			foreach(TimePeriod timePeriod in timePeriods)
			{
				IList<ITimeSource> sources = DetermineTimeSources(timePeriod.Duration);
				if(sources != null && sources.Count > 0)
				{
					timePeriod.TimeSources = sources;
					result.Add(timePeriod);
				}
			}

			return result;
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
			if (duration == null) throw new ArgumentNullException("duration");

			IList<ITimeSource> listTimeSource = new List<ITimeSource>();
			foreach (ITimeSource timeSource in this.TimeSources)
			{
				if (timeSource.IsValid(duration) && !listTimeSource.Contains(timeSource))
				{
					listTimeSource.Add(timeSource);
				}
			}

			return listTimeSource;
		}
		#endregion
	}
}
