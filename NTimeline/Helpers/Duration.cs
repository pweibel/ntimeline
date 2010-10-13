using System;

namespace EWeibel.NTimeline.Helpers
{
	public class Duration
	{
		#region Properties
		public DateTime From { get; private set; }
		public DateTime? Until { get; private set; }
		public bool IsDuration
		{
			get { return this.Until.HasValue; }
		}
		#endregion

		#region Constructors
		public Duration(DateTime dtFrom)
		{
			this.From = dtFrom;
		}

		public Duration(DateTime dtFrom, DateTime? dtUntil)
		{
			if(dtUntil != null && dtUntil < dtFrom) throw new ArgumentException(string.Format("Until date '{0}' is smaller than from date '{1}'.", dtUntil, dtFrom));

			this.From = dtFrom;
			this.Until = dtUntil;
		}
		#endregion
	}
}
