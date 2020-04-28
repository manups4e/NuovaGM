using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.Personale;
using System.Linq;
using NuovaGM.Shared;
using Logger;
using System.Drawing;

namespace NuovaGM.Client.Veicoli
{
	public static class FuelClient
	{
		public static bool curVehInit = false;
		public static Vehicle LastVehicle = new Vehicle(0);
		public static bool justPumped = false;
		public static bool fuelChecked = false;
		public static float FuelCapacity;
		public static float FuelRpmImpact;
		public static float FuelAccelImpact;
		public static float FuelTractionImpact;
		public static float addedFuel = 0.0f;
		public static int lastStation = 0;
		private static readonly string DecorName = "lprp_fuel";
		public static Vehicle jobTruck = new Vehicle(0);
		public static Vehicle jobTrailer = new Vehicle(0);
		public static Vehicle lastVehicle = new Vehicle(0);
		public static List<Blip> gsblips = new List<Blip>();
		public static int animState = 3;
		public static List<Blip> registryBlips = new List<Blip>();
		public static bool canRegisterForTanker = true;
		public static bool canPickupTanker = true;
		public static bool canUnloadFuel = true;
		public static int curRegPickup = 0;
		public static List<Blip> pickupBlip = new List<Blip>();
		public static bool hasTanker = false;
		public static int tankerfuel = 0;
		public static List<Blip> refuelBlips = new List<Blip>();
		public static List<string> trucks;
		public static string tanker;
		public static int maxtankerfuel;
		public static float refuelCost;
		public static bool canBuyFuel = true;
		public static bool distwarn = false;
		public static bool avviso1 = false;

		public static List<Vector3> registrySpots = new List<Vector3>()
		{
			new Vector3(42.683f, -2634.747f, 6.037f)
		};
		public static List<Vector3> refuelspots = new List<Vector3>()
		{
			new Vector3(75.688f, -2645.164f, 6.012f),
			new Vector3(1492.691f, -1939.486f, 70.837f)
		};

		public static List<Tanker> tankerSpots = new List<Tanker>()
		{
			new Tanker(new Vector3(-284.762f, 6054.931f, 31.515f), 43.926f, new Vector3(-277.335f, 6047.611f, 31.552f), 40.544f, 0.75f, 2100),
			new Tanker(new Vector3(113.258f, -2644.929f, 6.011f), 171.418f, new Vector3(114.388f, -2637.694f, 6.025f), 170.051f, 0.6f, 1100),
			new Tanker(new Vector3(342.647f, 3412.522f, 36.597f), 109.447f, new Vector3(350.287f, 3415.289f, 36.479f), 109.447f, 0.75f, 2600)
		};

		public static StazioneSingola lastStationFuel = new StazioneSingola();

