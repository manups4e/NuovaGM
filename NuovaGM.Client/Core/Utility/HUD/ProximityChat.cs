﻿using System;
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
			Ped io = new Ped(API.PlayerPedId());
			if (Messaggi.Count > 0)
			{
				foreach (var p in Messaggi)
				{
					Player player = new Player(API.GetPlayerFromServerId(p.Key));
					Ped ped = player.Character;
					if (io.IsInRangeOf(ped.Position, 19f))
					{

						if (p.Value.Count > 0)
						{
							foreach (var m in p.Value.ToList())
							{
								Color textColor = Colors.WhiteSmoke;
								switch (p.Value.Count - p.Value.IndexOf(m))
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
								if (m.Timer == 0) m.Timer = Game.GameTime;
								m.Position = ped.Bones[Bone.SKEL_Head].Position + new Vector3(0, 0, 0.4f + (p.Value.Count - p.Value.IndexOf(m)) * 0.24f);
								m.Color = textColor;
								m.Draw();
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
		public void Draw()
		{
			HUD.DrawText3D(Position, Color, Message, 0);
		}
	}
}
