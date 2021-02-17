using CitizenFX.Core;
using CitizenFX.Core.UI;
using Logger;
using Newtonsoft.Json;
using TheLastPlanet.Client.Core.CharCreation;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.Personale;
using TheLastPlanet.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.Core
{
	static class Main
	{
		public static string decorName;
		public static int decorInt;
		private static int HostId;
		public static bool spawned = false;
		public static ClientConfigKVP ImpostazioniClient;

		public static List<Gang> GangsAttive = new List<Gang>();

		// NetworkConcealPlayer(Player player, bool toggle, bool p2); **This is what R* uses to hide players in MP interiors.


		static List<Vector4> SelectFirstCoords = new List<Vector4>
		{
			new Vector4(-1503.000f, -1143.462f, 34.670f, 64.692f),
			new Vector4(747.339f, 525.837f, 345.395f, 39.975f),
			new Vector4(-2162.689f, -469.343f, 3.396f, 150.975f),
			new Vector4(-170.256f, -2357.418f, 100.596f, 95.975f),
			new Vector4(2126.171f, 3014.593f, 59.196f, 115.975f),
			new Vector4(-103.310f, -1215.578f, 53.796f, 270.975f),
			new Vector4(-3032.130f, 22.216f, 11.118f, 0f),
		};

		public static bool ispointing = false;
		private static RelationshipGroup player = Game.PlayerPed.RelationshipGroup;
		public static Vector4 charSelectCoords;
		public static Vector4 charCreateCoords = new Vector4(402.91f, -996.74f, -100.00025f, 180.086f);
		public static Vector4 firstSpawnCoords = new Vector4(Client.Impostazioni.Main.Firstcoords[0], Client.Impostazioni.Main.Firstcoords[1], Client.Impostazioni.Main.Firstcoords[2], Client.Impostazioni.Main.Firstcoords[3]);

		static List<string> tipi = new List<string>() { "CIVMALE", "CIVFEMALE", "COP", "WILD_ANIMAL", "SHARK", "COUGAR", "GUARD_DOG", "DOMESTIC_ANIMAL", "DEER", "SECURITY_GUARD", "PRIVATE_SECURITY", "FIREMAN", "GANG_1", "GANG_2", "GANG_9", "GANG_10", "AMBIENT_GANG_LOST", "AMBIENT_GANG_MEXICAN", "AMBIENT_GANG_FAMILY", "AMBIENT_GANG_BALLAS", "AMBIENT_GANG_MARABUNTE", "AMBIENT_GANG_CULT", "AMBIENT_GANG_SALVA", "AMBIENT_GANG_WEICHENG", "AMBIENT_GANG_HILLBILLY", "DEALER", "HATES_PLAYER", "HEN", "NO_RELATIONSHIP", "SPECIAL", "MISSION2", "MISSION3", "MISSION4", "MISSION5", "MISSION6", "MISSION7", "MISSION8", "AGGRESSIVE_INVESTIGATE", "MEDIC" };
		static List<string> pickupList = new List<string>();
		static List<uint> scopedWeapons = Client.Impostazioni.Main.ScopedWeapons;

		static private bool kickWarning;
		static Dictionary<long, float> recoils = new Dictionary<long, float>();

		public static Dictionary<string, KeyValuePair<String, string>> Textures = new Dictionary<string, KeyValuePair<string, string>>()
		{
			["ModGarage"] = new KeyValuePair<string, string>("shopui_title_ie_modgarage", "shopui_title_ie_modgarage"),
			["Michael"] = new KeyValuePair<string, string>("shopui_title_graphics_michael", "shopui_title_graphics_michael"),
			["AmmuNation"] = new KeyValuePair<string, string>("shopui_title_gunclub", "shopui_title_gunclub"),
			["Binco"] = new KeyValuePair<string, string>("shopui_title_lowendfashion2", "shopui_title_lowendfashion2"),
			["Discount"] = new KeyValuePair<string, string>("shopui_title_lowendfashion", "shopui_title_lowendfashion"),
			["Suburban"] = new KeyValuePair<string, string>("shopui_title_midfashion", "shopui_title_midfashion"),
			["Ponsombys"] = new KeyValuePair<string, string>("shopui_title_highendfashion", "shopui_title_highendfashion"),
			["Kuts"] = new KeyValuePair<string, string>("shopui_title_barber", "shopui_title_barber"),
			["Hawick"] = new KeyValuePair<string, string>("shopui_title_barber4", "shopui_title_barber4"),
			["Osheas"] = new KeyValuePair<string, string>("shopui_title_barber3", "shopui_title_barber3"),
			["Combo"] = new KeyValuePair<string, string>("shopui_title_barber2", "shopui_title_barber2"),
			["Mulet"] = new KeyValuePair<string, string>("shopui_title_highendsalon", "shopui_title_highendsalon"),
			["247"] = new KeyValuePair<string, string>("shopui_title_conveniencestore", "shopui_title_conveniencestore"),
			["ltd"] = new KeyValuePair<string, string>("shopui_title_gasstation", "shopui_title_gasstation"),
			["rq"] = new KeyValuePair<string, string>("shopui_title_liquorstore2", "shopui_title_liquorstore2"),
		};

		public static bool IsDead = false;
		public static bool isAdmin = false;
		public static bool LoadoutLoaded = false;
		private static bool passengerDriveBy = Client.Impostazioni.Main.PassengerDriveBy;

		public static void Init()
		{
			LoadMain();

			//Client.Instance.AddTick(Connesso);
			Client.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action(onPlayerSpawn));
			Client.Instance.AddEventHandler("onClientResourceStop", new Action<string>(OnClientResourceStop));
			Client.Instance.AddEventHandler("lprp:getHost", new Action<int>(GetHost));
			Client.Instance.AddEventHandler("lprp:AFKScelta", new Action<string>(AFKScelta));
			Screen.Fading.FadeOut(800);
		}

		private static void LoadMain()
		{
			try
			{
				ImpostazioniClient = Funzioni.CaricaKVP<ClientConfigKVP>("SettingsClient");
			}
			catch(Exception _)
			{
				Log.Printa(LogType.Debug, "No impostazioni trovate, settate default");
				ImpostazioniClient = new ClientConfigKVP();
			}

			SetNuiFocus(false, false);
			SetMapZoomDataLevel(0, 2.73f, 0.9f, 0.08f, 0.0f, 0.0f);
			SetMapZoomDataLevel(1, 2.8f, 0.9f, 0.08f, 0.0f, 0.0f);
			SetMapZoomDataLevel(2, 8.0f, 0.9f, 0.08f, 0.0f, 0.0f);
			SetMapZoomDataLevel(3, 11.3f, 0.9f, 0.08f, 0.0f, 0.0f);
			SetMapZoomDataLevel(4, 16f, 0.9f, 0.08f, 0.0f, 0.0f);
			SetMapZoomDataLevel(5, 55f, 0f, 0.1f, 2.0f, 1.0f);
			SetMapZoomDataLevel(6, 450f, 0f, 0f, 0.1f, 0.1f);
			SetMapZoomDataLevel(7, 4.5f, 0f, 0f, 0f, 0f);
			SetMapZoomDataLevel(8, 11f, 0f, 0f, 2.0f, 3.0f);
		}

		private static async void OnClientResourceStop(string resourceName)
		{
			if (resourceName == GetCurrentResourceName())
				Screen.Fading.FadeOut(800);
		}

		public static async void onPlayerSpawn()
		{
			Ped playerPed = Game.PlayerPed;
			SetEnablePedEnveffScale(playerPed.Handle, true);
			SetPlayerTargetingMode(2);
			Game.MaxWantedLevel = 0;
			SetCanAttackFriendly(playerPed.Handle, true, true);
			NetworkSetFriendlyFireOption(true);
			AddTextEntry("FE_THDR_GTAO", Client.Impostazioni.Main.NomeServer);
			scopedWeapons = Client.Impostazioni.Main.ScopedWeapons;
			passengerDriveBy = Client.Impostazioni.Main.PassengerDriveBy;
			kickWarning = Client.Impostazioni.Main.KickWarning;
			recoils = Client.Impostazioni.Main.recoils;
			pickupList = Client.Impostazioni.Main.pickupList;
			BaseScript.TriggerEvent("chat:addMessage", new { color = new[] { 71, 255, 95 }, multiline = true, args = new[] { "^4Benvenuto nel server test di Manups4e" } });
			BaseScript.TriggerEvent("chat:addMessage", new { color = new[] { 71, 255, 95 }, multiline = true, args = new[] { "^4QUESTO SERVER E' IN FASE ALPHA" } });
			SetPlayerHealthRechargeMultiplier(PlayerId(), -1.0f);
			Game.Player.GetPlayerData().StatiPlayer.Istanza.RimuoviIstanza();
			playerPed.IsVisible = true;
			spawned = true;
			Game.Player.GetPlayerData().status.spawned = true;
			BaseScript.TriggerServerEvent("lprp:updateCurChar", "char_current", Game.Player.GetPlayerData().CurrentChar.id);
			BaseScript.TriggerServerEvent("lprp:updateCurChar", "status", true);
			if (Game.Player.GetPlayerData().DeathStatus)
			{
				HUD.ShowNotification("Sei stato ucciso perche ti sei disconnesso da morto!", NotificationColor.Red, true);
				var now = DateTime.Now;
				BaseScript.TriggerServerEvent("lprp:serverlog", now.ToString("dd/MM/yyyy, HH:mm:ss") + " -- " + Game.Player.GetPlayerData().FullName + " e' spawnato morto poiché è sloggato da morto");
				playerPed.Health = -100;
				Game.Player.GetPlayerData().StatiPlayer.FinDiVita = false;

			}
			Peds();
			for (int i = 0; i < tipi.Count; i++)
				player.SetRelationshipBetweenGroups(new RelationshipGroup(GetHashKey(tipi[i])), Relationship.Neutral, true);
		}
		private static void GetHost(int hostid)
		{
			HostId = hostid;
		}

		private static float OttieniShake(WeaponHash weap)
		{
			switch (Game.PlayerPed.Weapons.Current.Hash)
			{
				case WeaponHash.StunGun:
				case WeaponHash.FlareGun:
					return 0.01f;
				case WeaponHash.SNSPistol:
					return 0.02f;
				case WeaponHash.SNSPistolMk2:
				case WeaponHash.VintagePistol:
				case WeaponHash.DoubleAction:
				case WeaponHash.Pistol:
					return 0.025f;
				case WeaponHash.PistolMk2:
				case WeaponHash.CombatPistol:
				case WeaponHash.HeavyPistol:
				case WeaponHash.MarksmanPistol:
					return 0.03f;
				case WeaponHash.MicroSMG:
				case WeaponHash.MachinePistol:
				case WeaponHash.MiniSMG:
					return 0.035f;
				case WeaponHash.Musket:
					return 0.04f;
				case WeaponHash.Revolver:
				case WeaponHash.SMG:
					return 0.045f;
				case WeaponHash.APPistol:
				case WeaponHash.Pistol50:
				case WeaponHash.Gusenberg:
				case WeaponHash.BullpupRifle:
				case WeaponHash.CompactRifle:
				case WeaponHash.DoubleBarrelShotgun:
				case WeaponHash.AssaultSMG:
					return 0.05f;
				case WeaponHash.RevolverMk2:
				case WeaponHash.CombatPDW:
				case WeaponHash.SMGMk2:
					return 0.055f;
				case WeaponHash.CarbineRifle:
				case WeaponHash.AdvancedRifle:
				case WeaponHash.SawnOffShotgun:
				case WeaponHash.SpecialCarbine:
					return 0.06f;
				case WeaponHash.CarbineRifleMk2:
				case WeaponHash.BullpupRifleMk2:
					return 0.065f;
				case WeaponHash.MG:
				case WeaponHash.PumpShotgun:
				case WeaponHash.AssaultRifle:
					return 0.07f;
				case WeaponHash.SpecialCarbineMk2:
					return 0.075f;
				case WeaponHash.CombatMG:
				case WeaponHash.CompactGrenadeLauncher:
				case WeaponHash.BullpupShotgun:
				case WeaponHash.GrenadeLauncher:
				case WeaponHash.SweeperShotgun:
					return 0.08f;
				case WeaponHash.CombatMGMk2:
				case WeaponHash.AssaultRifleMk2:
				case WeaponHash.PumpShotgunMk2:
					return 0.085f;
				case WeaponHash.MarksmanRifle:
				case WeaponHash.MarksmanRifleMk2:
					return 0.1f;
				case WeaponHash.AssaultShotgun:
					return 0.12f;
				case WeaponHash.HeavyShotgun:
					return 0.13f;
				case WeaponHash.Minigun:
				case WeaponHash.SniperRifle:
					return 0.2f;
				case WeaponHash.HeavySniper:
					return 0.3f;
				case WeaponHash.HeavySniperMk2:
					return 0.35f;
				case WeaponHash.Firework:
					return 0.5f;
				case WeaponHash.RPG:
				case WeaponHash.HomingLauncher:
					return 0.9f;
				case WeaponHash.Railgun:
					return 1.0f;
				default: return 0;
			}
		}

		public static async Task MainTick()
		{
			Ped p = Game.PlayerPed;
			Player pl = Game.Player;

			#region death?andweapon
			Game.DisableControlThisFrame(0, Control.SpecialAbilitySecondary);
			if (IsDead)
			{
				Game.DisableAllControlsThisFrame(0);
				EnableControlAction(0, 47, true);
				EnableControlAction(0, 245, true);
				EnableControlAction(0, 38, true);
				Game.EnableControlThisFrame(0, Control.FrontendPause);
			}
			if (IsPedArmed(p.Handle, 6))
			{
				Game.DisableControlThisFrame(0, Control.MeleeAttackLight);
				Game.DisableControlThisFrame(0, Control.MeleeAttackHeavy);
				Game.DisableControlThisFrame(0, Control.MeleeAttackAlternate);
				Game.DisableControlThisFrame(0, Control.MeleeAttack1);
				Game.DisableControlThisFrame(0, Control.MeleeAttack2);
			}
			#endregion

			if (p.IsJumping && Input.IsControlJustPressed(Control.Jump))
				p.Ragdoll(1);
			if (pl.WantedLevel != 0)
				pl.WantedLevel = 0;
			DisablePlayerVehicleRewards(pl.Handle);
			SetPedMinGroundTimeForStungun(p.Handle, 8000);
			if (p.IsInVehicle())
			{
				if (p.CurrentVehicle.Driver == p)
					SetPlayerCanDoDriveBy(pl.Handle, false);
				else if (passengerDriveBy)
					SetPlayerCanDoDriveBy(pl.Handle, true);
				else
					SetPlayerCanDoDriveBy(pl.Handle, false);
			}
			if ((Input.IsControlJustPressed(Control.MpTextChatTeam) || Input.IsDisabledControlJustPressed(Control.MpTextChatTeam)) && Game.CurrentInputMode == InputMode.MouseAndKeyboard)
			{
				if (!IsEntityPlayingAnim(p.Handle, "mp_arresting", "idle", 3))
					startPointing();
			}
			else if (Input.IsControlJustReleased(Control.MpTextChatTeam) || Input.IsDisabledControlJustReleased(Control.MpTextChatTeam))
				StopPointing();
			if (ispointing)
			{
				float camPitch = GetGameplayCamRelativePitch();
				if (camPitch < -70.0)
					camPitch = -70.0f;
				else if (camPitch > 42.0)
					camPitch = 42.0f;
				camPitch = (camPitch + 70.0f) / 112.0f;
				float camHeading = GetGameplayCamRelativeHeading();
				float cosCamHeading = Cos(camHeading);
				float sinCamHeading = Sin(camHeading);
				if (camHeading < -180.0)
					camHeading = -180.0f;
				else if (camHeading > 180.0)
					camHeading = 180.0f;
				camHeading = (camHeading + 180.0f) / 360.0f;
				bool blocked = false;
				int nn = 0;
				Vector3 coords = GetOffsetFromEntityInWorldCoords(p.Handle, (cosCamHeading * -0.2f) - (sinCamHeading * (0.4f * camHeading + 0.3f)), (sinCamHeading * -0.2f) + (cosCamHeading * (0.4f * camHeading + 0.3f)), 0.6f);
				int ray = StartShapeTestCapsule(coords.X, coords.Y, coords.Z - 0.2f, coords.X, coords.Y, coords.Z + 0.2f, 0.4f, 95, p.Handle, 7);
				GetShapeTestResult(ray, ref blocked, ref coords, ref coords, ref nn);
				SetTaskPropertyFloat(p.Handle, "Pitch", camPitch);
				SetTaskPropertyFloat(p.Handle, "Heading", camHeading * -1.0f + 1.0f);
				SetTaskPropertyBool(p.Handle, "isBlocked", blocked);
				SetTaskPropertyBool(p.Handle, "isFirstPerson", N_0xee778f8c7e1142e2(N_0x19cafa3c87f7c2ff()) == 4);
			}
			if (Game.IsPaused)
			{
				if (!Game.Player.GetPlayerData().StatiPlayer.InPausa)
					Game.Player.GetPlayerData().StatiPlayer.InPausa = true;
			}
			else
			{
				if (Game.Player.GetPlayerData().StatiPlayer.InPausa)
					Game.Player.GetPlayerData().StatiPlayer.InPausa = false;
			}
			pickupList.ForEach(x => RemoveAllPickupsOfType(Funzioni.HashUint(x)));
			for (int i = 1; i < 16; i++) EnableDispatchService(i, false);
		}

		public static async Task Recoil()
		{
			Ped p = Game.PlayerPed;
			Weapon weapon = p.Weapons.Current;
			ManageReticle(weapon);
			if (p.IsAiming || p.IsAimingFromCover || p.IsShooting)
				DisplayAmmoThisFrame(false);
			if (p.IsShooting)
			{
				ShakeGameplayCam("SMALL_EXPLOSION_SHAKE", OttieniShake(p.Weapons.Current.Hash));
				if (weapon.Hash == WeaponHash.FireExtinguisher)
					weapon.InfiniteAmmo = true;
				if (!p.IsDoingDriveBy)
				{
					Weapon wep = p.Weapons.Current;
					if (recoils.ContainsKey((uint)wep.Hash))
					{
						if (recoils[(uint)wep.Hash] != 0)
						{
							float tv = 0;
							if (GetFollowPedCamViewMode() != 4)
							{
								do
								{
									await BaseScript.Delay(0);
									SetGameplayCamRelativePitch(GetGameplayCamRelativePitch() + 0.1f, 0.2f);
									tv += 0.1f;
								} while (tv < recoils[(uint)wep.Hash]);
							}
							else
							{
								do
								{
									await BaseScript.Delay(0);
									if (recoils[(uint)wep.Hash] > 0.1)
									{
										SetGameplayCamRelativePitch(GetGameplayCamRelativePitch() + 0.6f, 1.2f);
										tv += 0.6f;
									}
									else
									{
										SetGameplayCamRelativePitch(GetGameplayCamRelativePitch() + 0.016f, 0.333f);
										tv += 0.1f;
									}
								} while (tv < recoils[(uint)wep.Hash]);
							}
						}
					}
				}
			}
		}

		private static async void startPointing()
		{
			ispointing = true;
			RequestAnimDict("anim@mp_point");
			while (!HasAnimDictLoaded("anim@mp_point")) await BaseScript.Delay(0);
			SetPedCurrentWeaponVisible(Game.PlayerPed.Handle, false, true, true, true);
			SetPedConfigFlag(Game.PlayerPed.Handle, 36, true);
			TaskMoveNetwork(Game.PlayerPed.Handle, "task_mp_pointing", 0.5f, false, "anim@mp_point", 24);
			RemoveAnimDict("anim@mp_point");
		}

		private static void StopPointing()
		{
			ispointing = false;
			N_0xd01015c7316ae176(Game.PlayerPed.Handle, "Stop");
			if (!Game.PlayerPed.IsInjured)
				Game.PlayerPed.Task.ClearSecondary();
			if (!Game.PlayerPed.IsInVehicle())
				SetPedCurrentWeaponVisible(Game.PlayerPed.Handle, true, true, true, true);
			SetPedConfigFlag(Game.PlayerPed.Handle, 36, false);
			Game.PlayerPed.Task.ClearSecondary();
		}

		public static int AFKTime = 600;
		public static bool abort = false;
		private static bool triggerato = false;
		private static Vector3 currentPosition;

		public static async Task AFK()
		{
			if (Game.Player.GetPlayerData().group_level < 3 && !(Creator.Creazione.Visible || Creator.Apparel.Visible || Creator.Apparenze.Visible || Creator.Dettagli.Visible || Creator.Genitori.Visible || Creator.Info.Visible)) // helper e moderatori sono inclusi (gradi 0,1,2)
			{
//				if (Ingresso.Ingresso.guiEnabled)
//				else if (Menus.Creazione.Visible || Menus.Apparel.Visible || Menus.Apparenze.Visible || Menus.Dettagli.Visible || Menus.Genitori.Visible || Menus.Info.Visible)

				currentPosition = Game.Player.GetPlayerData() == null ? Game.PlayerPed.Position : Game.Player.GetPlayerData().posizione.ToVector3();
				int t = (int)Math.Floor(GetTimeSinceLastInput(0) / 1000f);
				if (t >= Client.Impostazioni.Main.AFKCheckTime)
				{
					if (Vector3.Distance(Game.Player.GetPlayerData().posizione.ToVector3(), currentPosition) < 3f)
						BaseScript.TriggerServerEvent("lprp:dropPlayer", "Last Planet Shield 2.0:\nSei stato rilevato per troppo tempo AFK");
				}
				else
				{
					if (t > (Client.Impostazioni.Main.AFKCheckTime - (int)Math.Floor(Client.Impostazioni.Main.AFKCheckTime / 4f)))
					{
						if (kickWarning)
						{
							string Text = $"Sei stato rilevato AFK per troppo tempo\nVerrai kickato tra {(Client.Impostazioni.Main.AFKCheckTime - t)} secondi!";
							if (!triggerato)
							{
								Manager.ClientManager.WarningMessage("Last Planet Shield 2.0", "Sei stato rilevato AFK per troppo tempo", $"Verrai kickato tra {(Client.Impostazioni.Main.AFKCheckTime - t)} secondi!", 8, "lprp:AFKScelta");
								triggerato = true;
							}
							Manager.ClientManager.UpdateText("Sei stato rilevato AFK per troppo tempo", $"Verrai kickato tra {(Client.Impostazioni.Main.AFKCheckTime - t)} secondi!");
							Log.Printa(LogType.Debug, Text);
						}
					}
				}
			}
		}

		private static void AFKScelta(string scelta)
		{
			if (scelta == "select" || scelta == "back" || scelta == "alternative")
				triggerato = false;
		}

		private static void ManageReticle(Weapon weapon)
		{
			if (!scopedWeapons.Contains((uint)weapon.Hash))
				Screen.Hud.HideComponentThisFrame(HudComponent.Reticle);
		}

		public static void RespawnPed(Vector3 coords)
		{
			IsDead = false;
			Game.PlayerPed.Position = coords;
			NetworkResurrectLocalPlayer(coords.X, coords.Y, coords.Z, Game.PlayerPed.Heading, true, false);
			Game.PlayerPed.Health = 100;
			Game.PlayerPed.IsInvincible = false;
			Game.PlayerPed.ClearBloodDamage();
		}

		private static async void Peds()
		{
			await BaseScript.Delay(30000);
			while (Client.Impostazioni.Main.stripClub == null) await BaseScript.Delay(0);
			foreach (var stripper in Client.Impostazioni.Main.stripClub)
			{
				Ped ped = await World.CreatePed(new Model(GetHashKey(stripper.model)), new Vector3(stripper.coords[0], stripper.coords[1], stripper.coords[2]), stripper.heading);
				ped.CanRagdoll = false;
				ped.BlockPermanentEvents = true;
				SetEntityCanBeDamaged(ped.Handle, false);
				SetPedFleeAttributes(ped.Handle, 0, false);
				SetPedCombatAttributes(ped.Handle, 17, true);
				ped.Task.PlayAnimation(stripper.animDict, stripper.animName, -1, -1, AnimationFlags.Loop);
			}
			foreach (var market in Client.Impostazioni.Main.blackMarket)
			{
				Ped ped1 = await World.CreatePed(new Model(GetHashKey(market.model)), new Vector3(market.coords[0], market.coords[1], market.coords[2]), market.heading);
				ped1.CanRagdoll = false;
				ped1.BlockPermanentEvents = true;
				SetEntityCanBeDamaged(ped1.Handle, false);
				SetPedFleeAttributes(ped1.Handle, 0, false);
				SetPedCombatAttributes(ped1.Handle, 17, true);
				ped1.Task.StartScenario(market.animName, GetEntityCoords(ped1.Handle, true));
			}
			foreach (var illegal in Client.Impostazioni.Main.illegal_weapon_extra_shop)
			{
				Ped ped2 = await World.CreatePed(new Model(GetHashKey(illegal.model)), new Vector3(illegal.coords[0], illegal.coords[1], illegal.coords[2]), illegal.heading);
				ped2.CanRagdoll = false;
				ped2.BlockPermanentEvents = true;
				SetEntityCanBeDamaged(ped2.Handle, false);
				SetPedFleeAttributes(ped2.Handle, 0, false);
				SetPedCombatAttributes(ped2.Handle, 17, true);
				ped2.Task.StartScenario(illegal.animName, GetEntityCoords(ped2.Handle, true));
			}
		}

		public static async Task Armi()
		{
			if (IsDead) await BaseScript.Delay(500);
			else 
			{
				if (Game.PlayerPed.IsShooting)
				{
					Weapon weap = Game.PlayerPed.Weapons.Current;
					int ammo = weap.Ammo;
					BaseScript.TriggerServerEvent("lprp:aggiornaAmmo", weap.Hash, ammo);
				}
  			}
		}
	}

	public class GenericPeds
	{
		public Vector3 coords;
		public string animDict;
		public string animName;
		public string model;
		public float heading;
		public string scenario;

		public GenericPeds() { }
		public GenericPeds(Vector3 _c, string _ad, string _an, string _ml, float _h)
		{
			this.coords = _c;
			this.animDict = _ad;
			this.animName = _an;
			this.model = _ml;
			this.heading = _h;
		}
	}
}
