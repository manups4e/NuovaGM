using CitizenFX.Core;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.Personale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.Core
{
	public enum Mode
	{
		sottovoce = 0,
		normale,
		urla
	}

	internal static class Voice
	{
		private static List<Modes> VoiceMode = new List<Modes>() { new Modes(3, "Sei Sottovoce.", false), new Modes(8, "Parli Normalmente.", false), new Modes(14, "Urli.", false) };
		private static Dictionary<int, bool> Listeners = new Dictionary<int, bool>();
		private static Mode Mode = Mode.normale;
		private static float CheckDistance = 8.0f;
		private static bool OnlyVehicle = false;
		private static bool shouldReset = false;

		public static void Init()
		{
			Client.Instance.AddTick(OnTick);
			Client.Instance.AddTick(OnTick2);
			Client.Instance.GetPlayers.ToList().ForEach(x => SendVoiceToPlayer(x, false));
			NetworkSetTalkerProximity(-1000.0f);
		}

		public static void SendVoiceToPlayer(Player player, bool Send) { NetworkOverrideSendRestrictions(player.Handle, Send); }

		public static async void UpdateVoices()
		{
			Ped pl = Cache.Cache.MyPlayer.Ped;

			foreach (Player p in Client.Instance.GetPlayers.ToList())
			{
				int serverID = GetPlayerServerId(p.Handle);

				if (CanPedBeListened(pl, p.Character))
				{
					if (!Listeners.ContainsKey(serverID))
						Listeners.Add(serverID, true);
					else if (!Listeners[serverID]) Listeners[serverID] = true;
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

		private static Notifica a = null;

		public static async void OnModeModified()
		{
			Modes modeData = VoiceMode[(int)Mode];

			if (modeData != null)
			{
				if (a != null) a.Hide();
				CheckDistance = modeData.dist;
				OnlyVehicle = modeData.veh;
				UpdateVoices();
				a = HUD.ShowNotification(modeData.msg);
			}
		}

		public static bool CanPedBeListened(Ped ped, Ped otherPed)
		{
			Vector3 listenerHeadPos = otherPed.Bones[Bone.IK_Head].Position;
			//			bool InSameVeh = (ped.IsInVehicle() && otherPed.IsInVehicle() && ped.CurrentVehicle == otherPed.CurrentVehicle);
			bool InSameVeh = ped.IsInVehicle() && otherPed.IsInVehicle(ped.CurrentVehicle);
			float distance = Vector3.Distance(listenerHeadPos, ped.Position);

			return InSameVeh || !OnlyVehicle && (HasEntityClearLosToEntityInFront(ped.Handle, otherPed.Handle) || distance < Math.Max(0, Math.Min(18, CheckDistance)) * 0.6f) && distance < CheckDistance;
		}

		public static bool ShouldSendVoice() { return NetworkIsPlayerTalking(Cache.Cache.MyPlayer.Player.Handle) || Input.IsControlPressed(Control.PushToTalk); }

		public static async Task OnTick()
		{
			await BaseScript.Delay(300);

			if (ShouldSendVoice() && !shouldReset)
			{
				shouldReset = true;
			}
			else if (!ShouldSendVoice() && shouldReset)
			{
				shouldReset = false;
				Client.Instance.GetPlayers.ToList().ForEach(x => SendVoiceToPlayer(x, false));
				SetPedTalk(PlayerPedId());
			}

			UpdateVoices();
			await Task.FromResult(0);
		}

		public static void UpdateVocalMode(int mode)
		{
			int nextMode = mode;
			if (nextMode > 2 && !Cache.Cache.MyPlayer.Character.StatiPlayer.InVeicolo) nextMode = 0;
			Mode = (Mode)nextMode;
			OnModeModified();
		}

		public static void UpdateVocalMode()
		{
			int nextMode = (int)Mode + 1;
			if (nextMode > 2) nextMode = 0;
			Mode = (Mode)nextMode;
			OnModeModified();
		}

		private static bool Permesso = true;
		private static bool notif = false;

		public static async Task OnTick2()
		{
			Ped playerPed = Cache.Cache.MyPlayer.Ped;

			if (Permesso)
			{
				if (Input.IsControlPressed(Control.VehicleHeadlight, PadCheck.Keyboard, ControlModifier.Shift))
				{
					Vector3 headPos = playerPed.Bones[Bone.IK_Head].Position;
					World.DrawMarker(MarkerType.DebugSphere, headPos, Vector3.Zero, Vector3.Zero, new Vector3(CheckDistance), System.Drawing.Color.FromArgb(30, 20, 192, 255));
				}

				if (Input.IsControlJustPressed(Control.FrontendSocialClub, PadCheck.Keyboard, ControlModifier.Shift)) UpdateVocalMode();
			}

			if (Cache.Cache.MyPlayer.Character.StatiPlayer.InVeicolo)
			{
				Vehicle veh = playerPed.CurrentVehicle;

				if (veh == null) return;

				switch (veh.Windows.AreAllWindowsIntact)
				{
					case true when EventiPersonalMenu.WindowsGiu:
						Permesso = true;
						notif = false;

						break;
					case true when !EventiPersonalMenu.WindowsGiu:
					{
						if (!notif)
						{
							HUD.ShowNotification("Range vocale settato all'interno del veicolo\nPer parlare al di fuori abbassa i finestrini...... o rompili.");
							notif = true;
						}

						Permesso = false;

						foreach (Player p in Client.Instance.GetPlayers.ToList())
							if (CanPedBeListened(playerPed, p.Character))
							{
								if (!Listeners.ContainsKey(p.ServerId))
									Listeners.Add(p.ServerId, true);
								else
									Listeners[p.ServerId] = true;
								SendVoiceToPlayer(p, true);
							}
							else
							{
								if (!Listeners.ContainsKey(p.ServerId))
									Listeners.Add(p.ServerId, false);
								else
									Listeners[p.ServerId] = false;
								SendVoiceToPlayer(p, false);
							}

						break;
					}
					case false:
						Permesso = true;
						notif = false;

						break;
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
	}
}