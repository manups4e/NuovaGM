using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.MenuNativo;
using TheLastPlanet.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLastPlanet.Client.Veicoli;
using System.Drawing;
using Impostazioni.Client.Configurazione.Lavori.Generici;
using TheLastPlanet.Client.Interactions;
using TheLastPlanet.Client.Core;
using TheLastPlanet.Client.Handlers;
using TheLastPlanet.Client.SessionCache;

namespace TheLastPlanet.Client.Lavori.Generici.Taxi
{
	internal static class TaxiClient
	{
		private static TaxiFlags jobs = new TaxiFlags();
		private static TaxiMeter taximeter = new TaxiMeter();
		private static Tassisti taxi;
		private static Vehicle VeicoloServizio;
		private static Ped NPCPasseggero;
		private static bool InServizio = false;

		public static void Init()
		{
			taxi = Client.Impostazioni.Lavori.Generici.Tassista;
			Client.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action(Eccolo));
			TickController.TickAPiedi.Add(Markers);
		}

		private static void Eccolo()
		{
			Blip Tax = World.CreateBlip(taxi.PosAccettazione);
			Tax.Sprite = BlipSprite.PersonalVehicleCar;
			Tax.Color = BlipColor.Yellow;
			Tax.Name = "Taxi";
			Tax.IsShortRange = true;
			SetBlipDisplay(Tax.Handle, 4);
		}

		private static async Task TaximeterTick()
		{
			if (InServizio)
			{
				taximeter.CreateTaxiMeter(VeicoloServizio);
				taximeter.RenderMeter();
			}
		}

