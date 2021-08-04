﻿using CitizenFX.Core;
using CitizenFX.Core.UI;
using Logger;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.RolePlay.Core.Status
{
	internal static class Death
	{
		private static int ReviveReward;
		private static bool EarlyRespawnFine;
		private static int EarlyRespawnFineAmount;
		private static bool EarlyRespawn;
		private static bool canPayFine = false;

		public static bool guarito = true;
		public static bool ferito = false;
		public static TimeSpan EarlyRespawnTimer;
		public static TimeSpan BleedoutTimer;
		private static int earlySpawnTimer = 0;
		private static int bleedoutTimer = 0;

		private static List<Vector3> hospitals = new List<Vector3>()
		{
			new Vector3(311.947f, -1444.343f, 29.804f),
			new Vector3(-675.614f, 313.773f, 83.084f),
			new Vector3(357.692f, -591.341f, 28.788f),
			new Vector3(1838.892f, 3673.619f, 34.276f),
			new Vector3(-245.168f, 6327.249f, 32.426f)
		};

		public static void Init()
		{
			Client.Instance.AddEventHandler("tlg:roleplay:onPlayerSpawn", new Action(Spawnato));

			//Client.Instance.AddEventHandler("baseevents:onPlayerDied", new Action<int, List<dynamic>>(playerDied));
			//Client.Instance.AddEventHandler("baseevents:onPlayerKilled", new Action<int, dynamic>(playerKilled));
			Client.Instance.AddEventHandler("DamageEvents:PedKilledByVehicle", new Action<int, int>(PedKilledByVehicle));
			Client.Instance.AddEventHandler("DamageEvents:PedKilledByPlayer", new Action<int, int, uint, bool>(PedKilledByPlayer));
			Client.Instance.AddEventHandler("DamageEvents:PedKilledByPed", new Action<int, int, uint, bool>(PedKilledByPed));
			Client.Instance.AddEventHandler("DamageEvents:PedDied", new Action<int, int, uint, bool>(PedDied));
			//Client.Instance.AddEventHandler("lprp:onPlayerDeath", new Action<dynamic>(onPlayerDeath));
			Client.Instance.AddEventHandler("DamageEvents:EntityDamaged", new Action<int, int, uint, bool>(EntityDamaged));
			Client.Instance.AddEventHandler("lprp:iniziaConteggio", new Action(StartDeathTimer));
			Client.Instance.AddTick(Injuried);
		}

		public static void Stop()
		{
			Client.Instance.RemoveEventHandler("tlg:roleplay:onPlayerSpawn", new Action(Spawnato));

			//Client.Instance.RemoveEventHandler("baseevents:onPlayerDied", new Action<int, List<dynamic>>(playerDied));
			//Client.Instance.RemoveEventHandler("baseevents:onPlayerKilled", new Action<int, dynamic>(playerKilled));
			Client.Instance.RemoveEventHandler("DamageEvents:PedKilledByVehicle", new Action<int, int>(PedKilledByVehicle));
			Client.Instance.RemoveEventHandler("DamageEvents:PedKilledByPlayer", new Action<int, int, uint, bool>(PedKilledByPlayer));
			Client.Instance.RemoveEventHandler("DamageEvents:PedKilledByPed", new Action<int, int, uint, bool>(PedKilledByPed));
			Client.Instance.RemoveEventHandler("DamageEvents:PedDied", new Action<int, int, uint, bool>(PedDied));
			//Client.Instance.RemoveEventHandler("lprp:onPlayerDeath", new Action<dynamic>(onPlayerDeath));
			Client.Instance.RemoveEventHandler("DamageEvents:EntityDamaged", new Action<int, int, uint, bool>(EntityDamaged));
			Client.Instance.RemoveEventHandler("lprp:iniziaConteggio", new Action(StartDeathTimer));
			Client.Instance.RemoveTick(Injuried);
		}

		#region events

		private static async void EntityDamaged(int entity, int attacker, uint weaponHash, bool isMeleeDamage)
		{
			Entity ent = Entity.FromHandle(entity);

			if (ent is Ped ped)
				if (ped == Cache.PlayerCache.MyPlayer.Ped)
					if (ped.Health < 55 && !ped.IsDead && guarito && !ferito)
					{
						RequestAnimSet("move_injured_generic");
						while (!HasAnimSetLoaded("move_injured_generic")) await BaseScript.Delay(0);
						ped.MovementAnimationSet = "move_injured_generic";
						HUD.ShowNotification("Sei ferito ~b~gravemente~w~!! Hai bisogno di essere ~b~curato~w~ da un ~b~medico~w~!", NotificationColor.Red, true);
						ferito = true;
						guarito = false;
						RemoveAnimSet("move_injured_generic");
					}
		}

		private static void PedKilledByVehicle(int ped, int vehicle)
		{
			DatiMorte morte = new DatiMorte { Vittima = new Ped(ped), VeicoloAssassino = new Vehicle(vehicle) };
			morte.Assassino = morte.VeicoloAssassino.Driver;
			morte.PosizioneVittima = morte.Vittima.Position;

			if (ped != PlayerPedId()) return;
			onPlayerDeath(morte);
			StartDeathTimer();
		}

		private static void PedKilledByPlayer(int ped, int player, uint weaponHash, bool isMeleeDamage)
		{
			DatiMorte morte = new DatiMorte { Vittima = new Ped(ped), Assassino = new Ped(GetPlayerPed(player)), CausaDellaMorte = weaponHash };
			morte.PosizioneVittima = morte.Vittima.Position;
			morte.PosizioneAssassino = morte.Assassino.Position;

			if (ped != PlayerPedId()) return;
			onPlayerDeath(morte);
			StartDeathTimer();
		}

		private static void PedKilledByPed(int ped, int attackerPed, uint weaponHash, bool isMeleeDamage)
		{
			DatiMorte morte = new DatiMorte { Vittima = new Ped(ped), Assassino = new Ped(attackerPed), CausaDellaMorte = weaponHash };
			morte.PosizioneVittima = morte.Vittima.Position;
			morte.PosizioneAssassino = morte.Assassino.Position;

			if (ped != PlayerPedId()) return;
			onPlayerDeath(morte);
			StartDeathTimer();
		}

		private static void PedDied(int ped, int attacker, uint weaponHash, bool isMeleeDamage)
		{
			DatiMorte morte = new DatiMorte { Vittima = new Ped(ped), Assassino = new Ped(attacker), CausaDellaMorte = weaponHash };
			morte.PosizioneVittima = morte.Vittima.Position;
			morte.PosizioneAssassino = morte.Assassino.Position;

			if (ped != PlayerPedId()) return;
			StartDeathTimer();
			onPlayerDeath(morte);
		}

		public static void Spawnato()
		{
			ReviveReward = Client.Impostazioni.RolePlay.Main.ReviveReward;
			EarlyRespawnFine = Client.Impostazioni.RolePlay.Main.EarlyRespawnFine;
			EarlyRespawnFineAmount = Client.Impostazioni.RolePlay.Main.EarlyRespawnFineAmount;
			EarlyRespawn = Client.Impostazioni.RolePlay.Main.EarlyRespawn;
		}

		public static void onPlayerDeath(DatiMorte morte)
		{
			Cache.PlayerCache.MyPlayer.User.Status.RolePlayStates.FinDiVita = true;
			Main.IsDead = true;
			BaseScript.TriggerServerEvent("lprp:setDeathStatus", true);
			StartScreenEffect("DeathFailOut", 0, false);
		}

		#endregion

		/*
				public static void playerKilled(int killerId, dynamic Data)
				{
					int playerPed = PlayerPedId();
					int killer = GetPlayerFromServerId(killerId);
					if (NetworkIsPlayerActive(killer))
					{
						Vector3 victimCoords = new Vector3((float)Data.killerpos[0], (float)Data.killerpos[1], (float)Data.killerpos[2]);
						int weaponHash = Data.weaponhash;
						Data.killerpos = null;
						Data.weaponhash = null;
						int deathCause = GetPedCauseOfDeath(playerPed);
						int killerPed = GetPlayerPed(killer);
						Vector3 killerCoords = GetEntityCoords(killerPed, true);
						float distance = GetDistanceBetweenCoords(victimCoords.X, victimCoords.Y, victimCoords.Z, killerCoords.X, killerCoords.Y, killerCoords.Z, false);
						bool killed = true;
						List<dynamic> data = new List<dynamic>() { killed, victimCoords, weaponHash, deathCause, killerId, killerCoords, Math.Round(distance) };
						BaseScript.TriggerEvent("lprp:onPlayerDeath", data);
						BaseScript.TriggerServerEvent("lprp:onPlayerDeath", data);
						Cache.Char.Status.FinDiVita = true;
					}
					else
					{
						bool killed = false;
						int deathCause = GetPedCauseOfDeath(playerPed);
						List<dynamic> data = new List<dynamic>() { killed, deathCause };
						BaseScript.TriggerEvent("lprp:onPlayerDeath", data);
						BaseScript.TriggerServerEvent("lprp:onPlayerDeath", data);
						Cache.Char.Status.FinDiVita = true;
					}
				}

				public static void playerDied(int tipo, List<dynamic> Coords)
				{
					int playerPed = PlayerPedId();
					List<dynamic> data = new List<dynamic>();
					bool killed = false;
					int killerType = tipo;
					Vector3 deathCoords = new Vector3((float)Coords[0], (float)Coords[1], (float)Coords[2]); ;
					int deathCause = GetPedCauseOfDeath(playerPed);
					data.Add(killed);
					data.Add(killerType);
					data.Add(deathCoords);
					data.Add(deathCause);
					Cache.Char.Status.FinDiVita = true;
					BaseScript.TriggerEvent("lprp:onPlayerDeath", data);
					BaseScript.TriggerServerEvent("lprp:onPlayerDeath", data);
				}
		*/

		public static async void StartDeathTimer()
		{
			EarlyRespawnTimer = TimeSpan.FromSeconds(Client.Impostazioni.RolePlay.Main.EarlySpawnTimer);
			BleedoutTimer = TimeSpan.FromSeconds(Client.Impostazioni.RolePlay.Main.BleedoutTimer);
			BaseScript.TriggerServerEvent("lprp:setDeathStatus", true);
			Cache.PlayerCache.MyPlayer.User.Status.RolePlayStates.FinDiVita = true;
			Main.IsDead = true;
			if (EarlyRespawn)
				if (EarlyRespawnFine)
					if (Cache.PlayerCache.MyPlayer.User.Money >= EarlyRespawnFineAmount || Cache.PlayerCache.MyPlayer.User.Bank >= EarlyRespawnFineAmount)
						canPayFine = true;
			if (Main.IsDead) Client.Instance.AddTick(ConteggioMorte);
		}

		private static string text = "";

		public static async Task ConteggioMorte()
		{
			try
			{
				if (EarlyRespawn)
					if (GetGameTimer() - earlySpawnTimer > 1000)
					{
						if (EarlyRespawnTimer.TotalSeconds > 0) EarlyRespawnTimer = EarlyRespawnTimer.Subtract(TimeSpan.FromSeconds(1));
						earlySpawnTimer = GetGameTimer();
						// spostare text
						text = $"Avrai possibilità di respawnare tra ~b~ {EarlyRespawnTimer:mm\\:ss}";
					}

				if (EarlyRespawn && EarlyRespawnTimer.TotalSeconds == 0 || !EarlyRespawn)
					if (BleedoutTimer.TotalSeconds > 0)
					{
						if (EarlyRespawn && EarlyRespawnTimer.TotalSeconds == 0)
						{
							if (GetGameTimer() - bleedoutTimer > 1000)
							{
								if (BleedoutTimer.TotalSeconds > 0) BleedoutTimer = BleedoutTimer.Subtract(TimeSpan.FromSeconds(1));
								bleedoutTimer = GetGameTimer();
							}

							text = $"Morirai dissanguato tra ~b~{BleedoutTimer:mm\\:ss}~w~.";

							if (!EarlyRespawnFine)
							{
								text += "\nTieni premuto [~b~E~s~] per respawnare";

								if (await Input.IsControlStillPressedAsync(Control.Context))
								{
									Client.Instance.RemoveTick(ConteggioMorte);
									RemoveItemsAfterRPDeath();
									EarlyRespawnTimer = TimeSpan.FromSeconds(Client.Impostazioni.RolePlay.Main.EarlySpawnTimer);
									BleedoutTimer = TimeSpan.FromSeconds(Client.Impostazioni.RolePlay.Main.BleedoutTimer);
									text = "";

									return;
								}
							}
							else if (EarlyRespawnFine && canPayFine)
							{
								text = text + "\nTieni premuto [~b~E~s~] per respawnare pagando ~g~$ " + EarlyRespawnFineAmount.ToString() + "~s~";

								if (await Input.IsControlStillPressedAsync(Control.Context))
								{
									BaseScript.TriggerServerEvent("lprp:payFine", EarlyRespawnFineAmount);
									Client.Instance.RemoveTick(ConteggioMorte);
									RemoveItemsAfterRPDeath();
									EarlyRespawnTimer = TimeSpan.FromSeconds(Client.Impostazioni.RolePlay.Main.EarlySpawnTimer);
									BleedoutTimer = TimeSpan.FromSeconds(Client.Impostazioni.RolePlay.Main.BleedoutTimer);
									text = "";

									return;
								}
							}
							else if (EarlyRespawnFine && !canPayFine)
							{
								text = text + "\nPurtroppo non puoi respawnare pagando ~g~$ " + EarlyRespawnFineAmount.ToString() + "~s~, perché non hai abbastanza denaro.";
							}
						}
						else
						{
							if (GetGameTimer() - bleedoutTimer > 1000)
							{
								if (BleedoutTimer.TotalSeconds > 0) BleedoutTimer = BleedoutTimer.Subtract(TimeSpan.FromSeconds(1));
								bleedoutTimer = GetGameTimer();
							}

							text = $"Morirai dissanguato tra ~b~{BleedoutTimer:mm\\:ss}~w~.";
						}

						if (BleedoutTimer.TotalSeconds == 0 && Main.IsDead)
						{
							Client.Instance.RemoveTick(ConteggioMorte);
							RemoveItemsAfterRPDeath();
							EarlyRespawnTimer = TimeSpan.FromSeconds(Client.Impostazioni.RolePlay.Main.EarlySpawnTimer);
							BleedoutTimer = TimeSpan.FromSeconds(Client.Impostazioni.RolePlay.Main.BleedoutTimer);
							text = "";
						}
					}

				HUD.DrawText(text);
			}
			catch (Exception e)
			{
				Client.Logger.Error( e.ToString());
			}

			await Task.FromResult(0);
		}

		public static async void endConteggio()
		{
			Client.Instance.RemoveTick(ConteggioMorte);
			EarlyRespawnTimer = TimeSpan.FromSeconds(Client.Impostazioni.RolePlay.Main.EarlySpawnTimer);
			BleedoutTimer = TimeSpan.FromSeconds(Client.Impostazioni.RolePlay.Main.BleedoutTimer);
			text = "";
		}

		public static async void RemoveItemsAfterRPDeath()
		{
			Screen.Fading.FadeOut(800);
			while (!Screen.Fading.IsFadedOut) await BaseScript.Delay(10);
			BaseScript.TriggerServerEvent("lprp:removeItemsDeath");
			Vector3 pedCoords = Cache.PlayerCache.MyPlayer.Ped.Position;
			Vector3 pos = hospitals.OrderBy(x => Vector3.Distance(pedCoords, x)).FirstOrDefault();
			Main.RespawnPed(new Position(pos.X, pos.Y, pos.Z, Cache.PlayerCache.MyPlayer.Ped.Heading));
			while (!IsPedStill(PlayerPedId())) await BaseScript.Delay(50);
			Screen.Effects.Stop(ScreenEffect.DeathFailOut);
			Screen.Fading.FadeIn(800);
			BaseScript.TriggerServerEvent("lprp:setDeathStatus", false);
			Cache.PlayerCache.MyPlayer.User.Status.RolePlayStates.FinDiVita = false;
		}

		// -- AGGIUNGERE CONTROLLO PER PARTI DEL CORPO DANNEGGIATE E ARMI DA FUOCO
		public static async Task Injuried()
		{
			await BaseScript.Delay(1000);
			Ped playerPed = Cache.PlayerCache.MyPlayer.Ped;

			if (playerPed.Health > 55 && ferito && !guarito && StatsNeeds.Needs["Fame"].Val < 80 && StatsNeeds.Needs["Sete"].Val < 80)
			{
				playerPed.MovementAnimationSet = null;
				ferito = false;
				guarito = true;
			}
		}
	}

	public class DatiMorte
	{
		public Ped Vittima;
		public Ped Assassino;
		public Vector3 PosizioneVittima;
		public Vector3 PosizioneAssassino;
		public uint CausaDellaMorte;
		public Vehicle VeicoloAssassino;
		// aggiungere parti del corpo danneggiate
	}
}