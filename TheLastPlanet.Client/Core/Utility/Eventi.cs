using CitizenFX.Core;
using CitizenFX.Core.UI;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;
using TheLastPlanet.Client.Negozi;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Impostazioni.Shared.Configurazione.Generici;
using TheLastPlanet.Shared;
using Logger;
using TheLastPlanet.Client.Core.Status;

namespace TheLastPlanet.Client.Core.Utility
{
	internal static class Eventi
	{
		private static int timer = 0;

		public static void Init()
		{
			ClientSession.Instance.AddEventHandler("lprp:teleportCoords", new Action<float, float, float>(teleportCoords));
			//Client.Instance.AddEventHandler("lprp:onPlayerDeath", new Action<dynamic>(onPlayerDeath));
			ClientSession.Instance.AddEventHandler("lprp:sendUserInfo", new Action<string, uint, string>(sendUserInfo));
			ClientSession.Instance.AddEventHandler("lprp:ObjectDeleteGun", new Action<string>(DelGun));
			ClientSession.Instance.AddEventHandler("lprp:ShowNotification", new Action<string>(notification));
			ClientSession.Instance.AddEventHandler("lprp:death", new Action(death));
			ClientSession.Instance.AddEventHandler("lprp:announce", new Action<string>(announce));
			ClientSession.Instance.AddEventHandler("lprp:reviveChar", new Action(Revive));
			ClientSession.Instance.AddEventHandler("lprp:spawnVehicle", new Action<string>(SpawnVehicle));
			ClientSession.Instance.AddEventHandler("lprp:deleteVehicle", new Action(DeleteVehicle));
			ClientSession.Instance.AddEventHandler("lprp:mostrasalvataggio", new Action(Salva));
			ClientSession.Instance.AddEventHandler("lprp:addWeapon", new Action<string, int>(AddWeapon));
			ClientSession.Instance.AddEventHandler("lprp:removeWeapon", new Action<string>(RemoveWeapon));
			ClientSession.Instance.AddEventHandler("lprp:possiediArma", new Action<string, string>(PossiediArma));
			ClientSession.Instance.AddEventHandler("lprp:possiediTinta", new Action<string, int>(PossiediTinta));
			ClientSession.Instance.AddEventHandler("lprp:addWeaponComponent", new Action<string, string>(AddWeaponComponent));
			ClientSession.Instance.AddEventHandler("lprp:removeWeaponComponent", new Action<string, string>(RemoveWeaponComponent));
			ClientSession.Instance.AddEventHandler("lprp:addWeaponTint", new Action<string, int>(AddWeaponTint));
			ClientSession.Instance.AddEventHandler("lprp:restoreWeapons", new Action(RestoreWeapons));
			ClientSession.Instance.AddEventHandler("lprp:riceviOggettoAnimazione", new Action(AnimazioneRiceviOggetto));
			//Client.Instance.AddTick(Mappina);
			timer = GetGameTimer();
		}

		private static void AnimazioneRiceviOggetto()
		{
			SessionCache.Cache.MyPlayer.Ped.Task.PlayAnimation("mp_common", "givetake2_a");
		}

		public static async Task AggiornaPlayers()
		{
			SessionCache.Cache.GiocatoriOnline = await ClientSession.Instance.SistemaEventi.Request<Dictionary<string, PlayerChar.User>>("lprp:callPlayers");
		}

		public static async void LoadModel()
		{
			uint hash = Funzioni.HashUint(SessionCache.Cache.MyPlayer.User.CurrentChar.skin.model);
			RequestModel(hash);
			while (!HasModelLoaded(hash)) await BaseScript.Delay(1);
			SetPlayerModel(PlayerId(), hash);
			SessionCache.Cache.MyPlayer.UpdatePedId();
			await Funzioni.UpdateFace(SessionCache.Cache.MyPlayer.User.CurrentChar.skin);
			await Funzioni.UpdateDress(SessionCache.Cache.MyPlayer.User.CurrentChar.dressing);
			// TODO: Cambiare con request
			BaseScript.TriggerEvent("lprp:restoreWeapons");
		}

		public static async void teleportCoords(float x, float y, float z)
		{
			Screen.Fading.FadeOut(500);
			await BaseScript.Delay(1000);
			StartPlayerTeleport(PlayerId(), x, y, z, 0, true, true, true);
			while (!HasPlayerTeleportFinished(PlayerId())) await BaseScript.Delay(0);
			await BaseScript.Delay(2000);
			Screen.Fading.FadeIn(500);
			//Funzioni.Teleport(pos);
		}

		public static void sendUserInfo(string _char_data, uint _char_current, string _group)
		{
			Log.Printa(LogType.Debug, _char_data);
			SessionCache.Cache.MyPlayer.User.char_data = _char_data;
			SessionCache.Cache.MyPlayer.User.char_current = _char_current;
			SessionCache.Cache.MyPlayer.User.group = _group;
		}

