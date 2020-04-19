using CitizenFX.Core;
using CitizenFX.Core.UI;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;
using NuovaGM.Client.gmPrincipale.Personaggio;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.Negozi;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NuovaGM.Shared;

namespace NuovaGM.Client.gmPrincipale.Utility
{
	static class Eventi
	{
		public static PlayerChar Player;
		public static Dictionary<string, PlayerChar> GiocatoriOnline = new Dictionary<string, PlayerChar>();
		//
		public static void Init()
		{
			Client.Instance.AddEventHandler("lprp:setupClientUser", new Action<string>(setupClientUser));
			Client.Instance.AddEventHandler("lprp:teleportCoords", new Action<float, float, float>(teleportCoords));
			Client.Instance.AddEventHandler("lprp:onPlayerDeath", new Action<dynamic>(onPlayerDeath));
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
			Client.Instance.AddEventHandler("lprp:aggiornaPlayers", new Action<string>(AggiornaPlayers));
			Client.Instance.AddEventHandler("lprp:riceviOggettoAnimazione", new Action(AnimazioneRiceviOggetto));
			Client.Instance.AddEventHandler("lprp:triggerProximityDisplay", new Action<int, string, string, int, int, int>(TriggerProximtyDisplay));
			//			Client.Instance.AddTick(Mappina);
		}

		private static void AnimazioneRiceviOggetto()
		{
			Game.PlayerPed.Task.PlayAnimation("mp_common", "givetake2_a");
		}

		public static async void AggiornaPlayers(string jsonPlayers)
		{
			GiocatoriOnline.Clear();
			BaseScript.TriggerServerEvent("lprp:getPlayers", new Action<object>((arg) =>
			{
				GiocatoriOnline = JsonConvert.DeserializeObject<Dictionary<string, PlayerChar>>(arg as string);
			}));
			while (JsonConvert.SerializeObject(GiocatoriOnline) == "{}") await BaseScript.Delay(0);
		}

		public static async void LoadModel()
		{
			uint hash = (uint)GetHashKey(Game.Player.GetPlayerData().CurrentChar.skin.model);
			RequestModel(hash);
			while (!HasModelLoaded(hash))
			{
				await BaseScript.Delay(1);
			}

			SetPlayerModel(PlayerId(), hash);
			await Funzioni.UpdateFace(Game.Player.GetPlayerData().CurrentChar.skin);
			await Funzioni.UpdateDress(Game.Player.GetPlayerData().CurrentChar.dressing);
			BaseScript.TriggerEvent("lprp:restoreWeapons");
			BaseScript.TriggerEvent("lprp:StartLocationSave");
		}

		public static void setupClientUser(string data)
		{
			Player = JsonConvert.DeserializeObject<PlayerChar>(data);
			DisplayRadar(false);
			Player.Stanziato = true;
			Main.charSelect();
		}

		public static void teleportCoords(float x, float y, float z)
		{
			Vector3 pos = new Vector3(x, y, z);
			RequestCollisionAtCoord(pos.X, pos.Y, pos.Z);
			Game.PlayerPed.Position = pos;
		}

		public static void onPlayerDeath(dynamic data)
		{
			Client.Printa(LogType.Debug, JsonConvert.SerializeObject(data));
			Main.IsDead = true;
			BaseScript.TriggerServerEvent("lprp:setDeathStatus", true);
			BaseScript.TriggerEvent("lprp:iniziaConteggio");
			StartScreenEffect("DeathFailOut", 0, false);
		}

		public static void sendUserInfo(string _char_data, int _char_current, string _group)
		{
			List<Char_data> data = JsonConvert.DeserializeObject<List<Char_data>>(_char_data);
			Player.char_data.Clear();
			Player.char_data = data;
			Player.char_current = _char_current;
			Player.group = _group;
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
			Game.PlayerPed.Health = -100;
		}

		public static async void announce(string msg)
		{
			Game.PlaySound("DELETE", "HUD_DEATHMATCH_SOUNDSET");
			MenuNativo.BigMessageThread.MessageInstance.ShowSimpleShard("~r~ANNUNCIO AI GIOCATORI", msg);
		}

