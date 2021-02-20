using CitizenFX.Core;
using Newtonsoft.Json;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Shared;
using System;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Client.Core.status.Interfacce;
using System.Collections.Generic;
using System.Linq;
using Logger;
using TheLastPlanet.Client.Core.Personaggio;

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
		public static int StatusMax = 100;
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
		//private static bool stanchezza80 = false;
		//private static bool stanchezza100 = false;
		private static int SvieniTimer = 0;
		private static int UpdTimer = 0;
		public static Dictionary<string, Necessità> Needs = new Dictionary<string, Necessità>();
		public static Dictionary<string, Statistica> Statistics = new Dictionary<string, Statistica>();

		public static void Init()
		{
			Client.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action(Eccolo));
			Client.Instance.AddEventHandler("lprp:skills:registraSkill", new Action<string, float>(RegistraStats));

			Needs.Add("Fame", new Necessità("Fame", 0, 0.005f, new Action<Ped, Player, Necessità>(Fame)));
			Needs.Add("Sete", new Necessità("Sete", 0, 0.006f, new Action<Ped, Player, Necessità>(Sete)));
			Needs.Add("Stanchezza", new Necessità("Stanchezza", 0, 0.007f, new Action<Ped, Player, Necessità>(Stanchezza)));

			Statistics.Add("STAMINA", new Statistica("Stamina", "MP0_STAMINA", "PSF_STAMINA", new Action<Ped, Player, Statistica>(Stamina)));
			Statistics.Add("STRENGTH", new Statistica("Strenght", "MP0_STRENGTH", "PSF_STRENGTH", new Action<Ped, Player, Statistica>(Strenght)));
			Statistics.Add("FLYING_ABILITY", new Statistica("Flying_ability", "MP0_FLYING_ABILITY", "PSF_FLYING", new Action<Ped, Player, Statistica>(Flying)));
			Statistics.Add("LUNG_CAPACITY", new Statistica("Lung_capacity", "MP0_LUNG_CAPACITY", "PSF_LUNG", new Action<Ped, Player, Statistica>(Lung)));
			Statistics.Add("WHEELIE_ABILITY", new Statistica("Wheelie_ability", "MP0_WHEELIE_ABILITY", "PSF_DRIVING", new Action<Ped, Player, Statistica>(Driving)));
			Statistics.Add("SHOOTING_ABILITY", new Statistica("Mira", "SHOOTING_ABILITY", "PSF_SHOOTING", new Action<Ped, Player, Statistica>(Shooting)));
			Statistics.Add("FISHING", new Statistica("Pescatore", "Pescatore", "Pesca +", new Action<Ped, Player, Statistica>(Pescatore)));
			Statistics.Add("HUNTING", new Statistica("Cacciatore", "Cacciatore", "Caccia +", new Action<Ped, Player, Statistica>(Cacciatore)));
			Statistics.Add("DRUGS", new Statistica("Droghe", "Droghe", "Droga +", new Action<Ped, Player, Statistica>(Droga)));
			//PSF_SHOOTING aggiungere abilità sparatorie?
		}

		public static void Eccolo()
		{
			PlayerChar me = Eventi.Player;

			Needs["Fame"].Val = me.CurrentChar.needs.fame;
			Needs["Sete"].Val = me.CurrentChar.needs.sete;
			Needs["Stanchezza"].Val = me.CurrentChar.needs.stanchezza;
			//nee.malattia = m.CurrentChar.needs.malattia;

			Statistics["STAMINA"].Val = me.CurrentChar.statistiche.STAMINA;
			Statistics["STRENGTH"].Val = me.CurrentChar.statistiche.STRENGTH;
			Statistics["FLYING_ABILITY"].Val = me.CurrentChar.statistiche.FLYING_ABILITY;
			Statistics["LUNG_CAPACITY"].Val = me.CurrentChar.statistiche.LUNG_CAPACITY;
			Statistics["WHEELIE_ABILITY"].Val = me.CurrentChar.statistiche.WHEELIE_ABILITY;
			Statistics["DRUGS"].Val = me.CurrentChar.statistiche.DRUGS;
			Statistics["FISHING"].Val = me.CurrentChar.statistiche.FISHING;
			Statistics["HUNTING"].Val = me.CurrentChar.statistiche.HUNTING;

			StatSetInt(Funzioni.HashUint("MP0_STAMINA"), (int)Statistics["STAMINA"].Val, true);
			StatSetInt(Funzioni.HashUint("MP0_STRENGTH"), (int)Statistics["STRENGTH"].Val, true);
			StatSetInt(Funzioni.HashUint("MP0_FLYING_ABILITY"), (int)Statistics["FLYING_ABILITY"].Val, true);
			StatSetInt(Funzioni.HashUint("MP0_LUNG_CAPACITY"), (int)Statistics["LUNG_CAPACITY"].Val, true);
			StatSetInt(Funzioni.HashUint("MP0_WHEELIE_ABILITY"), (int)Statistics["WHEELIE_ABILITY"].Val, true);
		}

		public static void RegistraStats(Skills cap, float val)
		{
			if (Statistics.ContainsKey(cap.ToString()))
				Statistics[cap.ToString()].Val += val;
		}

		public static void RegistraStats(string cap, float val)
		{
			if (Statistics.ContainsKey(cap))
				Statistics[cap].Val += val;
		}

		private static async Task FameSete()
		{
			await BaseScript.Delay(1000);
			Ped playerPed = new Ped(PlayerPedId());
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
			Ped p = new Ped(PlayerPedId());
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
			Needs nee = new Needs()
			{
				fame = Needs["Fame"].Val,
				sete = Needs["Sete"].Val,
				stanchezza = Needs["Stanchezza"].Val,
				malattia = Eventi.Player.CurrentChar.needs.malattia
			};
			Statistiche skill = new Statistiche()
			{
				STAMINA = Statistics["STAMINA"].Val,
				STRENGTH = Statistics["STRENGTH"].Val,
				LUNG_CAPACITY = Statistics["LUNG_CAPACITY"].Val,
				SHOOTING_ABILITY = Statistics["SHOOTING_ABILITY"].Val,
				WHEELIE_ABILITY = Statistics["WHEELIE_ABILITY"].Val,
				FLYING_ABILITY = Statistics["FLYING_ABILITY"].Val,
				DRUGS = Statistics["DRUGS"].Val,
				FISHING = Statistics["FISHING"].Val,
				HUNTING = Statistics["HUNTING"].Val,
			};
			BaseScript.TriggerServerEvent("lprp:updateCurChar", "needs", nee.Serialize());
			BaseScript.TriggerServerEvent("lprp:updateCurChar", "skill", skill.Serialize());
			await Task.FromResult(0);
		}

		public static async Task GestioneStatsSkill()
		{
			await BaseScript.Delay(1000);
			Ped p = new Ped(PlayerPedId());
			Player m = new Player(PlayerId());
			Needs.Values.ToList().ForEach(x => x.OnTick(p, m));
			Statistics.Values.ToList().ForEach(x => x.OnTick(p, m));

			if (Game.GameTime - UpdTimer > 30000)//60000)
			{
				await Agg();
				UpdTimer = Game.GameTime;
			}

			await Task.FromResult(0);
			//Log.Printa(LogType.Debug, $"{n.Name} = {n.Val} [{n.GetPercent()}, {n.ChangeVal}]");
		}

		#region Needs
		private static void Fame(Ped playerPed, Player me, Necessità fame)
		{
			if (!playerPed.IsAiming && (playerPed.IsRunning || playerPed.IsSwimming || playerPed.IsJumping))
				fame.ChangeVal = 0.025f;
			else if (!playerPed.IsAiming && (playerPed.IsSwimmingUnderWater || playerPed.IsSprinting))
				fame.ChangeVal = 0.040f;
			else if (playerPed.IsInMeleeCombat)
				fame.ChangeVal = 0.015f;
			else
				fame.ChangeVal = 0.005f;

			fame.Val += fame.ChangeVal;


			if (fame.Val < 20.0f && (fame20 || fame60 || fame80 || fame100))
			{
				StatSetInt(Funzioni.HashUint("MP0_STAMINA"), (int)(Eventi.Player.CurrentChar.statistiche.STAMINA), true);
				fame20 = false;
				fame60 = false;
				fame80 = false;
				fame100 = false;
			}
			if (fame.Val >= 20f && !fame20)
			{
				HUD.ShowNotification("Senti un certo languorino... Stuzzicheresti volentieri qualcosa.", NotificationColor.GreenDark, true);
				fame20 = true;
			}
			if (fame.Val >= 60 && !fame60)
			{
				HUD.ShowNotification("Hai fame! forse dovresti mangiare qualcosa!", NotificationColor.Yellow, true);
				fame60 = true;
				int stam = 0;
				StatGetInt(Funzioni.HashUint("MP0_STAMINA"), ref stam, -1);
				StatSetInt(Funzioni.HashUint("MP0_STAMINA"), (int)(stam - (stam / 10f)), true);
			}
			if (fame.Val >= 80 && !fame80)
			{
				HUD.ShowNotification("Stai morendo di fame! Se continui così rischi di morire!.", NotificationColor.Red, true);
				fame80 = true;
				playerPed.Health -= 5;
				playerPed.MovementAnimationSet = "move_injured_generic";
			}
			if (fame.Val == 100 && !fame100)
			{
				HUD.ShowNotification("Stai morendo di fame!", NotificationColor.Red, true);
				fame100 = true;
				Client.Instance.AddTick(FameSete);
			}
		}

		private static void Sete(Ped playerPed, Player me, Necessità sete)
		{
			if (!playerPed.IsAiming && (playerPed.IsRunning || playerPed.IsSwimming || playerPed.IsJumping))
				sete.ChangeVal = 0.035f;
			else if (!playerPed.IsAiming && (playerPed.IsSwimmingUnderWater || playerPed.IsSprinting))
				sete.ChangeVal = 0.055f;
			else if (playerPed.IsInMeleeCombat)
				sete.ChangeVal = 0.033f;
			else
				sete.ChangeVal = 0.006f;
			sete.Val += sete.ChangeVal;


			if (sete.Val < 20.0f && (fame20 || fame60 || fame80 || fame100))
			{
				StatSetInt(Funzioni.HashUint("MP0_STAMINA"), (int)(Eventi.Player.CurrentChar.statistiche.STAMINA), true);
				fame20 = false;
				fame60 = false;
				fame80 = false;
				fame100 = false;
			}
			if (sete.Val >= 20f && !sete20)
			{
				HUD.ShowNotification("Hai la gola un po' secca... ti andrebbe una bibita.", NotificationColor.GreenDark, true);
				sete20 = true;
			}
			if (sete.Val >= 60 && !sete60)
			{
				HUD.ShowNotification("Hai sete! forse dovresti bere qualcosa!", NotificationColor.Yellow, true);
				sete60 = true;
				int stam = 0;
				StatGetInt(Funzioni.HashUint("MP0_STAMINA"), ref stam, -1);
				StatSetInt(Funzioni.HashUint("MP0_STAMINA"), (int)(stam - (stam / 10f)), true);
			}
			if (sete.Val >= 80 && !sete80)
			{
				HUD.ShowNotification("Stai morendo di sete! Se continui così rischi di morire!.", NotificationColor.Red, true);
				playerPed.Health -= 5;
				playerPed.MovementAnimationSet = "move_injured_generic";
				sete80 = true;
			}
			if (sete.Val == 100 && !sete100)
			{
				HUD.ShowNotification("Stai morendo di sete!", NotificationColor.Red, true);
				sete100 = true;
				Client.Instance.AddTick(FameSete);
			}
		}

		private static void Stanchezza(Ped playerPed, Player me, Necessità stanchezza)
		{
			if (!playerPed.IsAiming && (playerPed.IsRunning || playerPed.IsSwimming || playerPed.IsJumping))
				stanchezza.ChangeVal = 0.0225f;
			else if (!playerPed.IsAiming && (playerPed.IsSwimmingUnderWater || playerPed.IsSprinting))
				stanchezza.ChangeVal = 0.035f;
			else if (playerPed.IsInMeleeCombat)
				stanchezza.ChangeVal = 0.0285f;
			else
				stanchezza.ChangeVal = 0.0062f;
			stanchezza.Val += stanchezza.ChangeVal;
			if (World.CurrentDayTime.Hours >= 18 || World.CurrentDayTime.Hours <= 6)
				stanchezza.Val += 0.03f;


			if (stanchezza.Val < 20.0f && (stanchezza20 || stanchezza40 || stanchezza60))
			{
					StatSetInt(Funzioni.HashUint("MP0_STAMINA"), (int)(Eventi.Player.CurrentChar.statistiche.STAMINA), true);
					StatSetInt(Funzioni.HashUint("MP0_SHOOTING_ABILITY"), (int)(Eventi.Player.CurrentChar.statistiche.SHOOTING_ABILITY), true);
					stanchezza20 = false;
					stanchezza40 = false;
					stanchezza60 = false;
					//stanchezza80 = false;
					//stanchezza100 = false;
			}
			if (stanchezza.Val >= 20f && !stanchezza20)
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
			if (stanchezza.Val >= 40.0f && !stanchezza40)
			{
					StatSetInt(Funzioni.HashUint("MP0_STAMINA"), 1, true);
					StatSetInt(Funzioni.HashUint("MP0_SHOOTING_ABILITY"), 1, true);
					stanchezza40 = true;
			}
			if (stanchezza.Val >= 60.0f && !stanchezza60)
			{
					SetPlayerSprint(PlayerId(), false);
					stanchezza60 = true;
			}
			if (stanchezza.Val >= 80.0f)
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
			}
		}
		#endregion

		#region skills
		public static void Stamina(Ped playerPed, Player me, Statistica stam)
		{
			int baseStat = (int)Eventi.Player.CurrentChar.statistiche.STAMINA;

			stam.ChangeVal = !playerPed.IsInVehicle() ? playerPed.IsSprinting || playerPed.IsSwimmingUnderWater ? 0.002f :
				playerPed.IsRunning || playerPed.IsSwimming ? 0.001f : 0f : playerPed.CurrentVehicle.Model.IsBicycle ? 0.003f : 0f;


			stam.Val += stam.ChangeVal;

			if (stam.Val - baseStat >= 1f)
			{
				Eventi.Player.CurrentChar.statistiche.STAMINA = stam.Val;
				StatSetInt(Funzioni.HashUint("MP0_STAMINA"), (int)stam.Val, true);
				stam.ShowStatNotification();
			}
		}

		public static void Strenght(Ped playerPed, Player me, Statistica strenght)
		{
			int baseStat = (int)Eventi.Player.CurrentChar.statistiche.STRENGTH;

			strenght.ChangeVal = playerPed.IsInMeleeCombat ? 0.002f : 0f;

			strenght.Val += strenght.ChangeVal;

			if (strenght.Val - baseStat >= 1f)
			{
				Eventi.Player.CurrentChar.statistiche.STRENGTH = strenght.Val;
				StatSetInt(Funzioni.HashUint("MP0_STRENGTH"), (int)strenght.Val, true);
				strenght.ShowStatNotification();

			}
		}

		public static void Flying(Ped playerPed, Player me, Statistica flying)
		{
			int baseStat = (int)Eventi.Player.CurrentChar.statistiche.FLYING_ABILITY;

			// solo se in aereo
			if (playerPed.IsInPlane || playerPed.IsInHeli)
			{
				flying.ChangeVal = playerPed.SeatIndex == VehicleSeat.Driver && playerPed.CurrentVehicle.HeightAboveGround >= 15
					? 0.002f
					: 0f;
				flying.Val += flying.ChangeVal;
			}

			if (flying.Val - baseStat >= 1f)
			{
				Eventi.Player.CurrentChar.statistiche.FLYING_ABILITY = flying.Val;
				StatSetInt(Funzioni.HashUint("MP0_FLYING_ABILITY"), (int)flying.Val, true);
				flying.ShowStatNotification();

			}
		}

		public static void Lung(Ped playerPed, Player me, Statistica lung)
		{
			int baseStat = (int)Eventi.Player.CurrentChar.statistiche.LUNG_CAPACITY;

			// solo se è in acqua
			if (playerPed.IsInWater)
			{
				lung.ChangeVal = playerPed.IsSwimmingUnderWater ? 0.002f : 0f;
				lung.Val += lung.ChangeVal;
			}

			if (lung.Val - baseStat >= 1f)
			{
				Eventi.Player.CurrentChar.statistiche.LUNG_CAPACITY = lung.Val;
				StatSetInt(Funzioni.HashUint("MP0_LUNG_CAPACITY"), (int)lung.Val, true);
				lung.ShowStatNotification();

			}
		}

		public static void Driving(Ped playerPed, Player me, Statistica driving)
		{
			int baseStat = (int)Eventi.Player.CurrentChar.statistiche.WHEELIE_ABILITY;

			if (playerPed.IsInVehicle() && playerPed.SeatIndex == VehicleSeat.Driver)
			{
				if (playerPed.CurrentVehicle.Model.IsVehicle || playerPed.CurrentVehicle.Model.IsBike || playerPed.CurrentVehicle.Model.IsBoat || playerPed.CurrentVehicle.Model.IsQuadbike)
				{
					float maxVal = 0;
					float midVal = 0;
					float minVal = 0;
					switch (playerPed.CurrentVehicle.ClassType)
					{
						case VehicleClass.Commercial:
						case VehicleClass.Emergency:
						case VehicleClass.Industrial:
						case VehicleClass.Military:
						case VehicleClass.Service:
							maxVal = 0.004f;
							midVal = 0.003f;
							minVal = 0.002f;
							break;
						case VehicleClass.Compacts:
						case VehicleClass.SUVs:
						case VehicleClass.OffRoad:
						case VehicleClass.Vans:
						case VehicleClass.Utility:
						case VehicleClass.Boats:
							maxVal = 0.002f;
							midVal = 0.0015f;
							minVal = 0.0010f;
							break;
						case VehicleClass.Coupes:
						case VehicleClass.Motorcycles:
						case VehicleClass.Muscle:
						case VehicleClass.Sedans:
							maxVal = 0.0035f;
							midVal = 0.0020f;
							minVal = 0.0015f;
							break;
						case VehicleClass.SportsClassics:
						case VehicleClass.Sports:
							maxVal = 0.004f;
							midVal = 0.0035f;
							minVal = 0.003f;
							break;
						case VehicleClass.Super:
							maxVal = 0.005f;
							midVal = 0.0045f;
							minVal = 0.003f;
							break;
					}
					float speed = playerPed.CurrentVehicle.Speed * 3.6f;
					// altavelocita? --> velocità media? --> velocità minima?
					driving.ChangeVal = (speed > 150) ? maxVal : (speed > 90f && speed < 150f) ? midVal : (speed < 90f && speed > 30f) ? minVal : 0f;
				}
				// la skill viene gestita solo se siamo in macchina
				driving.Val += driving.ChangeVal;
			}

			if (driving.Val - baseStat >= 1f)
			{
				Eventi.Player.CurrentChar.statistiche.WHEELIE_ABILITY = driving.Val;
				StatSetInt(Funzioni.HashUint("MP0_WHEELIE_ABILITY"), (int)driving.Val, true);
				driving.ShowStatNotification();

			}
		}

		public static void Shooting(Ped playerPed, Player me, Statistica shoot)
		{
			int baseStat = (int)Eventi.Player.CurrentChar.statistiche.SHOOTING_ABILITY;

			// GESTIRE SPARATORIE

			if (shoot.Val - baseStat >= 1f)
			{
				Eventi.Player.CurrentChar.statistiche.LUNG_CAPACITY = shoot.Val;
				StatSetInt(Funzioni.HashUint("MP0_LUNG_CAPACITY"), (int)shoot.Val, true);
				shoot.ShowStatNotification();

			}
		}
		private static void Pescatore(Ped playerPed, Player me, Statistica pesca)
		{
			int baseStat = (int)Eventi.Player.CurrentChar.statistiche.FISHING;
			if (Lavori.Generici.Pescatore.PescatoreClient.Pescando)
			{
				pesca.ChangeVal = 0.003f;
				pesca.Val += pesca.ChangeVal;
			}
			else pesca.ChangeVal = 0;

			if (pesca.Val - baseStat >= 1f)
			{
				Eventi.Player.CurrentChar.statistiche.FISHING = pesca.Val;
				pesca.ShowStatNotification();
			}

		}
		private static void Cacciatore(Ped playerPed, Player me, Statistica caccia)
		{
			int baseStat = (int)Eventi.Player.CurrentChar.statistiche.HUNTING;

			if (caccia.Val - baseStat >= 1f)
			{
				Eventi.Player.CurrentChar.statistiche.HUNTING = caccia.Val;
				caccia.ShowStatNotification();
			}

		}
		private static void Droga(Ped playerPed, Player me, Statistica droga)
		{
			int baseStat = (int)Eventi.Player.CurrentChar.statistiche.DRUGS;

			if (droga.Val - baseStat >= 1f)
			{
				Eventi.Player.CurrentChar.statistiche.DRUGS = droga.Val;
				droga.ShowStatNotification();
			}

		}
		#endregion
	}
}
