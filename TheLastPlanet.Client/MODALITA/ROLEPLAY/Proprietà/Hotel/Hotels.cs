using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.NativeUI;
using TheLastPlanet.Client.Core.Utility;
using CitizenFX.Core.UI;
using TheLastPlanet.Client.Core;
using TheLastPlanet.Shared;
using Logger;
using System.Linq;
using TheLastPlanet.Client.Handlers;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Proprietà.Hotel
{
	internal static class Hotels
	{
		private static Vector3 OldPos;
		private static bool IsInPiccola;
		private static bool IsInMedia;
		private static bool IsInAppartamento;
		private static List<InputController> hotelInputs = new();
		private static List<Blip> blips = new();

		public static void Init()
		{
			foreach (Hotel t in Client.Impostazioni.RolePlay.Proprieta.hotels) hotelInputs.Add(new InputController(Control.Context, t.Coords.ToPosition(), $"~INPUT_CONTEXT~ per soggiornare al ~b~{t.Name}~w~.", new((MarkerType)(-1), t.Coords.ToPosition(), NativeUI.Colors.Transparent), ModalitaServer.Roleplay, PadCheck.Any, ControlModifier.None, new Action<Ped, object[]>(MenuHotel), t));
			InputHandler.AddInputList(hotelInputs);
			RegisterCommand("hash", new Action<int, List<dynamic>, string>((id, hash, comando) =>
			{
				Client.Logger.Debug("Hash = " + GetHashKey(hash[0] + ""));
			}), false);

			foreach (Blip p in Client.Impostazioni.RolePlay.Proprieta.hotels.Select(hotel => new Blip(AddBlipForCoord(hotel.Coords[0], hotel.Coords[1], hotel.Coords[2]))
			{
				Sprite = BlipSprite.Heist,
				Scale = 1.0f,
				Color = BlipColor.Yellow,
				IsShortRange = true,
				Name = "Hotel"
			})) { blips.Add(p); }

			RegisterCommand("weaphash", new Action<int, List<dynamic>, string>(async (id, hash, comando) =>
			{
				RequestWeaponAsset(Funzioni.HashUint(hash[0]), 31, 0);
				while (!HasWeaponAssetLoaded(Funzioni.HashUint(hash[0]))) await BaseScript.Delay(0);
				Prop pickupObject = new Prop(CreateWeaponObject(Funzioni.HashUint(hash[0]), 50, Cache.PlayerCache.MyPlayer.Posizione.X, Cache.PlayerCache.MyPlayer.Posizione.Y, Cache.PlayerCache.MyPlayer.Posizione.Z, true, 1.0f, 0));
				Client.Logger.Debug("Hash = " + pickupObject.Model.Hash);
			}), false);
		}

		public static void Stop()
		{
			InputHandler.RemoveInputList(hotelInputs);
			blips.ForEach(x => x.Delete());
			blips.Clear();
			hotelInputs.Clear();
		}

		private static async void MenuHotel(Ped _, object[] args)
		{
			Hotel hotel = (Hotel)args[0];
			UIMenu HotelMenu = new UIMenu(hotel.Name, "~b~Benvenuto.", new System.Drawing.PointF(50, 50));
			HUD.MenuPool.Add(HotelMenu);
			UIMenuItem stanzaPiccola = new UIMenuItem("Stanza Piccola", "Costa poco.. e ha un letto..");
			stanzaPiccola.SetRightLabel((Cache.PlayerCache.MyPlayer.User.Money >= hotel.Prezzi.StanzaPiccola || Cache.PlayerCache.MyPlayer.User.Bank >= hotel.Prezzi.StanzaPiccola ? "~g~$" : "~r~$") + hotel.Prezzi.StanzaPiccola);
			UIMenuItem stanzaMedia = new UIMenuItem("Stanza Media", "Costa un po' di più.. ed è un po' più confortevole");
			stanzaMedia.SetRightLabel((Cache.PlayerCache.MyPlayer.User.Money >= hotel.Prezzi.StanzaMedia || Cache.PlayerCache.MyPlayer.User.Bank >= hotel.Prezzi.StanzaMedia ? "~g~$" : "~r~$") + hotel.Prezzi.StanzaMedia);
			UIMenuItem appartamento = new UIMenuItem("Appartamento", "Vorresti viverci.. ma prima o poi dovrai andartene!");
			appartamento.SetRightLabel((Cache.PlayerCache.MyPlayer.User.Money >= hotel.Prezzi.Appartamento || Cache.PlayerCache.MyPlayer.User.Bank >= hotel.Prezzi.Appartamento ? "~g~$" : "~r~$") + hotel.Prezzi.Appartamento);
			HotelMenu.AddItem(stanzaPiccola);
			HotelMenu.AddItem(stanzaMedia);
			HotelMenu.AddItem(appartamento);
			HotelMenu.OnItemSelect += async (menu, item, index) =>
			{
				Vector3 pos = new Vector3(0);

				if (item == stanzaPiccola)
				{
					if (Cache.PlayerCache.MyPlayer.User.Money >= hotel.Prezzi.StanzaPiccola || Cache.PlayerCache.MyPlayer.User.Bank >= hotel.Prezzi.StanzaPiccola)
					{
						BaseScript.TriggerServerEvent(Cache.PlayerCache.MyPlayer.User.Money >= hotel.Prezzi.StanzaPiccola ? "lprp:removemoney" : "lprp:removebank", hotel.Prezzi.StanzaPiccola);
						pos = new Vector3(266.094f, -1007.487f, -101.800f);
						IsInPiccola = true;
					}
					else
					{
						HUD.ShowNotification("Non hai abbastanza fondi!", NotificationColor.Red, true);
					}
				}
				else if (item == stanzaMedia)
				{
					if (Cache.PlayerCache.MyPlayer.User.Money >= hotel.Prezzi.StanzaMedia || Cache.PlayerCache.MyPlayer.User.Bank >= hotel.Prezzi.StanzaMedia)
					{
						BaseScript.TriggerServerEvent(Cache.PlayerCache.MyPlayer.User.Money >= hotel.Prezzi.StanzaMedia ? "lprp:removemoney" : "lprp:removebank", hotel.Prezzi.StanzaMedia);
						pos = new Vector3(346.493f, -1013.031f, -99.196f);
						IsInMedia = true;
					}
					else
					{
						HUD.ShowNotification("Non hai abbastanza fondi!", NotificationColor.Red, true);
					}
				}
				else if (item == appartamento)
				{
					if (Cache.PlayerCache.MyPlayer.User.Money >= hotel.Prezzi.Appartamento || Cache.PlayerCache.MyPlayer.User.Bank >= hotel.Prezzi.Appartamento)
					{
						BaseScript.TriggerServerEvent(Cache.PlayerCache.MyPlayer.User.Money >= hotel.Prezzi.Appartamento ? "lprp:removemoney" : "lprp:removebank", hotel.Prezzi.Appartamento);
						pos = new Vector3(-1452.841f, -539.489f, 74.044f);
						IsInAppartamento = true;
					}
					else
					{
						HUD.ShowNotification("Non hai abbastanza fondi!", NotificationColor.Red, true);
					}
				}

				menu.Visible = false;
				Screen.Fading.FadeOut(800);
				await BaseScript.Delay(1000);
				OldPos = Cache.PlayerCache.MyPlayer.Posizione.ToVector3;
				RequestCollisionAtCoord(pos.X, pos.Y, pos.Z);
				Cache.PlayerCache.MyPlayer.Ped.Position = pos;
				await BaseScript.Delay(2000);
				Cache.PlayerCache.MyPlayer.User.Status.Istanza.Istanzia("Hotel");
				Cache.PlayerCache.MyPlayer.User.Status.RolePlayStates.InCasa = true;
				Screen.Fading.FadeIn(800);
				Client.Instance.AddTick(GestioneHotel);
			};
			HotelMenu.Visible = true;
		}

		public static async Task GestioneHotel()
		{
			if (!HUD.MenuPool.IsAnyMenuOpen)
			{
				if (IsInPiccola)
					if (Cache.PlayerCache.MyPlayer.Ped.IsInRangeOf(new Vector3(266.094f, -1007.487f, -101.800f), 1.3f))
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per uscire dalla stanza");

						if (Input.IsControlJustPressed(Control.Context))
						{
							Screen.Fading.FadeOut(800);
							await BaseScript.Delay(1000);
							RequestCollisionAtCoord(OldPos.X, OldPos.Y, OldPos.Z);
							Cache.PlayerCache.MyPlayer.Ped.Position = OldPos;
							await BaseScript.Delay(2000);
							Funzioni.RevealAllPlayers();
							Screen.Fading.FadeIn(800);
							IsInPiccola = false;
							Cache.PlayerCache.MyPlayer.User.Status.Istanza.RimuoviIstanza();
							Cache.PlayerCache.MyPlayer.User.Status.RolePlayStates.InCasa = false;
							Client.Instance.RemoveTick(GestioneHotel);
							BaseScript.TriggerEvent("lprp:StartLocationSave");
						}
					}

				if (IsInMedia)
					if (Cache.PlayerCache.MyPlayer.Ped.IsInRangeOf(new Vector3(346.493f, -1013.031f, -99.196f), 1.3f))
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per uscire dalla stanza");

						if (Input.IsControlJustPressed(Control.Context))
						{
							Screen.Fading.FadeOut(800);
							await BaseScript.Delay(1000);
							RequestCollisionAtCoord(OldPos.X, OldPos.Y, OldPos.Z);
							Cache.PlayerCache.MyPlayer.Ped.Position = OldPos;
							await BaseScript.Delay(2000);
							Funzioni.RevealAllPlayers();
							Screen.Fading.FadeIn(800);
							IsInMedia = false;
							Cache.PlayerCache.MyPlayer.User.Status.Istanza.RimuoviIstanza();
							Cache.PlayerCache.MyPlayer.User.Status.RolePlayStates.InCasa = false;
							Client.Instance.RemoveTick(GestioneHotel);
							BaseScript.TriggerEvent("lprp:StartLocationSave");
						}
					}

				if (IsInAppartamento)
					if (Cache.PlayerCache.MyPlayer.Ped.IsInRangeOf(new Vector3(-1452.164f, -540.640f, 74.044f), 1.3f))
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per uscire dall'appartamento");

						if (Input.IsControlJustPressed(Control.Context))
						{
							Screen.Fading.FadeOut(800);
							await BaseScript.Delay(1000);
							RequestCollisionAtCoord(OldPos.X, OldPos.Y, OldPos.Z);
							Cache.PlayerCache.MyPlayer.Ped.Position = OldPos;
							await BaseScript.Delay(2000);
							Funzioni.RevealAllPlayers();
							Screen.Fading.FadeIn(800);
							IsInAppartamento = false;
							Cache.PlayerCache.MyPlayer.User.Status.Istanza.RimuoviIstanza();
							Cache.PlayerCache.MyPlayer.User.Status.RolePlayStates.InCasa = false;
							Client.Instance.RemoveTick(GestioneHotel);
							BaseScript.TriggerEvent("lprp:StartLocationSave");
						}
					}
			}
		}
	}
}