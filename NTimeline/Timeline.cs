using System;
using System.Collections.Generic;

namespace NTimeline
{
	public class Timeline
	{
		#region Fields
		private readonly IList<ITimeSource> _listTimeSources = new List<ITimeSource>();
		private readonly SortedList<DateTime, TimeElement> _listTimeElements = new SortedList<DateTime, TimeElement>();
		#endregion

		#region Properties
		public ITimelineGenerator TimelineGenerator
		{
			get; 
			private set;
		}

		private IList<ITimeSource> TimeSources
		{
			get { return _listTimeSources; }
		}

		private SortedList<DateTime, TimeElement> TimeElements
		{
			get { return _listTimeElements;  }
		}
		#endregion

		#region Constructors
		public Timeline(ITimelineGenerator timelineGenerator)
		{
			if(timelineGenerator == null) throw new ArgumentNullException("timelineGenerator");

			this.TimelineGenerator = timelineGenerator;
		}
		#endregion

		#region Publics
		/// <summary>
		/// Fügt eine ZeitQuelle zum Zeitstrahl hinzu.
		/// </summary>
		/// <param name="timeSource">zeitQuelle die hinzugefügt werden soll</param>
		public void AddTimeSource(ITimeSource timeSource)
		{
			if(timeSource == null) throw new ArgumentNullException("timeSource");

			if(!this.TimeSources.Contains(timeSource))
			{
				this.TimeSources.Add(timeSource);
			}
		}

		/// <summary>
		/// Entfernt eine ZeitQuelle vom Zeitstrahl.
		/// </summary>
		/// <param name="timeSource">zeitQuelle die entfernt werden soll</param>
		public void RemoveTimeSource(ITimeSource timeSource)
		{
			if(timeSource == null) throw new ArgumentNullException("timeSource");

			this.TimeSources.Remove(timeSource);
		}

		/// <summary>
		/// Holt die Zeitelemente aller Quellen ein und fügt diese im Zeitstrahl ein.
		/// Zustätzlich werden alle Quellen über die hinzugefügten Elemente informiert.
		/// </summary>
		public void Generate()
		{
			//Alle bestehenden ZeitElemente löschen
			this.TimeElements.Clear();

			//Alle Zeitelemente der Quellen zum Zeitstrahl hinzufügen
			foreach(ITimeSource zeitQuelle in this.TimeSources)
			{
				IList<TimeElement> zeitElemente = zeitQuelle.CreateTimeElements();
				if(zeitElemente == null) continue;
				foreach(TimeElement zeitElement in zeitElemente)
				{
					AddOrCompleteZeitElement(zeitElement);
				}
			}
		}

		/// <summary>
		/// Gibt eine Liste aller ZeitPerioden zurück, welche auf dem Zeitstrahl eingetragen sind.
		///  </summary>
		/// <returns>Liste aller ZeitPerioden. Gibt es keine Einträge auf dem Zeitstrhal, wird eine leere Liste zurückgegeben.</returns>
		public IList<TimePeriode> BuildTimePeriods()
		{
			IList<TimePeriode> listTimePeriod = new List<TimePeriode>();

			if(this.TimeElements.Values.Count == 0) return listTimePeriod;

			for(int i=0; i < this.TimeElements.Values.Count; i++)
			{
				//Falls es ein Gültig Ab und ein GültigBis Datum gibt muss eine Zeitperiode mit Start und Ende
				//an diesem Datum (Eintägig)
				if(this.TimeElements.Values[i].IsGueltigAb && this.TimeElements.Values[i].IsGueltigBis)
				{
					//ZeitPeriode erstellen
					TimePeriode zeitPeriodeEintaegig = CreateZeitPeriode(this.TimeElements.Values[i], this.TimeElements.Values[i]);
					if(zeitPeriodeEintaegig != null && !listTimePeriod.Contains(zeitPeriodeEintaegig)) listTimePeriod.Add(zeitPeriodeEintaegig);
				}

				TimeElement timeElement1 = this.TimeElements.Values[i];

				//Das zweite Elemente wird nur gesetzt, falls es noch ein weiteres Element gibt
				TimeElement timeElement2 = i < this.TimeElements.Count -1 ? this.TimeElements.Values[i+1] : null;

				//Falls die zwei berücksichtigten ZeitElemente direkt auf einander folgen
				//Und das vorangehende ein GültigBis sowie das nachfolgende ein GültigAb Datum ist, so muss keine Periode generiert werden
				//da dies bereits zwei aufeinanderfolgende Perioden darstellt.
				if(timeElement2 != null && timeElement1.Datum.AddDays(1) == timeElement2.Datum && timeElement1.IsGueltigBis && timeElement2.IsGueltigAb) continue;

				//Periode erstellen
				TimePeriode zeitPeriode = CreateZeitPeriode(timeElement1, timeElement2);
				if(zeitPeriode != null && !listTimePeriod.Contains(zeitPeriode)) listTimePeriod.Add(zeitPeriode);
			}

			return listTimePeriod;
		}

