using CitizenFX.Core;
using CitizenFX.Core.UI;
using Logger;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Client.gmPrincipale.Status
{
	static class Death
	{
		static private int ReviveReward;
		static private bool EarlyRespawnFine;
		static private int EarlyRespawnFineAmount;
		static private bool earlyRespawn;
		static private int earlySpawnTimer;
		static private int bleedoutTimer;
		static private bool canPayFine = false;
		public static bool guarito = true;
		public static bool ferito = false;

		private static List<Vector3> hospitals = new List<Vector3>()
		{
			new Vector3(311.947f, -1444.343f, 29.804f),
			new Vector3(-675.614f, 313.773f, 83.084f),
			new Vector3(357.692f, -591.341f, 28.788f),
			new Vector3(1838.892f, 3673.619f, 34.276f),
			new Vector3(-245.168f, 6327.249f, 32.426f),
		};

		public static void Init()
		{
			Client.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action(Spawnato));
//			Client.Instance.AddEventHandler("DamageEvent:PedKilledByPed", new Action<int, List<dynamic>>(pedKilledByPed));
			Client.Instance.AddEventHandler("DamageEvents:PedKilledByPlayer", new Action<int, int, uint, bool>(pedKilledByPlayer));
//			Client.Instance.AddEventHandler("DamageEvents:PedDied", new Action<int, dynamic>(pedDied));
			


//			Client.Instance.AddEventHandler("baseevents:onPlayerDied", new Action<int, List<dynamic>>(playerDied));
//			Client.Instance.AddEventHandler("baseevents:onPlayerKilled", new Action<int, dynamic>(playerKilled));
			Client.Instance.AddEventHandler("lprp:iniziaConteggio", new Action(StartDeathTimer));
			Client.Instance.AddEventHandler("lprp:fineConteggio", new Action(endConteggio));
//			Client.Instance.AddTick(Injuried);
		}

		public static void Spawnato()
		{
			ReviveReward = Client.Impostazioni.Main.ReviveReward;
			EarlyRespawnFine = Client.Impostazioni.Main.EarlyRespawnFine;
			EarlyRespawnFineAmount = Client.Impostazioni.Main.EarlyRespawnFineAmount;
			earlyRespawn = Client.Impostazioni.Main.EarlyRespawn;
			earlySpawnTimer = Client.Impostazioni.Main.EarlySpawnTimer;
			bleedoutTimer = Client.Impostazioni.Main.BleedoutTimer;
		}

		private static void pedKilledByPlayer(int ped, int attackerPlayer, uint weaponHash, bool isMeleeDamage)
		{
			Player victimPlayer = new Player(NetworkGetPlayerIndexFromPed(ped));
			Player killerPlayer = new Player(attackerPlayer);

			if (NetworkIsPlayerActive(killerPlayer.Handle))
			{
				victimPlayer.Character.SetDecor("PlayerFinDiVita", true);
				Vector3 victimCoords = victimPlayer.Character.Position;
				string causeofdeath = SharedScript.DeatReasons[weaponHash];
				BaseScript.TriggerEvent("lprp:onPlayerDeath", new { victimPlayer = victimPlayer.Handle, killerPlayer = killerPlayer.Handle, victimCoords, causeofdeath });
				BaseScript.TriggerServerEvent("lprp:onPlayerDeath", new { victimPlayer = victimPlayer.Handle, killerPlayer = killerPlayer.Handle, victimCoords, causeofdeath });
			}
		}
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
				Game.PlayerPed.SetDecor("PlayerFinDiVita", true);
			}
			else
			{
				bool killed = false;
				int deathCause = GetPedCauseOfDeath(playerPed);
				List<dynamic> data = new List<dynamic>() { killed, deathCause };
				BaseScript.TriggerEvent("lprp:onPlayerDeath", data);
				BaseScript.TriggerServerEvent("lprp:onPlayerDeath", data);
				Game.PlayerPed.SetDecor("PlayerFinDiVita", true);
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
			Game.PlayerPed.SetDecor("PlayerFinDiVita", true);
			BaseScript.TriggerEvent("lprp:onPlayerDeath", data);
			BaseScript.TriggerServerEvent("lprp:onPlayerDeath", data);
		}
