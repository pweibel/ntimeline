using System;

namespace NTimeline
{
	/// <summary>
	/// A time element is a date on a timeline.
	/// </summary>
	public class TimeElement
	{
		#region Fields
		private readonly DateTime _dtDate;
		#endregion

		#region Properties
		public DateTime Date
		{
			get { return _dtDate; }
		}

		public bool IsFrom { get; set; }

		public bool IsUntil { get; set; }
		#endregion

		#region Constructors
		public TimeElement(DateTime dtDate, bool bIsFrom)
		{
			_dtDate = dtDate;
			IsFrom = bIsFrom;
			IsUntil = !bIsFrom;
		}
		#endregion

		#region Publics
		/// <summary>
		/// Returns the relevant date for the period.
		/// </summary>
		/// <param name="bIsFrom">Say if the date will be used as a from or until date.</param>
		/// <returns>Correct Date for the period</returns>
		public DateTime GetPeriodDate(bool bIsFrom)
		{
			DateTime dtPeriodDate = this.Date;

			if(bIsFrom && this.IsUntil) dtPeriodDate = dtPeriodDate.AddDays(1);

			if(!bIsFrom && this.IsFrom) dtPeriodDate = dtPeriodDate.AddDays(-1);

			return dtPeriodDate;
		}
		#endregion
	}
}
