using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.MenuNativo;
using NuovaGM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuovaGM.Client.Veicoli;
using System.Drawing;

namespace NuovaGM.Client.Lavori.Generici.Taxi
{
	static class TaxiClient
	{
		private static TaxiFlags jobs = new TaxiFlags();
		private static Tassisti taxi;
		private static Vehicle VeicoloServizio;
		private static Ped NPCPasseggero;
		private static bool InServizio = false;
		public static void Init()
		{
			taxi = Client.Impostazioni.Lavori.Generici.Tassista;
			// provvisorio
			Client.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action(Eccolo));
		}
		private static void Eccolo()
		{
			Client.Instance.AddTick(Markers);
		}

		public static async Task Markers()
		{
			if(Game.PlayerPed.IsInRangeOf(taxi.PosAccettazione, 100))
			{
				World.DrawMarker(MarkerType.ChevronUpx2, taxi.PosAccettazione, Vector3.Zero, Vector3.Zero, new Vector3(1.5f), Colors.Yellow, rotateY: true);
				if (Game.PlayerPed.IsInRangeOf(taxi.PosAccettazione, 1.375f))
				{
					if(Game.Player.GetPlayerData().CurrentChar.job.name.ToLower() != "taxi")
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per accettare il lavoro da tassista.");
						if (Input.IsControlJustPressed(Control.Context)) 
						{
							// entra in servizio.. setta il lavoro.. insomma ci siamo capiti
						}
					}
					else
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per smettere di lavorare.");
						if (Input.IsControlJustPressed(Control.Context))
						{
							// esci dal servizio.. torna senza lavoro.. insomma ci siamo capiti
						}
					}
				}
			}
			if (Game.Player.GetPlayerData().CurrentChar.job.name.ToLower() == "taxi")
			{
				if (Game.PlayerPed.IsInRangeOf(taxi.PosRitiroVeicolo, 100))
				{
					if (VeicoloServizio == null || (VeicoloServizio != null && !VeicoloServizio.Exists() || VeicoloServizio.IsDead))
					{
						World.DrawMarker(MarkerType.CarSymbol, taxi.PosRitiroVeicolo, Vector3.Zero, Vector3.Zero, new Vector3(1.5f), Colors.Yellow, rotateY: true);
						if (Game.PlayerPed.IsInRangeOf(taxi.PosRitiroVeicolo, 1.375f))
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per prendere il tuo veicolo di servizio.");
							if (Input.IsControlJustPressed(Control.Context))
							{
								if (Game.PlayerPed.IsVisible)
									NetworkFadeOutEntity(Game.PlayerPed.Handle, true, false);
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
				}
				if (Game.PlayerPed.IsInRangeOf(taxi.PosDepositoVeicolo, 100))
				{
					if (VeicoloServizio != null && !VeicoloServizio.IsDead && VeicoloServizio.Exists())
					{
						World.DrawMarker(MarkerType.CarSymbol, taxi.PosDepositoVeicolo, Vector3.Zero, Vector3.Zero, new Vector3(1.5f), Colors.Red, rotateY: true);
						if (Game.PlayerPed.IsInRangeOf(taxi.PosDepositoVeicolo, 1.375f))
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per parcheggiare il veicolo di servizio.");
							if (Input.IsControlJustPressed(Control.Context))
							{
								if (Game.PlayerPed.CurrentVehicle.IsVisible)
									NetworkFadeOutEntity(Game.PlayerPed.CurrentVehicle.Handle, true, false);
								Screen.Fading.FadeOut(800);
								await BaseScript.Delay(1000);
								VeicoloServizio.Delete();
								VeicoloServizio = null;
								Game.PlayerPed.Position = taxi.PosRitiroVeicolo;
								NetworkFadeInEntity(PlayerPedId(), true);
								Screen.Fading.FadeIn(500);
							}
						}
					}
				}
				if (Input.IsControlJustPressed(Control.SelectCharacterFranklin, PadCheck.Keyboard))
				{
					InServizio = !InServizio;
					if(VeicoloServizio != null && VeicoloServizio.IsAlive && VeicoloServizio.Exists())
						SetTaxiLights(VeicoloServizio.Handle, InServizio);
					if (InServizio)
					{
						Client.Instance.AddTick(ServizioTaxi);
						HUD.ShowAdvancedNotification("Centralino tassisti", "Messaggio all'autista", "Sei entrato in servizio. Guida per le strade in cerca di clienti.", NotificationIcon.Taxi, IconType.ChatBox);
					}
					else
					{
						Client.Instance.RemoveTick(ServizioTaxi);
						HUD.ShowAdvancedNotification("Centralino tassisti", "Messaggio all'autista", "Sei uscito dal servizio.", NotificationIcon.Taxi, IconType.ChatBox);
						VaiFuoriServizio(1);
					}
				}
			}
		}
		private static async Task ServizioTaxi()
		{
			if (InServizio && jobs.onJob == 0)
			{
				if (Game.PlayerPed.IsInVehicle(VeicoloServizio))
				{
					if(Game.PlayerPed.CurrentVehicle.Driver == Game.PlayerPed)
					{
						jobs.flag[0] = 0;
						jobs.flag[1] = 10;//59 + Funzioni.GetRandomInt(1, 61);
						jobs.onJob = 1;
					}
				}
			}
			else if (jobs.onJob == 1 && InServizio)
			{
				if(VeicoloServizio.Exists() && VeicoloServizio.IsDriveable)
				{
					if (Game.PlayerPed.IsSittingInVehicle(VeicoloServizio))
					{
						if(NPCPasseggero != null && NPCPasseggero.Exists())
						{
							if (IsPedFatallyInjured(NPCPasseggero.Handle))
							{
								NPCPasseggero.MarkAsNoLongerNeeded();
								if (NPCPasseggero.AttachedBlip != null && NPCPasseggero.AttachedBlip.Exists())
								{
									NPCPasseggero.AttachedBlip.Sprite = (BlipSprite)2;
									SetBlipDisplay(NPCPasseggero.AttachedBlip.Handle, 3);
								}
								NPCPasseggero = null;
								jobs.flag[0] = 0;
								jobs.flag[1] = 10;//59 + Funzioni.GetRandomInt(1, 61);
								if(jobs.blip != null && jobs.blip.Exists())
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
									if(jobs.flag[1] == 0)
									{
										if(NPCPasseggero.AttachedBlip != null && NPCPasseggero.AttachedBlip.Exists())
										{
											NPCPasseggero.AttachedBlip.Sprite = (BlipSprite)2;
											SetBlipDisplay(NPCPasseggero.AttachedBlip.Handle, 3);
										}
										NPCPasseggero.Task.ClearAllImmediately();
										NPCPasseggero.MarkAsNoLongerNeeded();
										NPCPasseggero = null;
										HUD.ShowAdvancedNotification("Centralino tassisti", "Messaggio all'autista", "Il tuo cliente si è spazientito e ha cancellato il servizio. Trovane ~y~un altro~w.", NotificationIcon.Taxi, IconType.ChatBox);
										jobs.flag[0] = 0;
										jobs.flag[1] = 10;//59 + Funzioni.GetRandomInt(1, 61);
									}
									else
									{
										if (Game.PlayerPed.IsSittingInVehicle(VeicoloServizio))
										{
											if (Vector3.Distance(Game.Player.GetPlayerData().posizione.ToVector3(), NPCPasseggero.Position) < 8.0001f)
											{
												NPCPasseggero.IsPersistent = true;
												NPCPasseggero.BlockPermanentEvents = true;
												SetPedCombatAttributes(NPCPasseggero.Handle, 17, true);
												var offs = GetOffsetFromEntityInWorldCoords(VeicoloServizio.Handle, 1.5f, 0.0f, 0.0f);
												var offs2 = GetOffsetFromEntityInWorldCoords(VeicoloServizio.Handle, -1.5f, 0.0f, 0.0f);
												if (Vector3.Distance(offs, NPCPasseggero.Position) < Vector3.Distance(offs2, NPCPasseggero.Position))
													TaskEnterVehicle(NPCPasseggero.Handle, VeicoloServizio.Handle, -1, 2, 1.0001f, 1, 0);
												else
													TaskEnterVehicle(NPCPasseggero.Handle, VeicoloServizio.Handle, -1, 1, 1.0001f, 1, 0);
												NPCPasseggero.AlwaysKeepTask = true;
												while (!NPCPasseggero.IsInVehicle(VeicoloServizio)) await BaseScript.Delay(0);
												VeicoloServizio.LockStatus = VehicleLockStatus.Locked;
												NPCPasseggero.BlockPermanentEvents = true;
												jobs.pedentpos = NPCPasseggero.Position;
												jobs.flag[0] = 2;
												jobs.flag[1] = 30;
											}
										}
									}
								}
								if (jobs.flag[0] == 2 && jobs.flag[1] > 0)
								{
									await BaseScript.Delay(1000);
									jobs.flag[1]--;
									if(jobs.flag[1] == 0)
									{
										if (NPCPasseggero.AttachedBlip != null && NPCPasseggero.AttachedBlip.Exists())
										{
											NPCPasseggero.AttachedBlip.Sprite = (BlipSprite)2;
											SetBlipDisplay(NPCPasseggero.AttachedBlip.Handle, 3);
										}
										NPCPasseggero.Task.ClearAllImmediately();
										NPCPasseggero.MarkAsNoLongerNeeded();
										NPCPasseggero = null;
										HUD.ShowAdvancedNotification("Centralino tassisti", "Messaggio all'autista", "Il tuo cliente non si sente al sicuro con te, fermati e fallo scendere, dovrai trovarne ~y~un altro~w.", NotificationIcon.Taxi, IconType.ChatBox);
										while (VeicoloServizio.Speed > 0) await BaseScript.Delay(1000);
										NPCPasseggero.Task.LeaveVehicle(VeicoloServizio, true);
										jobs.flag[0] = 0;
										jobs.flag[1] = 10;//59 + Funzioni.GetRandomInt(1, 61);
									}
									else
									{
										if (Game.PlayerPed.IsSittingInVehicle(VeicoloServizio))
										{
											if (NPCPasseggero.AttachedBlip != null && NPCPasseggero.AttachedBlip.Exists())
											{
												NPCPasseggero.AttachedBlip.Sprite = (BlipSprite)2;
												SetBlipDisplay(NPCPasseggero.AttachedBlip.Handle, 3);
											}
											jobs.flag[0] = 3;
											jobs.flag[1] = Funzioni.GetRandomInt(0, 92);

											uint str = 0; uint cross = 0;
											Vector3 pos = taxi.jobCoords[jobs.flag[1]];
											GetStreetNameAtCoord(pos.X, pos.Y, pos.Z, ref str, ref cross);
											string street = "";
											if(cross > 0)
												street = $"Mi porti verso {GetStreetNameFromHashKey(str)}, vicino {GetStreetNameFromHashKey(cross)}";
											else
												street = $"Mi porti verso {GetStreetNameFromHashKey(str)}";
											Screen.ShowSubtitle(street, 5000);
											float totalDist = CalculateTravelDistanceBetweenPoints(VeicoloServizio.Position.X, VeicoloServizio.Position.Y, VeicoloServizio.Position.Z, pos.X, pos.Y, pos.Z);
											jobs.jobPay = (int)Math.Round(totalDist * taxi.PrezzoModifier);
											jobs.blip = World.CreateBlip(pos);
											jobs.blip.Name = GetStreetNameFromHashKey(str);
											jobs.blip.ShowRoute = true;
										}
									}
								}
								if (jobs.flag[0] == 3)
								{
									if (Vector3.Distance(VeicoloServizio.Position, taxi.jobCoords[jobs.flag[1]]) > 4f)
										World.DrawMarker(MarkerType.VerticalCylinder, new Vector3(taxi.jobCoords[jobs.flag[1]].X, taxi.jobCoords[jobs.flag[1]].Y, taxi.jobCoords[jobs.flag[1]].Z - 1f), Vector3.Zero, Vector3.Zero, new Vector3(4f, 4f, 2f), Color.FromArgb(155, 178, 236, 93));
									else
									{
										if (VeicoloServizio.Speed < 2)
										{
											if (jobs.blip != null && jobs.blip.Exists())
											{
												jobs.blip.Delete();
												jobs.blip = null;
											}
											NPCPasseggero.Task.ClearAllImmediately();
											NPCPasseggero.Task.LeaveVehicle(LeaveVehicleFlags.None);
											NPCPasseggero.MarkAsNoLongerNeeded();
											NPCPasseggero = null;
											await BaseScript.Delay(2500);
											HUD.ShowNotification("Hai portato con successo a destinazione il tuo cliente.", NotificationColor.GreenDark, true);
											BaseScript.TriggerServerEvent("lprp:givemoney", jobs.jobPay);
											HUD.ShowNotification("Hai guadagnato $" + jobs.jobPay);
											await BaseScript.Delay(8000);
											HUD.ShowAdvancedNotification("Centralino tassisti", "Messaggio all'autista", "Guida per le strade in cerca di un passeggero.", NotificationIcon.Taxi, IconType.ChatBox);
											jobs.flag[0] = 0;
											jobs.flag[1] = 10;//59 + Funzioni.GetRandomInt(1, 61);

										}
									}
								}
							}
						}
						else
						{
							if(jobs.flag[0] > 0)
							{
								jobs.flag[0] = 0;
								jobs.flag[1] = 10;//59 + Funzioni.GetRandomInt(1, 61);
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
									if(jobs.flag[1] == 0)
									{
										Vector3 pos = Game.Player.GetPlayerData().posizione.ToVector3();
										Ped rand = new Ped(GetRandomPedAtCoord(pos.X, pos.Y, pos.Z, 35f, 35f, 35f, 26));
										if (rand.Exists())
										{
											NPCPasseggero = rand;
											jobs.flag[0] = 1;
											jobs.flag[1] = 19 + Funzioni.GetRandomInt(21);
											NPCPasseggero.Task.ClearAllImmediately();
											NPCPasseggero.BlockPermanentEvents = true;
											NPCPasseggero.Task.StandStill(1000*jobs.flag[1]);
											HUD.ShowAdvancedNotification("Centralino tassisti", "Messaggio all'autista", "Abbiamo un passeggero, vai a prenderlo.", NotificationIcon.Taxi, IconType.ChatBox);
											Blip lblip = World.CreateBlip(NPCPasseggero.Position);
											lblip.IsFriendly = true;
											lblip.Color = (BlipColor)2;
											SetBlipCategory(lblip.Handle, 3);
										}
										else
										{
											jobs.flag[0] = 0;
											jobs.flag[1] = 10;//59 + Funzioni.GetRandomInt(1, 61);
											HUD.ShowAdvancedNotification("Centralino tassisti", "Messaggio all'autista", "Guida per le strade in cerca di un passeggero.", NotificationIcon.Taxi, IconType.ChatBox);
										}
									}
								}
							}
						}
					}
					else
					{
						if (Vector3.Distance(Game.Player.GetPlayerData().posizione.ToVector3(), VeicoloServizio.Position) > 30f)
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
			else await BaseScript.Delay(1000);
		}

		private static void VaiFuoriServizio(int id)
		{
			if(NPCPasseggero != null && NPCPasseggero.Exists())
			{
				if(NPCPasseggero.AttachedBlip != null && NPCPasseggero.AttachedBlip.Exists())
				{
					NPCPasseggero.AttachedBlip.Sprite = (BlipSprite)2;
					SetBlipDisplay(NPCPasseggero.AttachedBlip.Handle, 3);
				}
				NPCPasseggero.Task.ClearAllImmediately();
				if (VeicoloServizio != null && VeicoloServizio.Exists() && VeicoloServizio.IsDriveable)
				{
					if (NPCPasseggero.IsSittingInVehicle(VeicoloServizio))
						NPCPasseggero.Task.LeaveVehicle(LeaveVehicleFlags.None);
				}
				NPCPasseggero.MarkAsNoLongerNeeded();
				NPCPasseggero = null;
				if (jobs.blip != null && jobs.blip.Exists())
				{
					jobs.blip.Delete();
					jobs.blip = null;
				}
				jobs = new TaxiFlags();
			}
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
}