		public static bool On = false;

		public static void DelGun(string toggle)
		{
			switch (toggle)
			{
				case "on" when !On:
					HUD.HUD.ShowNotification("~g~DelGun Attivata!");
					On = true;

					break;
				case "on" when On:
					HUD.HUD.ShowNotification("~y~DelGun già attivata!");

					break;
				case "off" when On:
					HUD.HUD.ShowNotification("~b~DelGun Disattivata!");
					On = false;

					break;
				case "off" when !On:
					HUD.HUD.ShowNotification("~y~DelGun già Disattivata!");

					break;
			}
		}

		public static void notification(string text)
		{
			HUD.HUD.ShowNotification(text);
		}

		public static void advancedNotification(string title, string subject, string msg, string icon, HUD.IconType iconType)
		{
			HUD.HUD.ShowAdvancedNotification(title, subject, msg, icon, iconType);
		}

		public static void death()
		{
			SessionCache.Cache.MyPlayer.Ped.Kill();
		}

		public static async void announce(string msg)
		{
			Game.PlaySound("DELETE", "HUD_DEATHMATCH_SOUNDSET");
			MenuNativo.BigMessageThread.MessageInstance.ShowSimpleShard("~r~ANNUNCIO AI GIOCATORI", msg);
		}

		public static async void Revive()
		{
			Screen.Fading.FadeOut(800);
			while (Screen.Fading.IsFadingOut) await BaseScript.Delay(50);
			Main.RespawnPed(SessionCache.Cache.MyPlayer.User.posizione.ToVector3());
			StatsNeeds.Needs["Fame"].Val = 0.0f;
			StatsNeeds.Needs["Sete"].Val = 0.0f;
			StatsNeeds.Needs["Stanchezza"].Val = 0.0f;
			SessionCache.Cache.MyPlayer.User.CurrentChar.needs.malattia = false;
			Needs nee = new() { fame = StatsNeeds.Needs["Fame"].Val, sete = StatsNeeds.Needs["Sete"].Val, stanchezza = StatsNeeds.Needs["Stanchezza"].Val, malattia = SessionCache.Cache.MyPlayer.User.CurrentChar.needs.malattia };
			BaseScript.TriggerServerEvent("lprp:updateCurChar", "needs", nee.SerializeToJson());
			BaseScript.TriggerServerEvent("lprp:setDeathStatus", false);
			Screen.Effects.Stop(ScreenEffect.DeathFailOut);
			Death.endConteggio();
			BaseScript.TriggerServerEvent("lprp:medici:rimuoviDaMorti");
			SessionCache.Cache.MyPlayer.User.StatiPlayer.FinDiVita = false;
			Screen.Fading.FadeIn(800);
		}

		public static async void SpawnVehicle(string model)
		{
			Vector3 coords = SessionCache.Cache.MyPlayer.User.posizione.ToVector3();
			Vehicle Veh = await Funzioni.SpawnVehicle(model, coords, SessionCache.Cache.MyPlayer.Ped.Heading);
			if (Veh != null) Veh.PreviouslyOwnedByPlayer = true;
		}

		public static void DeleteVehicle()
		{
			Entity vehicle = new Vehicle(Funzioni.GetVehicleInDirection());
			if (SessionCache.Cache.MyPlayer.User.StatiPlayer.InVeicolo) vehicle = SessionCache.Cache.MyPlayer.Ped.CurrentVehicle;
			if (vehicle.Exists()) DecorRemove(vehicle.Handle, Main.decorName);
			vehicle.Delete();
		}

		/*
				public static bool wasmenuopen = false;
				public static async Task Mappina()
				{
					if (Game.IsPaused && !wasmenuopen && !IsPedInAnyVehicle(PlayerPedId(), false))
					{
						SetCurrentPedWeapon(PlayerPedId(), 0xA2719263, true);
						TaskStartScenarioInPlace(PlayerPedId(), "WORLD_HUMAN_TOURIST_MAP", 0, false);
						wasmenuopen = true;
					}

					if (!Game.IsPaused && wasmenuopen && !IsPedInAnyVehicle(PlayerPedId(), false))
					{
						ClearPedTasks(PlayerPedId());
						ClearPedSecondaryTask(PlayerPedId());
						wasmenuopen = false;
					}
					await BaseScript.Delay(100);
				}
		*/

		public static async void Salva()
		{
			Screen.LoadingPrompt.Show("Salvataggio Personaggio...", LoadingSpinnerType.SocialClubSaving);
			await BaseScript.Delay(5000);
			Screen.LoadingPrompt.Hide();
		}

		public static void AddWeapon(string weaponName, int ammo)
		{
			WeaponHash weaponHash = (WeaponHash)Funzioni.HashUint(weaponName);
			SessionCache.Cache.MyPlayer.Ped.Weapons.Give(weaponHash, ammo, false, true);
			HUD.HUD.ShowNotification("Hai ottenuto un/a ~y~" + Funzioni.GetWeaponLabel((uint)weaponHash));
		}

