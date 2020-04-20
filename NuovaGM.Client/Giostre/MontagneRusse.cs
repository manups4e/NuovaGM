using CitizenFX.Core;
using CitizenFX.Core.Native;
using Logger;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.MenuNativo;
using NuovaGM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Client.Giostre
{
	static class MontagneRusse
	{
		// NEXT STEP => QUANDO IL PLAYER VUOLE SALIRE, TRIGGERA EVENTO E LUI SALE.. GLI ALTRI VEDONO IL SUO PED SALIRE..
		// ALTRIMENTI NON SO CHE FARE....
		//Montagna.Index == index

		static int iLocal_715 = 0;
		static float fLocal_716 = 0;
		static int ValoreIndiceSconosciuto = 0;
		private static readonly string RollerAnim = "anim@mp_rollarcoaster";
		private static string Posto;
		static MontagnaRussaNew Montagna = new MontagnaRussaNew();
		static Vector3 Coord = new Vector3(0);
		static bool Attivato = false;
		public static bool SonoSeduto = false;
		static bool Scaleform = false;
		static Scaleform Buttons = new Scaleform("instructional_buttons");
		static UITimerBarItem montagna = new UITimerBarItem("Montagne Russe:")
		{
			Enabled = false
		};

		static List<Vector3> ingressi = new List<Vector3>
		{
			new Vector3(-1644.316f, -1123.53f, 17.3447f),
			new Vector3(-1644.92f, -1124.281f, 17.3447f),
			new Vector3(-1645.845f, -1125.413f, 17.3447f),
			new Vector3(-1646.562f, -1126.302f, 17.3447f),
			new Vector3(-1647.498f, -1127.438f, 17.3447f),
			new Vector3(-1648.23f, -1128.184f, 17.3447f),
			new Vector3(-1649.233f, -1129.399f, 17.3447f),
			new Vector3(-1649.937f, -1130.203f, 17.3447f),
		};

		static List<Vector3> uscite = new List<Vector3>
		{
			new Vector3(-1641.914f, -1125.268f, 17.3424f),
			new Vector3(-1642.606f, -1126.24f, 17.3424f),
			new Vector3(-1643.573f, -1127.39f, 17.3424f ),
			new Vector3(-1644.271f, -1128.2f, 17.3424f),
			new Vector3(-1645.343f, -1129.313f, 17.3424f),
			new Vector3(-1645.966f, -1130.067f, 17.3424f),
			new Vector3(-1647.022f, -1131.291f, 17.3424f),
			new Vector3(-1647.645f, -1132.016f, 17.3424f),
		};

		static List<Vector3> necessari_non_so_a_cosa = new List<Vector3>
		{
			new Vector3(-1644.153f, -1125.433f, 18.3447f),
			new Vector3(-1645.723f, -1127.408f, 18.3447f ),
			new Vector3(-1647.315f, -1129.374f, 18.3447f),
			new Vector3(-1648.95f, -1131.299f, 18.3447f),
		};

		public static async void Init()
		{
			func_220();
			Client.Instance.AddEventHandler("lprp:montagnerusse:forceState", new Action<string>(ForceState));
			Client.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action(Spawnato));
			Client.Instance.AddEventHandler("lprp:montagnerusse:playerSale", new Action<int, int, int>(PlayerSale));
			Client.Instance.AddEventHandler("lprp:montagnerusse:playerScende", new Action<int>(PlayerScende));
			Client.Instance.AddEventHandler("lprp:montagnerusse:syncCarrelli", new Action<int, int>(SyncCarrelli));
			Client.Instance.AddEventHandler("onResourceStop", new Action<string>(OnStop));
			Blip roller = new Blip(AddBlipForCoord(-1651.641f, -1134.325f, 21.90398f))
			{
				Sprite = BlipSprite.Fairground,
				IsShortRange = true,
				Name = "Montagne Russe"
			};
			SetBlipDisplay(roller.Handle, 4);
			CaricaTutto();
		}

		private static async Task SpawnaMontagne()
		{
			func_191();
			Client.Instance.AddTick(ControlloMontagne);
			await Task.FromResult(0);
		}

		private static void OnStop(string name)
		{
			if (GetCurrentResourceName() == name)
				foreach (var v in Montagna.Carrelli)
					v.Entity.Delete();
		}

		private static async void CaricaTutto()
		{
			RequestModel((uint)GetHashKey("ind_prop_dlc_roller_car"));
			while (!HasModelLoaded((uint)GetHashKey("ind_prop_dlc_roller_car"))) await BaseScript.Delay(100);
			RequestModel((uint)GetHashKey("ind_prop_dlc_roller_car_02"));
			while (!HasModelLoaded((uint)GetHashKey("ind_prop_dlc_roller_car_02"))) await BaseScript.Delay(100);
			RequestAnimDict(RollerAnim);
			while (!HasAnimDictLoaded(RollerAnim)) await BaseScript.Delay(100);
			LoadStream("LEVIATHON_RIDE_MASTER", "");
			await SpawnaMontagne();
			Client.Instance.AddTick(MuoveMontagneRusse);
			HUD.TimerBarPool.Add(montagna);

		}

		private static async void Spawnato()
		{
			Client.Instance.AddTick(EliminaGialli);
		}

		static List<ObjectHash> DaEliminare = new List<ObjectHash>()
		{
			ObjectHash.prop_roller_car_01,
			ObjectHash.prop_roller_car_02
		};

		static Prop MRClosest = new Prop(0);

		private static async Task EliminaGialli()
		{
			try
			{
				MRClosest = World.GetAllProps().Select(o => new Prop(o.Handle)).Where(o => DaEliminare.Contains((ObjectHash)(uint)o.Model.Hash)).FirstOrDefault(o => o.Position.DistanceToSquared(Game.PlayerPed.Position) < Math.Pow(2 * 300f, 2));
				if (MRClosest != null && MRClosest.Exists()) MRClosest.Delete();
			}
			catch(Exception e)
			{
				Log.Printa(LogType.Error, "errore montagne\n" +e.ToString() +"\n" + e.StackTrace);
			}
			await BaseScript.Delay(500);
		}

		static int tempo = 30;
		private static async Task MuoveMontagneRusse()
		{
			try
			{
				switch (Montagna.State)
				{
					case "ATTESA":
						montagna.TextTimerBar.Caption = tempo + " sec.";
						while (tempo > 0)
						{
							await BaseScript.Delay(1000);
							tempo--;
							montagna.TextTimerBar.Caption = tempo + "sec.";
						}
						await BaseScript.Delay(6000);
						BaseScript.TriggerServerEvent("lprp:montagnerusse:syncState", "PARTENZA");
						break;
					case "PARTENZA":
						if (!Attivato)
						{
							Montagna.Carrelli.ToList().ForEach(o => PlayEntityAnim(o.Entity.Handle, "safety_bar_enter_roller_car", RollerAnim, 8f, false, true, false, 0f, 0));
							PlaySoundFromEntity(-1, "Bar_Lower_And_Lock", Montagna.Carrelli[1].Entity.Handle, "DLC_IND_ROLLERCOASTER_SOUNDS", false, 0); //safety_bar_enter_player_  + Posto per entrare
							if (SonoSeduto)
							{
								TaskPlayAnim(PlayerPedId(), RollerAnim, "safety_bar_enter_player_" + Posto, 8f, -8f, -1, 2, 0, false, false, false);
								while (GetEntityAnimCurrentTime(PlayerPedId(), RollerAnim, "safety_bar_enter_player_" + Posto) < 0.2f) await BaseScript.Delay(0);
							}
							else await BaseScript.Delay(5000);
							BaseScript.TriggerServerEvent("lprp:montagnerusse:syncState", "VIAGGIO");
							Attivato = true;
						}
						break;
					case "VIAGGIO":
						if (ValoreIndiceSconosciuto != 0)
						{
							func_46(true);
							if (SonoSeduto)
							{
								UpdateTasti();
								StartAudioScene("FAIRGROUND_RIDES_LEVIATHAN");
								PlayStreamFromPed(PlayerPedId());
								if (!IsEntityPlayingAnim(PlayerPedId(), RollerAnim, "safety_bar_grip_move_a_player_" + Posto, 3) && !IsEntityPlayingAnim(PlayerPedId(), RollerAnim, "hands_up_idle_a_player_" + Posto, 3) && !IsEntityPlayingAnim(PlayerPedId(), RollerAnim, "hands_up_idle_a_player_" + Posto, 3) && !IsEntityPlayingAnim(PlayerPedId(), RollerAnim, "hands_up_exit_player_" + Posto, 3))
									Game.PlayerPed.Task.PlayAnimation(RollerAnim, "safety_bar_grip_move_a_player_" + Posto, 8f, -1, AnimationFlags.Loop);
								if (Input.IsControlJustPressed(Control.FrontendX))
								{
									if (!IsEntityPlayingAnim(PlayerPedId(), RollerAnim, "hands_up_idle_a_player_" + Posto, 3))
									{
										Game.PlayerPed.Task.PlayAnimation(RollerAnim, "hands_up_enter_player_" + Posto, 8f, -1, AnimationFlags.StayInEndFrame);
										while (IsEntityPlayingAnim(PlayerPedId(), RollerAnim, "hands_up_enter_player_" + Posto, 3)) await BaseScript.Delay(0);
										Game.PlayerPed.Task.PlayAnimation(RollerAnim, "hands_up_idle_a_player_" + Posto, 8f, -1, AnimationFlags.Loop);
									}
									else if (IsEntityPlayingAnim(PlayerPedId(), RollerAnim, "hands_up_idle_a_player_" + Posto, 3))
									{
										Game.PlayerPed.Task.PlayAnimation(RollerAnim, "hands_up_exit_player_" + Posto, 8f, -1, AnimationFlags.StayInEndFrame);
										while (IsEntityPlayingAnim(PlayerPedId(), RollerAnim, "hands_up_exit_player_" + Posto, 3)) await BaseScript.Delay(0);
										Game.PlayerPed.Task.PlayAnimation(RollerAnim, "safety_bar_grip_move_a_player_" + Posto, 8f, -1, AnimationFlags.Loop);
									}
								}
							}
							else
								PlayStreamFromObject(Montagna.Carrelli[2].Entity.Handle);
						}
						else
							BaseScript.TriggerServerEvent("lprp:montagnerusse:syncState", "ARRIVO");
						break;
					case "ARRIVO":
						PlaySoundFromEntity(-1, "Ride_Stop", Montagna.Carrelli[1].Entity.Handle, "DLC_IND_ROLLERCOASTER_SOUNDS", false, 0);
						if (IsEntityPlayingAnim(PlayerPedId(), RollerAnim, "hands_up_idle_a_player_" + Posto, 3))
						{
							Game.PlayerPed.Task.PlayAnimation(RollerAnim, "hands_up_exit_player_" + Posto, 8f, -1, AnimationFlags.StayInEndFrame);
							while (GetEntityAnimCurrentTime(PlayerPedId(), RollerAnim, "hands_up_exit_player_" + Posto) < 0.99f) await BaseScript.Delay(0);
							Game.PlayerPed.Task.PlayAnimation(RollerAnim, "safety_bar_grip_move_a_player_" + Posto, 8f, -1, AnimationFlags.Loop);
						}
						BaseScript.TriggerServerEvent("lprp:montagnerusse:syncState", "FERMATA");
						break;
					case "FERMATA":
						if (Montagna.VariabileVelocità > 1)
						{
							func_46(true);
							if (SonoSeduto)
								PlayStreamFromPed(PlayerPedId());
							else
								PlayStreamFromObject(Montagna.Carrelli[2].Entity.Handle);
						}
						else
						{
							if (Attivato)
							{
								await BaseScript.Delay(1000);
								Montagna.Carrelli.ToList().ForEach(o => PlayEntityAnim(o.Entity.Handle, "safety_bar_exit_roller_car", RollerAnim, 8f, false, true, false, 0f, 0));
								PlaySoundFromEntity(-1, "Bar_Unlock_And_Raise", Montagna.Carrelli[1].Entity.Handle, "DLC_IND_ROLLERCOASTER_SOUNDS", false, 0); //safety_bar_enter_player_  + Postoper entrare
								if (SonoSeduto) BaseScript.TriggerServerEvent("lprp:montagnerusse:playerScende", Game.PlayerPed.NetworkId);
								Montagna.Carrelli.ToList().ForEach(o => o.Occupato = 0);
								Montagna.Carrelli.ToList().ForEach(o => BaseScript.TriggerServerEvent("lprp:montagnerusse:syncCarrelli", Montagna.Carrelli.ToList().IndexOf(o), 0));
								await BaseScript.Delay(1000);
								iLocal_715 = 0;
								fLocal_716 = 0;
								Buttons.Dispose();
								Scaleform = false;
								tempo = 30;
								BaseScript.TriggerServerEvent("lprp:montagnerusse:syncState", "ATTESA");
								Attivato = false;
							}
						}
						break;
				}
			}
			catch
			{
				Log.Printa(LogType.Error, "Problema Movimento Montagne Russe");
			}
		}

		private static async void SyncCarrelli(int carrello, int occupato) => Montagna.Carrelli[carrello].Occupato = occupato;

		private static async void PlayerSale(int playernetid, int index, int carrellonetid)
		{
			Ped personaggio = (Ped)Entity.FromNetworkId(playernetid);
			if (personaggio.NetworkId != Game.PlayerPed.NetworkId)
				if (!NetworkHasControlOfNetworkId(playernetid))
					while (!NetworkRequestControlOfNetworkId(playernetid)) await BaseScript.Delay(0);
			Prop Carrello = (Prop)Entity.FromNetworkId(carrellonetid);

			if (Carrello == null)
				Carrello = Montagna.Carrelli[index].Entity;
			switch (Montagna.Carrelli[index].Occupato)
			{
				case 0:
					Posto = "one";
					Montagna.Carrelli[index].Occupato = 1;
					break;
				case 1:
					Posto = "two";
					Montagna.Carrelli[index].Occupato = 2;
					break;
				case 2:
					HUD.ShowNotification("Questo carrello è pieno!", NotificationColor.Red, true);
					return;
			}
			Log.Printa(LogType.Debug, "Posto = " + Posto);
			TaskGoStraightToCoord(personaggio.Handle, Coord.X, Coord.Y, Coord.Z, 1f, -1, 229.3511f, 0.2f);
			await BaseScript.Delay(1000);
			int iLocal_1442 = NetworkCreateSynchronisedScene(Coord.X, Coord.Y, Coord.Z, 0f, 0f, 139.96f, 2, true, false, 1065353216, 0, 1065353216);
			NetworkAddPedToSynchronisedScene(personaggio.Handle, iLocal_1442, RollerAnim, "enter_player_" + Posto, 8f, -8f, 131072, 0, 1148846080, 0);
			NetworkStartSynchronisedScene(iLocal_1442);
			int iVar1 = NetworkConvertSynchronisedSceneToSynchronizedScene(iLocal_1442);
			if (GetSynchronizedScenePhase(iVar1) > 0.99f)
			{
				iLocal_1442 = NetworkCreateSynchronisedScene(Coord.X, Coord.Y, Coord.Z, 0f, 0f, 139.96f, 2, false, true, 1065353216, 0, 1065353216);
				NetworkAddPedToSynchronisedScene(personaggio.Handle, iLocal_1442, RollerAnim, "idle_a_player_" + Posto, 8f, -8f, 131072, 0, 1148846080, 0);
				NetworkStartSynchronisedScene(iLocal_1442);
			}
			await BaseScript.Delay(5000);
			Vector3 vVar0 = GetOffsetFromEntityGivenWorldCoords(Carrello.Handle, personaggio.Position.X, personaggio.Position.Y, personaggio.Position.Z);
			AttachEntityToEntity(personaggio.Handle, Carrello.Handle, 0, vVar0.X, vVar0.Y, vVar0.Z, 0, 0, (personaggio.Heading - 139.96f), false, false, false, false, 2, true);
			if (personaggio.NetworkId == Game.PlayerPed.NetworkId)
				SonoSeduto = true;
			BaseScript.TriggerServerEvent("lprp:montagnerusse:syncCarrelli", index, Montagna.Carrelli[index].Occupato);
		}

		private static async void PlayerScende(int playernetid)
		{
			Log.Printa(LogType.Debug, "playernetid = " + playernetid);
			Log.Printa(LogType.Debug, "game.playerped.networkdid = " + Game.PlayerPed.NetworkId);

			Ped personaggio = (Ped)Entity.FromNetworkId(playernetid);
			if (personaggio != null)
			{
				if (personaggio.NetworkId != Game.PlayerPed.NetworkId)
					if (!NetworkHasControlOfNetworkId(playernetid))
						while (!NetworkRequestControlOfNetworkId(playernetid)) await BaseScript.Delay(0);
				if (personaggio.IsAttached())
					personaggio.Detach();
				if (personaggio.NetworkId == Game.PlayerPed.NetworkId)
					SonoSeduto = false;
				int iLocal_1443 = NetworkCreateSynchronisedScene(Coord.X, Coord.Y, Coord.Z, 0f, 0f, 139.96f, 2, true, false, 1065353216, 0, 1065353216);
				NetworkAddPedToSynchronisedScene(personaggio.Handle, iLocal_1443, RollerAnim, "safety_bar_exit_player_" + Posto, 8f, -8f, 131072, 0, 1148846080, 0);
				NetworkStartSynchronisedScene(iLocal_1443);
				await BaseScript.Delay(3000);
				int iLocal_1442 = NetworkCreateSynchronisedScene(Coord.X, Coord.Y, Coord.Z, 0f, 0f, 139.96f, 2, true, false, 1065353216, 0, 1065353216);
				NetworkAddPedToSynchronisedScene(personaggio.Handle, iLocal_1442, RollerAnim, "exit_player_" + Posto, 8f, -8f, 131072, 0, 1148846080, 0);
				NetworkStartSynchronisedScene(iLocal_1442);
				await BaseScript.Delay(7000);
				personaggio.Task.ClearAll();
			}
		}

		private static async Task ControlloMontagne()
		{
			foreach (var v in ingressi)
			{
				if (World.GetDistance(Game.PlayerPed.Position, v) < 1.3f && Montagna.State == "ATTESA" && tempo > 0)
				{
					HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per salire sulle montagne russe");
					if (Input.IsControlJustPressed(Control.Context))
					{
						float fVar2;
						if (ingressi.IndexOf(v) % 2 != 0)
							fVar2 = 0;
						else
							fVar2 = -1.017f;
						Coord = GetOffsetFromEntityInWorldCoords(Montagna.Carrelli[(ingressi.IndexOf(v) / 2)].Entity.Handle, 0, fVar2, 0);
						BaseScript.TriggerServerEvent("lprp:montagnerusse:playerSale", Game.PlayerPed.NetworkId, ingressi.IndexOf(v) / 2, Montagna.Carrelli[(ingressi.IndexOf(v) / 2)].Entity.NetworkId);
					}
				}
			}
			if (SonoSeduto)
			{
				if (!LoadStreamWithStartOffset("Player_Ride", 0, "DLC_IND_ROLLERCOASTER_SOUNDS"))
					LoadStreamWithStartOffset("Player_Ride", 0, "DLC_IND_ROLLERCOASTER_SOUNDS");
			}
			else
			{
				if (!LoadStreamWithStartOffset("Ambient_Ride", 1, "DLC_IND_ROLLERCOASTER_SOUNDS"))
					LoadStreamWithStartOffset("Ambient_Ride", 1, "DLC_IND_ROLLERCOASTER_SOUNDS");
			}

			if (World.GetDistance(Game.PlayerPed.Position, new Vector3(-1646.863f, -1125.135f, 17.338f)) < 30f)
			{
				if (Montagna.State == "ATTESA" && tempo > 0)
					montagna.Enabled = true;
				else
					montagna.Enabled = false;
			}
			else
				montagna.Enabled = false;
		}

		static private async void UpdateTasti()
		{
			if (!Scaleform)
			{
				Buttons = new Scaleform("instructional_buttons");
				while (!HasScaleformMovieLoaded(Buttons.Handle)) await BaseScript.Delay(0);

				Buttons.CallFunction("CLEAR_ALL");
				Buttons.CallFunction("TOGGLE_MOUSE_BUTTONS", false);

				Buttons.CallFunction("SET_DATA_SLOT", 0, GetControlInstructionalButton(2, 203, 1), "Alza/Abbassa le braccia");
				if (IsInputDisabled(2))
					Buttons.CallFunction("SET_DATA_SLOT", 2, GetControlInstructionalButton(2, 0, 1), "Cambia Visuale");
				else
					Buttons.CallFunction("SET_DATA_SLOT", 2, GetControlInstructionalButton(2, 217, 1), "Cambia Visuale");
				Buttons.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
				Scaleform = true;
			}
			if (Scaleform)
				Buttons.Render2D();
		}

		static async void func_191()
		{
			int iVar0;
			int iVar1;
			Vector3 vVar2;
			for (iVar0=0; iVar0 < Montagna.Carrelli.Length; iVar0++)
			{
				iVar1 = func_138((Montagna.Velocità - (2.55f * (iVar0))), ValoreIndiceSconosciuto);
				vVar2 = func_50((Montagna.Velocità - (2.55f * (iVar0))), iVar1);
				if (iVar0 == 0)
					Montagna.Carrelli[0].Entity = new Prop(CreateObject(GetHashKey("ind_prop_dlc_roller_car"), func_51(1).X, func_51(1).Y, func_51(1).Z, false, false, false));
				else
				{
					Montagna.Carrelli[iVar0].Entity = new Prop(CreateObject(GetHashKey("ind_prop_dlc_roller_car_02"), vVar2.X, vVar2.Y, vVar2.Z, false, false, false));
					func_134(iVar0, iVar1, (Montagna.Velocità - (2.55f * (iVar0))));
				}
				FreezeEntityPosition(Montagna.Carrelli[iVar0].Entity.Handle, true);
				SetEntityLodDist(Montagna.Carrelli[iVar0].Entity.Handle, 300);
				SetEntityInvincible(Montagna.Carrelli[iVar0].Entity.Handle, true);
			}
			Montagna.Velocità = Montagna.Valore3[1 /*5*/];
			func_46(false);
			Montagna.Carrelli.ToList().ForEach(o => PlayEntityAnim(o.Entity.Handle, "idle_a_roller_car", RollerAnim, 8f, true, false, false, 0f, 0));
			iLocal_715 = 0;
			func_133();
		}

		static async void func_133()
		{
			int iVar0;
			int iVar1;
			Vector3 vVar2;

			for (iVar0 = 0; iVar0 < Montagna.Carrelli.Length; iVar0++)
			{
				iVar1 = func_138(Montagna.Velocità - (2.55f * iVar0), ValoreIndiceSconosciuto);
				vVar2 = func_50(Montagna.Velocità - (2.55f * iVar0), iVar1);
				SetEntityCoordsNoOffset(Montagna.Carrelli[iVar0].Entity.Handle, vVar2.X, vVar2.Y, vVar2.Z, true, false, false);
				func_134(iVar0, iVar1, Montagna.Velocità - (2.55f * iVar0));
			}
		}

		static async void func_46(bool bParam0)
		{
			float fVar0;
			bool bVar1;
			int iVar2;

			if (bParam0)
			{
				if (iLocal_715 != 0)
					fLocal_716 = ((GetTimeDifference(GetNetworkTimeAccurate(), iLocal_715)) / 1000f);
				iLocal_715 = GetNetworkTimeAccurate();
			}
			fVar0 = func_49();
			if (ValoreIndiceSconosciuto < 20)
			{
				if (Montagna.VariabileVelocità < 3f)
					Montagna.VariabileVelocità = (Montagna.VariabileVelocità + 0.3f);
				else
					Montagna.VariabileVelocità = (Montagna.VariabileVelocità - 0.3f);
				if (Absf((Montagna.VariabileVelocità - 3f)) < 0.3f)
					Montagna.VariabileVelocità = 3f;
			}
			else
			{
				Montagna.VariabileVelocità = (Montagna.VariabileVelocità + (fVar0 * fLocal_716));
				Montagna.VariabileVelocità = (Montagna.VariabileVelocità * 1f);
			}
			if ((Montagna.Velocità < Montagna.Valore3[1 /*5*/] && (Montagna.Velocità + (Montagna.VariabileVelocità * fLocal_716)) >= Montagna.Valore3[1 /*5*/]))
				Montagna.Velocità = Montagna.Valore3[1 /*5*/];
			else
				Montagna.Velocità = (Montagna.Velocità + (Montagna.VariabileVelocità * fLocal_716));
			bVar1 = false;
			if (Montagna.VariabileVelocità >= 0f)
			{
				if (Montagna.Velocità >= Montagna.Valore3[(Montagna.Index - 1) /*5*/])
				{
					if (Montagna.State != "ARRIVO") // aggiungere un qualche state di controllo
						Montagna.Velocità = (Montagna.Velocità - Montagna.Valore3[(Montagna.Index - 1) /*5*/]);
					else
						Montagna.Velocità = Montagna.Valore3[1 /*5*/];
					ValoreIndiceSconosciuto = 0;
				}
				iVar2 = func_48(ValoreIndiceSconosciuto);
				while (!bVar1)
				{
					if (Montagna.Velocità < Montagna.Valore3[iVar2 /*5*/])
					{
						bVar1 = true;
						if (ValoreIndiceSconosciuto != (iVar2 - 1))
							if (Montagna.Valore4[(iVar2 - 1) /*5*/] != Montagna.VariabileVelocità)
								Montagna.VariabileVelocità = Montagna.Valore4[(iVar2 - 1) /*5*/];
						ValoreIndiceSconosciuto = (iVar2 - 1);
					}
					iVar2 = func_48(iVar2);
				}
			}
			else
			{
				if (Montagna.Velocità < 0f)
				{
					Montagna.Velocità = (Montagna.Velocità + Montagna.Valore3[(Montagna.Index - 1) /*5*/]);
					ValoreIndiceSconosciuto = (Montagna.Index - 2);
				}
				iVar2 = ValoreIndiceSconosciuto;
				while (!bVar1)
				{
					if (Montagna.Valore3[iVar2 /*5*/] < Montagna.Velocità)
					{
						bVar1 = true;
						ValoreIndiceSconosciuto = iVar2;
					}
					iVar2 = func_47(iVar2);
				}
			}
			func_133();
		}

		static int func_47(int iParam0)
		{
			int iVar0;

			iVar0 = (iParam0 - 1);
			if (iVar0 < 0)
				iVar0 = (Montagna.Index - 2);
			return iVar0;
		}

		static int func_48(int iParam0)
		{
			int iVar0;
			iVar0 = iParam0 + 1;
			if (iVar0 >= Montagna.Index)
				iVar0 = 1;
			return iVar0;
		}

		static float func_49()
		{
			int iVar0;
			float fVar1;
			float fVar2;
			float fVar3;

			iVar0 = func_48(ValoreIndiceSconosciuto);
			fVar1 = (Montagna.Valore0[ValoreIndiceSconosciuto /*5*/].Z - Montagna.Valore0[iVar0 /*5*/].Z);
			fVar2 = (Montagna.Valore3[iVar0 /*5*/] - Montagna.Valore3[ValoreIndiceSconosciuto /*5*/]);
			if (fVar2 < 0f)
				fVar2 = (fVar2 + Montagna.Valore3[224 /*5*/]);
			fVar3 = Funzioni.Rad2deg((float)Math.Asin(Funzioni.Deg2rad(fVar1 / fVar2)));
			return (10f * Funzioni.Rad2deg((float)Math.Sin(Funzioni.Deg2rad(fVar3))));
		}

		static Vector3 func_50(float fParam0, int iParam1)
		{
			Vector3 vVar0;
			int iVar1;
			int iVar2;
			float fVar3;
			float fVar4;
			float fVar5;
			Vector3 vVar6;

			if (fParam0 < 0f)
				fParam0 = (fParam0 + Montagna.Valore3[(Montagna.Index - 1) /*5*/]);
			if (Montagna.VariabileVelocità >= 0f)
			{
				iVar1 = iParam1;
				iVar2 = func_48(iParam1);
			}
			else
			{
				iVar1 = func_48(iParam1);
				iVar2 = iParam1;
			}
			fVar3 = Absf((Montagna.Valore3[iVar2 /*5*/] - Montagna.Valore3[iVar1 /*5*/]));
			fVar4 = (fParam0 - Montagna.Valore3[iVar1 /*5*/]);
			fVar5 = (fVar4 / fVar3);
			vVar6 = Vector3.Subtract(func_51(iVar2), func_51(iVar1));
			if (Montagna.VariabileVelocità >= 0f)
				vVar0 = Vector3.Add(func_51(iVar1), Vector3.Multiply(vVar6, fVar5));
			else
				vVar0 = Vector3.Subtract(func_51(iVar1), Vector3.Multiply(vVar6, fVar5));
			return vVar0;
		}

		static async void func_134(int iParam0, int iParam1, float fParam2)
		{
			int iVar0;
			int iVar1;
			int iVar2;
			int iVar3;
			float fVar4;
			var uVar5 = new float[4];
			var uVar6 = new float[4];
			var uVar7 = new float[4];
			float fVar8;

			iVar0 = func_47(iParam1);
			iVar1 = iParam1;
			iVar2 = func_48(iParam1);
			iVar3 = func_48(iVar2);
			if (fParam2 < 0f)
				fParam2 = (fParam2 + Montagna.Valore3[(Montagna.Index - 1) /*5*/]);
			fVar8 = ((fParam2 - Montagna.Valore3[iVar1 /*5*/]) / (Montagna.Valore3[iVar2 /*5*/] - Montagna.Valore3[iVar1 /*5*/]));
			if (fVar8 < 0.5f)
			{
				fVar4 = (fVar8 + 0.5f);
				func_135(func_136(iVar0, iVar1), ref uVar5[0], ref uVar5[1], ref uVar5[2], ref uVar5[3]);
				func_135(func_136(iVar1, iVar2), ref uVar6[0], ref uVar6[1], ref uVar6[2], ref uVar6[3]);
			}
			else
			{
				fVar4 = (fVar8 - 0.5f);
				func_135(func_136(iVar1, iVar2), ref uVar5[0], ref uVar5[1], ref uVar5[2], ref uVar5[3]);
				func_135(func_136(iVar2, iVar3), ref uVar6[0], ref uVar6[1], ref uVar6[2], ref uVar6[3]);
			}
			SlerpNearQuaternion(fVar4, uVar5[0], uVar5[1], uVar5[2], uVar5[3], uVar6[0], uVar6[1], uVar6[2], uVar6[3], ref uVar7[0], ref uVar7[1], ref uVar7[2], ref uVar7[3]);
			SetEntityQuaternion(Montagna.Carrelli[iParam0].Entity.Handle, uVar7[0], uVar7[1], uVar7[2], uVar7[3]);
			if (World.GetDistance(Game.PlayerPed.Position, Montagna.Carrelli[0].Entity.Position) < 50f)
				if (iParam0 == 0 && iVar0 % 3 == 0)
					SetPadShake(0, 32, 32);
		}

		static void func_135(Vector3 Param0, ref float uParam1, ref float uParam2, ref float uParam3, ref float uParam4)
		{
			float fVar0;
			float fVar1;
			float fVar2;
			float fVar3;
			float fVar4;
			float fVar5;
			float fVar6;
			float fVar7;
			float fVar8;

			fVar0 = (Param0.Y / 2f);
			fVar1 = (Param0.Z / 2f);
			fVar2 = (Param0.X / 2f);
			fVar3 = Funzioni.Rad2deg((float)Math.Sin(Funzioni.Deg2rad(fVar0)));
			fVar4 = Funzioni.Rad2deg((float)Math.Sin(Funzioni.Deg2rad(fVar1)));
			fVar5 = Funzioni.Rad2deg((float)Math.Sin(Funzioni.Deg2rad(fVar2)));
			fVar6 = Funzioni.Rad2deg((float)Math.Cos(Funzioni.Deg2rad(fVar0)));
			fVar7 = Funzioni.Rad2deg((float)Math.Cos(Funzioni.Deg2rad(fVar1)));
			fVar8 = Funzioni.Rad2deg((float)Math.Cos(Funzioni.Deg2rad(fVar2)));
			uParam1 = (((fVar5 * fVar6) * fVar7) - ((fVar8 * fVar3) * fVar4));
			uParam2 = (((fVar8 * fVar3) * fVar7) + ((fVar5 * fVar6) * fVar4));
			uParam3 = (((fVar8 * fVar6) * fVar4) - ((fVar5 * fVar3) * fVar7));
			uParam4 = (((fVar8 * fVar6) * fVar7) + ((fVar5 * fVar3) * fVar4));
		}


		static Vector3 func_136(int iParam0, int iParam1)
		{
			Vector3 vVar0;
			float fVar1;
			float fVar2;

			vVar0 = func_137(Montagna.Valore0[iParam1 /*5*/] - Montagna.Valore0[iParam0 /*5*/]);
			fVar1 = Atan2(vVar0.X, vVar0.Y);
			fVar2 = Atan2(vVar0.Z, Sqrt(((vVar0.X * vVar0.X) + (vVar0.Y * vVar0.Y))));
			return new Vector3(-fVar2, 0f, (-fVar1 - 180f));
		}

		static Vector3 func_137(Vector3 vParam0)
		{
			float fVar0;
			float fVar1;

			fVar0 = Vmag(vParam0.X, vParam0.Y, vParam0.Z);
			if (fVar0 != 0f)
			{
				fVar1 = (1f / fVar0);
				vParam0 *= fVar1;
			}
			else
			{
				vParam0.X = 0f;
				vParam0.Y = 0f;
				vParam0.Z = 0f;
			}
			return vParam0;
		}

		static int func_138(float fParam0, int iParam1)
		{
			int iVar0;

			if (fParam0 <= 0f)
			{
				fParam0 = (fParam0 + Montagna.Valore3[(Montagna.Index - 1) /*5*/]);
				iParam1 = (Montagna.Index - 1);
			}
			iVar0 = iParam1;
			while (iVar0 >= 0)
			{
				if (Montagna.Valore3[iVar0 /*5*/] < fParam0)
					return iVar0;
				iVar0 = (iVar0 + -1);
			}
			return 0;
		}

		static void func_220()
		{
			func_226(new Vector3(-1659.01f, -1143.129f, 17.4192f));
			func_226(new Vector3(-1643.524f, -1124.681f, 17.4326f));
			func_226(new Vector3(-1639.621f, -1120.021f, 17.6357f));
			func_226(new Vector3(-1638.199f, -1118.316f, 17.9966f));
			func_226(new Vector3(-1637.011f, -1116.896f, 18.5407f));
			func_226(new Vector3(-1635.772f, -1115.417f, 19.2558f));
			func_226(new Vector3(-1634.227f, -1113.569f, 20.1725f));
			func_226(new Vector3(-1632.692f, -1111.734f, 21.0835f));
			func_226(new Vector3(-1631.179f, -1109.922f, 21.9826f));
			func_226(new Vector3(-1629.692f, -1108.145f, 22.865f));
			func_226(new Vector3(-1628.243f, -1106.411f, 23.7252f));
			func_226(new Vector3(-1626.84f, -1104.733f, 24.558f));
			func_226(new Vector3(-1625.491f, -1103.12f, 25.3588f));
			func_226(new Vector3(-1624.206f, -1101.582f, 26.1218f));
			func_226(new Vector3(-1622.992f, -1100.13f, 26.8424f));
			func_226(new Vector3(-1620.721f, -1097.416f, 28.1892f));
			func_226(new Vector3(-1618.866f, -1095.196f, 29.2895f));
			func_226(new Vector3(-1617.533f, -1093.603f, 30.0795f));
			func_226(new Vector3(-1616.778f, -1092.699f, 30.5401f));
			func_226(new Vector3(-1615.677f, -1091.388f, 30.9156f));
			func_226(new Vector3(-1614.829f, -1090.377f, 31.0008f));
			func_226(new Vector3(-1614.011f, -1089.406f, 30.9417f));
			func_226(new Vector3(-1612.615f, -1087.747f, 30.3463f));
			func_226(new Vector3(-1610.992f, -1085.82f, 29.1724f));
			func_226(new Vector3(-1609.228f, -1083.725f, 27.949f));
			func_226(new Vector3(-1608.295f, -1082.615f, 27.4861f));
			func_226(new Vector3(-1606.937f, -1081.002f, 27.4328f));
			func_226(new Vector3(-1605.471f, -1079.258f, 27.5762f));
			func_226(new Vector3(-1604.159f, -1077.701f, 28.0216f));
			func_226(new Vector3(-1602.511f, -1075.749f, 28.5244f));
			func_226(new Vector3(-1600.932f, -1073.873f, 28.9813f));
			func_226(new Vector3(-1599.342f, -1071.983f, 29.1756f));
			func_226(new Vector3(-1597.851f, -1070.067f, 29.1552f));
			func_226(new Vector3(-1596.723f, -1067.995f, 29.0611f));
			func_226(new Vector3(-1596.123f, -1065.708f, 28.9503f));
			func_226(new Vector3(-1595.991f, -1063.354f, 28.8316f));
			func_226(new Vector3(-1596.365f, -1061.041f, 28.7074f));
			func_226(new Vector3(-1597.254f, -1058.857f, 28.577f));
			func_226(new Vector3(-1598.562f, -1056.894f, 28.4423f));
			func_226(new Vector3(-1600.27f, -1055.292f, 28.3045f));
			func_226(new Vector3(-1602.288f, -1054.077f, 28.163f));
			func_226(new Vector3(-1604.497f, -1053.295f, 28.019f));
			func_226(new Vector3(-1606.845f, -1053.063f, 27.8712f));
			func_226(new Vector3(-1609.193f, -1053.3f, 27.7214f));
			func_226(new Vector3(-1611.416f, -1054.029f, 27.5695f));
			func_226(new Vector3(-1613.432f, -1055.248f, 27.4148f));
			func_226(new Vector3(-1615.167f, -1056.844f, 27.2581f));
			func_226(new Vector3(-1616.486f, -1058.782f, 27.0998f));
			func_226(new Vector3(-1617.371f, -1060.964f, 26.9395f));
			func_226(new Vector3(-1617.803f, -1063.281f, 26.7771f));
			func_226(new Vector3(-1617.669f, -1065.625f, 26.6138f));
			func_226(new Vector3(-1617.071f, -1067.903f, 26.4484f));
			func_226(new Vector3(-1616.006f, -1069.994f, 26.2817f));
			func_226(new Vector3(-1614.489f, -1071.798f, 26.1132f));
			func_226(new Vector3(-1612.646f, -1073.265f, 25.9435f));
			func_226(new Vector3(-1610.523f, -1074.272f, 25.7722f));
			func_226(new Vector3(-1608.231f, -1074.807f, 25.5996f));
			func_226(new Vector3(-1605.875f, -1074.877f, 25.4258f));
			func_226(new Vector3(-1603.576f, -1074.385f, 25.251f));
			func_226(new Vector3(-1601.417f, -1073.441f, 25.0748f));
			func_226(new Vector3(-1599.508f, -1072.067f, 24.8974f));
			func_226(new Vector3(-1597.961f, -1070.289f, 24.7188f));
			func_226(new Vector3(-1596.798f, -1068.241f, 24.5393f));
			func_226(new Vector3(-1596.121f, -1065.987f, 24.3586f));
			func_226(new Vector3(-1595.946f, -1063.637f, 24.177f));
			func_226(new Vector3(-1596.242f, -1061.301f, 23.9942f));
			func_226(new Vector3(-1597.079f, -1059.097f, 23.8103f));
			func_226(new Vector3(-1598.345f, -1057.109f, 23.6258f));
			func_226(new Vector3(-1599.996f, -1055.426f, 23.44f));
			func_226(new Vector3(-1601.991f, -1054.172f, 23.2534f));
			func_226(new Vector3(-1604.195f, -1053.339f, 23.0661f));
			func_226(new Vector3(-1606.533f, -1053.01f, 22.8773f));
			func_226(new Vector3(-1608.881f, -1053.199f, 22.6882f));
			func_226(new Vector3(-1611.144f, -1053.85f, 22.4984f));
			func_226(new Vector3(-1613.199f, -1055.015f, 22.3068f));
			func_226(new Vector3(-1614.982f, -1056.581f, 22.1126f));
			func_226(new Vector3(-1616.545f, -1058.398f, 21.7888f));
			func_226(new Vector3(-1618.098f, -1060.261f, 21.3373f));
			func_226(new Vector3(-1619.583f, -1062.043f, 20.7536f));
			func_226(new Vector3(-1621.058f, -1063.813f, 20.1778f));
			func_226(new Vector3(-1622.535f, -1065.582f, 19.6021f));
			func_226(new Vector3(-1624.009f, -1067.352f, 19.0262f));
			func_226(new Vector3(-1625.482f, -1069.119f, 18.4527f));
			func_226(new Vector3(-1626.88f, -1070.806f, 17.9515f));
			func_226(new Vector3(-1628.218f, -1072.426f, 17.7058f));
			func_226(new Vector3(-1629.509f, -1073.975f, 17.7076f));
			func_226(new Vector3(-1631.046f, -1075.816f, 17.7079f));
			func_226(new Vector3(-1632.36f, -1077.393f, 17.7079f));
			func_226(new Vector3(-1633.897f, -1079.234f, 17.7081f));
			func_226(new Vector3(-1635.397f, -1080.972f, 17.7074f));
			func_226(new Vector3(-1636.924f, -1082.801f, 17.7074f));
			func_226(new Vector3(-1638.383f, -1084.535f, 17.8395f));
			func_226(new Vector3(-1639.644f, -1086.005f, 18.3624f));
			func_226(new Vector3(-1640.985f, -1087.563f, 19.3675f));
			func_226(new Vector3(-1642.482f, -1089.276f, 20.5799f));
			func_226(new Vector3(-1644.108f, -1091.096f, 21.5914f));
			func_226(new Vector3(-1645.844f, -1092.97f, 21.9359f));
			func_226(new Vector3(-1647.561f, -1094.781f, 21.6225f));
			func_226(new Vector3(-1649.239f, -1096.506f, 20.9627f));
			func_226(new Vector3(-1650.894f, -1098.148f, 20.1969f));
			func_226(new Vector3(-1652.535f, -1099.704f, 19.525f));
			func_226(new Vector3(-1654.248f, -1101.247f, 18.9923f));
			func_226(new Vector3(-1656.05f, -1102.794f, 18.5631f));
			func_226(new Vector3(-1657.911f, -1104.315f, 18.2393f));
			func_226(new Vector3(-1659.798f, -1105.782f, 18.0219f));
			func_226(new Vector3(-1661.681f, -1107.168f, 17.911f));
			func_226(new Vector3(-1663.525f, -1108.445f, 17.9064f));
			func_226(new Vector3(-1665.293f, -1109.582f, 18.0057f));
			func_226(new Vector3(-1667.317f, -1110.773f, 18.2989f));
			func_226(new Vector3(-1669.263f, -1111.836f, 18.8213f));
			func_226(new Vector3(-1671.144f, -1112.787f, 19.5262f));
			func_226(new Vector3(-1673.022f, -1113.685f, 20.3691f));
			func_226(new Vector3(-1674.958f, -1114.582f, 21.2989f));
			func_226(new Vector3(-1676.995f, -1115.534f, 22.249f));
			func_226(new Vector3(-1679.084f, -1116.478f, 23.1368f));
			func_226(new Vector3(-1681.219f, -1117.389f, 23.9532f));
			func_226(new Vector3(-1683.374f, -1118.29f, 24.6845f));
			func_226(new Vector3(-1685.517f, -1119.208f, 25.3158f));
			func_226(new Vector3(-1687.62f, -1120.167f, 25.8329f));
			func_226(new Vector3(-1689.705f, -1121.188f, 26.2178f));
			func_226(new Vector3(-1691.772f, -1122.315f, 26.5427f));
			func_226(new Vector3(-1693.635f, -1123.75f, 26.845f));
			func_226(new Vector3(-1695.254f, -1125.474f, 27.1175f));
			func_226(new Vector3(-1696.581f, -1127.444f, 27.3373f));
			func_226(new Vector3(-1697.574f, -1129.602f, 27.5003f));
			func_226(new Vector3(-1698.207f, -1131.882f, 27.623f));
			func_226(new Vector3(-1698.465f, -1134.234f, 27.7231f));
			func_226(new Vector3(-1698.344f, -1136.602f, 27.7949f));
			func_226(new Vector3(-1697.841f, -1138.921f, 27.8328f));
			func_226(new Vector3(-1696.972f, -1141.131f, 27.8321f));
			func_226(new Vector3(-1695.759f, -1143.174f, 27.7892f));
			func_226(new Vector3(-1694.234f, -1144.995f, 27.7019f));
			func_226(new Vector3(-1692.435f, -1146.544f, 27.5687f));
			func_226(new Vector3(-1690.415f, -1147.784f, 27.39f));
			func_226(new Vector3(-1688.226f, -1148.682f, 27.1671f));
			func_226(new Vector3(-1685.926f, -1149.218f, 26.9025f));
			func_226(new Vector3(-1683.576f, -1149.376f, 26.5994f));
			func_226(new Vector3(-1681.237f, -1149.161f, 26.2631f));
			func_226(new Vector3(-1678.968f, -1148.578f, 25.8985f));
			func_226(new Vector3(-1676.825f, -1147.645f, 25.5118f));
			func_226(new Vector3(-1674.862f, -1146.388f, 25.1091f));
			func_226(new Vector3(-1673.124f, -1144.839f, 24.6968f));
			func_226(new Vector3(-1671.654f, -1143.039f, 24.2822f));
			func_226(new Vector3(-1670.486f, -1141.031f, 23.8716f));
			func_226(new Vector3(-1669.649f, -1138.865f, 23.472f));
			func_226(new Vector3(-1669.163f, -1136.595f, 23.0898f));
			func_226(new Vector3(-1669.04f, -1134.274f, 22.7307f));
			func_226(new Vector3(-1669.284f, -1131.962f, 22.4006f));
			func_226(new Vector3(-1669.888f, -1129.713f, 22.1045f));
			func_226(new Vector3(-1670.841f, -1127.585f, 21.8463f));
			func_226(new Vector3(-1672.12f, -1125.632f, 21.6294f));
			func_226(new Vector3(-1673.694f, -1123.904f, 21.4559f));
			func_226(new Vector3(-1675.523f, -1122.444f, 21.327f));
			func_226(new Vector3(-1677.563f, -1121.29f, 21.2429f));
			func_226(new Vector3(-1679.762f, -1120.476f, 21.2021f));
			func_226(new Vector3(-1682.064f, -1120.019f, 21.2025f));
			func_226(new Vector3(-1684.41f, -1119.933f, 21.2408f));
			func_226(new Vector3(-1686.742f, -1120.221f, 21.3125f));
			func_226(new Vector3(-1689f, -1120.877f, 21.4123f));
			func_226(new Vector3(-1691.128f, -1121.884f, 21.5343f));
			func_226(new Vector3(-1693.069f, -1123.218f, 21.672f));
			func_226(new Vector3(-1694.779f, -1124.849f, 21.8191f));
			func_226(new Vector3(-1695.93f, -1126.324f, 21.9029f));
			func_226(new Vector3(-1696.878f, -1127.99f, 21.9873f));
			func_226(new Vector3(-1697.674f, -1129.878f, 22.0778f));
			func_226(new Vector3(-1698.292f, -1131.96f, 22.1685f));
			func_226(new Vector3(-1698.699f, -1134.206f, 22.2533f));
			func_226(new Vector3(-1698.866f, -1136.587f, 22.3261f));
			func_226(new Vector3(-1698.764f, -1139.072f, 22.3807f));
			func_226(new Vector3(-1698.363f, -1141.633f, 22.4112f));
			func_226(new Vector3(-1697.633f, -1144.24f, 22.4113f));
			func_226(new Vector3(-1696.546f, -1146.863f, 22.3751f));
			func_226(new Vector3(-1695.061f, -1149.484f, 22.295f));
			func_226(new Vector3(-1693.239f, -1151.881f, 22.1555f));
			func_226(new Vector3(-1691.225f, -1153.872f, 21.965f));
			func_226(new Vector3(-1689.074f, -1155.483f, 21.737f));
			func_226(new Vector3(-1686.842f, -1156.74f, 21.485f));
			func_226(new Vector3(-1684.583f, -1157.674f, 21.2224f));
			func_226(new Vector3(-1682.351f, -1158.311f, 20.9625f));
			func_226(new Vector3(-1680.161f, -1158.67f, 20.7161f));
			func_226(new Vector3(-1678.176f, -1158.802f, 20.5023f));
			func_226(new Vector3(-1676.344f, -1158.712f, 20.3287f));
			func_226(new Vector3(-1674.755f, -1158.437f, 20.2097f));
			func_226(new Vector3(-1672.606f, -1157.658f, 20.0965f));
			func_226(new Vector3(-1670.584f, -1156.442f, 19.9803f));
			func_226(new Vector3(-1668.831f, -1154.866f, 19.8642f));
			func_226(new Vector3(-1667.418f, -1152.986f, 19.7482f));
			func_226(new Vector3(-1666.452f, -1150.833f, 19.6319f));
			func_226(new Vector3(-1665.911f, -1148.538f, 19.5158f));
			func_226(new Vector3(-1665.817f, -1146.181f, 19.3996f));
			func_226(new Vector3(-1666.207f, -1143.862f, 19.2836f));
			func_226(new Vector3(-1667.073f, -1141.668f, 19.1674f));
			func_226(new Vector3(-1668.339f, -1139.679f, 19.0512f));
			func_226(new Vector3(-1669.962f, -1137.965f, 18.935f));
			func_226(new Vector3(-1671.913f, -1136.65f, 18.8189f));
			func_226(new Vector3(-1674.087f, -1135.737f, 18.7028f));
			func_226(new Vector3(-1676.397f, -1135.256f, 18.5866f));
			func_226(new Vector3(-1678.751f, -1135.237f, 18.4705f));
			func_226(new Vector3(-1681.058f, -1135.73f, 18.3543f));
			func_226(new Vector3(-1683.23f, -1136.65f, 18.2382f));
			func_226(new Vector3(-1685.187f, -1137.968f, 18.1219f));
			func_226(new Vector3(-1686.824f, -1139.661f, 18.0059f));
			func_226(new Vector3(-1688.081f, -1141.657f, 17.8896f));
			func_226(new Vector3(-1688.938f, -1143.855f, 17.7735f));
			func_226(new Vector3(-1689.359f, -1146.177f, 17.6571f));
			func_226(new Vector3(-1689.26f, -1148.532f, 17.5411f));
			func_226(new Vector3(-1688.71f, -1150.826f, 17.425f));
			func_226(new Vector3(-1687.733f, -1152.976f, 17.3087f));
			func_226(new Vector3(-1686.342f, -1154.887f, 17.1759f));
			func_226(new Vector3(-1684.573f, -1156.462f, 17.0021f));
			func_226(new Vector3(-1682.54f, -1157.669f, 16.7987f));
			func_226(new Vector3(-1680.313f, -1158.466f, 16.5838f));
			func_226(new Vector3(-1677.973f, -1158.77f, 16.3415f));
			func_226(new Vector3(-1675.626f, -1158.601f, 16.0948f));
			func_226(new Vector3(-1673.361f, -1157.994f, 15.9205f));
			func_226(new Vector3(-1671.255f, -1156.966f, 15.8075f));
			func_226(new Vector3(-1669.435f, -1155.511f, 15.7719f));
			func_226(new Vector3(-1667.848f, -1153.66f, 15.766f));
			func_226(new Vector3(-1666.33f, -1151.852f, 15.7703f));
			func_226(new Vector3(-1664.875f, -1150.117f, 15.8984f));
			func_226(new Vector3(-1663.46f, -1148.431f, 16.198f));
			func_226(new Vector3(-1662.033f, -1146.731f, 16.6412f));
			func_226(new Vector3(-1660.556f, -1144.97f, 17.1643f));
			func_226(new Vector3(-1659.01f, -1143.129f, 17.4192f));
			func_225((Montagna.Index - 1), Montagna.Valore0[0 /*5*/]);
			func_221();
			Montagna.Valore4[0 /*5*/] = 0;
			Montagna.Valore4[1 /*5*/] = 0.3f;
			Montagna.Valore4[2 /*5*/] = 0.3f;
			Montagna.Valore4[3 /*5*/] = 3f;
			Montagna.Valore4[4 /*5*/] = 3f;
			Montagna.Valore4[5 /*5*/] = 3f;
			Montagna.Valore4[6 /*5*/] = 3f;
			Montagna.Valore4[7 /*5*/] = 3f;
			Montagna.Valore4[8 /*5*/] = 3f;
			Montagna.Valore4[9 /*5*/] = 3f;
			Montagna.Valore4[10 /*5*/] = 3f;
			Montagna.Valore4[11 /*5*/] = 3f;
			Montagna.Valore4[12 /*5*/] = 3f;
			Montagna.Valore4[13 /*5*/] = 3f;
			Montagna.Valore4[14 /*5*/] = 3f;
			Montagna.Valore4[15 /*5*/] = 3f;
			Montagna.Valore4[16 /*5*/] = 3f;
			Montagna.Valore4[17 /*5*/] = 3f;
			Montagna.Valore4[18 /*5*/] = 3f;
			Montagna.Valore4[19 /*5*/] = 3f;
			Montagna.Valore4[20 /*5*/] = 3f;
			Montagna.Valore4[21 /*5*/] = 3f;
			Montagna.Valore4[22 /*5*/] = 3.1794f;
			Montagna.Valore4[23 /*5*/] = 4.8025f;
			Montagna.Valore4[24 /*5*/] = 6.7585f;
			Montagna.Valore4[25 /*5*/] = 8.3448f;
			Montagna.Valore4[26 /*5*/] = 8.8436f;
			Montagna.Valore4[27 /*5*/] = 8.9045f;
			Montagna.Valore4[28 /*5*/] = 8.7073f;
			Montagna.Valore4[29 /*5*/] = 8.1965f;
			Montagna.Valore4[30 /*5*/] = 7.5921f;
			Montagna.Valore4[31 /*5*/] = 7.0097f;
			Montagna.Valore4[32 /*5*/] = 6.6959f;
			Montagna.Valore4[33 /*5*/] = 6.7221f;
			Montagna.Valore4[34 /*5*/] = 6.8771f;
			Montagna.Valore4[35 /*5*/] = 7.0232f;
			Montagna.Valore4[36 /*5*/] = 7.18f;
			Montagna.Valore4[37 /*5*/] = 7.3457f;
			Montagna.Valore4[38 /*5*/] = 7.5605f;
			Montagna.Valore4[39 /*5*/] = 7.6956f;
			Montagna.Valore4[40 /*5*/] = 7.8806f;
			Montagna.Valore4[41 /*5*/] = 8.0659f;
			Montagna.Valore4[42 /*5*/] = 8.2597f;
			Montagna.Valore4[43 /*5*/] = 8.4085f;
			Montagna.Valore4[44 /*5*/] = 8.6081f;
			Montagna.Valore4[45 /*5*/] = 8.7629f;
			Montagna.Valore4[46 /*5*/] = 8.9647f;
			Montagna.Valore4[47 /*5*/] = 9.1286f;
			Montagna.Valore4[48 /*5*/] = 9.2889f;
			Montagna.Valore4[49 /*5*/] = 9.4513f;
			Montagna.Valore4[50 /*5*/] = 9.6107f;
			Montagna.Valore4[51 /*5*/] = 9.8224f;
			Montagna.Valore4[52 /*5*/] = 9.9849f;
			Montagna.Valore4[53 /*5*/] = 10.1485f;
			Montagna.Valore4[54 /*5*/] = 10.3112f;
			Montagna.Valore4[55 /*5*/] = 10.4793f;
			Montagna.Valore4[56 /*5*/] = 10.648f;
			Montagna.Valore4[57 /*5*/] = 10.7657f;
			Montagna.Valore4[58 /*5*/] = 10.9378f;
			Montagna.Valore4[59 /*5*/] = 11.1113f;
			Montagna.Valore4[60 /*5*/] = 11.285f;
			Montagna.Valore4[61 /*5*/] = 11.4038f;
			Montagna.Valore4[62 /*5*/] = 11.5785f;
			Montagna.Valore4[63 /*5*/] = 11.7563f;
			Montagna.Valore4[64 /*5*/] = 11.8826f;
			Montagna.Valore4[65 /*5*/] = 12.0063f;
			Montagna.Valore4[66 /*5*/] = 12.1858f;
			Montagna.Valore4[67 /*5*/] = 12.311f;
			Montagna.Valore4[68 /*5*/] = 12.4905f;
			Montagna.Valore4[69 /*5*/] = 12.6186f;
			Montagna.Valore4[70 /*5*/] = 12.752f;
			Montagna.Valore4[71 /*5*/] = 12.9366f;
			Montagna.Valore4[72 /*5*/] = 13.069f;
			Montagna.Valore4[73 /*5*/] = 13.2002f;
			Montagna.Valore4[74 /*5*/] = 13.3271f;
			Montagna.Valore4[75 /*5*/] = 13.5131f;
			Montagna.Valore4[76 /*5*/] = 13.6428f;
			Montagna.Valore4[77 /*5*/] = 13.8557f;
			Montagna.Valore4[78 /*5*/] = 14.1467f;
			Montagna.Valore4[79 /*5*/] = 14.7078f;
			Montagna.Valore4[80 /*5*/] = 15.0933f;
			Montagna.Valore4[81 /*5*/] = 15.4812f;
			Montagna.Valore4[82 /*5*/] = 15.6897f;
			Montagna.Valore4[83 /*5*/] = 16.072f;
			Montagna.Valore4[84 /*5*/] = 16.4288f;
			Montagna.Valore4[85 /*5*/] = 16.6158f;
			Montagna.Valore4[86 /*5*/] = 16.615f;
			Montagna.Valore4[87 /*5*/] = 16.6148f;
			Montagna.Valore4[88 /*5*/] = 16.6148f;
			Montagna.Valore4[89 /*5*/] = 16.6147f;
			Montagna.Valore4[90 /*5*/] = 16.6151f;
			Montagna.Valore4[91 /*5*/] = 16.6151f;
			Montagna.Valore4[92 /*5*/] = 16.5645f;
			Montagna.Valore4[93 /*5*/] = 16.145f;
			Montagna.Valore4[94 /*5*/] = 15.4464f;
			Montagna.Valore4[95 /*5*/] = 14.6939f;
			Montagna.Valore4[96 /*5*/] = 14.0775f;
			Montagna.Valore4[97 /*5*/] = 13.7741f;
			Montagna.Valore4[98 /*5*/] = 13.9723f;
			Montagna.Valore4[99 /*5*/] = 14.39f;
			Montagna.Valore4[100 /*5*/] = 14.8988f;
			Montagna.Valore4[101 /*5*/] = 15.3516f;
			Montagna.Valore4[102 /*5*/] = 15.7118f;
			Montagna.Valore4[103 /*5*/] = 15.9945f;
			Montagna.Valore4[104 /*5*/] = 16.2069f;
			Montagna.Valore4[105 /*5*/] = 16.3518f;
			Montagna.Valore4[106 /*5*/] = 16.3944f;
			Montagna.Valore4[107 /*5*/] = 16.3977f;
			Montagna.Valore4[108 /*5*/] = 16.3236f;
			Montagna.Valore4[109 /*5*/] = 16.1241f;
			Montagna.Valore4[110 /*5*/] = 15.9292f;
			Montagna.Valore4[111 /*5*/] = 15.4375f;
			Montagna.Valore4[112 /*5*/] = 14.8409f;
			Montagna.Valore4[113 /*5*/] = 14.2057f;
			Montagna.Valore4[114 /*5*/] = 13.5752f;
			Montagna.Valore4[115 /*5*/] = 12.7336f;
			Montagna.Valore4[116 /*5*/] = 12.2095f;
			Montagna.Valore4[117 /*5*/] = 11.5221f;
			Montagna.Valore4[118 /*5*/] = 11.0986f;
			Montagna.Valore4[119 /*5*/] = 10.6031f;
			Montagna.Valore4[120 /*5*/] = 10.2269f;
			Montagna.Valore4[121 /*5*/] = 9.9111f;
			Montagna.Valore4[122 /*5*/] = 9.5337f;
			Montagna.Valore4[123 /*5*/] = 9.2694f;
			Montagna.Valore4[124 /*5*/] = 9.0583f;
			Montagna.Valore4[125 /*5*/] = 8.8516f;
			Montagna.Valore4[126 /*5*/] = 8.7288f;
			Montagna.Valore4[127 /*5*/] = 8.6021f;
			Montagna.Valore4[128 /*5*/] = 8.51f;
			Montagna.Valore4[129 /*5*/] = 8.4733f;
			Montagna.Valore4[130 /*5*/] = 8.4742f;
			Montagna.Valore4[131 /*5*/] = 8.5294f;
			Montagna.Valore4[132 /*5*/] = 8.6157f;
			Montagna.Valore4[133 /*5*/] = 8.7849f;
			Montagna.Valore4[134 /*5*/] = 8.9639f;
			Montagna.Valore4[135 /*5*/] = 9.2546f;
			Montagna.Valore4[136 /*5*/] = 9.514f;
			Montagna.Valore4[137 /*5*/] = 9.8293f;
			Montagna.Valore4[138 /*5*/] = 10.1114f;
			Montagna.Valore4[139 /*5*/] = 10.5312f;
			Montagna.Valore4[140 /*5*/] = 10.9067f;
			Montagna.Valore4[141 /*5*/] = 11.1773f;
			Montagna.Valore4[142 /*5*/] = 11.5836f;
			Montagna.Valore4[143 /*5*/] = 11.9962f;
			Montagna.Valore4[144 /*5*/] = 12.2748f;
			Montagna.Valore4[145 /*5*/] = 12.6632f;
			Montagna.Valore4[146 /*5*/] = 12.9328f;
			Montagna.Valore4[147 /*5*/] = 13.1711f;
			Montagna.Valore4[148 /*5*/] = 13.3959f;
			Montagna.Valore4[149 /*5*/] = 13.6873f;
			Montagna.Valore4[150 /*5*/] = 13.8612f;
			Montagna.Valore4[151 /*5*/] = 14.0083f;
			Montagna.Valore4[152 /*5*/] = 14.1297f;
			Montagna.Valore4[153 /*5*/] = 14.2155f;
			Montagna.Valore4[154 /*5*/] = 14.2732f;
			Montagna.Valore4[155 /*5*/] = 14.3011f;
			Montagna.Valore4[156 /*5*/] = 14.3008f;
			Montagna.Valore4[157 /*5*/] = 14.2746f;
			Montagna.Valore4[158 /*5*/] = 14.2251f;
			Montagna.Valore4[159 /*5*/] = 14.1577f;
			Montagna.Valore4[160 /*5*/] = 14.0733f;
			Montagna.Valore4[161 /*5*/] = 13.9385f;
			Montagna.Valore4[162 /*5*/] = 13.8385f;
			Montagna.Valore4[163 /*5*/] = 13.7996f;
			Montagna.Valore4[164 /*5*/] = 13.7301f;
			Montagna.Valore4[165 /*5*/] = 13.6586f;
			Montagna.Valore4[166 /*5*/] = 13.5902f;
			Montagna.Valore4[167 /*5*/] = 13.5308f;
			Montagna.Valore4[168 /*5*/] = 13.482f;
			Montagna.Valore4[169 /*5*/] = 13.4313f;
			Montagna.Valore4[170 /*5*/] = 13.4127f;
			Montagna.Valore4[171 /*5*/] = 13.4126f;
			Montagna.Valore4[172 /*5*/] = 13.433f;
			Montagna.Valore4[173 /*5*/] = 13.4949f;
			Montagna.Valore4[174 /*5*/] = 13.6046f;
			Montagna.Valore4[175 /*5*/] = 13.761f;
			Montagna.Valore4[176 /*5*/] = 13.8937f;
			Montagna.Valore4[177 /*5*/] = 14.1199f;
			Montagna.Valore4[178 /*5*/] = 14.2919f;
			Montagna.Valore4[179 /*5*/] = 14.4699f;
			Montagna.Valore4[180 /*5*/] = 14.6443f;
			Montagna.Valore4[181 /*5*/] = 14.7383f;
			Montagna.Valore4[182 /*5*/] = 14.889f;
			Montagna.Valore4[183 /*5*/] = 14.9538f;
			Montagna.Valore4[184 /*5*/] = 15.0334f;
			Montagna.Valore4[185 /*5*/] = 15.1141f;
			Montagna.Valore4[186 /*5*/] = 15.1923f;
			Montagna.Valore4[187 /*5*/] = 15.2712f;
			Montagna.Valore4[188 /*5*/] = 15.3499f;
			Montagna.Valore4[189 /*5*/] = 15.4286f;
			Montagna.Valore4[190 /*5*/] = 15.5073f;
			Montagna.Valore4[191 /*5*/] = 15.5512f;
			Montagna.Valore4[192 /*5*/] = 15.6299f;
			Montagna.Valore4[193 /*5*/] = 15.7082f;
			Montagna.Valore4[194 /*5*/] = 15.7859f;
			Montagna.Valore4[195 /*5*/] = 15.8647f;
			Montagna.Valore4[196 /*5*/] = 15.9434f;
			Montagna.Valore4[197 /*5*/] = 16.0226f;
			Montagna.Valore4[198 /*5*/] = 16.0645f;
			Montagna.Valore4[199 /*5*/] = 16.1422f;
			Montagna.Valore4[200 /*5*/] = 16.2213f;
			Montagna.Valore4[201 /*5*/] = 16.2996f;
			Montagna.Valore4[202 /*5*/] = 16.3783f;
			Montagna.Valore4[203 /*5*/] = 16.4226f;
			Montagna.Valore4[204 /*5*/] = 16.5003f;
			Montagna.Valore4[205 /*5*/] = 16.5796f;
			Montagna.Valore4[206 /*5*/] = 16.6622f;
			Montagna.Valore4[207 /*5*/] = 16.7055f;
			Montagna.Valore4[208 /*5*/] = 16.7837f;
			Montagna.Valore4[209 /*5*/] = 16.8729f;
			Montagna.Valore4[210 /*5*/] = 16.9907f;
			Montagna.Valore4[211 /*5*/] = 17.0661f;
			Montagna.Valore4[212 /*5*/] = 17.2127f;
			Montagna.Valore4[213 /*5*/] = 17.3761f;
			Montagna.Valore4[214 /*5*/] = 17.4689f;
			Montagna.Valore4[215 /*5*/] = 17.5883f;
			Montagna.Valore4[216 /*5*/] = 17.6648f;
			Montagna.Valore4[217 /*5*/] = 17.6783f;
			Montagna.Valore4[218 /*5*/] = 17.6822f;
			Montagna.Valore4[219 /*5*/] = 17.6806f;
			Montagna.Valore4[220 /*5*/] = 17.5908f;
			Montagna.Valore4[221 /*5*/] = 17.371f;
			Montagna.Valore4[222 /*5*/] = 17.1986f;
			Montagna.Valore4[223 /*5*/] = 16.8458f;
		}


		private static void ForceState(string state) => Montagna.State = state;

		static Vector3 func_51(int iParam0) => Montagna.Valore0[iParam0 /*5*/];


		static async void func_221()
		{
			int iVar0;
			float fVar1;

			fVar1 = 0f;
			for (iVar0 = 0; iVar0 < Montagna.Valore0.Length; iVar0++)
			{
				if (!func_222(iVar0))
				{
					Montagna.Valore3[iVar0 /*5*/] = fVar1;
					if (iVar0 < 224)
					{
						fVar1 = (fVar1 + Vdist(func_51(iVar0).X, func_51(iVar0).Y, func_51(iVar0).Z, func_51(iVar0 + 1).X, func_51(iVar0 + 1).Y, func_51(iVar0 + 1).Z));
					}
				}
				else return;
			}
		}

		static bool func_222(int iParam0) => func_223(Montagna.Valore0[iParam0 /*5*/]);
		static bool func_223(Vector3 vParam0) => func_224(vParam0, new Vector3(0), false);

		static bool func_224(Vector3 vParam0, Vector3 vParam1, bool bParam2)
		{
			if (bParam2) return vParam0.X == vParam1.X && vParam0.Y == vParam1.Y;
			return vParam0.X == vParam1.X && vParam0.Y == vParam1.Y && vParam0.Z == vParam1.Z;
		}

		static void func_225(int iParam0, Vector3 vParam1)
		{
			if (iParam0 >= 0 && iParam0 <= 225)
				Montagna.Valore0[iParam0 /*5*/] = vParam1;
		}

		static void func_226(Vector3 vParam0)
		{
			if (Montagna.Index < 225 && Montagna.Index >= 0)
			{
				func_225(Montagna.Index, vParam0);
				Montagna.Index++;
			}
		}
	}

	internal class MontagnaRussaNew
	{
		public float Velocità;
		public float VariabileVelocità = 0;
		public int Index = 0;
		public string State = "ATTESA";
		public CarrelloMontagna[] Carrelli = new CarrelloMontagna[4]
		{
			new CarrelloMontagna(),
			new CarrelloMontagna(),
			new CarrelloMontagna(),
			new CarrelloMontagna()
		};
		public Vector3[] Valore0 = new Vector3[225];
		public float[] Valore4 = new float[225];
		public float[] Valore3 = new float[225];
	}

	internal class CarrelloMontagna
	{
		public Prop Entity = new Prop(0);
		public int Occupato = 0;
	}

	internal class MontagnaRussaCamGioco
	{
		public Camera CamEntity = new Camera(0);
		public float Valore0 = 0;
		public Vector3 Valore1;
		public float Valore2 = 0;
		public float Valore3 = 0;
		public Vector3 Valore4;
		public float Valore5 = 0;
		public float Valore6 = 0;
		public float Valore7 = 0;
		public Vector3 Valore8;
		public float Valore9 = 0;
		public float Valore10 = 0;
		public Vector3 Valore11;
		public float Valore12 = 0;
		public float Valore13 = 0;
		public Vector3 Valore14;
		public float Valore15 = 0;
		public float Valore16 = 0;
		public float Valore17 = 0;
		public float Valore18 = 0;
		public int Valore19 = 0;
		public int Valore20 = 0;
		public int Valore21 = 0;
		public int Valore22 = 0;
		public int Valore23 = 0;
		public int Valore24 = 0;
		public int Valore25 = 0;
		public bool Valore26 = false;
		public float Valore27 = 0;
		public bool Valore28 = false;
		public float Valore29 = 0;
		public float Valore30 = 0;
	}

	internal class MontagnaRussaCamCinematica
	{
		public Camera CamEntity = new Camera(0);
		public bool Valore0 = false;
		public bool Valore1 = false;
		public Vector3 Valore2 = new Vector3(0);
		public float Valore3 = 0;
		public float Valore4;
		public Vector3 Valore5 = new Vector3(0);
		public float Valore6 = 0;
		public float Valore7 = 0;
		public int Valore8;
		public int Valore9 = 0;
		public float Valore10 = 0;
		public float Valore11;
		public float Valore12 = 0;
		public float Valore13 = 0;
		public bool Valore14 = true;
		public bool Valore15 = true;
		public float Valore16 = 0;
		public float Valore17 = 0;
		public float Valore18 = 0;
		public int Valore19 = 0;
		public int Valore20 = 0;
		public int Valore21 = 0;
		public int Valore22 = 0;
		public int Valore23 = 0;
		public int Valore24 = 0;
		public int Valore25 = 0;
		public float Valore26 = 0;
		public float Valore27 = 0;
		public float Valore28 = 0;
		public float Valore29 = 0;
		public float Valore30 = 0;
	}
}
