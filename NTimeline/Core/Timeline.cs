using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly IList<TimePeriod> timePeriods = new List<TimePeriod>();
        private ITimePeriodBuilder timePeriodBuilder = new DatePeriodBuilder();
        #endregion

        #region Properties
        public ITimePeriodBuilder TimePeriodBuilder
        { 
            get { return timePeriodBuilder; }
            set { timePeriodBuilder = value; }
        }

        public ReadOnlyCollection<TimePeriod> TimePeriods
        {
            get { return new ReadOnlyCollection<TimePeriod>(timePeriods); }
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
            if(timeSource.Timeline != null) throw new InvalidOperationException("Time source has already assigned to another timeline.");

            if(!this.TimeSources.Contains(timeSource))
            {
                timeSource.Timeline = this;
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

            if (this.TimeSources.Contains(timeSource))
            {
                this.TimeSources.Remove(timeSource);
            }
        }

        /// <summary>
        /// Collects all the time elements of the registered time sources and build the time periods with the help of the time period builder.
        /// </summary>
        public void Build()
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

            // Builds time periods and assign the valid time sources to each time period
            BuildTimePeriods();
        }

        /// <summary>
        /// Returns a list with time periods which are valid during the date
        /// </summary>
        /// <param name="dtDate">Date</param>
        /// <returns>All time periods which started before or at the date and ended after or at the date or are endless</returns>
        public IList<TimePeriod> GetTimePeriods(DateTime dtDate)
        {
            return (from timePeriod in this.timePeriods
                    let duration = timePeriod.Duration
                    where duration.From <= dtDate && (duration.Until == null || dtDate <= duration.Until)
                    select timePeriod).ToList();
        }

        /// <summary>
        /// Loops through all time periods of the timeline in chronological order.
        /// </summary>
        /// <param name="timelineVisitor">Visitor</param>
        public void VisitTimePeriods(ITimelineVisitor timelineVisitor)
        {
            if(timelineVisitor == null) throw new ArgumentNullException("timelineVisitor");
            if(this.timePeriods.Count == 0) return;

            foreach(TimePeriod period in this.timePeriods)
            {
                timelineVisitor.Visit(period);
            }
        }

        /// <summary>
        /// Loops through all time periods of the timeline in reverse chronological order.
        /// </summary>
        /// <param name="timelineVisitor">Visitor</param>
        public void VisitTimePeriodsInReverseOrder(ITimelineVisitor timelineVisitor)
        {
            if(timelineVisitor == null) throw new ArgumentNullException("timelineVisitor");
            if(this.timePeriods.Count == 0) return;

            for(int i = this.timePeriods.Count - 1; i >= 0; i--)
            {
                timelineVisitor.Visit(this.timePeriods[i]);
            }
        }
        #endregion

        #region Privates
        /// <summary>
        /// Returns a list of time periodes.
        /// </summary>
        private void BuildTimePeriods()
        {
            if (this.TimePeriodBuilder == null) throw new InvalidOperationException("No TimePeriodBuilder available.");
            this.timePeriods.Clear();

            IList<TimePeriod> timePeriodsWithoutSources = this.TimePeriodBuilder.BuildTimePeriods(this.TimeElements);

            foreach (TimePeriod timePeriod in timePeriodsWithoutSources)
            {
                IList<ITimeSource> sources = DetermineTimeSources(timePeriod.Duration);
                if (sources != null && sources.Count > 0)
                {
                    timePeriod.TimeSources = sources;
                    this.timePeriods.Add(timePeriod);
                }
            }
        }
        
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
