using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.MenuNativo;
using NuovaGM.Client.gmPrincipale.Utility;
using CitizenFX.Core.UI;
using NuovaGM.Client.gmPrincipale;

namespace NuovaGM.Client.Proprietà.Hotel
{
	static class Hotels
	{
		private static Vector3 OldPos;
		private static bool IsInPiccola;
		private static bool IsInMedia;
		private static bool IsInAppartamento;
		public static void Init()
		{
			RegisterCommand("hash", new Action<int, List<dynamic>, string>((id, hash, comando) =>
			{
				Debug.WriteLine("Hash = " + GetHashKey(hash[0]+""));
			}), false);
			foreach (var hotel in ConfigClient.Conf.Proprieta.hotels)
			{
				Blip p = new Blip(AddBlipForCoord(hotel.Coords[0], hotel.Coords[1], hotel.Coords[2]))
				{
					Sprite = BlipSprite.Heist,
					Scale = 1.0f,
					Color = BlipColor.Yellow,
					IsShortRange = true,
				};
			}
			Client.GetInstance.RegisterTickHandler(ControlloHotel);
		}

		public static async Task ControlloHotel()
		{
			for (int i=0; i< ConfigClient.Conf.Proprieta.hotels.Count; i++)
			{
				if (World.GetDistance(Game.PlayerPed.Position, ConfigClient.Conf.Proprieta.hotels[i].Coords.ToVector3()) < 3f && !HUD.MenuPool.IsAnyMenuOpen())
				{
					HUD.ShowHelp($"~INPUT_CONTEXT~ per soggiornare al ~b~{ConfigClient.Conf.Proprieta.hotels[i].Name}~w~.");
					if (Game.IsControlJustPressed(0, Control.Context))
						MenuHotel(ConfigClient.Conf.Proprieta.hotels[i]);
				}
			}
		}

		private static async void MenuHotel(Hotel hotel)
		{
			UIMenu HotelMenu = new UIMenu(hotel.Name, "~b~Benvenuto.", new System.Drawing.PointF(50, 50));
			HUD.MenuPool.Add(HotelMenu);
			UIMenuItem stanzaPiccola = new UIMenuItem("Stanza Piccola", "Costa poco.. e ha un letto..");
			stanzaPiccola.SetRightLabel((Eventi.Player.Money >= hotel.Prezzi.StanzaPiccola || Eventi.Player.Bank >= hotel.Prezzi.StanzaPiccola ? "~g~$" : "~r~$") + hotel.Prezzi.StanzaPiccola);
			UIMenuItem stanzaMedia = new UIMenuItem("Stanza Media", "Costa un po' di più.. ed è un po' più confortevole");
			stanzaMedia.SetRightLabel((Eventi.Player.Money >= hotel.Prezzi.StanzaMedia || Eventi.Player.Bank >= hotel.Prezzi.StanzaMedia ? "~g~$" : "~r~$") + hotel.Prezzi.StanzaMedia);
			UIMenuItem appartamento = new UIMenuItem("Appartamento", "Vorresti viverci.. ma prima o poi dovrai andartene!");
			appartamento.SetRightLabel((Eventi.Player.Money >= hotel.Prezzi.Appartamento || Eventi.Player.Bank >= hotel.Prezzi.Appartamento ? "~g~$" : "~r~$") + hotel.Prezzi.Appartamento);
			HotelMenu.AddItem(stanzaPiccola);
			HotelMenu.AddItem(stanzaMedia);
			HotelMenu.AddItem(appartamento);	

			HotelMenu.OnItemSelect += async (menu, item, index) =>
			{
				Vector3 pos = new Vector3(0);
				if (item == stanzaPiccola)
				{
					if (Eventi.Player.Money >= hotel.Prezzi.StanzaPiccola || Eventi.Player.Bank >= hotel.Prezzi.StanzaPiccola)
					{
						if (Eventi.Player.Money >= hotel.Prezzi.StanzaPiccola)
							BaseScript.TriggerServerEvent("lprp:removemoney", hotel.Prezzi.StanzaPiccola);
						else
							BaseScript.TriggerServerEvent("lprp:removebank", hotel.Prezzi.StanzaPiccola);
						pos = new Vector3(266.094f, -1007.487f, -101.800f);
						IsInPiccola = true;
					}
					else
						HUD.ShowNotification("Non hai abbastanza fondi!", NotificationColor.Red, true);
				}
				else if (item == stanzaMedia)
				{
					if (Eventi.Player.Money >= hotel.Prezzi.StanzaMedia || Eventi.Player.Bank >= hotel.Prezzi.StanzaMedia)
					{
						if (Eventi.Player.Money >= hotel.Prezzi.StanzaMedia)
							BaseScript.TriggerServerEvent("lprp:removemoney", hotel.Prezzi.StanzaMedia);
						else
							BaseScript.TriggerServerEvent("lprp:removebank", hotel.Prezzi.StanzaMedia);
						pos = new Vector3(346.493f, -1013.031f, -99.196f);
						IsInMedia = true;
					}
					else
						HUD.ShowNotification("Non hai abbastanza fondi!", NotificationColor.Red, true);
				}
				else if (item == appartamento)
				{
					if (Eventi.Player.Money >= hotel.Prezzi.Appartamento || Eventi.Player.Bank >= hotel.Prezzi.Appartamento)
					{
						if (Eventi.Player.Money >= hotel.Prezzi.Appartamento)
							BaseScript.TriggerServerEvent("lprp:removemoney", hotel.Prezzi.Appartamento);
						else
							BaseScript.TriggerServerEvent("lprp:removebank", hotel.Prezzi.Appartamento);
						pos = new Vector3(-1452.841f, -539.489f, 74.044f);
						IsInAppartamento = true;
					}
					else
						HUD.ShowNotification("Non hai abbastanza fondi!", NotificationColor.Red, true);
				}
				Client.GetInstance.DeregisterTickHandler(Eventi.LocationSave);
				menu.Visible = false;
				Screen.Fading.FadeOut(800);
				await BaseScript.Delay(1000);
				OldPos = Game.PlayerPed.Position;
				RequestCollisionAtCoord(pos.X, pos.Y, pos.Z);
				Game.PlayerPed.Position = pos;
				await BaseScript.Delay(2000);
				Eventi.Player.Stanziato = true;
				Eventi.Player.InCasa = true;
				Screen.Fading.FadeIn(800);
				Client.GetInstance.RegisterTickHandler(GestioneHotel);
			};
			HotelMenu.Visible = true;
		}

