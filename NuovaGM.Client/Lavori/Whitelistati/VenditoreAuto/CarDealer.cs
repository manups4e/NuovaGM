using CitizenFX.Core;
using Logger;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.MenuNativo;
using NuovaGM.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NuovaGM.Client.Lavori.Whitelistati.VenditoreAuto
{
	static class CarDealer
	{
		public static void Init()
		{
			Client.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action(Spawnato));
			Client.Instance.AddTick(Markers);
		}

		private static void Spawnato()
		{
//			Blip vend = World.CreateBlip()
		}


		public static async Task Markers()
		{
			if(Game.Player.GetPlayerData().CurrentChar.job.name.ToLower() == "cardealer")
			{
				// verrà sostiuito con il sedersi alla scrivania e mostrare al cliente
				if(Game.PlayerPed.IsInRangeOf(Client.Impostazioni.Lavori.VenditoriAuto.Config.MenuVendita, 1.375f))
				{
					HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per aprire il menu del venditore");
					if (Input.IsControlJustPressed(Control.Context))
						MenuVenditore();
				}
			}
			if(Game.Player.GetPlayerData().CurrentChar.job.grade > 1)
			{
				// verrà sostiuito con il sedersi alla scrivania 
				if(Game.PlayerPed.IsInRangeOf(Client.Impostazioni.Lavori.VenditoriAuto.Config.BossActions, 1.375f))
				{
					HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per aprire il menu boss");
					if (Input.IsControlJustPressed(Control.Context))
						MenuBoss();
				}
			}

		}


		private static async void MenuVenditore()
		{
			UIMenu menuVenditore = new UIMenu("Menu Venditore", "Tutto l'occorrente a portata di click");
			HUD.MenuPool.Add(menuVenditore);

			// la fattura sarà automatica all'acquisto da parte dell'acquirente (magari non ci sarà fattura ma acquisto automatico)
			// non sarà disponibile per l'affitto che partirà dalla consegna del veicolo

			UIMenuItem catalogoPriv = new UIMenuItem("Guarda catalogo", "Solo per te");
			menuVenditore.AddItem(catalogoPriv);
			UIMenu mostraCatalogo = menuVenditore.AddSubMenu("Mostra catalogo", "Scegli a chi");
			mostraCatalogo.OnMenuOpen += async (_menu) =>
			{
				_menu.Clear();
				List<Player> players = Funzioni.GetPlayersInArea(Game.PlayerPed.Position, 3f);
				List<string> texts = players.Select(x => x.GetPlayerData().FullName).ToList();
				string txt = "";
				foreach (var t in texts) txt = t + "~n~";
				Log.Printa(LogType.Debug, texts.Aggregate("", (current, s) => current + (s + "~n~")));
				UIMenuItem mostraTutti = new UIMenuItem("Mostra a tutti", $"{texts.Aggregate("", (current, s) => current + (s + "~n~"))}");
				UIMenu mostraCatalogoAlcuni = mostraCatalogo.AddSubMenu("Mostra a scelta");
				_menu.AddItem(mostraTutti);
				if (players.Count == 0)
				{
					mostraCatalogoAlcuni.ParentItem.Enabled = false;
					mostraCatalogoAlcuni.ParentItem.Description = "Non hai persone vicino!";
					mostraTutti.Enabled = false;
					mostraTutti.Description = "Non hai persone vicino!";
				}
				mostraTutti.Activated += (_submenu, _subitem) =>
				{
					BaseScript.TriggerServerEvent("lprp:cardealer:attivaCatalogoTutti", players.Select(x => x.ServerId).ToList());
				};
				mostraCatalogo.OnMenuOpen += async (_submenu) =>
				{
					_submenu.Clear();
					List<int> persone = new List<int>();
					foreach(var p in players)
					{
						UIMenuCheckboxItem persona = new UIMenuCheckboxItem(p.GetPlayerData().FullName, false);
						persona.CheckboxEvent += (_item, _activated) =>
						{
							if (_activated)
								if (!persone.Contains(p.ServerId)) persone.Add(p.ServerId);
							else
								if (persone.Contains(p.ServerId)) persone.Remove(p.ServerId);
						};
					}
					UIMenuItem mostra = new UIMenuItem("Mostra a selezionati", "", Colors.GreenDark, Colors.GreenLight);
					_submenu.AddItem(mostra);
					mostra.Activated += (menu, item) =>
					{
						if (persone.Count == 0)
						{
							HUD.ShowNotification("Non hai selezionato nessuno!");
							return;
						}
						BaseScript.TriggerEvent("lprp:cardealer:attivaCatalogoAlcuni", persone);

					};
				};
			};
			menuVenditore.Visible = true;
		}
		private static async void MenuBoss()
		{

		}
	}
}
