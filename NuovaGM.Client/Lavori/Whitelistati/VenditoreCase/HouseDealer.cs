using CitizenFX.Core;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.MenuNativo;
using NuovaGM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuovaGM.Client.Lavori.Whitelistati.VenditoreCase
{
	static class HouseDealer
	{
		private static ConfigVenditoriCase house;
		public static void Init()
		{
			house = Client.Impostazioni.Lavori.VenditoriCase;
			Client.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action(Spawnato));
		}

		private static async Task Markers()
		{
			if (!Game.Player.GetPlayerData().Istanza.Stanziato)
			{
				World.DrawMarker(MarkerType.VerticalCylinder, house.Config.Ingresso, Vector3.Zero, Vector3.Zero, new Vector3(1.375f, 1.375f, 0.4f), Colors.Blue);
				if (Game.PlayerPed.IsInRangeOf(house.Config.Ingresso, 1.375f))
				{
					if (Game.Player.GetPlayerData().CurrentChar.job.name.ToLower() == "venditorecase")
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per entrare in ufficio");
					else
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per entrare nell'ufficio dell'agenzia immobiliare");
					if (Input.IsControlJustPressed(Control.Context))
					{
						Funzioni.Teleport(house.Config.Dentro);
						Game.Player.GetPlayerData().Istanza.Istanzia("VenditoreCase");
					}
				}
			}
			else 
			{
				if (Game.Player.GetPlayerData().Istanza.Instance == "VenditoreCase")
				{
					World.DrawMarker(MarkerType.VerticalCylinder, house.Config.Ingresso, Vector3.Zero, Vector3.Zero, new Vector3(1.375f, 1.375f, 0.4f), Colors.Red);
					if (Game.PlayerPed.IsInRangeOf(house.Config.Uscita, 1.375f))
					{
						if (Game.Player.GetPlayerData().CurrentChar.job.name.ToLower() == "venditorecase")
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per uscire dall'ufficio");
						else
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per uscire dall'ufficio dell'agenzia immobiliare");
						if (Input.IsControlJustPressed(Control.Context))
						{
							Funzioni.Teleport(house.Config.Fuori);
							Game.Player.GetPlayerData().Istanza.RimuoviIstanza();
						}
					}
				}
			}


			if (Game.Player.GetPlayerData().CurrentChar.job.name.ToLower() == "venditorecase")
			{
				// verrà cambiato con il sedersi alla scrivania
				if (Game.PlayerPed.IsInRangeOf(house.Config.Actions, 1.375f))
				{
					HUD.ShowHelp("~INPUT_CONTEXT~ Apri il menu di vendita");
					if (Input.IsControlJustPressed(Control.Context) && !HUD.MenuPool.IsAnyMenuOpen)
						MenuVenditoreCase();
				}
			}
		}

		private static async void MenuVenditoreCase()
		{
			UIMenu venditore = new UIMenu("Agenzia Immobiliare", "Abbiamo la casa per tutte le esigenze!");
			HUD.MenuPool.Add(venditore);
			Dictionary<string, ConfigCase> Appartamenti = Client.Impostazioni.Proprieta.Appartamenti;
			Dictionary<string, Garages> Garages = Client.Impostazioni.Proprieta.Garages.Garages;
			UIMenu appart = venditore.AddSubMenu("Appartamenti");
			UIMenu gara = venditore.AddSubMenu("Garages");
			foreach (var app in Appartamenti)
			{
				UIMenu appartamento = appart.AddSubMenu(app.Value.Label);
			}
			foreach (var gar in Garages)
			{
				UIMenu garage = gara.AddSubMenu(gar.Value.Label);
			}
			venditore.Visible = true;
		}

		private static void Spawnato()
		{
			Client.Instance.AddTick(Markers);
		}
	}
}
