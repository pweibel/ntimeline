﻿using System;

namespace NTimeline
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
			this.From = dtFrom;
			this.Until = dtUntil;
		}
		#endregion
	}
}
