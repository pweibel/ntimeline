using System;

namespace NTimeline.Base
{
	public abstract class TimelineGeneratorBase : ITimelineGenerator
	{
		#region Properties
		public ITimeContext Context
		{
			get;
			protected set;
		}

		protected Timeline Timeline
		{
			get;
			private set;
		}
		#endregion

		#region Publics
		public void Generate(ITimeContext context)
		{
			if(context == null) throw new ArgumentNullException("context");

			// Set context
			this.Context = context;

			// Create new timeline
			this.Timeline = new Timeline(this);

			// Register new time sources
			RegisterTimeSources();

			// Generate timeline
			this.Timeline.Generate();

			GenerateTimeElements();
		}
		#endregion

		#region Protecteds
		protected abstract void RegisterTimeSources();
		protected abstract void GenerateTimeElements();
		#endregion
	}
}
