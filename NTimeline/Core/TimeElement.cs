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
