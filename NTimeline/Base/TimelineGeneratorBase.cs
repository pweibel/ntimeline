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

			//Context setzen
			this.Context = context;

			//Neuen Zeitstrahl erstellen
			this.Timeline = new Timeline(this);

			//ZeitQuellen registrieren (Implementation in Subklasse)
			RegisterTimeSources();

			//Den Zeitstrahl generieren lassen
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