		public static void RemoveWeapon(string weaponName)
		{
			WeaponHash weaponHash = (WeaponHash)GetHashKey(weaponName);
			RemoveWeaponFromPed(PlayerPedId(), (uint)weaponHash);
			SetPedAmmo(PlayerPedId(), (uint)weaponHash, 0);
			HUD.HUD.ShowNotification("Rimosso/a ~y~" + Funzioni.GetWeaponLabel((uint)weaponHash));
		}

		public static void PossiediArma(string weaponName, string componentName)
		{
			HUD.HUD.ShowNotification("Possiedi già la modifica: ~y~" + Funzioni.GetWeaponLabel(Funzioni.HashUint(componentName)) + "~w~ per ~b~" + Funzioni.GetWeaponLabel(Funzioni.HashUint(weaponName)) + "~w~.");
		}

		public static void PossiediTinta(string weaponName, int tinta)
		{
			HUD.HUD.ShowNotification("Possiedi già la modifica: ~y~" + Funzioni.GetWeaponLabel(Funzioni.HashUint(Armerie.tinte[tinta].name)) + "~w~ per ~b~" + Funzioni.GetWeaponLabel(Funzioni.HashUint(weaponName)) + "~w~.");
		}

		public static void AddWeaponComponent(string weaponName, string weaponComponent)
		{
			uint weaponHash = Funzioni.HashUint(weaponName);
			uint componentHash = Funzioni.HashUint(weaponComponent);

			if (!SessionCache.Cache.MyPlayer.User.HasWeaponComponent(weaponName, weaponComponent))
			{
				GiveWeaponComponentToPed(PlayerPedId(), weaponHash, componentHash);
				HUD.HUD.ShowNotification("Hai ottenuto un ~b~" + Funzioni.GetWeaponLabel(componentHash));
			}
			else
			{
				HUD.HUD.ShowNotification("Quest'arma ha già un" + Funzioni.GetWeaponLabel(componentHash));
			}
		}

		public static void RemoveWeaponComponent(string weaponName, string weaponComponent)
		{
			uint weaponHash = Funzioni.HashUint(weaponName);
			uint componentHash = Funzioni.HashUint(weaponComponent);
			RemoveWeaponComponentFromPed(PlayerPedId(), weaponHash, componentHash);
			HUD.HUD.ShowNotification("Rimosso/a ~b~" + Funzioni.GetWeaponLabel(componentHash));
		}

		public static void AddWeaponTint(string weaponName, int tint)
		{
			uint weaponHash = Funzioni.HashUint(weaponName);
			SetPedWeaponTintIndex(PlayerPedId(), weaponHash, tint);
		}

		public static void RestoreWeapons()
		{
			Dictionary<int, bool> ammoTypes = new();

			if (SessionCache.Cache.MyPlayer.User.CurrentChar.weapons.Count > 0)
			{
				SessionCache.Cache.MyPlayer.Ped.Weapons.RemoveAll();

				for (int i = 0; i < SessionCache.Cache.MyPlayer.User.GetCharWeapons(SessionCache.Cache.MyPlayer.User.char_current).Count; i++)
				{
					string weaponName = SessionCache.Cache.MyPlayer.User.GetCharWeapons(SessionCache.Cache.MyPlayer.User.char_current)[i].name;
					uint weaponHash = Funzioni.HashUint(weaponName);
					int tint = SessionCache.Cache.MyPlayer.User.GetCharWeapons(SessionCache.Cache.MyPlayer.User.char_current)[i].tint;
					SessionCache.Cache.MyPlayer.Ped.Weapons.Give((WeaponHash)weaponHash, 0, false, false);
					int ammoType = GetPedAmmoTypeFromWeapon(PlayerPedId(), weaponHash);

					if (SessionCache.Cache.MyPlayer.User.GetCharWeapons(SessionCache.Cache.MyPlayer.User.char_current)[i].components.Count > 0)
						foreach (Components weaponComponent in SessionCache.Cache.MyPlayer.User.GetCharWeapons(SessionCache.Cache.MyPlayer.User.char_current)[i].components)
						{
							uint componentHash = Funzioni.HashUint(weaponComponent.name);
							if (weaponComponent.active) GiveWeaponComponentToPed(PlayerPedId(), weaponHash, componentHash);
						}

					SetPedWeaponTintIndex(PlayerPedId(), weaponHash, tint);

					if (ammoTypes.ContainsKey(ammoType)) continue;
					AddAmmoToPed(PlayerPedId(), weaponHash, SessionCache.Cache.MyPlayer.User.GetCharWeapons(SessionCache.Cache.MyPlayer.User.char_current)[i].ammo);
					ammoTypes[ammoType] = true;
				}
			}

			Main.LoadoutLoaded = true;
		}
	}
}