		public static async Task Markers()
		{
			Ped p = Cache.MyPlayer.Ped;

			if (p.IsInRangeOf(taxi.PosAccettazione, 100))
			{
				World.DrawMarker(MarkerType.ChevronUpx2, taxi.PosAccettazione, Vector3.Zero, Vector3.Zero, new Vector3(1.5f), Colors.Yellow, rotateY: true);

				if (p.IsInRangeOf(taxi.PosAccettazione, 1.375f))
				{
					if (Cache.MyPlayer.User.CurrentChar.Job.Name.ToLower() != "taxi")
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per accettare il lavoro da tassista.");

						if (Input.IsControlJustPressed(Control.Context))
						{
							Job tass = new Job("Taxi", 0);
							//BaseScript.TriggerServerEvent("lprp:updateCurChar", "job", tass.ToJson());
							Cache.MyPlayer.User.CurrentChar.Job = tass;
						}
					}
					else
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per smettere di lavorare.");

						if (Input.IsControlJustPressed(Control.Context))
						{
							Job disoc = new Job("Disoccupato", 0);
							//BaseScript.TriggerServerEvent("lprp:updateCurChar", "job", disoc.ToJson());
							Cache.MyPlayer.User.CurrentChar.Job = disoc;
						}
					}
				}
			}

			if (Cache.MyPlayer.User.CurrentChar.Job.Name.ToLower() == "taxi")
			{
				if (p.IsInRangeOf(taxi.PosRitiroVeicolo, 100))
					if (VeicoloServizio == null || VeicoloServizio != null && !VeicoloServizio.Exists() || VeicoloServizio.IsDead)
					{
						World.DrawMarker(MarkerType.CarSymbol, taxi.PosRitiroVeicolo, Vector3.Zero, Vector3.Zero, new Vector3(1.5f), Colors.Yellow, rotateY: true);

						if (p.IsInRangeOf(taxi.PosRitiroVeicolo, 1.375f))
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per prendere il tuo veicolo di servizio.");

							if (Input.IsControlJustPressed(Control.Context))
							{
								if (p.IsVisible) NetworkFadeOutEntity(p.Handle, true, false);
								Screen.Fading.FadeOut(800);
								await BaseScript.Delay(1000);
								VeicoloServizio = await Funzioni.SpawnVehicle("taxi", taxi.PosSpawnVeicolo.ToVector3(), taxi.PosSpawnVeicolo.W);
								VeicoloServizio.SetVehicleFuelLevel(100f);
								VeicoloServizio.Mods.LicensePlate = "TAXI_" + Funzioni.GetRandomInt(000, 999).ToString("000");
								VeicoloServizio.IsEngineRunning = true;
								VeicoloServizio.IsDriveable = true;
								NetworkFadeInEntity(PlayerPedId(), true);
								Screen.Fading.FadeIn(500);
							}
						}
					}

				if (p.IsInRangeOf(taxi.PosDepositoVeicolo, 100))
					if (VeicoloServizio != null && !VeicoloServizio.IsDead && VeicoloServizio.Exists())
					{
						World.DrawMarker(MarkerType.CarSymbol, taxi.PosDepositoVeicolo, Vector3.Zero, Vector3.Zero, new Vector3(1.5f), Colors.Red, rotateY: true);

						if (p.IsInRangeOf(taxi.PosDepositoVeicolo, 1.375f))
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per parcheggiare il veicolo di servizio.");

							if (Input.IsControlJustPressed(Control.Context))
							{
								if (p.CurrentVehicle.IsVisible) NetworkFadeOutEntity(p.CurrentVehicle.Handle, true, false);
								Screen.Fading.FadeOut(800);
								await BaseScript.Delay(1000);
								taximeter.meter_entity.Delete();
								VeicoloServizio.Delete();
								VeicoloServizio = null;
								p.Position = taxi.PosRitiroVeicolo;
								NetworkFadeInEntity(PlayerPedId(), true);
								Screen.Fading.FadeIn(500);
								if (InServizio) VaiFuoriServizio(1);
							}
						}
					}

				if (Input.IsControlJustPressed(Control.SelectCharacterFranklin, PadCheck.Keyboard))
				{
					if (VeicoloServizio != null && VeicoloServizio.IsAlive && VeicoloServizio.Exists())
					{
						SetTaxiLights(VeicoloServizio.Handle, InServizio);
						InServizio = !InServizio;

						if (InServizio)
						{
							taximeter.Taximeter = new Scaleform("taxi_display");
							while (!taximeter.Taximeter.IsLoaded) await BaseScript.Delay(0);
							taximeter.meter_rt = RenderTargets.CreateNamedRenderTargetForModel("taxi", (uint)GetHashKey("prop_taxi_meter_2"));
							Client.Instance.AddTick(ServizioTaxi);
							HUD.ShowAdvancedNotification("Centralino tassisti", "Messaggio all'autista", "Sei entrato in servizio. Guida per le strade in cerca di clienti.", NotificationIcon.Taxi, IconType.ChatBox);
							if (p.IsInVehicle(VeicoloServizio)) Client.Instance.AddTick(TaximeterTick);
						}
						else
						{
							Client.Instance.RemoveTick(ServizioTaxi);
							HUD.ShowAdvancedNotification("Centralino tassisti", "Messaggio all'autista", "Sei uscito dal servizio.", NotificationIcon.Taxi, IconType.ChatBox);
							VaiFuoriServizio(1);
						}
					}
					else
					{
						HUD.ShowAdvancedNotification("Centralino tassisti", "Messaggio all'autista", "Non puoi entrare in servizio senza il tuo veicolo da lavoro!.", NotificationIcon.Taxi, IconType.ChatBox);
					}
				}
			}
		}

		private static async Task ServizioTaxi()
		{
			Ped p = Cache.MyPlayer.Ped;

			if (InServizio && jobs.onJob == 0)
			{
				if (p.IsInVehicle(VeicoloServizio))
					if (p.CurrentVehicle.Driver == Cache.MyPlayer.Ped)
					{
						jobs.flag[0] = 0;
						jobs.flag[1] = 59 + Funzioni.GetRandomInt(1, 61);
						jobs.onJob = 1;
					}
			}
			else if (jobs.onJob == 1 && InServizio)
			{
				if (VeicoloServizio.Exists() && VeicoloServizio.IsDriveable)
				{
					if (p.IsSittingInVehicle(VeicoloServizio))
					{
						if (NPCPasseggero != null && NPCPasseggero.Exists())
						{
							if (IsPedFatallyInjured(NPCPasseggero.Handle))
							{
								if (NPCPasseggero.AttachedBlip != null && NPCPasseggero.AttachedBlip.Exists()) NPCPasseggero.AttachedBlip.Delete();
								NPCPasseggero.MarkAsNoLongerNeeded();
								NPCPasseggero = null;
								jobs.flag[0] = 0;
								jobs.flag[1] = 59 + Funzioni.GetRandomInt(1, 61);

								if (jobs.blip != null && jobs.blip.Exists())
								{
									jobs.blip.Delete();
									jobs.blip = null;
								}

								HUD.ShowAdvancedNotification("Centralino tassisti", "Messaggio all'autista", "Il tuo cliente è ~r~morto~w~. Trova un altro cliente, abbiamo già avvertito la polizia", NotificationIcon.Taxi, IconType.ChatBox);
							}
							else
							{
								if (jobs.flag[0] == 1 && jobs.flag[1] > 0)
								{
									await BaseScript.Delay(1000);
									jobs.flag[1]--;

									if (jobs.flag[1] == 0)
									{
										if (NPCPasseggero.AttachedBlip != null && NPCPasseggero.AttachedBlip.Exists()) NPCPasseggero.AttachedBlip.Delete();
										NPCPasseggero.Task.ClearAllImmediately();
										NPCPasseggero.MarkAsNoLongerNeeded();
										NPCPasseggero = null;
										HUD.ShowAdvancedNotification("Centralino tassisti", "Messaggio all'autista", "Il tuo cliente si è spazientito e ha cancellato il servizio. Trovane ~y~un altro~w.", NotificationIcon.Taxi, IconType.ChatBox);
										jobs.flag[0] = 0;
										jobs.flag[1] = 59 + Funzioni.GetRandomInt(1, 61);
									}
									else
									{
										if (p.IsSittingInVehicle(VeicoloServizio))
											if (Cache.MyPlayer.User.Posizione.Distance(NPCPasseggero.Position) < 8.0001f)
											{
												NPCPasseggero.IsPersistent = true;
												NPCPasseggero.BlockPermanentEvents = true;
												SetPedCombatAttributes(NPCPasseggero.Handle, 17, true);
												Vector3 offs = GetOffsetFromEntityInWorldCoords(VeicoloServizio.Handle, 1.5f, 0.0f, 0.0f);
												Vector3 offs2 = GetOffsetFromEntityInWorldCoords(VeicoloServizio.Handle, -1.5f, 0.0f, 0.0f);
												if (Vector3.Distance(offs, NPCPasseggero.Position) < Vector3.Distance(offs2, NPCPasseggero.Position))
													TaskEnterVehicle(NPCPasseggero.Handle, VeicoloServizio.Handle, -1, 2, 1.0001f, 1, 0);
												else
													TaskEnterVehicle(NPCPasseggero.Handle, VeicoloServizio.Handle, -1, 1, 1.0001f, 1, 0);
												NPCPasseggero.AlwaysKeepTask = true;
												while (!NPCPasseggero.IsInVehicle(VeicoloServizio)) await BaseScript.Delay(0);
												VeicoloServizio.LockStatus = VehicleLockStatus.Locked;
												NPCPasseggero.BlockPermanentEvents = true;
												if (NPCPasseggero.AttachedBlip != null && NPCPasseggero.AttachedBlip.Exists()) NPCPasseggero.AttachedBlip.Delete();
												jobs.pedentpos = NPCPasseggero.Position;
												jobs.flag[0] = 2;
												jobs.flag[1] = 30;
											}
									}
								}

								if (jobs.flag[0] == 2 && jobs.flag[1] > 0)
								{
									await BaseScript.Delay(1000);
									jobs.flag[1]--;

									if (jobs.flag[1] == 0)
									{
										if (NPCPasseggero.AttachedBlip != null && NPCPasseggero.AttachedBlip.Exists()) NPCPasseggero.AttachedBlip.Delete();
										NPCPasseggero.Task.ClearAll();
										NPCPasseggero.MarkAsNoLongerNeeded();
										NPCPasseggero = null;
										HUD.ShowAdvancedNotification("Centralino tassisti", "Messaggio all'autista", "Il tuo cliente non si sente al sicuro con te, fermati e fallo scendere, dovrai trovarne ~y~un altro~w.", NotificationIcon.Taxi, IconType.ChatBox);
										while (VeicoloServizio.Speed > 0) await BaseScript.Delay(1000);
										NPCPasseggero.Task.LeaveVehicle(VeicoloServizio, true);
										jobs.flag[0] = 0;
										jobs.flag[1] = 59 + Funzioni.GetRandomInt(1, 61);
										taximeter.ClearDisplay();
									}
									else
									{
										if (p.IsSittingInVehicle(VeicoloServizio))
										{
											if (NPCPasseggero.AttachedBlip != null && NPCPasseggero.AttachedBlip.Exists()) NPCPasseggero.AttachedBlip.Delete();
											jobs.flag[0] = 3;
											jobs.flag[1] = Funzioni.GetRandomInt(0, 92);
											uint str = 0;
											uint cross = 0;
											Vector3 pos = taxi.jobCoords[jobs.flag[1]];
											GetStreetNameAtCoord(pos.X, pos.Y, pos.Z, ref str, ref cross);
											string street = "";
											if (cross > 0)
												street = $"Mi porti a {GetStreetNameFromHashKey(str)}, vicino {GetStreetNameFromHashKey(cross)}";
											else
												street = $"Mi porti a {GetStreetNameFromHashKey(str)}";
											Screen.ShowSubtitle(street, 5000);
											float totalDist = CalculateTravelDistanceBetweenPoints(VeicoloServizio.Position.X, VeicoloServizio.Position.Y, VeicoloServizio.Position.Z, pos.X, pos.Y, pos.Z);
											jobs.jobPay = (int)Math.Round(totalDist * taxi.PrezzoModifier);
											jobs.blip = World.CreateBlip(pos);
											jobs.blip.Name = GetStreetNameFromHashKey(str);
											jobs.blip.ShowRoute = true;
											taximeter.AddDestination(0, 2, 0, 0, 255, GetStreetNameFromHashKey(str), GetLabelText(GetNameOfZone(pos.X, pos.Y, pos.Z)), cross > 0 ? GetStreetNameFromHashKey(cross) : "", false);
											taximeter.SetPrice(jobs.jobPay);
											taximeter.HighlightDestination(false);
											taximeter.ShowDestination();
										}
									}
								}

								if (jobs.flag[0] == 3)
								{
									if (Vector3.Distance(VeicoloServizio.Position, taxi.jobCoords[jobs.flag[1]]) > 2f)
									{
										World.DrawMarker(MarkerType.VerticalCylinder, new Vector3(taxi.jobCoords[jobs.flag[1]].X, taxi.jobCoords[jobs.flag[1]].Y, taxi.jobCoords[jobs.flag[1]].Z - 1f), Vector3.Zero, Vector3.Zero, new Vector3(4f, 4f, 2f), Colors.Gold);

										if (VeicoloServizio.Speed > 130 * 3.6f)
										{
											if (jobs.flag[1] > 0)
											{
												await BaseScript.Delay(1000);
												jobs.flag[1]--;
												Screen.ShowSubtitle("Stai andando ~y~troppo veloce~w~! Rallenta o il tuo passeggero potrebbe ~r~spaventarsi~w~!", 1000);
											}

											if (jobs.flag[1] == 0)
											{
												if (NPCPasseggero.AttachedBlip != null && NPCPasseggero.AttachedBlip.Exists()) NPCPasseggero.AttachedBlip.Delete();
												NPCPasseggero.Task.ClearAll();
												NPCPasseggero.MarkAsNoLongerNeeded();
												NPCPasseggero = null;
												HUD.ShowAdvancedNotification("Centralino tassisti", "Messaggio all'autista", "Il tuo cliente non si sente al sicuro con te, fermati e fallo scendere, dovrai trovarne ~y~un altro~w.", NotificationIcon.Taxi, IconType.ChatBox);
												while (VeicoloServizio.Speed > 0) await BaseScript.Delay(1000);
												NPCPasseggero.Task.LeaveVehicle(VeicoloServizio, true);
												jobs.flag[0] = 0;
												jobs.flag[1] = 59 + Funzioni.GetRandomInt(1, 61);
												taximeter.ClearDisplay();
											}
										}
									}
									else
									{
										if (VeicoloServizio.Speed == 0)
										{
											if (jobs.blip != null && jobs.blip.Exists())
											{
												jobs.blip.Delete();
												jobs.blip = null;
											}

											if (NPCPasseggero.AttachedBlip != null && NPCPasseggero.AttachedBlip.Exists()) NPCPasseggero.AttachedBlip.Delete();
											NPCPasseggero.Task.LeaveVehicle(LeaveVehicleFlags.None);
											while (!NPCPasseggero.IsInVehicle(VeicoloServizio)) await BaseScript.Delay(0);
											NPCPasseggero.Task.ClearAll();
											NPCPasseggero.MarkAsNoLongerNeeded();
											NPCPasseggero = null;
											await BaseScript.Delay(2500);
											HUD.ShowNotification("Hai portato con successo a destinazione il tuo cliente.", NotificationColor.GreenDark, true);
											BaseScript.TriggerServerEvent("lprp:givemoney", jobs.jobPay);
											HUD.ShowNotification("Hai guadagnato $" + jobs.jobPay);
											await BaseScript.Delay(8000);
											HUD.ShowAdvancedNotification("Centralino tassisti", "Messaggio all'autista", "Guida per le strade in cerca di un passeggero.", NotificationIcon.Taxi, IconType.ChatBox);
											jobs.flag[0] = 0;
											jobs.flag[1] = 59 + Funzioni.GetRandomInt(1, 61);
											taximeter.ClearDisplay();
										}
									}
								}
							}
						}
						else
						{
							if (jobs.flag[0] > 0)
							{
								jobs.flag[0] = 0;
								jobs.flag[1] = 59 + Funzioni.GetRandomInt(1, 61);
								HUD.ShowAdvancedNotification("Centralino tassisti", "Messaggio all'autista", "Guida per le strade in cerca di un passeggero.", NotificationIcon.Taxi, IconType.ChatBox);

								if (jobs.blip != null && jobs.blip.Exists())
								{
									jobs.blip.Delete();
									jobs.blip = null;
								}
							}

							if (jobs.flag[0] == 0 && jobs.flag[1] > 0)
							{
								await BaseScript.Delay(1000);
								jobs.flag[1]--;
								{
									if (jobs.flag[1] == 0)
									{
										Vector3 pos = Cache.MyPlayer.User.Posizione.ToVector3;
										Ped rand = new Ped(GetRandomPedAtCoord(pos.X, pos.Y, pos.Z, taxi.pickupRange, taxi.pickupRange, taxi.pickupRange, 26));

										if (rand.Exists())
										{
											NPCPasseggero = rand;
											jobs.flag[0] = 1;
											jobs.flag[1] = 19 + Funzioni.GetRandomInt(21);
											NPCPasseggero.Task.ClearAll();
											NPCPasseggero.BlockPermanentEvents = true;
											NPCPasseggero.Task.StandStill(1000 * jobs.flag[1]);
											HUD.ShowAdvancedNotification("Centralino tassisti", "Messaggio all'autista", "Abbiamo un passeggero, vai a prenderlo.", NotificationIcon.Taxi, IconType.ChatBox);
											Blip lblip = NPCPasseggero.AttachBlip();
											lblip.IsFriendly = true;
											lblip.Sprite = BlipSprite.Friend;
											lblip.Color = (BlipColor)3;
											SetBlipCategory(lblip.Handle, 3);
										}
										else
										{
											jobs.flag[0] = 0;
											jobs.flag[1] = 59 + Funzioni.GetRandomInt(1, 61);
											HUD.ShowAdvancedNotification("Centralino tassisti", "Messaggio all'autista", "Guida per le strade in cerca di un passeggero.", NotificationIcon.Taxi, IconType.ChatBox);
										}
									}
								}
							}
						}
					}
					else
					{
						if (Vector3.Distance(Cache.MyPlayer.User.Posizione.ToVector3, VeicoloServizio.Position) > 30f)
							VaiFuoriServizio(1);
						else
							Screen.ShowSubtitle("Torna sulla tua auto per ~g~continuare~w~ o ~r~allontanati~w~ dal taxi per smettere di lavorare");
					}
				}
				else
				{
					VaiFuoriServizio(1);
					HUD.ShowAdvancedNotification("Centralino tassisti", "Messaggio all'autista", "Il tuo taxi è rotto! Sarai multato per questo!.", NotificationIcon.Taxi, IconType.ChatBox);
				}
			}
			else
			{
				await BaseScript.Delay(1000);
			}
		}

		private static void VaiFuoriServizio(int id)
		{
			if (NPCPasseggero != null && NPCPasseggero.Exists())
			{
				if (NPCPasseggero.AttachedBlip != null && NPCPasseggero.AttachedBlip.Exists())
				{
					NPCPasseggero.AttachedBlip.Sprite = (BlipSprite)2;
					SetBlipDisplay(NPCPasseggero.AttachedBlip.Handle, 3);
				}

				NPCPasseggero.Task.ClearAllImmediately();
				if (VeicoloServizio != null && VeicoloServizio.Exists() && VeicoloServizio.IsDriveable)
					if (NPCPasseggero.IsSittingInVehicle(VeicoloServizio))
						NPCPasseggero.Task.LeaveVehicle(LeaveVehicleFlags.None);
				NPCPasseggero.MarkAsNoLongerNeeded();
				NPCPasseggero = null;

				if (jobs.blip != null && jobs.blip.Exists())
				{
					jobs.blip.Delete();
					jobs.blip = null;
				}

				jobs = new TaxiFlags();
			}

			Client.Instance.RemoveTick(TaximeterTick);
		}
	}

	public class TaxiFlags
	{
		public int[] flag = new int[2];
		public int onJob = 0;
		public Blip blip;
		public Vector3 pedentpos;
		public int jobPay;
	}

	internal class TaxiMeter
	{
		public int meter_rt = 0;
		public Prop meter_entity;
		public Scaleform Taximeter = new Scaleform("taxi_display");
		public void AddDestination(int index, int blipIndex, int blipR, int blipG, int blipB, string destinationStr, string addressStr1, string addressStr2, bool isAsian) { Taximeter.CallFunction("ADD_TAXI_DESTINATION", index, blipIndex, blipR, blipG, blipB, destinationStr, addressStr1, addressStr2, false); } // isasian sempre false.. tanto vale

		public void ClearDisplay() { Taximeter.CallFunction("CLEAR_TAXI_DISPLAY"); }

		public void ShowDestination() { Taximeter.CallFunction("SHOW_TAXI_DESTINATION"); }

		public void SetPrice(int price) { Taximeter.CallFunction("SET_TAXI_PRICE", price.ToString(), false); }

		public void HighlightDestination(bool force) { Taximeter.CallFunction("HIGHLIGHT_DESTINATION", force); }

		public void RenderMeter()
		{
			SetTextRenderId(meter_rt);
			Set_2dLayer(4);
			SetScriptGfxDrawBehindPausemenu(true);
			DrawScaleformMovie(Taximeter.Handle, 0.201000005f, 0.351f, 0.4f, 0.6f, 0, 0, 0, 255, 0);
			SetTextRenderId(1);
		}

		public Prop CreateTaxiMeter(Vehicle veh)
		{
			Vector3 c = veh.Position;
			int meter = GetClosestObjectOfType(c.X, c.Y, c.Z, 2.0f, (uint)GetHashKey("prop_taxi_meter_2"), false, false, false);

			if (DoesEntityExist(meter)) return new Prop(meter);
			meter = CreateObject(GetHashKey("prop_taxi_meter_2"), c.X, c.Y, c.Z, true, true, false);
			AttachEntityToEntity(meter, veh.Handle, GetEntityBoneIndexByName(veh.Handle, "Chassis"), -0.01f, 0.6f, 0.24f, -5.0f, 0.0f, 0.0f, false, false, false, false, 2, true);

			return new Prop(meter);
		}
	}
}