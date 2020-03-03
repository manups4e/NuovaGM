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

namespace NuovaGM.Client.Lavori.Generici.Pescatore
{
	static class PescatoreClient
	{
		private static Pescatori PuntiPesca;
		private static string scenario = "WORLD_HUMAN_STAND_FISHING";
		private static string AnimDict = "amb@world_human_stand_fishing@base";
		private static bool LavoroAccettato = false;

		// oggetti: canna da pesca, esche, pesci, frutti di mare magari, gamberi.. crostacei
		// considerare spogliatoio (obbligatorio / opzionale)

		public static async void Init()
		{
			RequestAnimDict(AnimDict);
			PuntiPesca = ConfigClient.Conf.Lavori.Generici.Pescatore;
			Client.GetInstance.RegisterTickHandler(ControlloPesca);
		}

		public async static Task ControlloPesca()
		{
			foreach (var punto in PuntiPesca.PuntiDiPesca)
			{
				if (World.GetDistance(Game.PlayerPed.Position, punto.Contratto.ToVector3()) < 2f)
				{
					HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per accettare un contratto da ~b~Pescatore~w~.");
					if (Game.IsControlJustPressed(0, Control.Context))
					{
						LavoroAccettato = true;
						HUD.ShowNotification("Accettato");
					}
				}
				if (World.GetDistance(Game.PlayerPed.Position, punto.AffittoBarca.ToVector3()) < 2f)
				{
					if (LavoroAccettato)
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per scegliere una ~b~barca~w~.");
						if (Game.IsControlJustPressed(0, Control.Context))
						{
							HUD.ShowNotification("Scelto");
						}
					}
				}
			}
		}
	}
}
