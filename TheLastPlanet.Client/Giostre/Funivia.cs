using CitizenFX.Core;
using CitizenFX.Core.Native;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.Giostre
{
	internal static class Funivia
	{
		private static float[] Speeds = new float[2] { 1.0f, .05f };

		private static Dictionary<int, List<Vector3>> Tracks = new Dictionary<int, List<Vector3>>
		{
			[0] = new List<Vector3>
			{
				new Vector3(-740.911f, 5599.341f, 47.25f),
				new Vector3(-739.557f, 5599.346f, 46.997f),
				new Vector3(-581.009f, 5596.517f, 77.379f),
				new Vector3(-575.717f, 5596.388f, 79.22f),
				new Vector3(-273.805f, 5590.844f, 240.795f),
				new Vector3(-268.707f, 5590.744f, 243.395f),
				new Vector3(6.896f, 5585.668f, 423.614f),
				new Vector3(11.774f, 5585.591f, 426.711f),
				new Vector3(236.82f, 5581.445f, 599.642f),
				new Vector3(241.365f, 5581.369f, 603.183f),
				new Vector3(412.855f, 5578.216f, 774.401f),
				new Vector3(417.541f, 5578.124f, 777.688f),
				new Vector3(444.93f, 5577.589f, 786.535f),
				new Vector3(446.288f, 5577.59f, 786.75f)
			},
			[1] = new List<Vector3>
			{
				new Vector3(446.291f, 5566.377f, 786.75f),
				new Vector3(444.937f, 5566.383f, 786.551f),
				new Vector3(417.371f, 5567.001f, 777.708f),
				new Vector3(412.661f, 5567.085f, 774.439f),
				new Vector3(241.31f, 5570.594f, 603.137f),
				new Vector3(236.821f, 5570.663f, 599.561f),
				new Vector3(11.35f, 5575.298f, 426.629f),
				new Vector3(6.575f, 5575.391f, 423.57f),
				new Vector3(-268.965f, 5580.996f, 243.386f),
				new Vector3(-273.993f, 5581.124f, 240.808f),
				new Vector3(-575.898f, 5587.286f, 79.251f),
				new Vector3(-581.321f, 5587.4f, 77.348f),
				new Vector3(-739.646f, 5590.614f, 47.006f),
				new Vector3(-740.97f, 5590.617f, 47.306f)
			}
		};

		private static TrenoFunivia[] Cable_cars = new TrenoFunivia[2] { new TrenoFunivia(null, null, null, null, null, 0, new Vector3(0), 1, 0, 0, 0, 0, 0, 0, true, false, 0.65f, "IDLE", new Vector3(-0.2f, 0.0f, 0.0f)), new TrenoFunivia(null, null, null, null, null, 1, new Vector3(0), 1, 0, 0, 0, 0, 0, 0, true, false, 0.65f, "IDLE", new Vector3(-0.2f, 0.0f, 0.0f)) };

		public static void Init()
		{
			Blip Sopra = new Blip(AddBlipForCoord(446.8f, 5571.1f, 780.7f)) { Sprite = BlipSprite.Lift, IsShortRange = true, Name = "Funivia" };
			Blip Sotto = new Blip(AddBlipForCoord(-740.3f, 5594.5f, 41.2f)) { Sprite = BlipSprite.Lift, IsShortRange = true, Name = "Funivia" };
			SetBlipDisplay(Sopra.Handle, 4);
			SetBlipDisplay(Sotto.Handle, 4);
			ClientSession.Instance.AddEventHandler("onResourceStop", new Action<string>(OnStop));
			ClientSession.Instance.AddEventHandler("omni:cablecar:forceState", new Action<int, string>(ForceState));
			CaricaTutto();
		}

		private static async void CaricaTutto()
		{
			RequestModel((uint)GetHashKey("p_cablecar_s"));
			while (!HasModelLoaded((uint)GetHashKey("p_cablecar_s"))) await BaseScript.Delay(100);
			RequestModel((uint)GetHashKey("p_cablecar_s_door_l"));
			while (!HasModelLoaded((uint)GetHashKey("p_cablecar_s_door_l"))) await BaseScript.Delay(100);
			RequestModel((uint)GetHashKey("p_cablecar_s_door_r"));
			while (!HasModelLoaded((uint)GetHashKey("p_cablecar_s_door_r"))) await BaseScript.Delay(100);
			RequestScriptAudioBank("CABLE_CAR", false);
			RequestScriptAudioBank("CABLE_CAR_SOUNDS", false);
			LoadStream("CABLE_CAR", "CABLE_CAR_SOUNDS");
			LoadStream("CABLE_CAR_SOUNDS", "CABLE_CAR");
			Cable_cars[0].Entity = new Prop(CreateObjectNoOffset((uint)GetHashKey("p_cablecar_s"), 446.291f, 5566.377f, 786.75f, true, true, false));
			Cable_cars[0].DoorLL = new Prop(CreateObjectNoOffset((uint)GetHashKey("p_cablecar_s_door_l"), -740.911f, 5599.341f, 47.25f, false, true, false));
			Cable_cars[0].DoorLR = new Prop(CreateObjectNoOffset((uint)GetHashKey("p_cablecar_s_door_r"), -740.911f, 5599.341f, 47.25f, false, true, false));
			AttachEntityToEntity(Cable_cars[0].DoorLL.Handle, Cable_cars[0].Entity.Handle, 0, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, false, false, true, false, 2, true);
			AttachEntityToEntity(Cable_cars[0].DoorLR.Handle, Cable_cars[0].Entity.Handle, 0, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, false, false, true, false, 2, true);
			Cable_cars[0].DoorRL = new Prop(CreateObjectNoOffset((uint)GetHashKey("p_cablecar_s_door_l"), -740.911f, 5599.341f, 47.25f, false, true, false));
			Cable_cars[0].DoorRR = new Prop(CreateObjectNoOffset((uint)GetHashKey("p_cablecar_s_door_r"), -740.911f, 5599.341f, 47.25f, false, true, false));
			AttachEntityToEntity(Cable_cars[0].DoorRL.Handle, Cable_cars[0].Entity.Handle, 0, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 180.0f, false, false, true, false, 2, true);
			AttachEntityToEntity(Cable_cars[0].DoorRR.Handle, Cable_cars[0].Entity.Handle, 0, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 180.0f, false, false, true, false, 2, true);
			Cable_cars[1].Entity = new Prop(CreateObjectNoOffset((uint)GetHashKey("p_cablecar_s"), -740.911f, 5599.341f, 47.25f, true, true, false));
			Cable_cars[1].DoorLL = new Prop(CreateObjectNoOffset((uint)GetHashKey("p_cablecar_s_door_l"), -740.911f, 5599.341f, 47.25f, false, true, false));
			Cable_cars[1].DoorLR = new Prop(CreateObjectNoOffset((uint)GetHashKey("p_cablecar_s_door_r"), -740.911f, 5599.341f, 47.25f, false, true, false));
			AttachEntityToEntity(Cable_cars[1].DoorLL.Handle, Cable_cars[1].Entity.Handle, 0, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, false, false, true, false, 2, true);
			AttachEntityToEntity(Cable_cars[1].DoorLR.Handle, Cable_cars[1].Entity.Handle, 0, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, false, false, true, false, 2, true);
			Cable_cars[1].DoorRL = new Prop(CreateObjectNoOffset((uint)GetHashKey("p_cablecar_s_door_l"), -740.911f, 5599.341f, 47.25f, false, true, false));
			Cable_cars[1].DoorRR = new Prop(CreateObjectNoOffset((uint)GetHashKey("p_cablecar_s_door_r"), -740.911f, 5599.341f, 47.25f, false, true, false));
			AttachEntityToEntity(Cable_cars[1].DoorRL.Handle, Cable_cars[1].Entity.Handle, 0, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 180.0f, false, false, true, false, 2, true);
			AttachEntityToEntity(Cable_cars[1].DoorRR.Handle, Cable_cars[1].Entity.Handle, 0, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 180.0f, false, false, true, false, 2, true);
			Cable_cars[0].Entity.IsPositionFrozen = true;
			Cable_cars[0].Entity.Rotation = new Vector3(0.0f, 0.0f, 270.0f);
			Cable_cars[1].Entity.IsPositionFrozen = true;
			Cable_cars[1].Entity.Rotation = new Vector3(0.0f, 0.0f, 90.0f);
			Cable_cars[0].State = "MOVE_TO_IDLE_TOP";
			Cable_cars[1].State = "MOVE_TO_IDLE_TOP";
			ClientSession.Instance.AddTick(UpdateCableMovement);
		}

		private static void OnStop(string name)
		{
			if (name == GetCurrentResourceName())
			{
				if (Cable_cars[0].Is_player_seated) KickPlayerOutOfMyCablecar(Cable_cars[0]);
				if (Cable_cars[1].Is_player_seated) KickPlayerOutOfMyCablecar(Cable_cars[1]);
				Cable_cars[0].Entity.Delete();
				Cable_cars[0].DoorLL.Delete();
				Cable_cars[0].DoorLR.Delete();
				Cable_cars[0].DoorRL.Delete();
				Cable_cars[0].DoorRR.Delete();
				Cable_cars[1].Entity.Delete();
				Cable_cars[1].DoorLL.Delete();
				Cable_cars[1].DoorLR.Delete();
				Cable_cars[1].DoorRL.Delete();
				Cable_cars[1].DoorRR.Delete();
			}
		}

		private static async Task UpdateCableMovement()
		{
			for (int i = 0; i < Cable_cars.Length; i++)
				if (Cable_cars[i].State == "MOVE_UP")
				{
					Cable_cars[i].Direction = 1.0f;
					Vector3 Prev = Tracks[Cable_cars[i].Index][Cable_cars[i].Gradient];
					Vector3 Next = Tracks[Cable_cars[i].Index][Cable_cars[i].Gradient + 1];
					if (Cable_cars[i].Gradient_distance == 0.0f) Cable_cars[i].Gradient_distance = Vector3.Distance(Prev, Next);
					float veraspeed;
					if (Cable_cars[i].Gradient % 2 == 0)
						veraspeed = Speeds[0];
					else
						veraspeed = Speeds[1];
					float dist = Cable_cars[i].Gradient_distance;
					float speed = 1.0f / dist * Timestep() * veraspeed;
					Cable_cars[i].Run_timer += speed;

					if (Cable_cars[i].Run_timer > 1f)
					{
						Cable_cars[i].Gradient++;
						Cable_cars[i].Run_timer = 0f;

						if (Cable_cars[i].Gradient >= Tracks[Cable_cars[i].Index].Count - 1)
						{
							Cable_cars[i].State = "MOVE_TO_IDLE_TOP";
							Cable_cars[i].Gradient_distance = 0f;

							return;
						}

						Prev = Tracks[Cable_cars[i].Index][Cable_cars[i].Gradient];
						Next = Tracks[Cable_cars[i].Index][Cable_cars[i].Gradient + 1];
						RequestAnimDict("p_cablecar_s");
						while (!HasAnimDictLoaded("p_cablecar_s")) await BaseScript.Delay(100);
						UpdateCablecarGradient(Cable_cars[i]);
						RemoveAnimDict("p_cablecar_s");
					}
					else
					{
						Cable_cars[i].Position = VecLerp(Prev.X, Prev.Y, Prev.Z, Next.X, Next.Y, Next.Z, Cable_cars[i].Run_timer, true);
					}

					float zLerp = 0.0f;
					if (Cable_cars[i].Gradient_distance > 30.0) zLerp = (-1.0f + Math.Abs(Lerp(1.0f, -1.0f, Cable_cars[i].Run_timer))) * 0.25f;
					Function.Call(Hash.SET_ENTITY_COORDS, Cable_cars[i].Entity.Handle, Vector3.Add(Vector3.Add(Cable_cars[i].Position, Cable_cars[i].Offset), new Vector3(0.0f, 0.0f, zLerp)).X, Vector3.Add(Vector3.Add(Cable_cars[i].Position, Cable_cars[i].Offset), new Vector3(0.0f, 0.0f, zLerp)).Y, Vector3.Add(Vector3.Add(Cable_cars[i].Position, Cable_cars[i].Offset), new Vector3(0.0f, 0.0f, zLerp)).Z, true, false, false, true);
					GivePlayerOptionToJoinMyCablecar(Cable_cars[i], true);
				}
				else if (Cable_cars[i].State == "MOVE_DOWN")
				{
					Cable_cars[i].Direction = -1f;
					Vector3 Prev = Tracks[Cable_cars[i].Index][Cable_cars[i].Gradient];
					Vector3 Next = Tracks[Cable_cars[i].Index][Cable_cars[i].Gradient - 1];
					if (Cable_cars[i].Gradient_distance == 0.0f) Cable_cars[i].Gradient_distance = Vector3.Distance(Prev, Next);
					float veraspeed;
					if (Cable_cars[i].Gradient % 2 == 0)
						veraspeed = Speeds[1];
					else
						veraspeed = Speeds[0];
					float dist = Cable_cars[i].Gradient_distance;
					float speed = 1.0f / dist * Timestep() * veraspeed;
					Cable_cars[i].Run_timer += speed;

					if (Cable_cars[i].Run_timer > 1f)
					{
						Cable_cars[i].Gradient--;
						Cable_cars[i].Run_timer = 0f;

						if (Cable_cars[i].Gradient <= 0)
						{
							Cable_cars[i].State = "IDLE";
							Cable_cars[i].Gradient_distance = 0f;
							BaseScript.TriggerServerEvent("omni:cablecar:host:sync", i, "IDLE_BOTTOM");

							return;
						}

						Prev = Tracks[Cable_cars[i].Index][Cable_cars[i].Gradient];
						Next = Tracks[Cable_cars[i].Index][Cable_cars[i].Gradient - 1];
						RequestAnimDict("p_cablecar_s");
						while (!HasAnimDictLoaded("p_cablecar_s")) await BaseScript.Delay(100);
						UpdateCablecarGradient(Cable_cars[i]);
						RemoveAnimDict("p_cablecar_s");
					}
					else
					{
						Cable_cars[i].Position = VecLerp(Prev.X, Prev.Y, Prev.Z, Next.X, Next.Y, Next.Z, Cable_cars[i].Run_timer, true);
					}

					float zLerp = 0.0f;
					if (Cable_cars[i].Gradient_distance > 20.0) zLerp = (-1.0f + Math.Abs(Lerp(1.0f, -1.0f, Cable_cars[i].Run_timer))) * 0.25f;
					Function.Call(Hash.SET_ENTITY_COORDS, Cable_cars[i].Entity.Handle, Vector3.Add(Vector3.Add(Cable_cars[i].Position, Cable_cars[i].Offset), new Vector3(0.0f, 0.0f, zLerp)).X, Vector3.Add(Vector3.Add(Cable_cars[i].Position, Cable_cars[i].Offset), new Vector3(0.0f, 0.0f, zLerp)).Y, Vector3.Add(Vector3.Add(Cable_cars[i].Position, Cable_cars[i].Offset), new Vector3(0.0f, 0.0f, zLerp)).Z, true, false, false, true);
					GivePlayerOptionToJoinMyCablecar(Cable_cars[i], true);
				}
				else if (Cable_cars[i].State == "IDLE_TO_MOVE_UP")
				{
					Cable_cars[i].Gradient = 0;
					Cable_cars[i].Gradient_distance = 0.0f;
					Cable_cars[i].Run_timer = 0.0f;

					if (Cable_cars[i].Is_player_seated) { }
					// AGGIUNGERE UNA TELECAMERA
					else
					{
						CheckIfPlayerShouldBeKickedOut(Cable_cars[i]);
					}

					SetCablecarDoors(Cable_cars[i], false);
					Cable_cars[i].Audio = GetSoundId();
					PlaySoundFromEntity(Cable_cars[i].Audio, "Running", Cable_cars[i].Entity.Handle, "CABLE_CAR_SOUNDS", false, 0);
					Cable_cars[i].State = "MOVE_UP";
				}
				else if (Cable_cars[i].State == "IDLE_TO_MOVE_DOWN")
				{
					Cable_cars[i].Gradient = Tracks[Cable_cars[i].Index].Count - 1;
					Cable_cars[i].Gradient_distance = 0.0f;
					Cable_cars[i].Run_timer = 0.0f;

					if (Cable_cars[i].Is_player_seated) { }
					// AGGIUNGERE UNA TELECAMERA
					else
					{
						CheckIfPlayerShouldBeKickedOut(Cable_cars[i]);
					}

					SetCablecarDoors(Cable_cars[i], false);
					Cable_cars[i].Audio = GetSoundId();
					PlaySoundFromEntity(Cable_cars[i].Audio, "Running", Cable_cars[i].Entity.Handle, "CABLE_CAR_SOUNDS", false, 0);
					Cable_cars[i].State = "MOVE_DOWN";
				}
				else if (Cable_cars[i].State == "MOVE_TO_IDLE_TOP")
				{
					Cable_cars[i].Position = Tracks[Cable_cars[i].Index][Cable_cars[i].Gradient];
					Function.Call(Hash.SET_ENTITY_COORDS, Cable_cars[i].Entity.Handle, Vector3.Add(Cable_cars[i].Position, Cable_cars[i].Offset).X, Vector3.Add(Cable_cars[i].Position, Cable_cars[i].Offset).Y, Vector3.Add(Cable_cars[i].Position, Cable_cars[i].Offset).Z, true, false, false, true);

					if (Cable_cars[i].Is_player_seated)
					{
						//KickPlayerOutOfMyCablecar(v)
						//Cable_cars[i].is_player_seated = false
					}

					SetCablecarDoors(Cable_cars[i], true);
					ReleaseRunningSound(Cable_cars[i]);
					Cable_cars[i].State = "IDLE_TOP";
					Cable_cars[i].Run_timer = 0.0f;
				}
				else if (Cable_cars[i].State == "MOVE_TO_IDLE_BOTTOM")
				{
					Cable_cars[i].Position = Tracks[Cable_cars[i].Index][0];
					Function.Call(Hash.SET_ENTITY_COORDS, Cable_cars[i].Entity.Handle, Vector3.Add(Cable_cars[i].Position, Cable_cars[i].Offset).X, Vector3.Add(Cable_cars[i].Position, Cable_cars[i].Offset).Y, Vector3.Add(Cable_cars[i].Position, Cable_cars[i].Offset).Z, true, false, false, true);

					if (Cable_cars[i].Is_player_seated)
					{
						//KickPlayerOutOfMyCablecar(v)
						//Cable_cars[i].is_player_seated = false
					}

					SetCablecarDoors(Cable_cars[i], true);
					ReleaseRunningSound(Cable_cars[i]);
					Cable_cars[i].State = "IDLE_BOTTOM";
					Cable_cars[i].Run_timer = 0.0f;
				}
				else if (Cable_cars[i].State == "IDLE_TOP")
				{
					Cable_cars[i].Run_timer += Timestep() / 20.0f;

					if (Cable_cars[i].Run_timer > 1.0f)
					{
						Cable_cars[i].State = "IDLE_TO_MOVE_DOWN";
						Cable_cars[i].Run_timer = 0.0f;
					}

					GivePlayerOptionToJoinMyCablecar(Cable_cars[i], false);
				}
				else if (Cable_cars[i].State == "IDLE_BOTTOM")
				{
					Cable_cars[i].Run_timer += Timestep() / 20.0f;

					if (Cable_cars[i].Run_timer > 1.0f)
					{
						Cable_cars[i].State = "IDLE_TO_MOVE_UP";
						Cable_cars[i].Run_timer = 0.0f;
					}

					GivePlayerOptionToJoinMyCablecar(Cable_cars[i], false);
				}
				else if (Cable_cars[i].State == "IDLE")
				{
				}
		}

		private static void ForceState(int index, string state)
		{
			if (state == "IDLE_BOTTOM")
			{
				Cable_cars[index].State = "MOVE_TO_IDLE_BOTTOM";
				Cable_cars[index].Run_timer = 0.0f;
			}

			if (state == "IDLE_TOP")
			{
				Cable_cars[index].State = "MOVE_TO_IDLE_TOP";
				Cable_cars[index].Run_timer = 0.0f;
			}

			if (state == "MOVE_DOWN")
			{
				Cable_cars[index].State = "IDLE_TO_MOVE_DOWN";
				Cable_cars[index].Gradient = Tracks[index].Count - 1;
				Cable_cars[index].Gradient_distance = 0.0f;
				Cable_cars[index].Run_timer = 0.0f;
			}

			if (state == "MOVE_UP")
			{
				Cable_cars[index].State = "IDLE_TO_MOVE_UP";
				Cable_cars[index].Gradient = 0;
				Cable_cars[index].Gradient_distance = 0.0f;
				Cable_cars[index].Run_timer = 0.0f;
			}
		}

		private static void ReleaseRunningSound(TrenoFunivia treno)
		{
			if (treno.Audio != -1)
			{
				StopSound(treno.Audio);
				ReleaseSoundId(treno.Audio);
				treno.Audio = -1;
			}
		}

		private static void SetCablecarDoors(TrenoFunivia treno, bool state)
		{
			float doorOffset = 0.0f;

			if (state)
			{
				doorOffset = 2.0f;
				Audio.PlaySoundFromEntity(treno.Entity, "Arrive_Station", "CABLE_CAR_SOUNDS");
				Audio.PlaySoundFromEntity(treno.Entity, "DOOR_OPEN", "CABLE_CAR_SOUNDS");
			}
			else
			{
				doorOffset = 0.0f;
				Audio.PlaySoundFromEntity(treno.Entity, "Leave_Station", "CABLE_CAR_SOUNDS");
				Audio.PlaySoundFromEntity(treno.Entity, "DOOR_CLOSE", "CABLE_CAR_SOUNDS");
			}

			DetachEntity(treno.DoorLL.Handle, false, false);
			DetachEntity(treno.DoorLR.Handle, false, false);
			DetachEntity(treno.DoorRL.Handle, false, false);
			DetachEntity(treno.DoorRR.Handle, false, false);
			AttachEntityToEntity(treno.DoorLL.Handle, treno.Entity.Handle, 0, 0.0f, doorOffset, 0.0f, 0.0f, 0.0f, 0.0f, false, false, true, false, 2, true);
			AttachEntityToEntity(treno.DoorLR.Handle, treno.Entity.Handle, 0, 0.0f, -doorOffset, 0.0f, 0.0f, 0.0f, 0.0f, false, false, true, false, 2, true);
			AttachEntityToEntity(treno.DoorRL.Handle, treno.Entity.Handle, 0, 0.0f, doorOffset, 0.0f, 0.0f, 0.0f, 180.0f, false, false, true, false, 2, true);
			AttachEntityToEntity(treno.DoorRR.Handle, treno.Entity.Handle, 0, 0.0f, -doorOffset, 0.0f, 0.0f, 0.0f, 180.0f, false, false, true, false, 2, true);
		}

		private static int WhatDirectionDoesMyCablecarGo(TrenoFunivia treno)
		{
			if (treno.Index == 0)
			{
				if (treno.Direction >= 0)
					return 0;
				else
					return 1;
			}
			else
			{
				if (treno.Direction >= 0)
					return 1;
				else
					return 0;
			}
		}

		private static void CheckIfPlayerShouldBeKickedOut(TrenoFunivia treno)
		{
			Vector3 pos = Vector3.Add(treno.Position, new Vector3(0.0f, 0.0f, -5.3f));
			float dist = Vector3.Distance(pos, SessionCache.Cache.MyPlayer.Ped.Position);
			if (dist < 3.0f) KickPlayerOutOfMyCablecar(treno);
		}

		private static void KickPlayerOutOfMyCablecar(TrenoFunivia treno)
		{
			treno.Is_player_seated = false;
			SessionCache.Cache.MyPlayer.Ped.Detach();
			Vector3 not1 = new Vector3(0);
			Vector3 Destra = new Vector3(0);
			Vector3 not2 = new Vector3(0);
			Vector3 not3 = new Vector3(0);
			GetEntityMatrix(treno.Entity.Handle, ref Destra, ref not1, ref not2, ref not3);
			Vector3 right = Vector3.Multiply(Destra, 3.5f);
			SessionCache.Cache.MyPlayer.Ped.Position = Vector3.Add(Vector3.Add(treno.Position, right), new Vector3(0.0f, 0.0f, -5.3f));
		}

		private static void GivePlayerOptionToJoinMyCablecar(TrenoFunivia treno, bool moving)
		{
			Vector3 pos = Vector3.Add(treno.Position, new Vector3(0f, 0f, -5.3f));

			if (!treno.Is_player_seated)
			{
				Vector3 plypos = SessionCache.Cache.MyPlayer.Ped.Position;
				float dist = Vector3.Distance(pos, SessionCache.Cache.MyPlayer.Ped.Position);

				if (dist < 3.0f)
				{
					HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per entrare nella funivia");

					if (Input.IsControlJustPressed(Control.Context))
					{
						treno.Is_player_seated = true;
						SessionCache.Cache.MyPlayer.Ped.AttachTo(treno.Entity, Vector3.Subtract(SessionCache.Cache.MyPlayer.Ped.Position, treno.Position), SessionCache.Cache.MyPlayer.Ped.Rotation);
					}
				}
			}
			else
			{
				if (!moving)
				{
					HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per uscire dalla funivia");

					if (Input.IsControlJustPressed(Control.Context))
					{
						treno.Is_player_seated = false;
						SessionCache.Cache.MyPlayer.Ped.Detach();
					}
				}
			}
		}

		private static int UpdateCablecarGradient(TrenoFunivia treno)
		{
			string text = "C" + (treno.Index + 1);

			if (WhatDirectionDoesMyCablecarGo(treno) == 0)
			{
				Dictionary<int, string> data = new Dictionary<int, string>();
				data.Add(0, "_up_9");
				data.Add(1, "_up_1");
				data.Add(3, "_up_3");
				data.Add(5, "_up_4");
				data.Add(7, "_up_5");
				data.Add(9, "_up_6");
				data.Add(11, "_up_8");
				data.Add(12, "_up_9");

				if (data.ContainsKey(treno.Gradient - 1))
					text = text + data[treno.Gradient - 1];
				else
					return 0;
			}
			else
			{
				Dictionary<int, string> data = new Dictionary<int, string>();
				data.Add(0, "_down_1");
				data.Add(1, "_down_2");
				data.Add(3, "_down_3");
				data.Add(5, "_down_4");
				data.Add(7, "_down_5");
				data.Add(9, "_down_6");
				data.Add(11, "_down_8");
				data.Add(12, "_down_9");

				if (data.ContainsKey(treno.Gradient - 1))
					text = text + data[treno.Gradient - 1];
				else
					return 0;
			}

			PlayEntityAnim(treno.Entity.Handle, text, "p_cablecar_s", 8.0f, false, true, false, 0, 0);

			return 1;
		}

		private static float degToRad(float degs) { return degs * 3.141592653589793f / 180; }

		private static float Lerp(float a, float b, float t) { return a + (b - a) * t; }

		private static Vector3 VecLerp(float x1, float y1, float z1, float x2, float y2, float z2, float l, bool clamp)
		{
			if (clamp)
			{
				if (l < 0.0) l = 0.0f;
				if (l > 1.0f) l = 1.0f;
			}

			float x = Lerp(x1, x2, l);
			float y = Lerp(y1, y2, l);
			float z = Lerp(z1, z2, l);

			return new Vector3(x, y, z);
		}
	}

	public class TrenoFunivia
	{
		public Prop Entity;
		public Prop DoorLL;
		public Prop DoorLR;
		public Prop DoorRL;
		public Prop DoorRR;
		public int Index;
		public int Audio = -1;
		public Vector3 Position = new Vector3(0); // The current position of the car
		public float Direction;                   // What direction we're moving (up or down)
		public int Gradient;                      // Believed to be the gradient during research, but was actually just the current node we're moving from
		public float Run_timer;                   // Scale used for lerping
		public float Altitude;                    // Used for the scenic camera in SP, not used here
		public float Activation_timer;            // Not used here
		public float Gradient_distance;           // Distance between the current node we're moving from and the next node
		public float Offset_modifier;             // Something believed to be an offset modifier
		public bool Can_move;                     // Determine if the car can move, not actually used here though
		public bool Is_player_seated;             // Another value from the SP script, not actually used because fucking hell I'm tired
		public float Speed;                       // Movement speed modifier, determines the speed of the car on the track
		public string State;                      // The current state of the car
		public Vector3 Offset = new Vector3(0);

		public TrenoFunivia(Prop entity, Prop dll, Prop dlr, Prop drl, Prop drr, int idx, Vector3 pos, float dir, int grad, float runt, float alt, float timer, float grad_dist, float offset_mod, bool canmove, bool seated, float speed, string state, Vector3 offs)
		{
			Entity = entity;
			DoorLL = dll;
			DoorLR = dlr;
			DoorRL = drl;
			DoorRR = drr;
			Index = idx;
			Position = pos;
			Direction = dir;
			Gradient = grad;
			Run_timer = runt;
			Altitude = alt;
			Activation_timer = timer;
			Gradient_distance = grad_dist;
			Offset_modifier = offset_mod;
			Can_move = canmove;
			Is_player_seated = seated;
			Speed = speed;
			State = state;
			Offset = offs;
		}
	}
}