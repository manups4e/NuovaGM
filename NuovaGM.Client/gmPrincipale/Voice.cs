using CitizenFX.Core;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.Personale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Client.gmPrincipale
{
	static class Voice
	{
		static List<Modes> VoiceMode = new List<Modes>()
		{
			new Modes(3, "Sei Sottovoce.", false),
			new Modes(8, "Parli Normalmente.", false),
			new Modes(14, "Urli.", false),
		};
		static Dictionary<int, bool> Listeners = new Dictionary<int, bool>();
		static int Mode = 1;
		static float Distance = 8.0f;
		static bool OnlyVehicle = false;
		static bool shouldReset = false;

		public static void Init()
		{
			Client.GetInstance.RegisterTickHandler(OnTick);
			Client.GetInstance.RegisterTickHandler(OnTick2);
		}

		public static void SendVoiceToPlayer(Player player, bool Send)
		{
			NetworkOverrideSendRestrictions(player.Handle, Send);
		}

		public static void UpdateVoices()
		{
			foreach (Player p in Client.GetInstance.GetPlayers.ToList())
			{
				Ped otherPed = new Ped(p.Handle);
				int serverID = GetPlayerServerId(p.Handle);
				if (otherPed.Exists() && CanPedBeListened(Game.PlayerPed, otherPed))
				{
					if (!Listeners.ContainsKey(serverID))
					{
						Listeners.Add(serverID, true);
					}
					else if (!Listeners[serverID])
					{
						Listeners[serverID] = true;
					}

					SendVoiceToPlayer(p, true);
				}
				else
				{
					if (!Listeners.ContainsKey(serverID))
					{
						Listeners.Add(serverID, false);
					}
					else if (Listeners[serverID])
					{
						Listeners[serverID] = false;
						SendVoiceToPlayer(p, false);
					}
				}
			}
		}
		static Notifica a = null;
		public static async void OnModeModified()
		{
			Modes modeData = VoiceMode[Mode];
			if (modeData != null)
			{
				if (a != null)
				{
					a.Hide();
				}
				Distance = modeData.dist;
				OnlyVehicle = modeData.veh;
				UpdateVoices();
				a = HUD.ShowNotification(modeData.msg);
			}
		}

		public static bool CanPedBeListened(Ped ped, Ped otherPed)
		{
			Vector3 listenerHeadPos = otherPed.Bones[Bone.IK_Head].Position;
			bool InSameVeh = (ped.IsInVehicle() && otherPed.IsInVehicle() && ped.CurrentVehicle == otherPed.CurrentVehicle);
			float distance = World.GetDistance(listenerHeadPos, ped.Position);
			float CheckDistance = Distance;
			return InSameVeh || (!OnlyVehicle && (HasEntityClearLosToEntityInFront(ped.Handle, otherPed.Handle) || distance < (Math.Max(0, Math.Min(18, CheckDistance)) * 0.6f)) && distance < CheckDistance);
		}

		public static bool ShouldSendVoice()
		{
			return NetworkIsPlayerTalking(Game.Player.Handle) || Input.IsControlPressed(Control.PushToTalk);
		}

		private static bool FirstTick = true;
		public static async Task OnTick()
		{
			if (FirstTick)
			{
				foreach (Player p in Client.GetInstance.GetPlayers.ToList())
					SendVoiceToPlayer(p, false);
				NetworkSetTalkerProximity(-1000.0f);
				FirstTick = false;
			}
			await BaseScript.Delay(300);
			bool sendVoice = ShouldSendVoice();
			if (sendVoice && !shouldReset)
			{
				shouldReset = true;
			}
			else if (!sendVoice && shouldReset)
			{
				shouldReset = false;
				foreach (Player p in Client.GetInstance.GetPlayers.ToList())
				{
					SendVoiceToPlayer(p, false);
				}

				SetPedTalk(PlayerPedId());
			}
			UpdateVoices();
			await Task.FromResult(0);
		}

		public static void UpdateVocalMode(int mode)
		{
			int nextMode = mode;
			if (nextMode > 2 && !Game.PlayerPed.IsInVehicle())
			{
				nextMode = 0;
			}

			Mode = nextMode;
			OnModeModified();
		}

		public static void UpdateVocalMode()
		{
			int nextMode = Mode + 1;
			if (nextMode > 2)
			{
				nextMode = 0;
			}

			Mode = nextMode;
			OnModeModified();
		}

		static bool Permesso = true;
		static bool notif = false;
		public static async Task OnTick2()
		{
			if (Permesso)
			{
				if (Game.IsControlJustPressed(1, Control.VehicleHeadlight) && Game.IsControlPressed(1, Control.Sprint) && Game.CurrentInputMode == InputMode.MouseAndKeyboard)
				{
					UpdateVocalMode();
				}

				if (Game.IsControlPressed(1, Control.VehicleHeadlight) && Game.IsControlPressed(1, Control.Sprint) && Game.CurrentInputMode == InputMode.MouseAndKeyboard)
				{
					Vector3 headPos = Game.PlayerPed.Bones[Bone.IK_Head].Position;
					World.DrawMarker(MarkerType.DebugSphere, headPos, new Vector3(0), new Vector3(0), new Vector3(Distance, Distance, Distance), System.Drawing.Color.FromArgb(30, 20, 192, 255));
				}
			}
			if (Game.PlayerPed.IsInVehicle())
			{
				if (Game.PlayerPed.CurrentVehicle.Windows.AreAllWindowsIntact && EventiPersonalMenu.WindowsGiu)
				{
					Permesso = true;
					notif = false;
				}
				else if (Game.PlayerPed.CurrentVehicle.Windows.AreAllWindowsIntact && !EventiPersonalMenu.WindowsGiu)
				{
					if (!notif)
					{
						HUD.ShowNotification("Range vocale settato all'interno del veicolo\nPer parlare al di fuori abbassa i finestrini...... o rompili.");
						notif = true;
					}
					Permesso = false;
					foreach (Player p in Client.GetInstance.GetPlayers.ToList())
					{
						Ped otherPed = new Ped(p.Handle);
						if (CanPedBeListened(Game.PlayerPed, otherPed))
						{
							if (!Listeners.ContainsKey(GetPlayerServerId(p.Handle)))
							{
								Listeners.Add(GetPlayerServerId(p.Handle), true);
							}
							else
							{
								Listeners[GetPlayerServerId(p.Handle)] = true;
							}

							SendVoiceToPlayer(p, true);
						}
						else
						{
							if (!Listeners.ContainsKey(GetPlayerServerId(p.Handle)))
							{
								Listeners.Add(GetPlayerServerId(p.Handle), false);
							}
							else
							{
								Listeners[GetPlayerServerId(p.Handle)] = false;
							}

							SendVoiceToPlayer(p, false);
						}
					}
				}
				else if (!Game.PlayerPed.CurrentVehicle.Windows.AreAllWindowsIntact)
				{
					Permesso = true;
					notif = false;
				}
			}
			else
			{
				Permesso = true;
			}

			await Task.FromResult(0);
		}
	}

	public class Modes
	{
		public int dist;
		public string msg;
		public bool veh;
		public Modes(int d, string m, bool v)
		{
			dist = d;
			msg = m;
			veh = v;
		}
		public bool Veh()
		{
			return Game.PlayerPed.IsInVehicle();
		}
	}
}
