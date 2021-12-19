using CitizenFX.Core;
using Impostazioni.Shared;
using System;
using System.Threading.Tasks;
using TheLastPlanet.Shared;

namespace TheLastPlanet.Server.TimeWeather
{
	internal static class OrarioServer
	{

        public static ServerTime TempoOrario { get; set; }
		private static float _timeBuffer;

		public static void Init()
		{
			TempoOrario = new ServerTime
			{
				SecondOfDay = new Random().Next(0, 23) * 3600 + new Random().Next(0, 59) * 60 + new Random().Next(0, 59),
				Date = DateTime.Now,
				Frozen = false
            };
			Server.Instance.Events.Mount("UpdateFromCommandTime", new Action<int>(Update));
			Task.Run(UpdateTime);
		}

		public static void Update(int time)
		{
			TempoOrario.SecondOfDay = time;
			Server.Instance.ServerState.Set("Orario", TempoOrario.ToBytes(), true);
			Server.Instance.Events.Send(Server.Instance.Clients, "UpdateFromCommandTime", time);
		}

		public static void FreezeTime(bool freeze) { TempoOrario.Frozen = freeze; }

		public static async Task UpdateTime()
		{
			while (true)
			{
				await BaseScript.Delay(33);

				if (!TempoOrario.Frozen)
				{
					_timeBuffer += 0.9900f;

					if (_timeBuffer > 1f)
					{
						TempoOrario.SecondOfDay += (int)Math.Floor(_timeBuffer);
						_timeBuffer -= (int)Math.Floor(_timeBuffer);
						if (TempoOrario.SecondOfDay > 86399) TempoOrario.SecondOfDay = 0;
					}
				}
				TempoOrario.Date = DateTime.Now;
				Server.Instance.ServerState.Set("Orario", TempoOrario.ToBytes(), true);
			}
		}
	}
}