*/
		static int timeHeld = 0;
		public static async void StartDeathTimer()
		{
			if (earlyRespawn)
			{
				if (EarlyRespawnFine)
				{
					if (Game.Player.GetPlayerData().Money >= EarlyRespawnFineAmount)
					{
						canPayFine = true;
					}
				}

				if (earlySpawnTimer > 0 && Main.IsDead)
				{
					Client.Instance.AddTick(conteggioSangue);
				}
			}
			else
			{
				if (bleedoutTimer > 0 && Main.IsDead)
				{
					Client.Instance.AddTick(conteggioMorte);
				}
			}
			Client.Instance.AddTick(Testo);
		}

		static string text = "";
		public static async Task conteggioSangue()
		{
			await BaseScript.Delay(1000);
			if (earlySpawnTimer > 0)
			{
				--earlySpawnTimer;
			}

			int mins = Funzioni.secondsToClock(earlySpawnTimer).Item1; int secs = Funzioni.secondsToClock(earlySpawnTimer).Item2;
			text = "Avrai possibilità di respawnare in ~b~ " + mins + " minuti ~s~e ~b~" + secs + " secondi~s~";
			if (earlySpawnTimer < 1)
			{
				Client.Instance.AddTick(conteggioMorte);
			}

			await Task.FromResult(0);
		}



		public static async Task conteggioMorte()
		{
			await BaseScript.Delay(1000);
			if (earlyRespawn && earlySpawnTimer < 1)
			{
				if (bleedoutTimer > 0)
				{
					--bleedoutTimer;
				}

				int mins = Funzioni.secondsToClock(bleedoutTimer).Item1; int secs = Funzioni.secondsToClock(bleedoutTimer).Item2;
				text = "Morirai dissanguato in ~b~ " + mins + " minuti ~s~e ~b~" + secs + " secondi~s~\n";
				if (!EarlyRespawnFine)
				{
					text = text + "Tieni premuto [~b~E~s~] per respawnare";
					if (await Input.WaitForKeyRelease(Control.Context))
					{
						Client.Instance.RemoveTick(conteggioSangue);
						Client.Instance.RemoveTick(conteggioMorte);
						Client.Instance.RemoveTick(Testo);
						RemoveItemsAfterRPDeath();
						earlySpawnTimer = Client.Impostazioni.Main.EarlySpawnTimer;
						bleedoutTimer = Client.Impostazioni.Main.BleedoutTimer;
						text = "";
						return;
					}
				}
				else if (EarlyRespawnFine && canPayFine)
				{
					text = text + "Tieni premuto [~b~E~s~] per respawnare pagando ~g~$ " + EarlyRespawnFineAmount.ToString() + "~s~";
					if (await Input.WaitForKeyRelease(Control.Context))
					{
						BaseScript.TriggerServerEvent("lprp:payFine", EarlyRespawnFineAmount);
						Client.Instance.RemoveTick(conteggioSangue);
						Client.Instance.RemoveTick(conteggioMorte);
						Client.Instance.RemoveTick(Testo);
						RemoveItemsAfterRPDeath();
						earlySpawnTimer = Client.Impostazioni.Main.EarlySpawnTimer;
						bleedoutTimer = Client.Impostazioni.Main.BleedoutTimer;
						text = "";
						return;
					}
				}
				else if (EarlyRespawnFine && !canPayFine)
				{
					Screen.ShowNotification("Non hai abbastanza soldi!!");
				}
			}
			else
			{
				if (bleedoutTimer > 0)
				{
					--bleedoutTimer;
				}

				int mins = Funzioni.secondsToClock(bleedoutTimer).Item1; int secs = Funzioni.secondsToClock(bleedoutTimer).Item2;
				text = "Morirai dissanguato in ~b~ " + mins + " minuti ~s~e ~b~" + secs + " secondi~s~\n";
			}
			if (bleedoutTimer < 1 && Main.IsDead)
			{
				Client.Instance.RemoveTick(conteggioSangue);
				Client.Instance.RemoveTick(conteggioMorte);
				Client.Instance.RemoveTick(Testo);
				RemoveItemsAfterRPDeath();
				earlySpawnTimer = Client.Impostazioni.Main.EarlySpawnTimer;
				bleedoutTimer = Client.Impostazioni.Main.BleedoutTimer;
				text = "";
			}
			await Task.FromResult(0);
		}



		public static async Task Testo()
		{
			if (Input.IsControlPressed(Control.Context) && earlyRespawn)
				++timeHeld;
			else
				timeHeld = 0;

			if (earlyRespawn && earlySpawnTimer < 1)
				Client.Instance.RemoveTick(conteggioSangue);

			HUD.DrawText(text);
			await Task.FromResult(0);
		}

		public static async void endConteggio()
		{
			Client.Instance.RemoveTick(conteggioSangue);
			Client.Instance.RemoveTick(conteggioMorte);
			Client.Instance.RemoveTick(Testo);
			earlySpawnTimer = Client.Impostazioni.Main.EarlySpawnTimer;
			bleedoutTimer = Client.Impostazioni.Main.BleedoutTimer;
			text = "";
		}

		public static async void RemoveItemsAfterRPDeath()
		{
			BaseScript.TriggerServerEvent("lprp:setDeathStatus", false);
			List<float> coords = new List<float>();
			Screen.Fading.FadeOut(800);
			while (!Screen.Fading.IsFadedOut)
			{
				await BaseScript.Delay(10);
			}

			BaseScript.TriggerServerEvent("lprp:removeItemsDeath");
			Vector3 pedCoords = GetEntityCoords(PlayerPedId(), false);
			for (int i = 0; i < hospitals.Count; i++)
			{
				coords.Add(CalculateTravelDistanceBetweenPoints(pedCoords.X, pedCoords.Y, pedCoords.Z, hospitals[i].X, hospitals[i].Y, hospitals[i].Z));
			}

			float vicino = coords.Min();

			for (int i = 0; i < hospitals.Count; i++)
			{
				if (Vdist(pedCoords.X, pedCoords.Y, pedCoords.Z, hospitals[i].X, hospitals[i].Y, hospitals[i].Z) <= vicino)
				{
					Vector3 pos = new Vector3(hospitals[i].X, hospitals[i].Y, hospitals[i].Z);
					Main.RespawnPed(pos);
				}
				while (!IsPedStill(PlayerPedId()))
				{
					await BaseScript.Delay(50);
				}

				Screen.Effects.Stop(ScreenEffect.DeathFailOut);
				Screen.Fading.FadeIn(800);
			}
		}

		// -- AGGIUNGERE CONTROLLO PER PARTI DEL CORPO DANNEGGIATE E ARMI DA FUOCO
		public static async Task Injuried()
		{
			await BaseScript.Delay(1000);
			if (((Game.PlayerPed.Health < 55) && guarito && !ferito && Game.PlayerPed.Health > 0))
			{
				Game.PlayerPed.MovementAnimationSet = "move_injured_generic";
				HUD.ShowNotification("Sei ferito ~b~gravemente~w~!! Hai bisogno di essere ~b~curato~w~ da un ~b~medico~w~!", NotificationColor.Red, true);
				ferito = true;
				guarito = false;
			}
			else if ((Game.PlayerPed.Health > 55) && ferito && !guarito && (StatsNeeds.nee.fame < 80 && StatsNeeds.nee.sete < 80))
			{
				ResetPedMovementClipset(PlayerPedId(), 0.0f);
				ferito = false;
				guarito = true;
			}
		}
	}
}
