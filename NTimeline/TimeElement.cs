using System;

namespace NTimeline
{
	/// <summary>
	/// A time element is a date on a timeline.
	/// </summary>
	public class TimeElement
	{
		#region Fields
		private DateTime _dtDate;
		private bool _bIsFrom;
		private bool _bIsUntil;
		#endregion

		#region Properties
		public DateTime Date
		{
			get { return _dtDate; }
		}

		public bool IsFrom
		{
			get { return _bIsFrom; }
			set { _bIsFrom = value; }
		}

		public bool IsUntil
		{
			get { return _bIsUntil; }
			set { _bIsUntil = value; }
		}
		#endregion

		#region Constructors
		public TimeElement(DateTime dtDate, bool bIsFrom)
		{
			_dtDate = dtDate;
			_bIsFrom = bIsFrom;
			_bIsUntil = !bIsFrom;
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
