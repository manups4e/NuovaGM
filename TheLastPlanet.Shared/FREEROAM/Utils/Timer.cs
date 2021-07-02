using CitizenFX.Core.Native;
using System;
using TheLastPlanet.Shared.Internal.Events.Attributes;

namespace TheLastPlanet.Shared
{
	[Serialization]
	public partial class Timer
	{
		public long StartTime;
		public void New(int startTime = 0)
		{
			StartTime = API.GetGameTimer();
			if (startTime > 0)
				StartTime += startTime;
		}

		public long Elapsed()
		{
			return API.GetGameTimer() - StartTime;
		}

		public long Restart()
		{
			var elapsed = Elapsed();
			StartTime = API.GetGameTimer();
			return elapsed;
		}
	}
}