		/// <summary>
		/// Gibt eine Liste aller ZeitPerioden zurück, welche auf dem Zeitstrahl eingetragen sind und
		/// während oder nach dem übergebenen GültigPer Datum gültig sind.
		/// </summary>
		/// <param name="dtGueltigPer">GültigPer Datum</param>
		/// <returns></returns>
		public IList<TimePeriode> BuildTimePeriods(DateTime dtGueltigPer)
		{
			IList<TimePeriode> listTimePeriod = BuildTimePeriods();

			IList<TimePeriode> listValidTimePeriod = new List<TimePeriode>();

			foreach(TimePeriode zeitPeriode in listTimePeriod)
			{
				
				//Eine Periode muss berücksichtigt, wenn sie während oder nach dem GültigPer-Datum gültig ist.
				//Weiter gibt es das Szenario, dass eine Periode einen Tag vor dem GültigPer beendet werden kann.
				//Eine solche Periode muss auch berücksichtigt werden.
				Duration duration = zeitPeriode.GetPeriode();
				if((!duration.IsDuration) || (duration.IsDuration && duration.Until >= dtGueltigPer.AddDays(-1)))
				{
					listValidTimePeriod.Add(zeitPeriode);
				}
			}

			return listValidTimePeriod;
		}
		#endregion

		#region Privates
		/// <summary>
		/// Fügt ein ZeitElement in den Zeitstrahl ein, falls das Datum noch nicht im Zeitstrahl erfasst ist.
		/// Ist das Datum im Zeitstrahl bereits erfasst, so werden dessen Daten denen des übergebenen ZeitElements
		/// ergänzt.
		/// </summary>
		/// <param name="timeElementNew">Das ZeitElement, welches zum Zeitstrahl hinzugefügt werden soll.</param>
		private void AddOrCompleteZeitElement(TimeElement timeElementNew)
		{
			if(timeElementNew == null) throw new ArgumentNullException("timeElementNew");

			TimeElement timeElement;
			if(this.TimeElements.ContainsKey(timeElementNew.Datum))
			{
				timeElement = this.TimeElements[timeElementNew.Datum];

				//Gültig ab übernehmen
				if(timeElementNew.IsGueltigAb) timeElement.IsGueltigAb = true;

				//Gültig bis übernehmen
				if(timeElementNew.IsGueltigBis) timeElement.IsGueltigBis = true;
			}
			else
			{
				//Das Zeitelement exisitert noch nicht im Zeitstrahl und muss hinzugefügt werden
				this.TimeElements.Add(timeElementNew.Datum, timeElementNew);
			}
		}

		/// <summary>
		/// Fragt alle Quellen des Zeitstrahls an, ob sie am Referenzdatum
		/// relevant sind. Alle relevanten Quellen werden zurückgegeben.
		/// </summary>
		/// <param name="duration">Periode</param>
		/// <returns>Liste aller relevanten Quellen</returns>
		private IList<ITimeSource> DetermineZeitQuellen(Duration duration)
		{
			if(duration == null) throw new ArgumentNullException("duration");

			IList<ITimeSource> listTimeSource = new List<ITimeSource>();
			foreach(ITimeSource zeitQuelle in this.TimeSources)
			{
				if(zeitQuelle.IsValid(duration) && !listTimeSource.Contains(zeitQuelle))
				{
					listTimeSource.Add(zeitQuelle);
				}
			}

			return listTimeSource;
		}

		/// <summary>
		/// Erstellt eine ZeitPeriode und bestimmt die Quellen, welche für die ZeitPeriode
		/// relevant sind.
		/// </summary>
		/// <param name="timeElementFrom">Ab ZeitElement</param>
		/// <param name="timeElementUntil">Bis ZeitElement. Falls es kein Bis ZeitElement gibt, wird NULL übergeben.</param>
		/// <returns>Erstellte Zeitperiode oder NULL, wenn die ZeitPeriode nicht erstellt wurde</returns>
		private TimePeriode CreateZeitPeriode(TimeElement timeElementFrom, TimeElement timeElementUntil)
		{
			if(timeElementFrom == null) throw new ArgumentNullException("timeElementFrom");

			TimePeriode timePeriode = timeElementUntil == null ? new TimePeriode(timeElementFrom) : new TimePeriode(timeElementFrom, timeElementUntil);

			timePeriode.TimeSources = DetermineZeitQuellen(timePeriode.GetPeriode());

			if(timePeriode.TimeSourcesView.Count == 0) return null;

			return timePeriode;
		}
		#endregion
	}
}
