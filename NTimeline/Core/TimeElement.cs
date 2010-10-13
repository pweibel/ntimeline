using System;

namespace NTimeline.Core
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

		/// <summary>
		/// Returns the relevant from date for the period.
		/// </summary>
		/// <returns>Correct from Date for the period</returns>
		public DateTime FromPeriodDate
		{
			get
			{
				DateTime dtPeriodDate = this.Date;

				if(this.IsUntil) dtPeriodDate = dtPeriodDate.AddDays(1);

				return dtPeriodDate;
			}
		}

		/// <summary>
		/// Returns the relevant until date for the period.
		/// </summary>
		/// <returns>Correct until Date for the period</returns>
		public DateTime UntilPeriodDate
		{
			get
			{
				DateTime dtPeriodDate = this.Date;

				if(this.IsFrom) dtPeriodDate = dtPeriodDate.AddDays(-1);

				return dtPeriodDate;
			}
		}
		#endregion

		#region Constructors
		public TimeElement(DateTime dtDate, bool bIsFrom)
		{
			_dtDate = dtDate;
			IsFrom = bIsFrom;
			IsUntil = !bIsFrom;
		}
		#endregion
	}
}
