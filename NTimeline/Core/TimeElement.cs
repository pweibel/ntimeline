using System;

namespace NTimeline.Core
{
	/// <summary>
	/// A time element is a date on a timeline.
	/// </summary>
	public class TimeElement
	{
		#region Fields
		private readonly DateTime dtDate;
		#endregion

		#region Properties
		public DateTime Date
		{
			get { return this.dtDate; }
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
			this.dtDate = dtDate;
			this.IsFrom = bIsFrom;
			this.IsUntil = !bIsFrom;
		}
		#endregion
	}
}
