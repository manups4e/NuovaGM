using CitizenFX.Core;
using CitizenFX.Core.UI;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;
using TheLastPlanet.Client.Core.Personaggio;
using TheLastPlanet.Client.Negozi;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheLastPlanet.Shared;
using Logger;
using TheLastPlanet.Client.Core.Ingresso;
using TheLastPlanet.Client.Core.Status;

namespace TheLastPlanet.Client.Core.Utility
{
	static class Eventi
	{
		public static Dictionary<string, PlayerChar> GiocatoriOnline = new Dictionary<string, PlayerChar>();
		private static int timer = 0;

		public static void Init()
		{
			Client.Instance.AddEventHandler("lprp:setupClientUser", new Action<string>(setupClientUser));
			Client.Instance.AddEventHandler("lprp:teleportCoords", new Action<float, float, float>(teleportCoords));
			//Client.Instance.AddEventHandler("lprp:onPlayerDeath", new Action<dynamic>(onPlayerDeath));
			Client.Instance.AddEventHandler("lprp:sendUserInfo", new Action<string, int, string>(sendUserInfo));
			Client.Instance.AddEventHandler("lprp:ObjectDeleteGun", new Action<string>(DelGun));
			Client.Instance.AddEventHandler("lprp:ShowNotification", new Action<string>(notification));
			Client.Instance.AddEventHandler("lprp:death", new Action(death));
			Client.Instance.AddEventHandler("lprp:announce", new Action<string>(announce));
			Client.Instance.AddEventHandler("lprp:reviveChar", new Action(Revive));
			Client.Instance.AddEventHandler("lprp:spawnVehicle", new Action<string>(SpawnVehicle));
			Client.Instance.AddEventHandler("lprp:deleteVehicle", new Action(DeleteVehicle));
			Client.Instance.AddEventHandler("lprp:mostrasalvataggio", new Action(Salva));
			Client.Instance.AddEventHandler("lprp:StartLocationSave", new Action(StartLocationSave));
			Client.Instance.AddEventHandler("lprp:addWeapon", new Action<string, int>(AddWeapon));
			Client.Instance.AddEventHandler("lprp:removeWeapon", new Action<string>(RemoveWeapon));
			Client.Instance.AddEventHandler("lprp:possiediArma", new Action<string, string>(PossiediArma));
			Client.Instance.AddEventHandler("lprp:possiediTinta", new Action<string, int>(PossiediTinta));
			Client.Instance.AddEventHandler("lprp:addWeaponComponent", new Action<string, string>(AddWeaponComponent));
			Client.Instance.AddEventHandler("lprp:removeWeaponComponent", new Action<string, string>(RemoveWeaponComponent));
			Client.Instance.AddEventHandler("lprp:addWeaponTint", new Action<string, int>(AddWeaponTint));
			Client.Instance.AddEventHandler("lprp:restoreWeapons", new Action(RestoreWeapons));
			Client.Instance.AddEventHandler("lprp:riceviOggettoAnimazione", new Action(AnimazioneRiceviOggetto));
			//Client.Instance.AddTick(Mappina);
			timer = GetGameTimer();
		}

		private static void AnimazioneRiceviOggetto()
		{
			Game.PlayerPed.Task.PlayAnimation("mp_common", "givetake2_a");
		}

		public static async Task AggiornaPlayers()
		{
			GiocatoriOnline.Clear();
			Client.Instance.TriggerServerCallback("ChiamaPlayersOnline", new Action<dynamic>((arg) =>
 		    {
				 GiocatoriOnline = (arg as string).Deserialize<Dictionary<string, PlayerChar>>();
		    }));
			while (GiocatoriOnline.Serialize() == "{}") await BaseScript.Delay(0);
		}

		public static async void LoadModel()
		{
			uint hash = Funzioni.HashUint(Cache.Player.CurrentChar.skin.model);
			RequestModel(hash);
			while (!HasModelLoaded(hash)) await BaseScript.Delay(1);

			SetPlayerModel(PlayerId(), hash);
			await Funzioni.UpdateFace(Cache.Player.CurrentChar.skin);
			await Funzioni.UpdateDress(Cache.Player.CurrentChar.dressing);
			BaseScript.TriggerEvent("lprp:restoreWeapons");
			BaseScript.TriggerEvent("lprp:StartLocationSave");
		}

