using CitizenFX.Core;
using CitizenFX.Core.UI;
using Newtonsoft.Json;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Shared;
using System;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Client.gmPrincipale.Status
{
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

		public static void Init()
		{
			Client.GetInstance.RegisterEventHandler("lprp:onPlayerSpawn", new Action(Eccolo));
			Client.GetInstance.RegisterEventHandler("lprp:skills:registraSkill", new Action<string, int>(RegistraStats));
		}

		public static async void Eccolo()
		{
			nee.fame = Eventi.Player.CurrentChar.needs.fame;
			nee.sete = Eventi.Player.CurrentChar.needs.sete;
			nee.stanchezza = Eventi.Player.CurrentChar.needs.stanchezza;
			nee.malattia = Eventi.Player.CurrentChar.needs.malattia;
			skill.STAMINA = Eventi.Player.CurrentChar.statistiche.STAMINA;
			skill.STRENGTH = Eventi.Player.CurrentChar.statistiche.STRENGTH;
			skill.FLYING_ABILITY = Eventi.Player.CurrentChar.statistiche.FLYING_ABILITY;
			skill.LUNG_CAPACITY = Eventi.Player.CurrentChar.statistiche.LUNG_CAPACITY;
			skill.WHEELIE_ABILITY = Eventi.Player.CurrentChar.statistiche.WHEELIE_ABILITY;
			skill.FLYING_ABILITY = Eventi.Player.CurrentChar.statistiche.FLYING_ABILITY;
			skill.DRUGS = Eventi.Player.CurrentChar.statistiche.DRUGS;
			skill.FISHING = Eventi.Player.CurrentChar.statistiche.FISHING;
			StatSetInt((uint)Game.GenerateHash("MP0_STAMINA"), (int)Eventi.Player.CurrentChar.statistiche.STAMINA, true);
			StatSetInt((uint)Game.GenerateHash("MP0_STRENGTH"), (int)Eventi.Player.CurrentChar.statistiche.STRENGTH, true);
			StatSetInt((uint)Game.GenerateHash("MP0_FLYING_ABILITY"), (int)Eventi.Player.CurrentChar.statistiche.FLYING_ABILITY, true);
			StatSetInt((uint)Game.GenerateHash("MP0_LUNG_CAPACITY"), (int)Eventi.Player.CurrentChar.statistiche.LUNG_CAPACITY, true);
			StatSetInt((uint)Game.GenerateHash("MP0_WHEELIE_ABILITY"), (int)Eventi.Player.CurrentChar.statistiche.WHEELIE_ABILITY, true);
			StatSetInt((uint)Game.GenerateHash("MP0_FLYING_ABILITY"), (int)Eventi.Player.CurrentChar.statistiche.FLYING_ABILITY, true);
		}

		public static async void RegistraStats(string name, int val)
		{
			if (name == "DRUGS")
			{
				skill.DRUGS = val;
			}
			else if (name == "FISHING")
			{
				skill.FISHING = val;
			}
			else
			{
				StatSetInt((uint)Game.GenerateHash("MP0_" + name), val, true);
			}
		}


		public static async Task Aggiornamento()
		{
			if (Game.PlayerPed.IsRunning || Game.PlayerPed.IsSwimming || Game.PlayerPed.IsJumping)
			{
				nee.fame += 0.025f;
				nee.sete += 0.035f;
				nee.stanchezza += 0.005f;
			}
			else if (Game.PlayerPed.IsSwimmingUnderWater || Game.PlayerPed.IsSprinting)
			{
				nee.fame += 0.040f;
				nee.sete += 0.055f;
				nee.stanchezza += 0.070f;
			}
			else if (Game.PlayerPed.IsInMeleeCombat)
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
			{
				nee.stanchezza += 0.07f;
			}
			if (nee.fame >= 100.0f)
			{
				nee.fame = 100.0f;
			}
			else if (nee.fame <= 0.0f)
			{
				nee.fame = 0.0f;
			}

			if (nee.sete >= 100.0f)
			{
				nee.sete = 100.0f;
			}
			else if (nee.sete <= 0.0f)
			{
				nee.sete = 0.0f;
			}

			if (nee.stanchezza >= 100.0f)
			{
				nee.stanchezza = 100.0f;
			}
			else if (nee.stanchezza <= 0.0f)
			{
				nee.stanchezza = 0.0f;
			}

			if (Game.PlayerPed.IsSprinting || Game.PlayerPed.IsSwimmingUnderWater)
			{
				skill.STAMINA += 0.002f;
			}
			else if (Game.PlayerPed.IsRunning || Game.PlayerPed.IsSwimming)
			{
				skill.STAMINA += +0.001f;
			}

			if (Game.PlayerPed.IsSwimmingUnderWater)
			{
				skill.LUNG_CAPACITY += 0.002f;
			}

			if (Game.PlayerPed.IsInMeleeCombat)
			{
				skill.STRENGTH += 0.002f;
			}

			if (Game.PlayerPed.IsOnBike && Game.PlayerPed.CurrentVehicle.GetPedOnSeat(VehicleSeat.Driver) == Game.PlayerPed)
			{
				float speed = Game.PlayerPed.CurrentVehicle.Speed * 3.6f;
				if (speed > 150f)
				{
					skill.WHEELIE_ABILITY += 0.003f;
				}
				else if (speed > 90f && speed < 150)
				{
					skill.WHEELIE_ABILITY += 0.002f;
				}
				else if (speed < 90f)
				{
					skill.WHEELIE_ABILITY += 0.001f;
				}
			}
			else if ((Game.PlayerPed.IsInPlane || Game.PlayerPed.IsInHeli) && Game.PlayerPed.CurrentVehicle.GetPedOnSeat(VehicleSeat.Driver) == Game.PlayerPed)
			{
				if (Game.PlayerPed.CurrentVehicle.HeightAboveGround >= 15)
				{
					skill.FLYING_ABILITY += 0.002f;
				}
			}
			if (Lavori.Generici.Pescatore.PescatoreClient.Pescando)
			{
				skill.FISHING += 0.005f;
			}
			/*
			if (Lavori.Droghe.generico)
			{
				skill.DRUGS += 0.0002f;
			}
			*/

			if (MathUtil.WithinEpsilon(skill.WHEELIE_ABILITY % 1f, 0.0f, 0.002f) && skill.WHEELIE_ABILITY != 0)
			{
				Eventi.Player.CurrentChar.statistiche.WHEELIE_ABILITY = skill.WHEELIE_ABILITY;
				StatSetInt((uint)Game.GenerateHash("MP0_WHEELIE_ABILITY"), (int)(skill.WHEELIE_ABILITY), true);
				HUD.ShowNotification("Complimenti! Hai aumentato la tua ~y~Abilità in Moto~w~ di 1 punto! Il tuo livello attuale è di ~b~" + (int)skill.WHEELIE_ABILITY + "/100!");
			}
			if (MathUtil.WithinEpsilon(skill.FLYING_ABILITY % 1f, 0.0f, 0.002f) && skill.FLYING_ABILITY != 0)
			{
				Eventi.Player.CurrentChar.statistiche.FLYING_ABILITY = skill.FLYING_ABILITY;
				StatSetInt((uint)Game.GenerateHash("MP0_FLYING_ABILITY"), (int)(skill.FLYING_ABILITY), true);
				HUD.ShowNotification("Complimenti! Hai aumentato la tua ~y~Abilità in Volo~w~ di 1 punto! Il tuo livello attuale è di ~b~" + (int)skill.FLYING_ABILITY + "/100!");
			}
			if (MathUtil.WithinEpsilon(skill.STAMINA % 1f, 0.0f, 0.002f) && skill.STAMINA != 0)
			{
				Eventi.Player.CurrentChar.statistiche.STAMINA = skill.STAMINA;
				StatSetInt((uint)Game.GenerateHash("MP0_STAMINA"), (int)(skill.STAMINA), true);
				HUD.ShowNotification("Complimenti! Hai aumentato la tua ~y~Resistenza~w~ di 1 punto! Il tuo livello attuale è di ~b~" + (int)skill.STAMINA + "/100!");
			}
			if (MathUtil.WithinEpsilon(skill.STRENGTH % 1f, 0.0f, 0.002f) && skill.STRENGTH != 0)
			{
				Eventi.Player.CurrentChar.statistiche.STRENGTH = skill.STRENGTH;
				StatSetInt((uint)Game.GenerateHash("MP0_STRENGTH"), (int)(skill.STRENGTH), true);
				HUD.ShowNotification("Complimenti! Hai aumentato la tua ~y~Forza~w~ di 1 punto! Il tuo livello attuale è di ~b~" + (int)skill.STRENGTH + "/100!");
			}
			if (MathUtil.WithinEpsilon(skill.LUNG_CAPACITY % 1f, 0.0f, 0.002f) && skill.LUNG_CAPACITY != 0)
			{
				Eventi.Player.CurrentChar.statistiche.LUNG_CAPACITY = skill.LUNG_CAPACITY;
				StatSetInt((uint)Game.GenerateHash("MP0_STRENGTH"), (int)(skill.LUNG_CAPACITY), true);
				HUD.ShowNotification("Complimenti! Hai aumentato il tuo ~y~Fiato sott'Acqua~w~ di 1 punto! Il tuo livello attuale è di ~b~" + (int)skill.LUNG_CAPACITY + "/100!");
			}
			if (MathUtil.WithinEpsilon(skill.FISHING%1f, 0.0f, 0.002f) && skill.FISHING != 0)
//			if (skill.FISHING >= (Eventi.Player.CurrentChar.statistiche.FISHING + 1))
			{
				Eventi.Player.CurrentChar.statistiche.FISHING = skill.FISHING;
				//Lavori.Pescatore.set(skill.FISHING);
				HUD.ShowNotification("Complimenti! Hai aumentato il tuo ~y~Livello di Pesca~w~ di 1 punto! Il tuo livello attuale è di ~b~" + (int)skill.FISHING + "/100!");
			}
			if (MathUtil.WithinEpsilon(skill.DRUGS % 1f, 0.0f, 0.002f) && skill.DRUGS != 0)
			{
				Eventi.Player.CurrentChar.statistiche.DRUGS = skill.DRUGS;
				//Lavori.Droghe.set(skill.DRUGS);
				HUD.ShowNotification("Complimenti! Hai aumentato il tuo ~y~Livello di Droga~w~ di 1 punto! Il tuo livello attuale è di ~b~" + (int)skill.DRUGS + "/100!");
			}
			await BaseScript.Delay(2500);
		}

		public static async Task Conseguenze()
		{
			int val = 0;
			int val1 = 0;
			bool bol = StatGetInt((uint)Game.GenerateHash("MP0_STAMINA"), ref val, -1);
			if (nee.stanchezza >= 20.0f)
			{
				bool bol1 = StatGetInt((uint)Game.GenerateHash("MP0_SHOOTING_ABILITY"), ref val1, -1);
				if (val > 10)
				{
					StatSetInt((uint)Game.GenerateHash("MP0_STAMINA"), (int)(val / 10), true);
				}

				if (val1 > 10)
				{
					StatSetInt((uint)Game.GenerateHash("MP0_SHOOTING_ABILITY"), (int)(val / 10), true);
				}
			}
			else if (nee.stanchezza >= 40.0f)
			{
				StatSetInt((uint)Game.GenerateHash("MP0_STAMINA"), 1, true);
				StatSetInt((uint)Game.GenerateHash("MP0_SHOOTING_ABILITY"), 1, true);
			}
			else if (nee.stanchezza < 20.0f)
			{
				StatSetInt((uint)Game.GenerateHash("MP0_STAMINA"), (int)(Eventi.Player.CurrentChar.statistiche.STAMINA), true);
				StatSetInt((uint)Game.GenerateHash("MP0_SHOOTING_ABILITY"), (int)(Eventi.Player.CurrentChar.statistiche.SHOOTING_ABILITY), true);
			}
			if (nee.stanchezza >= 60.0f)
			{
				SetPlayerSprint(PlayerId(), false);
				Game.PlayerPed.Accuracy = 15;
			}
			if (nee.stanchezza >= 80.0f)
			{
				Game.PlayerPed.Accuracy = 0;
				if (Funzioni.GetRandomInt(100) > 85)
				{
					if (Game.PlayerPed.IsWalking)
					{
						Game.PlayerPed.Ragdoll(30000, RagdollType.Normal);
						HUD.ShowNotification("Sei svenuto perche eri troppo stanco.. Trova un posto per riposare!!");
						await BaseScript.Delay(30000);
						Game.PlayerPed.CancelRagdoll();
					}
					else if (Game.PlayerPed.IsInVehicle() || Game.PlayerPed.IsInFlyingVehicle)
					{
						SetBlockingOfNonTemporaryEvents(PlayerPedId(), true);
						HUD.ShowNotification("Sei svenuto perche eri troppo stanco.. Se sopravvivi trova un posto per riposare!!");
						Game.PlayerPed.Task.PlayAnimation("rcmnigel2", "die_horn", 4f, -1, AnimationFlags.StayInEndFrame);
						Client.GetInstance.RegisterTickHandler(Horn);
						await BaseScript.Delay(30000);
						Game.PlayerPed.CancelRagdoll();
						Client.GetInstance.DeregisterTickHandler(Horn);
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
				StatSetInt((uint)Game.GenerateHash("MP0_STAMINA"), (int)(val / 10), true);
			}
			else if (nee.fame >= 80)
			{
				if (!fame80 && fame20 && fame60)
				{
					HUD.ShowNotification("Stai morendo di fame! Se continui così rischi di morire!.", NotificationColor.Red, true);
					fame80 = true;
				}
				Game.PlayerPed.Health -= 5;
				Game.PlayerPed.MovementAnimationSet = "move_injured_generic";
			}
			else if (nee.fame == 100)
			{
				if (!fame100 && fame20 && fame60 && fame80)
				{
					HUD.ShowNotification("Stai morendo di fame!", NotificationColor.Red, true);
					fame100 = true;
				}
				while (Game.PlayerPed.Health > 0 && nee.fame == 100)
				{
					await BaseScript.Delay(0);
					Game.PlayerPed.Ragdoll(-1, RagdollType.Normal);
				}
			}
			else if (nee.fame < 20.0f)
			{
				StatSetInt((uint)Game.GenerateHash("MP0_STAMINA"), (int)(Eventi.Player.CurrentChar.statistiche.STAMINA), true);
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
				StatSetInt((uint)Game.GenerateHash("MP0_STAMINA"), (int)(val / 10), true);
			}
			else if (nee.sete >= 80)
			{
				if (!sete80 && sete60 && sete20)
				{
					HUD.ShowNotification("Stai morendo di sete! Se continui così rischi di morire!.", NotificationColor.Red, true);
					sete80 = true;
				}
				Game.PlayerPed.Health -= 5;
				Game.PlayerPed.MovementAnimationSet = "move_injured_generic";
			}
			else if (nee.sete == 100)
			{
				if (!sete100 && sete80 && sete60 && sete20)
				{
					HUD.ShowNotification("Stai morendo di sete!", NotificationColor.Red, true);
					sete100 = true;
				}
				while (Game.PlayerPed.Health > 0 && nee.sete == 100)
				{
					await BaseScript.Delay(0);
					Game.PlayerPed.Ragdoll(-1, RagdollType.Normal);
				}
			}
			else if (nee.sete < 20.0f)
			{
				StatSetInt((uint)Game.GenerateHash("MP0_STAMINA"), (int)(Eventi.Player.CurrentChar.statistiche.STAMINA), true);
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
			BaseScript.TriggerServerEvent("lprp:updateCurChar", "needs", JsonConvert.SerializeObject(nee));
			BaseScript.TriggerServerEvent("lprp:updateCurChar", "skill", JsonConvert.SerializeObject(skill));
		}
	}
}
