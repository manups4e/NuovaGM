using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using TheLastPlanet.Client.MenuNativo;

namespace TheLastPlanet.Client.Core.Utility.HUD
{
	static class ProximityChat
	{
		public static void Init()
		{
			Client.Instance.AddEventHandler("lprp:triggerProximityDisplay", new Action<int, string, string>(TriggerProximtyDisplay));
		}

		private static Dictionary<int, List<ProxMess>> Messaggi = new Dictionary<int, List<ProxMess>>();

		public static void TriggerProximtyDisplay(int player, string title, string text)
		{
			Player target = new Player(API.GetPlayerFromServerId(player));
			if (Messaggi.ContainsKey(player))
				Messaggi[player].Add(new ProxMess(title + text, Colors.WhiteSmoke, target.Character.Bones[Bone.SKEL_Head].Position));
			else
				Messaggi.Add(player, new List<ProxMess>() { new ProxMess(title + text, Colors.WhiteSmoke, target.Character.Bones[Bone.SKEL_Head].Position) });
		}

		public static async Task Prossimità()
		{
			bool canDraw = false;
			Ped myPed = new Ped(API.PlayerPedId());
			if (Messaggi.Count > 0)
			{
				foreach (var p in Messaggi)
				{
					Player player = new Player(API.GetPlayerFromServerId(p.Key));
					Ped ped = player.Character;
					if (myPed.IsInRangeOf(ped.Position, 19f))
					{
						if (p.Value.Count > 0)
						{
							canDraw = ProximityVehCheck(myPed, ped);
							foreach (var m in p.Value.ToList())
							{
								if (canDraw)
									m.Draw(p.Value.Count - p.Value.IndexOf(m), ped);
								if (Game.GameTime - m.Timer >= 1000)
								{
									m.Tempo = m.Tempo.Subtract(TimeSpan.FromSeconds(1));
									m.Timer = Game.GameTime;
									if (m.Tempo.TotalSeconds == TimeSpan.Zero.TotalSeconds)
										p.Value.Remove(m);
								}
							}
						}
						//else Messaggi.Remove(p.Key);
					}
				}
			}
			await Task.FromResult(0);
		}

		private static bool ProximityVehCheck(Ped io, Ped lui)
		{
			bool ioInVeh = io.IsInVehicle();
			bool luiInVeh = lui.IsInVehicle();

			if (ioInVeh || luiInVeh)
			{
				if (ioInVeh && !luiInVeh)
				{
					if (!io.CurrentVehicle.Windows.AreAllWindowsIntact)
						return true;
					else
						return false;
				}
				else if (!ioInVeh && luiInVeh)
				{
					if (!lui.CurrentVehicle.Windows.AreAllWindowsIntact)
						return true;
					else
						return false;
				}
				if (ioInVeh && luiInVeh)
				{
					if (io.CurrentVehicle == lui.CurrentVehicle)
						return true;
					else if (!lui.CurrentVehicle.Windows.AreAllWindowsIntact && !io.CurrentVehicle.Windows.AreAllWindowsIntact)
						return true;
					else
						return false;
				}
			}
			return true;
		}

		private static bool ProximityCheck(Ped myPed, Ped lui)
		{

			return true;
		}
	}


	public class ProxMess
	{

		public string Message;
		public Color Color;
		public Vector3 Position;
		public TimeSpan Tempo = new TimeSpan(0, 0, 5); // cambiare con 5 secondi
		public int Timer = 0;
		public ProxMess(string mess, Color color, Vector3 pos)
		{
			Message = mess;
			Color = color;
			Position = pos;
		}

		/// <summary>
		/// Chiamata ad ogni frame!
		/// </summary>
		public void Draw(int index, Ped p)
		{
			if (Timer == 0) Timer = Game.GameTime;
			Color textColor = Colors.WhiteSmoke;
			switch (index)
			{
				case 1:
					textColor = Colors.Green;
					break;
				case 2:
					textColor = Colors.Cyan;
					break;
				case 3:
					textColor = Colors.PurpleLight;
					break;
				case 4:
					textColor = Colors.RedLight;
					break;
				case 5:
					textColor = Colors.Orange;
					break;
				case 6:
					textColor = Colors.Yellow;
					break;
			}
			Color = textColor;
			Position = p.Bones[Bone.SKEL_Head].Position + new Vector3(0, 0, 0.4f + index * 0.24f);
			HUD.DrawText3D(Position, Color, Message, CitizenFX.Core.UI.Font.ChaletLondon);
		}
	}
}