		public static async Task GestioneHotel()
		{
			if (!HUD.MenuPool.IsAnyMenuOpen())
			{
				if (IsInPiccola)
				{
					if (World.GetDistance(Game.PlayerPed.Position, new Vector3(266.094f, -1007.487f, -101.800f)) < 1.3f)
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per uscire dalla stanza");
						if (Game.IsControlJustPressed(0, Control.Context))
						{
							Screen.Fading.FadeOut(800);
							await BaseScript.Delay(1000);
							RequestCollisionAtCoord(OldPos.X, OldPos.Y, OldPos.Z);
							Game.PlayerPed.Position = OldPos;
							await BaseScript.Delay(2000);
							Funzioni.RevealAllPlayers();
							Screen.Fading.FadeIn(800);
							IsInPiccola = false;
							Eventi.Player.InCasa = false;
							Eventi.Player.Stanziato = false;
							Client.GetInstance.DeregisterTickHandler(GestioneHotel);
							BaseScript.TriggerEvent("lprp:StartLocationSave");
						}
					}
				}
				if (IsInMedia)
				{
					if (World.GetDistance(Game.PlayerPed.Position, new Vector3(346.493f, -1013.031f, -99.196f)) < 1.3f)
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per uscire dalla stanza");
						if (Game.IsControlJustPressed(0, Control.Context))
						{
							Screen.Fading.FadeOut(800);
							await BaseScript.Delay(1000);
							RequestCollisionAtCoord(OldPos.X, OldPos.Y, OldPos.Z);
							Game.PlayerPed.Position = OldPos;
							await BaseScript.Delay(2000);
							Funzioni.RevealAllPlayers();
							Screen.Fading.FadeIn(800);
							IsInMedia = false;
							Eventi.Player.InCasa = false;
							Eventi.Player.Stanziato = false;
							Client.GetInstance.DeregisterTickHandler(GestioneHotel);
							BaseScript.TriggerEvent("lprp:StartLocationSave");
						}
					}
				}
				if (IsInAppartamento)
				{
					if (World.GetDistance(Game.PlayerPed.Position, new Vector3(-1452.164f, -540.640f, 74.044f)) < 1.3f)
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per uscire dall'appartamento");
						if (Game.IsControlJustPressed(0, Control.Context))
						{
							Screen.Fading.FadeOut(800);
							await BaseScript.Delay(1000);
							RequestCollisionAtCoord(OldPos.X, OldPos.Y, OldPos.Z);
							Game.PlayerPed.Position = OldPos;
							await BaseScript.Delay(2000);
							Funzioni.RevealAllPlayers();
							Screen.Fading.FadeIn(800);
							IsInAppartamento = false;
							Eventi.Player.InCasa = false;
							Eventi.Player.Stanziato = false;
							Client.GetInstance.DeregisterTickHandler(GestioneHotel);
							BaseScript.TriggerEvent("lprp:StartLocationSave");
						}
					}
				}
			}
		}
	}
}
