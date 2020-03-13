using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using static CitizenFX.Core.Native.API;
using NuovaGM.Client.gmPrincipale;
using NuovaGM.Client.gmPrincipale.Personaggio;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.MenuNativo;
using NuovaGM.Client.Veicoli;
using NuovaGM.Shared;
using Newtonsoft.Json;

namespace NuovaGM.Client.Lavori.Generici.Cacciatore
{
	static class CacciatoreClient
	{
		private static WeaponHash DaFuoco = WeaponHash.SniperRifle;
		private static WeaponHash Bianca = WeaponHash.Knife;

		public static void Init()
		{
			Client.GetInstance.RegisterTickHandler(ControlloCaccia);
		}

		public static async Task ControlloCaccia()
		{

		}
	}
}
