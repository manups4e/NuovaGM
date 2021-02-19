using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace TheLastPlanet.Client.Core.Utility.HUD
{
	static class ProximityChat
	{
		public static void Init()
		{
			Client.Instance.AddEventHandler("lprp:triggerProximityDisplay", new Action<int, string, string, int, int, int>(TriggerProximtyDisplay));
			Client.Instance.AddTick(Prossimità);
		}

		static Dictionary<int, List<ProxMess>> Messaggi = new Dictionary<int, List<ProxMess>>();

		public static void TriggerProximtyDisplay(int id, string title, string text, int a, int b, int c)
		{
			Player target = new Player(API.GetPlayerFromServerId(id));
			if (Game.PlayerPed.IsInRangeOf(target.Character.Position, 19f))
			{
				if (Messaggi.ContainsKey(id))
					Messaggi[id].Add(new ProxMess(text, Color.FromArgb(200, a, b, c), target.Character.Bones[Bone.SKEL_Head].Position, Messaggi[id].Count));
				else
					Messaggi.Add(id, new List<ProxMess>() { new ProxMess(text, Color.FromArgb(200, a, b, c), target.Character.Bones[Bone.SKEL_Head].Position, 1) });
			}
		}

		public static async Task Prossimità()
		{
			foreach(var p in Messaggi)
			{
				Player player = new Player(API.GetPlayerFromServerId(p.Key));
				Ped ped = player.Character;
				foreach (var m in p.Value.ToList())
				{
					m.Position = ped.Bones[Bone.SKEL_Head].Position + new Vector3(0, 0, 1 + ((p.Value.Count - m.Id) * 0.25f));
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
			// p.Value.Count - m.Id (per far scorrere al contrario)
			await Task.FromResult(0);
		}
	}

	public class ProxMess
	{
		public string Message;
		public Color Color;
		public Vector3 Position;
		public int Id;
		public TimeSpan Tempo = new TimeSpan(0, 0, 3);
		public int Timer = 0;
		public ProxMess(string mess, Color color, Vector3 pos, int id)
		{
			Message = mess;
			Color = color;
			Position = pos;
			Id = id;
		}

		/// <summary>
		/// Chiamata ad ogni frame!
		/// </summary>
		public void Draw()
		{
			if (Timer == 0) Timer = Game.GameTime;
			HUD.DrawText3D(Position, Color, Message, 0);
		}
	}
}
