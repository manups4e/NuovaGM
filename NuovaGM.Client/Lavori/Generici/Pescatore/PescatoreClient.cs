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



	// NON E PIU UN LAVORO... LIBERO PER TUTTI.. CREARE PUNTI DI AFFITTO BARCHE PER CHI LE VUOLE..
	// CREARE PUNTI GENERICI DI VENDITA DEL PESCE, E PUNTI GENERICI DI ATTACCO / STACCO BARCHE PER CHI LE POSSIEDE

	static class PescatoreClient
	{
		private static Pescatori PuntiPesca;
		private static string scenario = "WORLD_HUMAN_STAND_FISHING";
		private static string AnimDict = "amb@world_human_stand_fishing@base";
		private static bool LavoroAccettato = false;
		private static Vehicle lastVehicle;
		public static bool Pescando = false;
		public static bool CannaInMano = false;
		private static int TipoCanna = -1;
		private static Prop CannaDaPesca;
		private static Vector3 LuogoDiPesca = new Vector3(0);
		// oggetti: canna da pesca, esche, pesci, frutti di mare magari, gamberi.. crostacei
		// considerare spogliatoio (obbligatorio / opzionale)

		// N_0xc54a08c85ae4d410 mentre peschi (0.0 normale, 1.0 acqua liscia, 3.0 acqua mossa)
		// benson per portare il pesce


		public static async void Init()
		{
			RequestAnimDict(AnimDict);
			RequestAnimDict("amb@code_human_wander_drinking@beer@male@base");
			PuntiPesca = ConfigClient.Conf.Lavori.Generici.Pescatore;
			Client.GetInstance.RegisterTickHandler(ControlloPesca);

			SharedScript.ItemList["cannadapescabase"].Usa += async (item, index) =>
			{
				CannaDaPesca = new Prop(CreateObject(Funzioni.HashInt("prop_fishing_rod_01"), 1729.73f, 6403.90f, 34.56f, true, true, true));
				AttachEntityToEntity(CannaDaPesca.Handle, PlayerPedId(), GetPedBoneIndex(PlayerPedId(), /*60309*/ 57005), 0.10f, 0, -0.001f, 80.0f, 150.0f, 200.0f, false, false, false, false, 1, true);
				TaskPlayAnim(PlayerPedId(), "amb@code_human_wander_drinking@beer@male@base", "static", 3.5f, -8, -1, 49, 0, false, false, false);
				CannaInMano = true;
				TipoCanna = 0;
				Client.GetInstance.RegisterTickHandler(Pesca);
			};
			SharedScript.ItemList["cannadapescamedia"].Usa += async (item, index) =>
			{
				CannaDaPesca = new Prop(CreateObject(Funzioni.HashInt("prop_fishing_rod_01"), 1729.73f, 6403.90f, 34.56f, true, true, true));
				AttachEntityToEntity(CannaDaPesca.Handle, PlayerPedId(), GetPedBoneIndex(PlayerPedId(), /*60309*/ 57005),0.10f, 0, -0.001f, 80.0f, 150.0f, 200.0f, false, false, false, false, 1, true);
				TaskPlayAnim(PlayerPedId(), "amb@code_human_wander_drinking@beer@male@base", "static", 3.5f, -8, -1, 49, 0, false, false, false);
				CannaInMano = true;
				TipoCanna = 1;
				Client.GetInstance.RegisterTickHandler(Pesca);
			};
			SharedScript.ItemList["cannadapescaavanzata"].Usa += async (item, index) =>
			{
				CannaDaPesca = new Prop(CreateObject(Funzioni.HashInt("prop_fishing_rod_01"), 1729.73f, 6403.90f, 34.56f, true, true, true));
				AttachEntityToEntity(CannaDaPesca.Handle, PlayerPedId(), GetPedBoneIndex(PlayerPedId(), /*60309*/ 57005),0.10f, 0, -0.001f, 80.0f, 150.0f, 200.0f, false, false, false, false, 1, true);
				TaskPlayAnim(PlayerPedId(), "amb@code_human_wander_drinking@beer@male@base", "static", 3.5f, -8, -1, 49, 0, false, false, false);
				CannaInMano = true;
				TipoCanna = 2;
				Client.GetInstance.RegisterTickHandler(Pesca);
			};
		}

		public async static Task ControlloPesca()
		{
/*			if (World.GetDistance(Game.PlayerPed.Position, PuntiPesca.AffittoBarca.ToVector3()) < 2f && !Game.PlayerPed.IsInVehicle())
			{
				HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per scegliere una ~b~barca~w~.");
				if (Game.IsControlJustPressed(0, Control.Context))
				{
					MenuBarche();
				}
			}
*/
		}

		public static async Task Pesca()
		{
			if (CannaInMano)
			{
				Game.DisableControlThisFrame(0, Control.FrontendX);
				Game.DisableControlThisFrame(0, Control.FrontendY);
				if(!Pescando)
					HUD.ShowHelp("Premi ~INPUT_FRONTEND_X~ per iniziare a pescare.~n~Premi ~INPUT_FRONTEND_Y~ per posare la canna da pesca");
				if (Game.IsDisabledControlJustPressed(0, Control.FrontendX))
				{
					float altezza = 0;
					if(GetWaterHeightNoWaves(Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y, Game.PlayerPed.Position.Z, ref altezza)) 
					{ 
						Game.PlayerPed.IsPositionFrozen = true;
						SetEnableHandcuffs(PlayerPedId(), true);
						CannaDaPesca.Detach();
						AttachEntityToEntity(CannaDaPesca.Handle, PlayerPedId(), GetPedBoneIndex(PlayerPedId(), 60309), 0, 0, 0, 0, 0, 0, false, false, false, false, 2, true);
						TaskPlayAnim(PlayerPedId(), AnimDict, "base", 8.0f, -8, -1, 34, 0, false, false, false);
						Pescando = true;
					}
					else
						HUD.ShowNotification("Non puoi pescare qui, assicurati di avere almeno i piedi nell'acqua!", true);
				}
				if (Game.IsDisabledControlJustPressed(0, Control.FrontendY))
				{
					CannaDaPesca.Delete();
					Client.GetInstance.DeregisterTickHandler(Pesca);
					Game.PlayerPed.Task.ClearAll();
					CannaInMano = false;
					TipoCanna = -1;
				}
			}
			if (Pescando)
			{
				HUD.ShowHelp("Premi ~INPUT_FRONTEND_Y~ per smettere di pescare.", 5000);
				if (PuntiPesca.TempoPescaDinamico)
					await BaseScript.Delay(Funzioni.GetRandomInt(30000, 120000));
				else
					await BaseScript.Delay(PuntiPesca.TempoFisso * 1000);
				if(Pescando)
					await ControlliEPesca();
			}
		}

		private static async Task ControlliEPesca()
		{
			int TocchiTotali = Funzioni.GetRandomInt(20, 40);
			int tocchiEffettuati = 0;
			int contogenerico = 0;
			if (TipoCanna != -1) 
			{
				while (tocchiEffettuati < TocchiTotali)
				{
					await BaseScript.Delay(0);
					Game.DisableControlThisFrame(0, Control.Attack);
					Game.DisableAllControlsThisFrame(1);
					if (Game.CurrentInputMode == InputMode.GamePad)
					{
						HUD.ShowHelp("Ha abboccato qualcosa!! Gira velocemente ~INPUT_LOOK_UD~ per pescarla!");
						if (Game.IsDisabledControlJustPressed(1, Control.LookUpDown))
							tocchiEffettuati += 1;
					}
					else
					{
						HUD.ShowHelp("Ha abboccato qualcosa!! Premi velocemente ~INPUT_ATTACK~ per pescarla!");
						if (Game.IsDisabledControlJustPressed(0, Control.Attack))
							tocchiEffettuati += 1;
					}
					contogenerico += 1;
					HUD.DrawText(0.4f, 0.9f, "Conto generico = " + contogenerico);
					HUD.DrawText(0.4f, 0.925f, "TocchiTotali = " + TocchiTotali);
					HUD.DrawText(0.4f, 0.95f, "Conto tocchi effettuati = " + tocchiEffettuati);
					if (contogenerico > 1000) break;
				}

				if (Funzioni.GetRandomInt(0, 100) < 90 && contogenerico < 1000)
				{
					string pesce = "";
					if (TipoCanna == 0)
						pesce = PuntiPesca.Pesci.facile[Funzioni.GetRandomInt(0, PuntiPesca.Pesci.facile.Count-1)];
					else if (TipoCanna == 1)
						pesce = PuntiPesca.Pesci.medio[Funzioni.GetRandomInt(0, PuntiPesca.Pesci.medio.Count - 1)];
					else if (TipoCanna == 2)
						pesce = PuntiPesca.Pesci.avanzato[Funzioni.GetRandomInt(0, PuntiPesca.Pesci.avanzato.Count - 1)];
					HUD.ShowNotification($"Hai pescato un bell'esemplare di {pesce}");
				}
				else
					HUD.ShowNotification("Il pesce è scappato! Andrà meglio la prossima volta..", true);
				Pescando = false;
				Game.PlayerPed.Task.ClearAll();
				CannaDaPesca.Detach();
				AttachEntityToEntity(CannaDaPesca.Handle, PlayerPedId(), GetPedBoneIndex(PlayerPedId(), /*60309*/ 57005), 0.10f, 0, -0.001f, 80.0f, 150.0f, 200.0f, false, false, false, false, 1, true);
				TaskPlayAnim(PlayerPedId(), "amb@code_human_wander_drinking@beer@male@base", "static", 3.5f, -8, -1, 49, 0, false, false, false);
				Game.PlayerPed.IsPositionFrozen = false;
				SetEnableHandcuffs(PlayerPedId(), false);
				await Task.FromResult(0);
			}
		}

		private async static void MenuBarche()
		{
			UIMenu Barche = new UIMenu("Pescatore", "Scegli la barca", new System.Drawing.PointF(50,50));
			HUD.MenuPool.Add(Barche);
			foreach (var barca in PuntiPesca.Barche)
			{
				UIMenuItem boat = new UIMenuItem(GetLabelText(barca), "~y~Se sei in compagnia dei tuoi amici~w~ potete usare una barca sola insieme e risparmiare nell'affitto!");
				Barche.AddItem(boat);
			}

			Vehicle veh = new Vehicle(0);
			Barche.OnIndexChange += async (menu, index) =>
			{
				if (veh.Exists()) veh.Delete();
				veh = await Funzioni.SpawnLocalVehicle(PuntiPesca.Barche[index], new Vector3(PuntiPesca.SpawnBarca[0], PuntiPesca.SpawnBarca[1], PuntiPesca.SpawnBarca[2]), PuntiPesca.SpawnBarca[3]);
			};
			Barche.OnItemSelect += async (menu, item, index) =>
			{
				HUD.MenuPool.CloseAllMenus();
				if (veh.Exists()) veh.Delete();
				Vehicle newveh = await Funzioni.SpawnVehicleNoPlayerInside(PuntiPesca.Barche[index], new Vector3(PuntiPesca.SpawnBarca[0], PuntiPesca.SpawnBarca[1], PuntiPesca.SpawnBarca[2]), PuntiPesca.SpawnBarca[3]);
				VeicoloLavorativoEAffitto vehlav = new VeicoloLavorativoEAffitto(PuntiPesca.Barche[index], newveh.Mods.LicensePlate, newveh.Model.Hash, newveh.Handle, Eventi.Player.FullName);
				BaseScript.TriggerServerEvent("lprp:registraVeicoloLavorativoENon", JsonConvert.SerializeObject(vehlav));
			};
			Barche.Visible = true;
		}
	}
}
