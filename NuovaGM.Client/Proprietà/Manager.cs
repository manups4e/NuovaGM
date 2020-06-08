using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using Newtonsoft.Json;
using NuovaGM.Shared;

namespace NuovaGM.Client.Proprietà
{
	static class Manager
	{
		private static ConfigProprieta Proprietà;
		public static void Init()
		{
			Proprietà = Client.Impostazioni.Proprieta;
		}

		public static async Task MarkerBlipHandler()
		{
			foreach (var app in Proprietà.Appartamenti.LowEnd) 
			{
				if(Game.PlayerPed.IsInRangeOf(app.Value.MarkerEntrata.ToVector3(), 1.375f))
				{

				}
			}
		}
	}
}