		public static void Init()
		{
			Client.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action(FuelSpawn));
			Client.Instance.AddEventHandler("lprp:fuel:checkfuelforstation", new Action<int, int>(checkfuel));
			Client.Instance.AddEventHandler("lprp:fuel:addfueltovehicle", new Action<bool, string, int>(AddFuelToVeh));
			Client.Instance.AddEventHandler("lprp:fuel:fillFuel", new Action(FillFuel));
			Client.Instance.AddEventHandler("lprp:fuel:fuelLevel", new Action<float>(FuelLevel));
			Client.Instance.AddEventHandler("frfuel:filltankForVeh", new Action<int>(FillTankForVeh));
			Client.Instance.AddEventHandler("lprp:fuel:depositfuel", new Action<bool, string, string, string>(DepositFuel));
			Client.Instance.AddEventHandler("lprp:fuel:depositfuelnotowned", new Action<bool, string, string, string, string>(DepositFuelNotOwned));
			Client.Instance.AddEventHandler("lprp:fuel:buytanker", new Action<bool, string>(BuyTanker));
			Client.Instance.AddEventHandler("lprp:fuel:stationnotowned", new Action(StationNotOwned));
			Client.Instance.AddEventHandler("lprp:fuel:buyfuelfortanker", new Action<bool, string>(BuyFuelForTanker));
			Client.Instance.AddEventHandler("lprp:fuel:settankerfuel", new Action<int>(SetTankerFuel));
			Client.Instance.AddEventHandler("lprp:fuel:saddfuel", new Action<int>(SAddFuel));
			Client.Instance.AddEventHandler("lprp:fuel:saddmoney", new Action<int>(SAddMoney));
			Client.Instance.AddEventHandler("lprp:fuel:sresetmanage", new Action<int>(SResetManage));
			Client.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action(Spawnato));
		}

		public static void Spawnato()
		{
			FuelCapacity = Client.Impostazioni.Veicoli.DanniVeicoli.FuelCapacity;
			FuelRpmImpact = Client.Impostazioni.Veicoli.DanniVeicoli.FuelRpmImpact;
			FuelAccelImpact = Client.Impostazioni.Veicoli.DanniVeicoli.FuelAccelImpact;
			FuelTractionImpact = Client.Impostazioni.Veicoli.DanniVeicoli.FuelTractionImpact;
			trucks = Client.Impostazioni.Veicoli.DanniVeicoli.trucks;
			tanker = Client.Impostazioni.Veicoli.DanniVeicoli.tanker;
			maxtankerfuel = Client.Impostazioni.Veicoli.DanniVeicoli.maxtankerfuel;
			refuelCost = Client.Impostazioni.Veicoli.DanniVeicoli.refuelCost;
		}

		public static string getRandomPlate()
		{
			List<char> charset = new List<char>();
			Random random = new Random(GetGameTimer());
			for (int i = 65; i < 90; i++)
				charset.Add((char)i);

			for (int i = 97; i < 122; i++)
				charset.Add((char)i);

			string rndstr = "";
			for (int i = 1; i < 6; i++)
				rndstr += charset[random.Next(1, charset.Count)];

			return rndstr;
		}

		public static void checkfuel(int fuel, int price)
		{
			lastStationFuel = new StazioneSingola(fuel, price);
		}

		public static void FuelSpawn()
		{
			CaricaBlipStazioni();
		}

		public static void CaricaBlipStazioni()
		{
			foreach (GasStation p in ConfigShared.SharedConfig.Main.Veicoli.gasstations)
			{
				Blip blip = new Blip(AddBlipForCoord(p.pos[0], p.pos[1], p.pos[2]))
				{
					Sprite = BlipSprite.JerryCan,
					Scale = 0.85f,
					IsShortRange = true,
					Name = "Stazione di Rifornimento"
				};
				SetBlipDisplay(blip.Handle, 4);
				gsblips.Add(blip);
			}

			foreach (Vector3 v in registrySpots)
			{
				Blip blip = new Blip(AddBlipForCoord(v.X, v.Y, v.Z))
				{
					Sprite = (BlipSprite)(67), // sennò (BlipSprite)(67)
					Scale = 0.88f,
					IsShortRange = true,
					Name = "Registrazione Cisterna"
				};
				SetBlipColour(blip.Handle, 17);
				SetBlipDisplay(blip.Handle, 4);
				registryBlips.Add(blip);
			}
		}

		public static void ShowRefuelBlips()
		{
			foreach (Vector3 v in refuelspots)
			{
				Blip blip = new Blip(AddBlipForCoord(v.X, v.Y, v.Z))
				{
					Sprite = BlipSprite.JerryCan,
					Scale = 0.85f,
					Color = (BlipColor)35,
					IsShortRange = true,
					Name = "Stazione rifornimento Cisterne"
				};
				SetBlipDisplay(blip.Handle, 4);
				refuelBlips.Add(blip);
			}
		}

		public static void HideRefuelBlips()
		{
			for (int i = 0; i < refuelBlips.Count; i++)
				refuelBlips[i].Delete();
			refuelBlips.Clear();
		}

		public static float RandomFuelLevel(float cap)
		{
			float min = cap / 3.0f;
			float max = cap - (cap / 4);
			return (new Random(GetGameTimer()).NextFloat() * (max - min)) + min;
		}

		public static void SetVehicleFuelLevel(this Vehicle veh, float fuel)
		{
			float maxfuel = Client.Impostazioni.Veicoli.DanniVeicoli.FuelCapacity;
			if (fuel > maxfuel) fuel = maxfuel;

			veh.SetDecor(DecorName, fuel);
			veh.FuelLevel = veh.GetDecor<float>(DecorName);
		}

		public static void AddFuelToVeh(bool success, string msg, int fuelval)
		{
			if (success)
			{
				SetVehicleFuelLevel(LastVehicle, fuelval);
				if(LastVehicle.PassengerCount > 0)
					LastVehicle.Passengers.ToList().ForEach(x => SetVehicleFuelLevel(x.CurrentVehicle, fuelval));
			}
			if (msg != null)
				HUD.ShowNotification(msg, true);
		}

		public static void initFuel(Vehicle veh)
		{
			curVehInit = true;
			float fuelCapacity = Client.Impostazioni.Veicoli.DanniVeicoli.FuelCapacity;
			if (!veh.HasDecor(DecorName))
				veh.SetDecor(DecorName, RandomFuelLevel(fuelCapacity));
			veh.FuelLevel = veh.GetDecor<float>(DecorName);
		}

		public static bool modelValid(Vehicle veh)
		{
			return veh.Model.IsBike | veh.Model.IsCar | veh.Model.IsQuadbike;
		}

		public static float vehicleFuelLevel(Vehicle veh)
		{
			return veh.HasDecor(DecorName) ? veh.GetDecor<float>(DecorName) : Client.Impostazioni.Veicoli.DanniVeicoli.FuelCapacity;
		}

		public static void ConsumeFuel(Vehicle veh)
		{
			float fuel = vehicleFuelLevel(veh);
			if (fuel > 0 && veh.IsEngineRunning)
			{
				float rpm = (float)Math.Pow(veh.CurrentRPM, 2.5f);
				fuel -= rpm * FuelRpmImpact;
				fuel -= veh.Acceleration * FuelAccelImpact;
				fuel -= veh.MaxTraction * FuelTractionImpact;
				if (fuel < 0.0f)
					fuel = 0f;
				SetVehicleFuelLevel(veh, fuel);
			}
		}

		public static bool withinDist(Vector3 pos, Entity ent)
		{
			Vector3 epos = ent.Position;
			float dist = World.GetDistance(pos, epos);
			return dist <= 2.05f ? true : false;
		}

		public static async void spawnTanker(Tanker spot, string plate)
		{
			Random random = new Random();
			string rnd = trucks[random.Next(trucks.Count)];

			Model truck = new Model(rnd);
			if (!truck.IsLoaded)
				await truck.Request(3000); // for when you stream resources.

			Model trailer = new Model(tanker);
			if (!trailer.IsLoaded)
				await trailer.Request(3000); // for when you stream resources.

			jobTruck = await World.CreateVehicle(truck, spot.pos, spot.heading);
			jobTruck.PlaceOnGround();
			Game.PlayerPed.SetIntoVehicle(jobTruck, VehicleSeat.Driver);
			SetVehicleNumberPlateText(jobTruck.Handle, plate);
			BaseScript.TriggerEvent("frfuel:filltankForVeh", jobTruck.Handle);
			string plat = GetVehicleNumberPlateText(jobTruck.Handle).ToLower();
			BaseScript.TriggerServerEvent("lprp:vehicles:registerJobVehicle", plat);

			jobTrailer = await World.CreateVehicle(trailer, spot.t, spot.theading);
			jobTrailer.PlaceOnGround();
			SetVehicleNumberPlateText(jobTrailer.Handle, plate);
			AttachVehicleToTrailer(jobTruck.Handle, jobTrailer.Handle, 10.0f);
			hasTanker = true;
		}

		public static void FillFuel()
		{
			if (Game.PlayerPed.IsInVehicle())
			{
				SetVehicleFuelLevel(Game.PlayerPed.CurrentVehicle, Client.Impostazioni.Veicoli.DanniVeicoli.FuelCapacity);
				HUD.ShowNotification("Il tuo carburante è stato riempito. Usalo SOLO in caso di ~r~EMERGENZE~w~!");
			}
		}

		public static void FuelLevel(float level)
		{
			if (level > Client.Impostazioni.Veicoli.DanniVeicoli.FuelCapacity)
				level = Client.Impostazioni.Veicoli.DanniVeicoli.FuelCapacity;
			else if (level < 0f)
				level = 0f;
			SetVehicleFuelLevel(Game.PlayerPed.CurrentVehicle, level);
			HUD.ShowNotification("Carburante settato, Usalo SOLO in caso di ~r~EMERGENZA~w~!");
		}

		public static void FillTankForVeh(int veh)
		{
			SetVehicleFuelLevel(new Vehicle(veh), Client.Impostazioni.Veicoli.DanniVeicoli.FuelCapacity);
		}

		public static void DepositFuel(bool success, string tankerful, string stationfuel, string overflow)
		{
			if (success)
			{
				int over = Convert.ToInt32(overflow);
				int stationf = Convert.ToInt32(stationfuel);
				if (over > 0)
					tankerfuel = over;
				else
					tankerfuel = 0;

				HUD.ShowNotification($"Hai consegnato ~b~{tankerful}~w~ litri di carburante. La stazione ora ha ~b~{stationf}~b~ litri.\nLa tua cisterna ha ~b~{tankerfuel}~w~ litri di carburante rimanenti.");
			}
			else
				HUD.ShowNotification(tankerful);

			canUnloadFuel = true;
			ShowRefuelBlips();
		}

		public static void DepositFuelNotOwned(bool success, string tankerful, string stationfuel, string pay, string overflow)
		{
			if (success)
			{
				int over = Convert.ToInt32(overflow);
				int pai = Convert.ToInt32(pay);

				if (over > 0)
					tankerfuel = over;
				else
					tankerfuel = 0;

				if (pai == 0)
					HUD.ShowNotification($"Hai consegnato ~b~{tankerful}~w~ litri di carburante. La stazione ora ha ~b~{stationfuel}~w~ litri di carburante.\nLa tua cisterna ha ~b~{tankerfuel}~w~ litri di carburante rimanenti.");
				else
					HUD.ShowNotification($"Hai consegnato ~b~{tankerful}~w~ litri di carburante. La stazione ora ha ~b~{stationfuel}~w~ litri di carburante.\nSei stato pagato ~g~{pay}$~w~ per la consegna.<br><br>La tua cisterna ha ~b~{tankerfuel}~w~ litri di carburante rimanenti.");
			}
			else
				HUD.ShowNotification(tankerful);
			canUnloadFuel = true;
			ShowRefuelBlips();
		}

		public static void BuyTanker(bool success, string JSON)
		{
			if (success)
			{
				Tanker t = JsonConvert.DeserializeObject<Tanker>(JSON);
				tankerfuel = t.fuelForTanker;
				spawnTanker(t, getRandomPlate());
				HUD.ShowNotification("Hai comprato una cisterna piena di carburante.");
			}
			else
			{
				HUD.ShowNotification(JSON);
				canPickupTanker = true;
			}
			if (pickupBlip.Count > 0)
			{
				foreach (Blip b in pickupBlip)
					b.Delete();

				pickupBlip.Clear();
			}
		}

		public static void StationNotOwned()
		{
			canUnloadFuel = true;
			HUD.ShowNotification("Questa stazione non è di nessuno. Forse dovresti considerare la possibilità di acquistarla...");
		}

		public static void BuyFuelForTanker(bool success, string msg)
		{
			if (success)
			{
				tankerfuel += Convert.ToInt32(msg);
				HUD.ShowNotification("La cisterna è stata riempita di carburante.");
			}
			else
				HUD.ShowNotification(msg);
			canBuyFuel = true;
		}

		public static void SetTankerFuel(int level)
		{
			tankerfuel = level;
			HUD.ShowNotification("Cisterna di carburante aggiornata.");
		}

		public static void SAddFuel(int amount)
		{
			int cl = 0;
			for (int i = 0; i < ConfigShared.SharedConfig.Main.Veicoli.gasstations.Count; i++)
			{
				float dist = World.GetDistance(Game.PlayerPed.Position, ConfigShared.SharedConfig.Main.Veicoli.gasstations[i].pos.ToVector3());
				if (dist < 100)
					cl = i;
				if (cl > 0)
					BaseScript.TriggerServerEvent("lprp:businesses:saddfuel", cl, amount);
				else
					HUD.ShowNotification("Nessuna stazione di rifornimento nelle vicinanze.");
			}
		}

		public static void SAddMoney(int amount)
		{
			int cl = 0;
			for (int i = 0; i < ConfigShared.SharedConfig.Main.Veicoli.gasstations.Count; i++)
			{
				float dist = World.GetDistance(Game.PlayerPed.Position, ConfigShared.SharedConfig.Main.Veicoli.gasstations[i].pos.ToVector3());
				if (dist < 100)
					cl = i;
				if (cl > 0)
					BaseScript.TriggerServerEvent("lprp:businesses:saddmoney", cl, amount);
				else
					HUD.ShowNotification("Nessuna stazione di rifornimento nelle vicinanze.");
			}
		}

		public static void SResetManage(int amount)
		{
			int cl = 0;
			for (int i = 0; i < ConfigShared.SharedConfig.Main.Veicoli.gasstations.Count; i++)
			{
				float dist = World.GetDistance(Game.PlayerPed.Position, ConfigShared.SharedConfig.Main.Veicoli.gasstations[i].pos.ToVector3());
				if (dist < 100)
					cl = i;
				if (cl > 0)
					BaseScript.TriggerServerEvent("lprp:businesses:sresetmanage", cl);
				else
					HUD.ShowNotification("Nessuna stazione di rifornimento nelle vicinanze.");
			}
		}

		static Vehicle veh = new Vehicle(0);
		static Vehicle lastveh = new Vehicle(0);

		public static async Task FuelCount()
		{
			try
			{
				if (Game.PlayerPed.IsInVehicle())
				{
					if (Game.PlayerPed.CurrentVehicle.Driver == Game.PlayerPed)
						veh = Game.PlayerPed.CurrentVehicle;
					if (Game.PlayerPed.LastVehicle != null)
						lastveh = Game.PlayerPed.LastVehicle;
				}

				if (Game.PlayerPed.IsInVehicle() && veh.Driver == Game.PlayerPed && modelValid(veh) && !veh.IsDead)
				{
					if (LastVehicle != veh)
					{
						LastVehicle = veh;
						curVehInit = false;
					}
					if (!curVehInit)
						initFuel(veh);

					ConsumeFuel(veh);
					if (!EventiPersonalMenu.DoHideHud)
						HUD.DrawText(0.195f, 0.96f, $"Carburante: {(int)Math.Floor(veh.FuelLevel / FuelCapacity * 100)}%", Color.FromArgb(255, 135, 206, 250));
					if (vehicleFuelLevel(veh) < 0.99f)
					{
						veh.IsEngineRunning = false;
						veh.IsDriveable = false;
					}
				}
				else if(Game.PlayerPed.IsInVehicle() && veh.Driver != Game.PlayerPed && modelValid(veh) && !veh.IsDead)
				{
					veh.FuelLevel = veh.HasDecor(DecorName) ? veh.GetDecor<float>(DecorName) : Client.Impostazioni.Veicoli.DanniVeicoli.FuelCapacity;
					curVehInit = false;
				}

				if (veh.Exists() || lastveh.Exists())
				{
					for (int i = 0; i < ConfigShared.SharedConfig.Main.Veicoli.gasstations.Count; i++)
					{
						float dist = World.GetDistance(Game.PlayerPed.Position, ConfigShared.SharedConfig.Main.Veicoli.gasstations[i].pos.ToVector3());
						if (dist <= 80f)
						{
							lastStation = i + 1;
							if (fuelChecked == false && Game.PlayerPed.IsInVehicle())
							{
								fuelChecked = true;
								BaseScript.TriggerServerEvent("lprp:businesses:checkfuelforstation", lastStation);
							}
							for (int j = 0; j < ConfigShared.SharedConfig.Main.Veicoli.gasstations[i].pumps.Count; j++)
							{
								if (Game.PlayerPed.IsInVehicle())
								{
									if (veh.ClassType == VehicleClass.Industrial || lastveh.ClassType == VehicleClass.Industrial || veh.ClassType == VehicleClass.Commercial || lastveh.ClassType == VehicleClass.Commercial)
										World.DrawMarker(MarkerType.TruckSymbol, new Vector3(ConfigShared.SharedConfig.Main.Veicoli.gasstations[i].pumps[j].ToVector3().X, ConfigShared.SharedConfig.Main.Veicoli.gasstations[i].pumps[j].ToVector3().Y, ConfigShared.SharedConfig.Main.Veicoli.gasstations[i].pumps[j].ToVector3().Z + 1), new Vector3(0), new Vector3(0), new Vector3(2.0f, 2.0f, 1.8f), System.Drawing.Color.FromArgb(180, 255, 255, 0), false, false, true);
									else if (veh.Model.IsCar || lastveh.Model.IsCar)
										World.DrawMarker(MarkerType.CarSymbol, new Vector3(ConfigShared.SharedConfig.Main.Veicoli.gasstations[i].pumps[j].ToVector3().X, ConfigShared.SharedConfig.Main.Veicoli.gasstations[i].pumps[j].ToVector3().Y, ConfigShared.SharedConfig.Main.Veicoli.gasstations[i].pumps[j].ToVector3().Z + 1), new Vector3(0), new Vector3(0), new Vector3(2.0f, 2.0f, 1.8f), System.Drawing.Color.FromArgb(180, 255, 255, 0), false, false, true);
									else if (veh.Model.IsBike || lastveh.Model.IsBike)
										World.DrawMarker(MarkerType.BikeSymbol, new Vector3(ConfigShared.SharedConfig.Main.Veicoli.gasstations[i].pumps[j].ToVector3().X, ConfigShared.SharedConfig.Main.Veicoli.gasstations[i].pumps[j].ToVector3().Y, ConfigShared.SharedConfig.Main.Veicoli.gasstations[i].pumps[j].ToVector3().Z + 1), new Vector3(0), new Vector3(0), new Vector3(2.0f, 2.0f, 1.8f), System.Drawing.Color.FromArgb(180, 255, 255, 0), false, false, true);
								}

								float pdist = World.GetDistance(Game.PlayerPed.Position, ConfigShared.SharedConfig.Main.Veicoli.gasstations[i].pumps[j].ToVector3());

								if ((pdist < 3.05) && LastVehicle.Exists() && withinDist(Game.PlayerPed.Position, LastVehicle) && !Game.PlayerPed.IsInVehicle())
								{
									DisableControlAction(2, 22, true);
									if (lastStationFuel.stationfuel > 0)
									{
										int benz = lastStationFuel.stationfuel;
										HUD.ShowHelp("~w~Tieni Premuto ~INPUT_CONTEXT~ per riempire il ~b~serbatoio~w~.~n~Carburante: " + benz + " litri, Prezzo: $ " + lastStationFuel.stationprice + "/Litro");
										if (Input.IsControlPressed(Control.Context) || Input.IsDisabledControlPressed(Control.Context))
										{
											if (!LastVehicle.IsEngineRunning)
											{
												if (!IsEntityPlayingAnim(PlayerPedId(), "timetable@gardener@filling_can", "gar_ig_5_filling_can", 3))
												{
													TaskTurnPedToFaceEntity(PlayerPedId(), LastVehicle.Handle, 1000);
													await BaseScript.Delay(1000);
													await Game.PlayerPed.Task.PlayAnimation("timetable@gardener@filling_can", "gar_ig_5_filling_can", 2f, 8f, -1, (AnimationFlags)50, 0);
												}
												if (LastVehicle.FuelLevel < 100)
												{
													justPumped = true;
													float fuel = LastVehicle.FuelLevel;
													float maxfuel = Client.Impostazioni.Veicoli.DanniVeicoli.FuelCapacity;
													float afuel = fuel + addedFuel;
													if (afuel <= maxfuel)
													{
														addedFuel += 0.1f;
														HUD.DrawText(0.195f, 0.96f, $"Carburante: {(int)Math.Floor(fuel + addedFuel / maxfuel * 100)}%", Color.FromArgb(255, 135, 206, 250));
													}
												}
												else
													HUD.ShowNotification("Il tuo veicolo è pieno!", NotificationColor.Red, true);
											}
											else
												HUD.ShowNotification("Il motore del veicolo deve essere SPENTO", NotificationColor.Red, true);
										}
										if (Input.IsControlJustReleased(Control.Context) || Input.IsDisabledControlJustReleased(Control.Context))
										{
											if (IsEntityPlayingAnim(PlayerPedId(), "timetable@gardener@filling_can", "gar_ig_5_filling_can", 3))
												Game.PlayerPed.Task.ClearAll();
											if (justPumped)
											{
												justPumped = false;
												float fuel = vehicleFuelLevel(LastVehicle);
												BaseScript.TriggerServerEvent("lprp:fuel:payForFuel", i + 1, addedFuel, fuel + addedFuel);
												addedFuel = 0.0f;
											}
										}
									}
									else
										HUD.ShowNotification("Questa stazione di rifornimento è a secco di carburante. Provane un'altra.", true);
								}
							}
						}
						if (lastStation > 0)
						{
							float dista = World.GetDistance(Game.PlayerPed.Position, ConfigShared.SharedConfig.Main.Veicoli.gasstations[lastStation - 1].pos.ToVector3());
							if (dista > 80f)
							{
								lastStation = 0;
								fuelChecked = false;
							}
						}
					}
				}
				Weapon wep = Game.PlayerPed.Weapons.Current;
				if (wep.Hash == WeaponHash.PetrolCan && LastVehicle.Exists())
				{
					float dist = World.GetDistance(Game.PlayerPed.Position, LastVehicle.Position);
					if (dist < 2 && LastVehicle.HasDecor(DecorName))
					{
						float max = Client.Impostazioni.Veicoli.DanniVeicoli.FuelCapacity;
						float fuel = vehicleFuelLevel(lastVehicle);
						if (max - fuel < 0.5)
							HUD.ShowNotification("Il serbatoio è già pieno.");
						else
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per riempire il serbatoio con la tanica");
						if (Input.IsControlPressed( Control.Context))
						{
							if (animState == 3)
							{
								Game.PlayerPed.Task.PlayAnimation("weapon@w_sp_jerrycan", "fire_intro");
								animState = 1;
							}
							else if (animState == 1)
							{
								if (!IsEntityPlayingAnim(Game.PlayerPed.Handle, "weapon@w_sp_jerrycan", "fire_intro", 3))
								{
									Game.PlayerPed.Task.PlayAnimation("weapon@w_sp_jerrycan", "fire");
									animState = 2;
								}
							}
							if (fuel < max)
							{
								HUD.DrawText(0.195f, 0.96f, $"Carburante: {(int)Math.Floor(fuel / max * 100)}%", Color.FromArgb(255, 135, 206, 250));
								if (fuel + 0.1 >= max)
									SetVehicleFuelLevel(lastVehicle, max);
								else
									SetVehicleFuelLevel(lastVehicle, fuel + 0.2f);
							}
						}
						if (Input.IsControlJustReleased(Control.Context))
						{
							StopEntityAnim(Game.PlayerPed.Handle, "fire", "weapon@w_sp_jerrycan", 3);
							Game.PlayerPed.Task.PlayAnimation("weapon@w_sp_jerrycan", "fire_outro");
							animState = 3;
						}
					}
				}
				await Task.FromResult(0);
			}
			catch (Exception e)
			{
				Log.Printa(LogType.Error, "Errore in fuelClient = " + e);
			}
		}

		public static async Task FuelTruck()
		{
			if (jobTruck.Handle == 0)
			{
				for (int i = 0; i < registrySpots.Count; i++)
				{
					float dist = World.GetDistance(Game.PlayerPed.Position, registrySpots[i]);
					if (dist < 80)
					{
						World.DrawMarker(MarkerType.TruckSymbol, registrySpots[i], new Vector3(0), new Vector3(0), new Vector3(1.1f, 1.1f, 1.3f), System.Drawing.Color.FromArgb(170, 0, 0, 255), false, false, true);
						if (dist < 1.15)
						{
							if (canRegisterForTanker)
							{
								HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per registrare il prelevamento di una cisterna.");
								if (Input.IsControlJustPressed(Control.Context))
								{
									canRegisterForTanker = false;
									curRegPickup = Funzioni.GetRandomInt(1, tankerSpots.Count);
									SetNewWaypoint(tankerSpots[curRegPickup].pos.X, tankerSpots[curRegPickup].pos.Y);
									if (pickupBlip.Count > 0)
									{
										foreach (Blip a in pickupBlip)
											a.Delete();
										pickupBlip.Clear();
									}
									Blip b = new Blip(AddBlipForCoord(tankerSpots[curRegPickup].pos.X, tankerSpots[curRegPickup].pos.Y, tankerSpots[curRegPickup].pos.Z))
									{
										Sprite = (BlipSprite)67,
										Scale = 0.85f,
										Color = (BlipColor)14,
										IsShortRange = true,
										Name = "Raccolta Cisterna"
									};
									SetBlipDisplay(b.Handle, 4);
									pickupBlip.Add(b);
									HUD.ShowNotification("Il punto di raccolta della cisterna è stato impostato sul tuo ~b~GPS~w~.");
								}
							}
						}
					}
				}
			}

			if (curRegPickup > 0)
			{
				Vector3 spot = tankerSpots[curRegPickup].pos;
				World.DrawMarker(MarkerType.TruckSymbol, new Vector3(spot.X, spot.Y, spot.Z), new Vector3(0), new Vector3(0), new Vector3(2.1f, 2.1f, 1.3f), System.Drawing.Color.FromArgb(170, 0, 255, 0), false, false, true);
				float dist = World.GetDistance(Game.PlayerPed.Position, spot);
				if (dist < 2.1)
				{
					Tanker info = tankerSpots[curRegPickup];
					if (canPickupTanker)
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per prendere la cisterna. Ti costerà " + (info.ppu * info.fuelForTanker) + " per il carburante.");
						if (Input.IsControlJustPressed(Control.Context))
						{
							canPickupTanker = false;
							BaseScript.TriggerServerEvent("lprp:fuel:buytanker", JsonConvert.SerializeObject(info));
						}
					}
				}
			}
			if (jobTruck.Handle != 0 && jobTrailer.Handle != 0)
			{
				Vehicle vehicle = Game.PlayerPed.CurrentVehicle;
				if (vehicle == jobTruck && IsVehicleAttachedToTrailer(jobTruck.Handle))
				{
					HUD.DrawText(0.9f, 0.935f, $"Carburante Cisterna: {tankerfuel}", Color.FromArgb(255, 135, 206, 235));
					for (int i = 0; i < ConfigShared.SharedConfig.Main.Veicoli.gasstations.Count; i++)
					{
						float dis = World.GetDistance(Game.PlayerPed.Position, ConfigShared.SharedConfig.Main.Veicoli.gasstations[i].pos.ToVector3());
						if (dis < 80)
						{
							World.DrawMarker(MarkerType.VerticalCylinder, new Vector3(ConfigShared.SharedConfig.Main.Veicoli.gasstations[i].pos.ToVector3().X, ConfigShared.SharedConfig.Main.Veicoli.gasstations[i].pos.ToVector3().Y, ConfigShared.SharedConfig.Main.Veicoli.gasstations[i].pos.ToVector3().Z - 1.00001f), new Vector3(0), new Vector3(0), new Vector3(10.1f, 10.1f, 1.3f), System.Drawing.Color.FromArgb(170, 0, 255, 0));
							if (dis < 10.1)
							{
								if (tankerfuel > 0)
								{
									HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per scaricare il carburante nella stazione.");
									if (canUnloadFuel)
									{
										if (Input.IsControlJustPressed(Control.Context))
										{
											canUnloadFuel = false;
											BaseScript.TriggerServerEvent("lprp:businesses:depositfuel", i, tankerfuel);
										}
									}
								}
								else
								{
									HUD.ShowNotification("La tua cisterna è vuota. Puoi acquistare carburante alla Stazione Rifornimento delle Cisterne.", NotificationColor.Red, true);
									HUD.ShowHelp("Per smettere di lavorare, allontanati dal tuo veicolo.");
								}
							}
						}
					}
					for (int i = 0; i < refuelspots.Count; i++)
					{
						float di = World.GetDistance(Game.PlayerPed.Position, refuelspots[i]);
						if (di < 80)
						{
							World.DrawMarker(MarkerType.VerticalCylinder, new Vector3(refuelspots[i].X, refuelspots[i].Y, refuelspots[i].Z - 1.00001f), new Vector3(0), new Vector3(0), new Vector3(3.1f, 3.1f, 1.3f), System.Drawing.Color.FromArgb(170, 255, 165, 0));
							if (di < 3.1)
							{
								int fuel = 500;
								int maxfuel = maxtankerfuel;
								if ((tankerfuel + fuel) <= maxfuel)
									maxfuel = fuel;
								else
									maxfuel = maxtankerfuel - tankerfuel;

								if (maxfuel > 0)
								{
									HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per riempire la cisterna.~n~Ti costerà " + refuelCost * maxfuel + " per " + maxfuel + " litri di carburante.");
									if (canBuyFuel)
									{
										if (Input.IsControlJustPressed(Control.Context))
										{
											canBuyFuel = false;
											BaseScript.TriggerServerEvent("lprp:fuel:buyfuelfortanker", (int)(maxfuel * refuelCost), maxfuel);
										}
									}
								}
								else
									HUD.ShowNotification("La tua cisterna è gia piena!");
							}
						}
					}
				}
				else
				{
					if (!avviso1)
					{
						HUD.ShowNotification("Devi essere in un camion da lavoro e avere una cisterna carica attaccata per fare questo lavoro!");
						avviso1 = true;
					}
				}
				float dist = World.GetDistance(Game.PlayerPed.Position, jobTruck.Position);
				if (dist > 40f)
				{
					if (!distwarn)
					{
						distwarn = true;
						HUD.ShowNotification("Se ti allontani troppo dal tuo veicolo il contratto di lavoro verrà annullato.", NotificationColor.Yellow);
					}
					if (dist > 60f)
					{
						jobTruck.Delete();
						jobTrailer.Delete();
						jobTruck = new Vehicle(0);
						jobTrailer = new Vehicle(0);
						curRegPickup = 0;
						canRegisterForTanker = true;
						canPickupTanker = true;
						canUnloadFuel = true;
						distwarn = false;
						hasTanker = false;
						HideRefuelBlips();
						HUD.ShowNotification("Ti sei allontanato troppo dal tuo camion ed è stato rimorchiato.", NotificationColor.Red, true);
					}
				}
				if (hasTanker)
				{
					if (!jobTruck.Exists())
					{
						string plate = GetVehicleNumberPlateText(jobTruck.Handle);
						jobTruck.Delete();
						jobTrailer.Delete();
						jobTruck = new Vehicle(0);
						jobTrailer = new Vehicle(0);
						if (plate != "")
							BaseScript.TriggerServerEvent("lprp:vehicles:unregisterJobVehicle", plate);

						HUD.ShowNotification("Hai perso il tuo camion o la tua cisterna. La consegna è stata cancellata.", NotificationColor.Red);
						curRegPickup = 0;
						canRegisterForTanker = true;
						canPickupTanker = true;
						canUnloadFuel = true;
						hasTanker = false;
						HideRefuelBlips();
					}
				}
			}
			await Task.FromResult(0);
		}
	}

	public class StazioneSingola
	{
		public int stationfuel = 0;
		public int stationprice = 0;
		public StazioneSingola() { }
		public StazioneSingola(int fuel, int price)
		{
			stationfuel = fuel;
			stationprice = price;
		}
	}

	public class Tanker
	{
		public Vector3 pos;
		public float heading;
		public Vector3 t;
		public float theading;
		public float ppu;
		public int fuelForTanker;
		public Tanker(Vector3 p, float h, Vector3 t, float th, float ppu, int fuel)
		{
			pos = p;
			heading = h;
			this.t = t;
			theading = th;
			this.ppu = ppu;
			fuelForTanker = fuel;
		}
	}
}
