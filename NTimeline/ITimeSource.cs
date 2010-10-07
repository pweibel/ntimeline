using System.Collections.Generic;

namespace NTimeline
{
	/// <summary>
	/// Eine ZeitQuelle dient als Datenlieferant.
	/// Jeder der Daten für den Zeitstrahl liefern will, muss dieses Interface implementieren.
	/// </summary>
	public interface ITimeSource
	{
		/// <summary>
		/// Zeitstrahl, bei welchem die Zeitquelle registriert ist.
		/// </summary>
		Timeline Timeline { get; }

		/// <summary>
		/// Erstellt eine Liste von ZeitElementen.
		/// Aus welchen Daten diese ZeitElemente generiert werden, hängt von der spezifischen ZeitQuelle ab.
		/// </summary>
		/// <returns>Liste von ZeitElementen, falls keine ZeitElemente vorhanden sind, eine leere Liste.</returns>
		IList<TimeElement> CreateTimeElements();

		/// <summary>
		/// Wird vom Zeitstrahl aufgerufen um festzustellen ob eine ZeitQuelle in der Periode gültig ist.
		/// </summary>
		/// <param name="duration">Periode</param>
		/// <returns>TRUE, wenn die Quelle in der Periode gültig ist, ansonsten FALSE</returns>
		bool IsValid(Duration duration);
	}
}
