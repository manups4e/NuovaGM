using CitizenFX.Core;
using NuovaGM.Client.gmPrincipale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuovaGM.Client.Lavori.Whitelistati.VenditoreAuto
{
	static class CarDealer
	{
		public static void Init()
		{
			Client.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action(Spawnato));
		}

		private static void Spawnato()
		{
//			Blip vend = World.CreateBlip()
		}


	}
}
