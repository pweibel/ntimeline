using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NTimeline
{
	/// <summary>
	/// Eine ZeitPeriode repräsentiert den Abschnitt zwischen zwei ZeitElementen.
	/// Das zweite ZeitElement kann auch leer sein, was einer endlosen Periode gleichkommt.
	/// Zusätzlich sind in der Zeitperiode alle während dieser Zeit gültigen Quellen gespeichert.
	/// </summary>
	public class TimePeriode
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
		public TimePeriode(TimeElement timeElementFrom)
		{
			if(timeElementFrom == null) throw new ArgumentNullException("timeElementFrom");
			_timeElementFrom = timeElementFrom;
		}

		public TimePeriode(TimeElement timeElementFrom, TimeElement timeElementUntil) : this(timeElementFrom)
		{
			if(timeElementFrom == null) throw new ArgumentNullException("timeElementFrom");
			if(timeElementUntil == null) throw new ArgumentNullException("timeElementUntil");
			if(timeElementFrom == timeElementUntil && !(timeElementUntil.IsGueltigAb && timeElementUntil.IsGueltigBis))
				throw new Exception("Eine Zeitperiode darf ein ZeitElement nur dann als Ab und Bis gesetzt haben, wenn das ZeitElement sowhol ein GültigAb, als auch ein GültigBis Datum ist.");

			_timeElementUntil = timeElementUntil;
		}
		#endregion

		#region Publics
		/// <summary>
		/// Liefert iene DateTimeCondition welche die Periode zwischen den zwei ZeitElementen darstellt.
		/// </summary>
		/// <returns>DateTimeCondition mit einem oder zwei Daten gesetzt.</returns>
		public Duration GetPeriode()
		{
			if(this.From == this.Until && !(this.From.IsGueltigAb && this.From.IsGueltigBis))
				throw new Exception("Eine Zeitperiode darf ein ZeitElement nur dann als Ab und Bis gesetzt haben, wenn das ZeitElement sowhol ein GültigAb, als auch ein GültigBis Datum ist.");

			//Falls das ab und das bis die gleichen Zeitelemente sind, braucht es eine spezielle Behandlung,
			//Da es sich hier um eine eintägige Periode handelt
			if(this.From == this.Until) return new Duration(this.From.Datum, this.Until.Datum);

			DateTime dtGueltigAb = this.From.GetPeriodenDatum(true);

			Duration cnd;
			if(this.Until == null)
			{
				cnd = new Duration(dtGueltigAb);
			}
			else
			{
				DateTime dtGueltigBis = this.Until.GetPeriodenDatum(false);
				cnd = new Duration(dtGueltigAb, dtGueltigBis);
			}

			return cnd;
		}
		#endregion
	}
}
