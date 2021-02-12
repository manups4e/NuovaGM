using CitizenFX.Core;
using Newtonsoft.Json;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Shared;
using System;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.Core.Status
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
		private static bool stanchezza20 = false;
		private static bool stanchezza40 = false;
		private static bool stanchezza60 = false;
		private static bool stanchezza80 = false;
		private static bool stanchezza100 = false;
		private static int stamina = 0;
		private static int strength = 0;
		private static int fly = 0;
		private static int lung = 0;
		private static int wheelie = 0;
		private static int drugs = 0;
		private static int fishing = 0;
		private static int hunting = 0;
		private static int AggTimer = 0;
		private static int SvieniTimer = 0;
		private static int UpdTimer = 0;
		
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


		public static async Task AggiornamentoEConseguenze()
		{
			await BaseScript.Delay(625); // ogni 4 volte
			if (Game.GameTime - AggTimer > 2500)
			{
				Player me = Game.Player;
				Ped playerPed = me.Character;

				#region Aggiornamento
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
					nee.sete += 0.006f;
					nee.stanchezza += 0.007f;
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

				if (playerPed.IsOnBike && playerPed.CurrentVehicle.GetPedOnSeat(VehicleSeat.Driver) == playerPed)
				{
					float speed = playerPed.CurrentVehicle.Speed * 3.6f;
					if (speed > 150f)
						skill.WHEELIE_ABILITY += 0.003f;
					else if (speed > 90f && speed < 150f)
						skill.WHEELIE_ABILITY += 0.002f;
					else if (speed < 90f)
						skill.WHEELIE_ABILITY += 0.001f;
				}
				else if ((playerPed.IsInPlane || playerPed.IsInHeli) && playerPed.CurrentVehicle.GetPedOnSeat(VehicleSeat.Driver) == playerPed)
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

				if (skill.WHEELIE_ABILITY - wheelie >= 1f)
				{
					wheelie = (int)Math.Floor(skill.WHEELIE_ABILITY);
					me.GetPlayerData().CurrentChar.statistiche.WHEELIE_ABILITY = skill.WHEELIE_ABILITY;
					StatSetInt(Funzioni.HashUint("MP0_WHEELIE_ABILITY"), wheelie, true);
					HUD.ShowStatNotification(wheelie, "PSF_DRIVING");
				}

				if (skill.FLYING_ABILITY - fly >= 1f)
				{
					fly = (int)Math.Floor(skill.FLYING_ABILITY);
					me.GetPlayerData().CurrentChar.statistiche.FLYING_ABILITY = skill.FLYING_ABILITY;
					StatSetInt(Funzioni.HashUint("MP0_FLYING_ABILITY"), fly, true);
					HUD.ShowStatNotification(fly, "PSF_FLYING");
				}

				if (skill.STAMINA - stamina >= 1f)
				{
					stamina = (int)Math.Floor(skill.STAMINA);
					me.GetPlayerData().CurrentChar.statistiche.STAMINA = skill.STAMINA;
					StatSetInt(Funzioni.HashUint("MP0_STAMINA"), stamina, true);
					HUD.ShowNotification($"Complimenti! Hai aumentato la tua ~y~Resistenza~w~ di 1 punto! Il tuo livello attuale è di ~b~{stamina}/100~w~!");
				}

				if (skill.STRENGTH - strength >= 1f)
				{
					strength = (int)Math.Floor(skill.STRENGTH);
					me.GetPlayerData().CurrentChar.statistiche.STRENGTH = skill.STRENGTH;
					StatSetInt(Funzioni.HashUint("MP0_STRENGTH"), strength, true);
					HUD.ShowStatNotification(strength, "PSF_STRENGTH");
				}

				if (skill.LUNG_CAPACITY - lung >= 1f)
				{
					lung = (int)Math.Floor(skill.LUNG_CAPACITY);
					me.GetPlayerData().CurrentChar.statistiche.LUNG_CAPACITY = skill.LUNG_CAPACITY;
					StatSetInt(Funzioni.HashUint("MP0_STRENGTH"), lung, true);
					HUD.ShowStatNotification(lung, "PSF_LUNG");
				}

				if (skill.FISHING - fishing >= 1f)
				{
					fishing = (int)Math.Floor(skill.FISHING);
					me.GetPlayerData().CurrentChar.statistiche.FISHING = skill.FISHING;
					HUD.ShowStatNotification(lung, "Pescatore +");
				}

				if (skill.HUNTING - hunting >= 1f)
				{
					hunting = (int)Math.Floor(skill.HUNTING);
					HUD.ShowStatNotification(lung, "Cacciatore +");
					me.GetPlayerData().CurrentChar.statistiche.HUNTING = skill.HUNTING;
				}

				if (skill.DRUGS - drugs >= 1f)
				{
					drugs = (int)Math.Floor(skill.DRUGS);
					//Lavori.Droghe.set(skill.DRUGS);
				}
				#endregion

				#region Conseguenze
				if (nee.stanchezza < 20.0f)
				{
					if (stanchezza20 || stanchezza40 || stanchezza60 || stanchezza80 || stanchezza100)
					{
						StatSetInt(Funzioni.HashUint("MP0_STAMINA"), (int)(me.GetPlayerData().CurrentChar.statistiche.STAMINA), true);
						StatSetInt(Funzioni.HashUint("MP0_SHOOTING_ABILITY"), (int)(me.GetPlayerData().CurrentChar.statistiche.SHOOTING_ABILITY), true);
						stanchezza20 = false;
						stanchezza40 = false;
						stanchezza60 = false;
						stanchezza80 = false;
						stanchezza100 = false;
					}
				}
				if (nee.stanchezza >= 20f)
				{
					if (!stanchezza20)
					{
						int stam = 0;
						int shot = 0;
						StatGetInt(Funzioni.HashUint("MP0_STAMINA"), ref stam, -1);
						StatGetInt(Funzioni.HashUint("MP0_SHOOTING_ABILITY"), ref shot, -1);
						if (stam > 10)
							StatSetInt(Funzioni.HashUint("MP0_STAMINA"), (int)(stam - (stam / 20f)), true);
						if (shot > 10)
							StatSetInt(Funzioni.HashUint("MP0_SHOOTING_ABILITY"), (int)(shot - (shot / 20f)), true);
						stanchezza20 = true;
					}
				}
				if (nee.stanchezza >= 40.0f)
				{
					if (!stanchezza40)
					{
						StatSetInt(Funzioni.HashUint("MP0_STAMINA"), 1, true);
						StatSetInt(Funzioni.HashUint("MP0_SHOOTING_ABILITY"), 1, true);
						stanchezza40 = true;
					}
				}
				if (nee.stanchezza >= 60.0f)
				{
					if (!stanchezza60)
					{
						SetPlayerSprint(PlayerId(), false);
						stanchezza60 = true;
					}
				}
				if (nee.stanchezza >= 80.0f)
				{
					if (Game.GameTime - SvieniTimer > 600000)
					{
						if (Funzioni.GetRandomInt(100) > 85)
						{
							if (playerPed.IsWalking)
							{
								playerPed.Ragdoll(30000, RagdollType.Normal);
								HUD.ShowNotification("Sei svenuto perche sei troppo stanco.. Trova un posto per riposare!!");
								Clacson();
							}
							else if (playerPed.IsInVehicle() || playerPed.IsInFlyingVehicle)
							{
								SetBlockingOfNonTemporaryEvents(PlayerPedId(), true);
								HUD.ShowNotification("Sei svenuto perche sei troppo stanco.. Se sopravvivi trova un posto per riposare!!");
								playerPed.Task.PlayAnimation("rcmnigel2", "die_horn", 4f, -1, AnimationFlags.StayInEndFrame);
								Clacson();
							}
						}
						SvieniTimer = Game.GameTime;
					}
					stanchezza80 = true;
				}
				if (nee.fame < 20.0f)
				{
					if (fame20 || fame60 || fame80 || fame100)
					{
						StatSetInt(Funzioni.HashUint("MP0_STAMINA"), (int)(me.GetPlayerData().CurrentChar.statistiche.STAMINA), true);
						fame20 = false;
						fame60 = false;
						fame80 = false;
						fame100 = false;
					}
				}
				if (nee.fame >= 20f)
				{
					if (!fame20)
					{
						HUD.ShowNotification("Senti un certo languorino... Stuzzicheresti volentieri qualcosa.", NotificationColor.GreenDark, true);
						fame20 = true;
					}
				}
				if (nee.fame >= 60)
				{
					if (!fame60)
					{
						HUD.ShowNotification("Hai fame! forse dovresti mangiare qualcosa!", NotificationColor.Yellow, true);
						fame60 = true;
						int stam = 0;
						StatGetInt(Funzioni.HashUint("MP0_STAMINA"), ref stam, -1);
						StatSetInt(Funzioni.HashUint("MP0_STAMINA"), (int)(stam - (stam / 10f)), true);
					}
				}
				if (nee.fame >= 80)
				{
					if (!fame80)
					{
						HUD.ShowNotification("Stai morendo di fame! Se continui così rischi di morire!.", NotificationColor.Red, true);
						fame80 = true;
						playerPed.Health -= 5;
						playerPed.MovementAnimationSet = "move_injured_generic";
					}
				}
				if (nee.fame == 100)
				{
					if (!fame100)
					{
						HUD.ShowNotification("Stai morendo di fame!", NotificationColor.Red, true);
						fame100 = true;	
						Client.Instance.AddTick(FameSete);
					}
				}

				if (nee.sete < 20.0f)
				{
					StatSetInt(Funzioni.HashUint("MP0_STAMINA"), (int)(me.GetPlayerData().CurrentChar.statistiche.STAMINA), true);
					fame20 = false;
					fame60 = false;
					fame80 = false;
					fame100 = false;
				}
				if (nee.sete >= 20f)
				{
					if (!sete20)
					{
						HUD.ShowNotification("Hai la gola un po' secca... ti andrebbe una bibita.", NotificationColor.GreenDark, true);
						sete20 = true;
					}
				}
				if (nee.sete >= 60)
				{
					if (!sete60)
					{
						HUD.ShowNotification("Hai sete! forse dovresti bere qualcosa!", NotificationColor.Yellow, true);
						sete60 = true;
						int stam = 0;
						StatGetInt(Funzioni.HashUint("MP0_STAMINA"), ref stam, -1);
						StatSetInt(Funzioni.HashUint("MP0_STAMINA"), (int)(stam - (stam / 10f)), true);
					}
				}
				if (nee.sete >= 80)
				{
					if (!sete80)
					{
						HUD.ShowNotification("Stai morendo di sete! Se continui così rischi di morire!.", NotificationColor.Red, true);
						playerPed.Health -= 5;
						playerPed.MovementAnimationSet = "move_injured_generic";
						sete80 = true;
					}
				}
				if (nee.sete == 100)
				{
					if (!sete100)
					{
						HUD.ShowNotification("Stai morendo di sete!", NotificationColor.Red, true);
						sete100 = true;
						Client.Instance.AddTick(FameSete);
					}
				}
				AggTimer = Game.GameTime;
				#endregion
			}
			if (Game.GameTime - UpdTimer > 5000)//60000)
			{
				await Agg();
				UpdTimer = Game.GameTime;
			}
		}

		private static async Task FameSete()
		{
			await BaseScript.Delay(1000);
			Ped playerPed = Game.PlayerPed;
			if (playerPed.Health > 0 && (fame100 || sete100))
			{
				if (playerPed.Health <= 50)
					playerPed.Health -= 5;
				else
					playerPed.Health -= 1;
			}
			else
				Client.Instance.RemoveTick(FameSete);
		}

		private static async void Clacson()
		{
			Ped p = Game.PlayerPed;
			if (p.IsInVehicle())
				Client.Instance.AddTick(Horn);
			await BaseScript.Delay(30000);
			p.CancelRagdoll();
			if (p.IsInVehicle())
				Client.Instance.RemoveTick(Horn);
		}
		public static async Task Horn()
		{
			SoundVehicleHornThisFrame(GetVehiclePedIsUsing(PlayerPedId()));
			await Task.FromResult(0);
		}

		public static async Task Agg()
		{
			BaseScript.TriggerServerEvent("lprp:updateCurChar", "needs", nee.Serialize());
			BaseScript.TriggerServerEvent("lprp:updateCurChar", "skill", skill.Serialize());
			await Task.FromResult(0);
		}
	}
}