		public static async void Revive()
		{
			Screen.Fading.FadeOut(800);
			while (Screen.Fading.IsFadingOut)
			{
				await BaseScript.Delay(50);
			}

			Main.RespawnPed(Game.PlayerPed.Position);
			Status.StatsNeeds.nee.fame = 0.0f;
			Status.StatsNeeds.nee.sete = 0.0f;
			Status.StatsNeeds.nee.stanchezza = 0.0f;
			Status.StatsNeeds.nee.malattia = false;
			BaseScript.TriggerServerEvent("lprp:updateCurChar", "needs", JsonConvert.SerializeObject(Status.StatsNeeds.nee));
			BaseScript.TriggerServerEvent("lprp:setDeathStatus", false);
			Screen.Effects.Stop(ScreenEffect.DeathFailOut);
			BaseScript.TriggerEvent("lprp:fineConteggio");
			BaseScript.TriggerServerEvent("lprp:medici:rimuoviDaMorti");
			Screen.Fading.FadeIn(800);
		}

		public static async void SpawnVehicle(string model)
		{
			Vector3 coords = Game.PlayerPed.Position;
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
		static int time = 3000;
		static int nbrDisplaying = 1;
		public static void TriggerProximtyDisplay(int id, string title, string text, int a, int b, int c)
		{
			float offset = 1 + (nbrDisplaying * 0.14f);
			int source = PlayerId();
			int target = GetPlayerFromServerId(id);
			if (World.GetDistance(Game.PlayerPed.Position, GetEntityCoords(GetPlayerPed(target), true)) < 19f)
			{
				Display(target, title, text, a, b, c, offset);
			}
		}

		public static async void Display(int MePlayer, string title, string text, int a, int b, int c, float offset)
		{

			bool displaying = true;
			displaying = true;
			ciao();
			async void ciao()
			{
				await BaseScript.Delay(time);
				displaying = false;
			}
			display();
			async void display()
			{
				nbrDisplaying += 1;
				while (displaying)
				{
					await BaseScript.Delay(0);
					Vector3 coordsMe = GetEntityCoords(GetPlayerPed(MePlayer), false);
					HUD.HUD.ShowFloatingHelpNotification(title + " " + text, coordsMe + new Vector3(0, 0, offset));
//					HUD.HUD.DrawText3D(new Vector3(coordsMe.X, coordsMe.Y, coordsMe.Z + offset), System.Drawing.Color.FromArgb(a, b, c), title + " " + text, 0);
				}
				nbrDisplaying -= 1;
			}
			await Task.FromResult(0);
		}

		public static async void Salva()
		{
			Screen.LoadingPrompt.Show("Salvataggio Personaggio...", LoadingSpinnerType.SocialClubSaving);
			await BaseScript.Delay(5000);
			Screen.LoadingPrompt.Hide();
		}

		public static async void StartLocationSave()
		{
			Client.Instance.AddTick(LocationSave);
		}

		public static async Task LocationSave()
		{
			await BaseScript.Delay(10000);
			BaseScript.TriggerServerEvent("lprp:updateCurChar", "charlocation", Game.PlayerPed.Position, Game.PlayerPed.Heading);
			await Task.FromResult(0);
		}

		public static void AddWeapon(string weaponName, int ammo)
		{
			WeaponHash weaponHash = (WeaponHash)GetHashKey(weaponName);
			Game.PlayerPed.Weapons.Give(weaponHash, ammo, false, true);
			HUD.HUD.ShowNotification("Hai ottenuto un/a ~y~" + GetLabelText(Funzioni.GetWeaponLabel((uint)weaponHash)));
		}

		public static void RemoveWeapon(string weaponName)
		{
			WeaponHash weaponHash = (WeaponHash)GetHashKey(weaponName);
			RemoveWeaponFromPed(PlayerPedId(), (uint)weaponHash);
			SetPedAmmo(PlayerPedId(), (uint)weaponHash, 0);
			HUD.HUD.ShowNotification("Rimosso/a ~y~" + GetLabelText(Funzioni.GetWeaponLabel((uint)weaponHash)));
		}

		public static void PossiediArma(string weaponName, string componentName)
		{
			HUD.HUD.ShowNotification("Possiedi già la modifica: ~y~" + GetLabelText(Funzioni.GetWeaponLabel((uint)GetHashKey(componentName))) + "~w~ per ~b~" + GetLabelText(Funzioni.GetWeaponLabel((uint)GetHashKey(weaponName))) + "~w~.");
		}
		public static void PossiediTinta(string weaponName, int tinta)
		{
			HUD.HUD.ShowNotification("Possiedi già la modifica: ~y~" + GetLabelText(Funzioni.GetWeaponLabel((uint)GetHashKey(Armerie.tinte[tinta].name))) + "~w~ per ~b~" + GetLabelText(Funzioni.GetWeaponLabel((uint)GetHashKey(weaponName))) + "~w~.");
		}

		public static void AddWeaponComponent(string weaponName, string weaponComponent)
		{
			uint weaponHash = (uint)GetHashKey(weaponName);
			uint componentHash = (uint)GetHashKey(weaponComponent);
			if (!Player.hasWeaponComponent(weaponName, weaponComponent))
			{
				GiveWeaponComponentToPed(PlayerPedId(), weaponHash, componentHash);
				HUD.HUD.ShowNotification("Hai ottenuto un ~b~" + GetLabelText(Funzioni.GetWeaponLabel(componentHash)));
			}
			else
			{
				HUD.HUD.ShowNotification("Quest'arma ha già un" + GetLabelText(Funzioni.GetWeaponLabel(componentHash)));
			}
		}

		public static void RemoveWeaponComponent(string weaponName, string weaponComponent)
		{
			uint weaponHash = (uint)GetHashKey(weaponName);
			uint componentHash = (uint)GetHashKey(weaponComponent);
			RemoveWeaponComponentFromPed(PlayerPedId(), weaponHash, componentHash);
			HUD.HUD.ShowNotification("Rimosso/a ~b~" + GetLabelText(Funzioni.GetWeaponLabel(componentHash)));
		}

		public static void AddWeaponTint(string weaponName, int tint)
		{
			uint weaponHash = (uint)GetHashKey(weaponName);
			SetPedWeaponTintIndex(PlayerPedId(), weaponHash, tint);
		}

		public static void RestoreWeapons()
		{
			Dictionary<int, bool> ammoTypes = new Dictionary<int, bool>();
			if (Game.Player.GetPlayerData().CurrentChar.weapons.Count > 0)
			{
				Game.PlayerPed.Weapons.RemoveAll();
				for (int i = 0; i < Player.getCharWeapons(Player.char_current).Count; i++)
				{
					string weaponName = Player.getCharWeapons(Player.char_current)[i].name;
					uint weaponHash = (uint)GetHashKey(weaponName);
					int tint = Player.getCharWeapons(Player.char_current)[i].tint;
					Game.PlayerPed.Weapons.Give((WeaponHash)weaponHash, 0, false, false);
					int ammoType = GetPedAmmoTypeFromWeapon(PlayerPedId(), weaponHash);
					if (Player.getCharWeapons(Player.char_current)[i].components.Count > 0)
					{
						for (int j = 0; j < Player.getCharWeapons(Player.char_current)[i].components.Count; j++)
						{
							Components weaponComponent = Player.getCharWeapons(Player.char_current)[i].components[j];
							uint componentHash = (uint)GetHashKey(weaponComponent.name);
							if (weaponComponent.active)
							{
								GiveWeaponComponentToPed(PlayerPedId(), weaponHash, componentHash);
							}
						}
					}
					SetPedWeaponTintIndex(PlayerPedId(), weaponHash, tint);
					if (!ammoTypes.ContainsKey(ammoType))
					{
						AddAmmoToPed(PlayerPedId(), weaponHash, Player.getCharWeapons(Player.char_current)[i].ammo);
						ammoTypes[ammoType] = true;
					}
				}
			}
			Main.LoadoutLoaded = true;
		}
	}
}