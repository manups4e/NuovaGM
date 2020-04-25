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
using NuovaGM.Shared;
using Logger;

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
				Log.Printa(LogType.Debug, "Hash = " + GetHashKey(hash[0] + ""));
			}), false);
			foreach (var hotel in Client.Impostazioni.Proprieta.hotels)
			{
				Blip p = new Blip(AddBlipForCoord(hotel.Coords[0], hotel.Coords[1], hotel.Coords[2]))
				{
					Sprite = BlipSprite.Heist,
					Scale = 1.0f,
					Color = BlipColor.Yellow,
					IsShortRange = true,
					Name = "Hotel"
				};
			}

			RegisterCommand("weaphash", new Action<int, List<dynamic>, string>(async(id, hash, comando) =>
			{
				RequestWeaponAsset(Funzioni.HashUint(hash[0]), 31, 0);
				while (!HasWeaponAssetLoaded(Funzioni.HashUint(hash[0]))) await BaseScript.Delay(0);
				Prop pickupObject = new Prop(CreateWeaponObject(Funzioni.HashUint(hash[0]), 50, Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y, Game.PlayerPed.Position.Z, true, 1.0f, 0));

				Log.Printa(LogType.Debug, "Hash = " + pickupObject.Model.Hash);
			}), false);
			foreach (var hotel in Client.Impostazioni.Proprieta.hotels)
			{
				Blip p = new Blip(AddBlipForCoord(hotel.Coords[0], hotel.Coords[1], hotel.Coords[2]))
				{
					Sprite = BlipSprite.Heist,
					Scale = 1.0f,
					Color = BlipColor.Yellow,
					IsShortRange = true,
					Name = "Hotel"
				};
			}
		}

		public static async Task ControlloHotel()
		{
			for (int i=0; i< Client.Impostazioni.Proprieta.hotels.Count; i++)
			{
				if (World.GetDistance(Game.PlayerPed.Position, Client.Impostazioni.Proprieta.hotels[i].Coords.ToVector3()) < 3f && !HUD.MenuPool.IsAnyMenuOpen())
				{
					HUD.ShowHelp($"~INPUT_CONTEXT~ per soggiornare al ~b~{Client.Impostazioni.Proprieta.hotels[i].Name}~w~.");
					if (Input.IsControlJustPressed(Control.Context))
						MenuHotel(Client.Impostazioni.Proprieta.hotels[i]);
				}
			}
		}

		private static async void MenuHotel(Hotel hotel)
		{
			UIMenu HotelMenu = new UIMenu(hotel.Name, "~b~Benvenuto.", new System.Drawing.PointF(50, 50));
			HUD.MenuPool.Add(HotelMenu);
			UIMenuItem stanzaPiccola = new UIMenuItem("Stanza Piccola", "Costa poco.. e ha un letto..");
			stanzaPiccola.SetRightLabel((Game.Player.GetPlayerData().Money >= hotel.Prezzi.StanzaPiccola || Game.Player.GetPlayerData().Bank >= hotel.Prezzi.StanzaPiccola ? "~g~$" : "~r~$") + hotel.Prezzi.StanzaPiccola);
			UIMenuItem stanzaMedia = new UIMenuItem("Stanza Media", "Costa un po' di più.. ed è un po' più confortevole");
			stanzaMedia.SetRightLabel((Game.Player.GetPlayerData().Money >= hotel.Prezzi.StanzaMedia || Game.Player.GetPlayerData().Bank >= hotel.Prezzi.StanzaMedia ? "~g~$" : "~r~$") + hotel.Prezzi.StanzaMedia);
			UIMenuItem appartamento = new UIMenuItem("Appartamento", "Vorresti viverci.. ma prima o poi dovrai andartene!");
			appartamento.SetRightLabel((Game.Player.GetPlayerData().Money >= hotel.Prezzi.Appartamento || Game.Player.GetPlayerData().Bank >= hotel.Prezzi.Appartamento ? "~g~$" : "~r~$") + hotel.Prezzi.Appartamento);
			HotelMenu.AddItem(stanzaPiccola);
			HotelMenu.AddItem(stanzaMedia);
			HotelMenu.AddItem(appartamento);	

			HotelMenu.OnItemSelect += async (menu, item, index) =>
			{
				Vector3 pos = new Vector3(0);
				if (item == stanzaPiccola)
				{
					if (Game.Player.GetPlayerData().Money >= hotel.Prezzi.StanzaPiccola || Game.Player.GetPlayerData().Bank >= hotel.Prezzi.StanzaPiccola)
					{
						if (Game.Player.GetPlayerData().Money >= hotel.Prezzi.StanzaPiccola)
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
					if (Game.Player.GetPlayerData().Money >= hotel.Prezzi.StanzaMedia || Game.Player.GetPlayerData().Bank >= hotel.Prezzi.StanzaMedia)
					{
						if (Game.Player.GetPlayerData().Money >= hotel.Prezzi.StanzaMedia)
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
					if (Game.Player.GetPlayerData().Money >= hotel.Prezzi.Appartamento || Game.Player.GetPlayerData().Bank >= hotel.Prezzi.Appartamento)
					{
						if (Game.Player.GetPlayerData().Money >= hotel.Prezzi.Appartamento)
							BaseScript.TriggerServerEvent("lprp:removemoney", hotel.Prezzi.Appartamento);
						else
							BaseScript.TriggerServerEvent("lprp:removebank", hotel.Prezzi.Appartamento);
						pos = new Vector3(-1452.841f, -539.489f, 74.044f);
						IsInAppartamento = true;
					}
					else
						HUD.ShowNotification("Non hai abbastanza fondi!", NotificationColor.Red, true);
				}
				Client.Instance.RemoveTick(Eventi.LocationSave);
				menu.Visible = false;
				Screen.Fading.FadeOut(800);
				await BaseScript.Delay(1000);
				OldPos = Game.PlayerPed.Position;
				RequestCollisionAtCoord(pos.X, pos.Y, pos.Z);
				Game.PlayerPed.Position = pos;
				await BaseScript.Delay(2000);
				Game.PlayerPed.SetDecor("PlayerStanziato", true);
				Game.PlayerPed.SetDecor("PlayerInCasa", true);
				Screen.Fading.FadeIn(800);
				Client.Instance.AddTick(GestioneHotel);
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
						if (Input.IsControlJustPressed(Control.Context))
						{
							Screen.Fading.FadeOut(800);
							await BaseScript.Delay(1000);
							RequestCollisionAtCoord(OldPos.X, OldPos.Y, OldPos.Z);
							Game.PlayerPed.Position = OldPos;
							await BaseScript.Delay(2000);
							Funzioni.RevealAllPlayers();
							Screen.Fading.FadeIn(800);
							IsInPiccola = false;
							Game.PlayerPed.SetDecor("PlayerInCasa", false);
							Game.PlayerPed.SetDecor("PlayerStanziato", false);
							Client.Instance.RemoveTick(GestioneHotel);
							BaseScript.TriggerEvent("lprp:StartLocationSave");
						}
					}
				}
				if (IsInMedia)
				{
					if (World.GetDistance(Game.PlayerPed.Position, new Vector3(346.493f, -1013.031f, -99.196f)) < 1.3f)
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per uscire dalla stanza");
						if (Input.IsControlJustPressed(Control.Context))
						{
							Screen.Fading.FadeOut(800);
							await BaseScript.Delay(1000);
							RequestCollisionAtCoord(OldPos.X, OldPos.Y, OldPos.Z);
							Game.PlayerPed.Position = OldPos;
							await BaseScript.Delay(2000);
							Funzioni.RevealAllPlayers();
							Screen.Fading.FadeIn(800);
							IsInMedia = false;
							Game.PlayerPed.SetDecor("PlayerInCasa", false);
							Game.PlayerPed.SetDecor("PlayerStanziato", false);
							Client.Instance.RemoveTick(GestioneHotel);
							BaseScript.TriggerEvent("lprp:StartLocationSave");
						}
					}
				}
				if (IsInAppartamento)
				{
					if (World.GetDistance(Game.PlayerPed.Position, new Vector3(-1452.164f, -540.640f, 74.044f)) < 1.3f)
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per uscire dall'appartamento");
						if (Input.IsControlJustPressed(Control.Context))
						{
							Screen.Fading.FadeOut(800);
							await BaseScript.Delay(1000);
							RequestCollisionAtCoord(OldPos.X, OldPos.Y, OldPos.Z);
							Game.PlayerPed.Position = OldPos;
							await BaseScript.Delay(2000);
							Funzioni.RevealAllPlayers();
							Screen.Fading.FadeIn(800);
							IsInAppartamento = false;
							Game.PlayerPed.SetDecor("PlayerInCasa", false);
							Game.PlayerPed.SetDecor("PlayerStanziato", false);
							Client.Instance.RemoveTick(GestioneHotel);
							BaseScript.TriggerEvent("lprp:StartLocationSave");
						}
					}
				}
			}
		}
	}
}
