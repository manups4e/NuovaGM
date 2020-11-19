using CitizenFX.Core;
using Newtonsoft.Json;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Shared;
using System;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Client.gmPrincipale.Status
{
	public enum Skills
	{
		STAMINA,
		STRENGTH,
		FLYING_ABILITY,
		LUNG_CAPACITY,
		WHEELIE_ABILITY,
		DRUGS,
		FISHING,
		HUNTING
	}
	static class StatsNeeds
	{
		public static Needs nee = new Needs();
		public static Statistiche skill = new Statistiche();
		private static bool fame20 = false;
		private static bool fame60 = false;
		private static bool fame80 = false;
		private static bool fame100 = false;
		private static bool sete20 = false;
		private static bool sete60 = false;
		private static bool sete80 = false;
		private static bool sete100 = false;
		private static int stamina = 0;
		private static int strength = 0;
		private static int fly = 0;
		private static int lung = 0;
		private static int wheelie = 0;
		private static int drugs = 0;
		private static int fishing = 0;
		private static int hunting = 0;

		public static void Init()
		{
			Client.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action(Eccolo));
			Client.Instance.AddEventHandler("lprp:skills:registraSkill", new Action<string, float>(RegistraStats));
		}
		
		public static async void Eccolo()
		{
			nee.fame = Game.Player.GetPlayerData().CurrentChar.needs.fame;
			nee.sete = Game.Player.GetPlayerData().CurrentChar.needs.sete;
			nee.stanchezza = Game.Player.GetPlayerData().CurrentChar.needs.stanchezza;
			nee.malattia = Game.Player.GetPlayerData().CurrentChar.needs.malattia;
			skill.STAMINA = Game.Player.GetPlayerData().CurrentChar.statistiche.STAMINA;
			skill.STRENGTH = Game.Player.GetPlayerData().CurrentChar.statistiche.STRENGTH;
			skill.FLYING_ABILITY = Game.Player.GetPlayerData().CurrentChar.statistiche.FLYING_ABILITY;
			skill.LUNG_CAPACITY = Game.Player.GetPlayerData().CurrentChar.statistiche.LUNG_CAPACITY;
			skill.WHEELIE_ABILITY = Game.Player.GetPlayerData().CurrentChar.statistiche.WHEELIE_ABILITY;
			skill.DRUGS = Game.Player.GetPlayerData().CurrentChar.statistiche.DRUGS;
			skill.FISHING = Game.Player.GetPlayerData().CurrentChar.statistiche.FISHING;
			skill.HUNTING = Game.Player.GetPlayerData().CurrentChar.statistiche.HUNTING;
			stamina = (int)(Game.Player.GetPlayerData().CurrentChar.statistiche.STAMINA);
			strength = (int)(Game.Player.GetPlayerData().CurrentChar.statistiche.STRENGTH);
			fly = (int)(Game.Player.GetPlayerData().CurrentChar.statistiche.FLYING_ABILITY);
			lung = (int)(Game.Player.GetPlayerData().CurrentChar.statistiche.LUNG_CAPACITY);
			wheelie = (int)(Game.Player.GetPlayerData().CurrentChar.statistiche.WHEELIE_ABILITY);
			drugs = (int)(Game.Player.GetPlayerData().CurrentChar.statistiche.DRUGS);
			fishing = (int)(Game.Player.GetPlayerData().CurrentChar.statistiche.FISHING);
			hunting = (int)(Game.Player.GetPlayerData().CurrentChar.statistiche.HUNTING);
			StatSetInt(Funzioni.HashUint("MP0_STAMINA"), (int)Game.Player.GetPlayerData().CurrentChar.statistiche.STAMINA, true);
			StatSetInt(Funzioni.HashUint("MP0_STRENGTH"), (int)Game.Player.GetPlayerData().CurrentChar.statistiche.STRENGTH, true);
			StatSetInt(Funzioni.HashUint("MP0_FLYING_ABILITY"), (int)Game.Player.GetPlayerData().CurrentChar.statistiche.FLYING_ABILITY, true);
			StatSetInt(Funzioni.HashUint("MP0_LUNG_CAPACITY"), (int)Game.Player.GetPlayerData().CurrentChar.statistiche.LUNG_CAPACITY, true);
			StatSetInt(Funzioni.HashUint("MP0_WHEELIE_ABILITY"), (int)Game.Player.GetPlayerData().CurrentChar.statistiche.WHEELIE_ABILITY, true);
		}

		public static async void RegistraStats(Skills cap, float val)
		{
			switch (cap)
			{
				case Skills.STAMINA:
					skill.STAMINA += val;
					break;
				case Skills.STRENGTH:
					skill.STRENGTH += val;
					break;
				case Skills.FLYING_ABILITY:
					skill.FLYING_ABILITY += val;
					break;
				case Skills.LUNG_CAPACITY:
					skill.LUNG_CAPACITY += val;
					break;
				case Skills.WHEELIE_ABILITY:
					skill.WHEELIE_ABILITY += val;
					break;
				case Skills.DRUGS:
					skill.DRUGS += val;
					break;
				case Skills.FISHING:
					skill.FISHING += val;
					break;
				case Skills.HUNTING:
					skill.HUNTING += val;
					break;
			}
			await Task.FromResult(0);
		}

		public static async void RegistraStats(string cap, float val)
		{
			switch (cap)
			{
				case "STAMINA":
					skill.STAMINA += val;
					break;
				case "STRENGTH":
					skill.STRENGTH += val;
					break;
				case "FLYING_ABILITY":
					skill.FLYING_ABILITY += val;
					break;
				case "LUNG_CAPACITY":
					skill.LUNG_CAPACITY += val;
					break;
				case "WHEELIE_ABILITY":
					skill.WHEELIE_ABILITY += val;
					break;
				case "DRUGS":
					skill.DRUGS += val;
					break;
				case "FISHING":
					skill.FISHING += val;
					break;
				case "HUNTING":
					skill.HUNTING += val;
					break;
			}
			await Task.FromResult(0);
		}


		public static async Task Aggiornamento()
		{
			Ped playerPed = Game.PlayerPed;
			if (playerPed.IsRunning || playerPed.IsSwimming || playerPed.IsJumping)
			{
				nee.fame += 0.025f;
				nee.sete += 0.035f;
				nee.stanchezza += 0.005f;
			}
			else if (playerPed.IsSwimmingUnderWater || playerPed.IsSprinting)
			{
				nee.fame += 0.040f;
				nee.sete += 0.055f;
				nee.stanchezza += 0.070f;
			}
			else if (playerPed.IsInMeleeCombat)
			{
				nee.fame += 0.015f;
				nee.sete += 0.033f;
				nee.stanchezza += 0.057f;
			}
			else
			{
				nee.fame += 0.005f;
				nee.sete += 0.010f;
				nee.stanchezza += 0.015f;
			}
			if (World.CurrentDayTime.Hours >= 18 || World.CurrentDayTime.Hours <= 6)
				nee.stanchezza += 0.07f;
			if (nee.fame >= 100.0f)
				nee.fame = 100.0f;
			else if (nee.fame <= 0.0f)
				nee.fame = 0.0f;

			if (nee.sete >= 100.0f)
				nee.sete = 100.0f;
			else if (nee.sete <= 0.0f)
				nee.sete = 0.0f;

			if (nee.stanchezza >= 100.0f)
				nee.stanchezza = 100.0f;
			else if (nee.stanchezza <= 0.0f)
				nee.stanchezza = 0.0f;

			if (playerPed.IsSprinting || playerPed.IsSwimmingUnderWater)
				skill.STAMINA += 0.002f;
			else if (playerPed.IsRunning || playerPed.IsSwimming)
				skill.STAMINA += +0.001f;

			if (playerPed.IsSwimmingUnderWater)
				skill.LUNG_CAPACITY += 0.002f;

			if (playerPed.IsInMeleeCombat)
				skill.STRENGTH += 0.002f;

			if (playerPed.IsOnBike && playerPed.CurrentVehicle.GetPedOnSeat(VehicleSeat.Driver) == Game.PlayerPed)
			{
				float speed = playerPed.CurrentVehicle.Speed * 3.6f;
				if (speed > 150f)
					skill.WHEELIE_ABILITY += 0.003f;
				else if (speed > 90f && speed < 150f)
					skill.WHEELIE_ABILITY += 0.002f;
				else if (speed < 90f)
					skill.WHEELIE_ABILITY += 0.001f;
			}
			else if ((playerPed.IsInPlane || playerPed.IsInHeli) && playerPed.CurrentVehicle.GetPedOnSeat(VehicleSeat.Driver) == Game.PlayerPed)
			{
				if (playerPed.CurrentVehicle.HeightAboveGround >= 15)
					skill.FLYING_ABILITY += 0.002f;
			}
			if (Lavori.Generici.Pescatore.PescatoreClient.Pescando)
				skill.FISHING += 0.003f;
			/*
			if (Lavori.Droghe.generico)
			{
				skill.DRUGS += 0.002f;
			}
			*/

			if (skill.WHEELIE_ABILITY - wheelie >= 1)
			{
				wheelie = (int)Math.Floor(skill.WHEELIE_ABILITY);
				Game.Player.GetPlayerData().CurrentChar.statistiche.WHEELIE_ABILITY = skill.WHEELIE_ABILITY;
				StatSetInt(Funzioni.HashUint("MP0_WHEELIE_ABILITY"), wheelie, true);
				HUD.ShowStatNotification(wheelie, "PSF_DRIVING");
//				HUD.ShowNotification($"Complimenti! Hai aumentato la tua ~y~Abilità in Moto~w~ di 1 punto! Il tuo livello attuale è di ~b~{wheelie}/100~w~!");
			}

			if (skill.FLYING_ABILITY - fly >= 1)
			{
				fly = (int)Math.Floor(skill.FLYING_ABILITY);
				Game.Player.GetPlayerData().CurrentChar.statistiche.FLYING_ABILITY = skill.FLYING_ABILITY;
				StatSetInt(Funzioni.HashUint("MP0_FLYING_ABILITY"), fly, true);
				HUD.ShowStatNotification(fly, "PSF_FLYING");
//				HUD.ShowNotification($"Complimenti! Hai aumentato la tua ~y~Abilità in Volo~w~ di 1 punto! Il tuo livello attuale è di ~b~{fly}/100~w~!");
			}

			if (skill.STAMINA - stamina >= 1)
			{
				stamina = (int)Math.Floor(skill.STAMINA);
				Game.Player.GetPlayerData().CurrentChar.statistiche.STAMINA = skill.STAMINA;
				StatSetInt(Funzioni.HashUint("MP0_STAMINA"), stamina, true);
				HUD.ShowNotification($"Complimenti! Hai aumentato la tua ~y~Resistenza~w~ di 1 punto! Il tuo livello attuale è di ~b~{stamina}/100~w~!");
//				HUD.ShowStatNotification(stamina, "PSF_STAMINA");
			}

			if (skill.STRENGTH - strength >= 1)
			{
				strength = (int)Math.Floor(skill.STRENGTH);
				Game.Player.GetPlayerData().CurrentChar.statistiche.STRENGTH = skill.STRENGTH;
				StatSetInt(Funzioni.HashUint("MP0_STRENGTH"), strength, true);
				HUD.ShowStatNotification(strength, "PSF_STRENGTH");
//				HUD.ShowNotification($"Complimenti! Hai aumentato la tua ~y~Forza~w~ di 1 punto! Il tuo livello attuale è di ~b~{strength}/100~w~!");
			}

			if (skill.LUNG_CAPACITY - lung >= 1)
			{
				lung = (int)Math.Floor(skill.LUNG_CAPACITY);
				Game.Player.GetPlayerData().CurrentChar.statistiche.LUNG_CAPACITY = skill.LUNG_CAPACITY;
				StatSetInt(Funzioni.HashUint("MP0_STRENGTH"), lung, true);
				HUD.ShowStatNotification(lung, "PSF_LUNG");
//				HUD.ShowNotification($"Complimenti! Hai aumentato il tuo ~y~Fiato sott'Acqua~w~ di 1 punto! Il tuo livello attuale è di ~b~{lung}/100~w~!");
			}

			if (skill.FISHING - fishing >= 1)
			{
				fishing = (int)Math.Floor(skill.FISHING);
				Game.Player.GetPlayerData().CurrentChar.statistiche.FISHING = skill.FISHING;
				HUD.ShowStatNotification(lung, "Pescatore +");
//				HUD.ShowNotification($"Complimenti! Hai aumentato il tuo ~y~Livello di Pesca~w~ di 1 punto! Il tuo livello attuale è di ~b~{fishing}/100~w~!");
			}

			if (skill.HUNTING - hunting >= 1)
			{
				hunting = (int)Math.Floor(skill.HUNTING);
				HUD.ShowStatNotification(lung, "Cacciatore +");
				Game.Player.GetPlayerData().CurrentChar.statistiche.HUNTING = skill.HUNTING;
//				HUD.ShowNotification($"Complimenti! Hai aumentato il tuo ~y~Livello di Caccia~w~ di 1 punto! Il tuo livello attuale è di ~b~{hunting}/100~w~!");
			}

			if (skill.DRUGS - drugs >= 1)
			{
				drugs = (int)Math.Floor(skill.DRUGS);
				//Lavori.Droghe.set(skill.DRUGS);
//				HUD.ShowNotification($"Complimenti! Hai aumentato il tuo ~y~Livello di Droga~w~ di 1 punto! Il tuo livello attuale è di ~b~{drugs}/100~w~!");
			}
			await BaseScript.Delay(2500);
		}

		public static async Task Conseguenze()
		{
			Ped playerPed = Game.PlayerPed;
			int val = 0;
			int val1 = 0;
			bool bol = StatGetInt(Funzioni.HashUint("MP0_STAMINA"), ref val, -1);
			if (nee.stanchezza >= 20.0f)
			{
				bool bol1 = StatGetInt(Funzioni.HashUint("MP0_SHOOTING_ABILITY"), ref val1, -1);
				if (val > 10)
				{
					StatSetInt(Funzioni.HashUint("MP0_STAMINA"), (int)(val / 10), true);
				}

				if (val1 > 10)
				{
					StatSetInt(Funzioni.HashUint("MP0_SHOOTING_ABILITY"), (int)(val / 10), true);
				}
			}
			else if (nee.stanchezza >= 40.0f)
			{
				StatSetInt(Funzioni.HashUint("MP0_STAMINA"), 1, true);
				StatSetInt(Funzioni.HashUint("MP0_SHOOTING_ABILITY"), 1, true);
			}
			else if (nee.stanchezza < 20.0f)
			{
				StatSetInt(Funzioni.HashUint("MP0_STAMINA"), (int)(Game.Player.GetPlayerData().CurrentChar.statistiche.STAMINA), true);
				StatSetInt(Funzioni.HashUint("MP0_SHOOTING_ABILITY"), (int)(Game.Player.GetPlayerData().CurrentChar.statistiche.SHOOTING_ABILITY), true);
			}
			if (nee.stanchezza >= 60.0f)
			{
				SetPlayerSprint(PlayerId(), false);
				playerPed.Accuracy = 15;
			}
			if (nee.stanchezza >= 80.0f)
			{
				playerPed.Accuracy = 0;
				if (Funzioni.GetRandomInt(100) > 85)
				{
					if (playerPed.IsWalking)
					{
						playerPed.Ragdoll(30000, RagdollType.Normal);
						HUD.ShowNotification("Sei svenuto perche eri troppo stanco.. Trova un posto per riposare!!");
						await BaseScript.Delay(30000);
						playerPed.CancelRagdoll();
					}
					else if (playerPed.IsInVehicle() || playerPed.IsInFlyingVehicle)
					{
						SetBlockingOfNonTemporaryEvents(PlayerPedId(), true);
						HUD.ShowNotification("Sei svenuto perche eri troppo stanco.. Se sopravvivi trova un posto per riposare!!");
						playerPed.Task.PlayAnimation("rcmnigel2", "die_horn", 4f, -1, AnimationFlags.StayInEndFrame);
						Client.Instance.AddTick(Horn);
						await BaseScript.Delay(30000);
						playerPed.CancelRagdoll();
						Client.Instance.RemoveTick(Horn);
					}
				}
				await BaseScript.Delay(600000);
			}
			if (nee.fame >= 25)
			{
				if (!fame20)
				{
					HUD.ShowNotification("Senti un certo languorino... Stuzzicheresti volentieri qualcosa.", NotificationColor.GreenDark, true);
					fame20 = true;
				}
			}
			else if (nee.fame >= 60)
			{
				if (!fame60 && fame20)
				{
					HUD.ShowNotification("Hai fame! forse dovresti mangiare qualcosa!", NotificationColor.Yellow, true);
					fame60 = true;
				}
				StatSetInt(Funzioni.HashUint("MP0_STAMINA"), (int)(val / 10), true);
			}
			else if (nee.fame >= 80)
			{
				if (!fame80 && fame20 && fame60)
				{
					HUD.ShowNotification("Stai morendo di fame! Se continui così rischi di morire!.", NotificationColor.Red, true);
					fame80 = true;
				}
				playerPed.Health -= 5;
				playerPed.MovementAnimationSet = "move_injured_generic";
			}
			else if (nee.fame == 100)
			{
				if (!fame100 && fame20 && fame60 && fame80)
				{
					HUD.ShowNotification("Stai morendo di fame!", NotificationColor.Red, true);
					fame100 = true;
				}
				while (playerPed.Health > 0 && nee.fame == 100)
				{
					await BaseScript.Delay(0);
					playerPed.Ragdoll(-1, RagdollType.Normal);
				}
			}
			else if (nee.fame < 20.0f)
			{
				StatSetInt(Funzioni.HashUint("MP0_STAMINA"), (int)(Game.Player.GetPlayerData().CurrentChar.statistiche.STAMINA), true);
				fame20 = false;
				fame60 = false;
				fame80 = false;
				fame100 = false;
			}
			if (nee.sete >= 25)
			{
				if (!sete20)
				{
					HUD.ShowNotification("Hai la gola un po' secca... ti andrebbe una bibita.", NotificationColor.GreenDark, true);
					sete20 = true;
				}
			}
			else if (nee.sete >= 60)
			{
				if (!sete60 && sete20)
				{
					HUD.ShowNotification("Hai sete! forse dovresti bere qualcosa!", NotificationColor.Yellow, true);
					sete60 = true;
				}
				StatSetInt(Funzioni.HashUint("MP0_STAMINA"), (int)(val / 10), true);
			}
			else if (nee.sete >= 80)
			{
				if (!sete80 && sete60 && sete20)
				{
					HUD.ShowNotification("Stai morendo di sete! Se continui così rischi di morire!.", NotificationColor.Red, true);
					sete80 = true;
				}
				playerPed.Health -= 5;
				playerPed.MovementAnimationSet = "move_injured_generic";
			}
			else if (nee.sete == 100)
			{
				if (!sete100 && sete80 && sete60 && sete20)
				{
					HUD.ShowNotification("Stai morendo di sete!", NotificationColor.Red, true);
					sete100 = true;
				}
				while (playerPed.Health > 0 && nee.sete == 100)
				{
					await BaseScript.Delay(0);
					playerPed.Ragdoll(-1, RagdollType.Normal);
				}
			}
			else if (nee.sete < 20.0f)
			{
				StatSetInt(Funzioni.HashUint("MP0_STAMINA"), (int)(Game.Player.GetPlayerData().CurrentChar.statistiche.STAMINA), true);
				fame20 = false;
				fame60 = false;
				fame80 = false;
				fame100 = false;
			}
			await BaseScript.Delay(2500);
		}

		public static async Task Horn()
		{
			SoundVehicleHornThisFrame(GetVehiclePedIsUsing(PlayerPedId()));
			await Task.FromResult(0);
		}

		public static async Task Agg()
		{
			await BaseScript.Delay(60000);
			BaseScript.TriggerServerEvent("lprp:updateCurChar", "needs", nee.Serialize());
			BaseScript.TriggerServerEvent("lprp:updateCurChar", "skill", skill.Serialize());
		}
	}
}
