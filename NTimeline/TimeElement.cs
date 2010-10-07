using System;

namespace NTimeline
{
	/// <summary>
	/// Ein Zeitelement stellt ein Datum in einem Zeitstrahl dar.
	/// Zusätzlich wird angegeben, ob ein Zeitelement aus einem GülitAb- oder einem GültigBis-Datum
	/// einer Quelle entstanden ist. Ein ZeitElement kann auch ein GültigAb und ein GültigBis gleichzeitig repräsentieren.
	/// Zum Beispiel, wenn das selbe DAtum in einer Quelle als GültigAb und in einer anderen Quelle als GültigBis
	/// eingetragen ist.
	/// </summary>
	public class TimeElement
	{
		#region Fields
		private DateTime _dtRefDat;
		private bool _bIsGueltigAb;
		private bool _bIsGueltigBis;
		#endregion

		#region Properties
		public DateTime Datum
		{
			get { return _dtRefDat; }
		}

		public bool IsGueltigAb
		{
			get { return _bIsGueltigAb; }
			set { _bIsGueltigAb = value; }
		}

		public bool IsGueltigBis
		{
			get { return _bIsGueltigBis; }
			set { _bIsGueltigBis = value; }
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="dtRefDat">Das Datum im Zeitstrahl</param>
		/// <param name="bIsGueltigAb">Gibt an ob das Datum ein GueltigAb-Datum ist. Wird hier FALSE übergeben,
		/// so wird das Datum als GueltigBis-Datum interpretiert.</param>
		public TimeElement(DateTime dtRefDat, bool bIsGueltigAb)
		{
			_dtRefDat = dtRefDat;
			_bIsGueltigAb = bIsGueltigAb;
			_bIsGueltigBis = !bIsGueltigAb;
		}
		#endregion

		#region Publics
		/// <summary>
		/// Liefert das Datum, welches für eine Periode relevant ist.
		/// </summary>
		/// <param name="bIsGueltigAb">Gibt an ob das befragte Zeitelement als ein Ab- oder ein Bis-Datum verwendet wird.</param>
		/// <returns>Für die Periode relevantes Datum</returns>
		public DateTime GetPeriodenDatum(bool bIsGueltigAb)
		{
			DateTime dtPeriodenDatum = this.Datum;

			if(bIsGueltigAb && this.IsGueltigBis) dtPeriodenDatum = dtPeriodenDatum.AddDays(1);

			if(!bIsGueltigAb && this.IsGueltigAb) dtPeriodenDatum = dtPeriodenDatum.AddDays(-1);

			return dtPeriodenDatum;
		}
		#endregion
	}
}