		public static void setupClientUser(string data)
		{
			Cache.AddPlayer(data);
			DisplayRadar(false);
			LogIn.charSelect();
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


		public static void sendUserInfo(string _char_data, int _char_current, string _group)
		{
			List<Char_data> data = _char_data.Deserialize<List<Char_data>>(true);
			Cache.Player.char_data.Clear();
			Cache.Player.char_data = data;
			Cache.Player.char_current = _char_current;
			Cache.Player.group = _group;
		}

		public static bool On = false;
		public static void DelGun(string toggle)
		{
			if (toggle == "on" && (!On))
			{
				HUD.HUD.ShowNotification("~g~DelGun Attivata!");
				On = true;
			}
			else if (toggle == "on" && On)
			{
				HUD.HUD.ShowNotification("~y~DelGun già attivata!");
			}
			else if (toggle == "off" && On)
			{
				HUD.HUD.ShowNotification("~b~DelGun Disattivata!");
				On = false;
			}
			else if (toggle == "off" && (!On))
			{
				HUD.HUD.ShowNotification("~y~DelGun già Disattivata!");
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
			Game.PlayerPed.Kill();
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

			Main.RespawnPed(Cache.Player.posizione.ToVector3());
			StatsNeeds.Needs["Fame"].Val = 0.0f;
			StatsNeeds.Needs["Sete"].Val = 0.0f;
			StatsNeeds.Needs["Stanchezza"].Val = 0.0f;
			Cache.Player.CurrentChar.needs.malattia = false;
			Needs nee = new Needs()
			{
				fame = StatsNeeds.Needs["Fame"].Val,
				sete = StatsNeeds.Needs["Sete"].Val,
				stanchezza = StatsNeeds.Needs["Stanchezza"].Val,
				malattia = Cache.Player.CurrentChar.needs.malattia
			};
			BaseScript.TriggerServerEvent("lprp:updateCurChar", "needs", nee.Serialize());
			BaseScript.TriggerServerEvent("lprp:setDeathStatus", false);
			Screen.Effects.Stop(ScreenEffect.DeathFailOut);
			Death.endConteggio();
			BaseScript.TriggerServerEvent("lprp:medici:rimuoviDaMorti");
			Cache.Player.StatiPlayer.FinDiVita = false;
			Screen.Fading.FadeIn(800);
		}

		public static async void SpawnVehicle(string model)
		{
			Vector3 coords = Cache.Player.posizione.ToVector3();
			var Veh = await Funzioni.SpawnVehicle(model, coords, Game.PlayerPed.Heading);
			if (Veh != null)
				Veh.PreviouslyOwnedByPlayer = true;
		}

		public static void DeleteVehicle()
		{
			Entity vehicle = new Vehicle(Funzioni.GetVehicleInDirection());
			if (Game.PlayerPed.IsInVehicle())
			{
				vehicle = Game.PlayerPed.CurrentVehicle;
			}

			if (vehicle.Exists())
			{
				DecorRemove(vehicle.Handle, Main.decorName);
			}

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

		public static void StartLocationSave()
		{
			Client.Instance.AddTick(LocationSave);
		}

		public static async Task LocationSave()
		{
			await BaseScript.Delay(1000);
			Cache.Player.posizione = new Vector4(GetEntityCoords(PlayerPedId(), false), GetEntityHeading(PlayerPedId()));
			if (Cache.Player.StatiPlayer.Istanza.Stanziato) return;
			if (GetGameTimer() - timer >= 10000)
			{
				BaseScript.TriggerServerEvent("lprp:updateCurChar", "charlocation", Cache.Player.posizione.ToVector3(), Cache.Player.posizione.W);
				timer = GetGameTimer();
			}
			await Task.FromResult(0);
		}

		public static void AddWeapon(string weaponName, int ammo)
		{
			WeaponHash weaponHash = (WeaponHash)Funzioni.HashUint(weaponName);
			Game.PlayerPed.Weapons.Give(weaponHash, ammo, false, true);
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
			if (!Cache.Player.hasWeaponComponent(weaponName, weaponComponent))
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
			Dictionary<int, bool> ammoTypes = new Dictionary<int, bool>();
			if (Cache.Player.CurrentChar.weapons.Count > 0)
			{
				Game.PlayerPed.Weapons.RemoveAll();
				for (int i = 0; i < Cache.Player.getCharWeapons(Cache.Player.char_current).Count; i++)
				{
					string weaponName = Cache.Player.getCharWeapons(Cache.Player.char_current)[i].name;
					uint weaponHash = Funzioni.HashUint(weaponName);
					int tint = Cache.Player.getCharWeapons(Cache.Player.char_current)[i].tint;
					Game.PlayerPed.Weapons.Give((WeaponHash)weaponHash, 0, false, false);
					int ammoType = GetPedAmmoTypeFromWeapon(PlayerPedId(), weaponHash);
					if (Cache.Player.getCharWeapons(Cache.Player.char_current)[i].components.Count > 0)
					{
						for (int j = 0; j < Cache.Player.getCharWeapons(Cache.Player.char_current)[i].components.Count; j++)
						{
							Components weaponComponent = Cache.Player.getCharWeapons(Cache.Player.char_current)[i].components[j];
							uint componentHash = Funzioni.HashUint(weaponComponent.name);
							if (weaponComponent.active)
							{
								GiveWeaponComponentToPed(PlayerPedId(), weaponHash, componentHash);
							}
						}
					}
					SetPedWeaponTintIndex(PlayerPedId(), weaponHash, tint);
					if (!ammoTypes.ContainsKey(ammoType))
					{
						AddAmmoToPed(PlayerPedId(), weaponHash, Cache.Player.getCharWeapons(Cache.Player.char_current)[i].ammo);
						ammoTypes[ammoType] = true;
					}
				}
			}
			Main.LoadoutLoaded = true;
		}
	}
}