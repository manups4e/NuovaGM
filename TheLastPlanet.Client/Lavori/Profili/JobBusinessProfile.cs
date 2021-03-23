using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Logger;
using TheLastPlanet.Client.Core.PlayerChar;
using TheLastPlanet.Client.SessionCache;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.SistemaEventi;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.Lavori.Profili
{
	public class JobBusinessProfile : JobProfile
	{
		public override JobProfile[] Dependencies { get; set; }
		public string Seed { get; set; }
		public Business Business { get; set; }

		public override async void Begin(LavoroBase job)
		{
			await Cache.Loaded();
			Seed = $"lprp:businesses::{job.Lavoro.ToString().ToLower()}";

			Business = await Client.Instance.SistemaEventi.Request<Business>("lprp:business:fetch", Seed) ?? new Business()
			{
				Seed = Seed,
				Balance = 0,
				Registered = DateTime.Now.Ticks
			};

			Client.Instance.SistemaEventi.Attach("lprp:business:update", new EventCallback(a =>
			{
				var busi = a.Find<Business>(0);
				Business.Balance = busi.Balance;
				return null;
			}));
		}

		public void Commit() => Client.Instance.SistemaEventi.Send("lprp:business:update", Business);

	}
}
