using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;
using TheLastPlanet.Client.Core.Status;
using TheLastPlanet.Client.Core.Utility.HUD;

namespace TheLastPlanet.Client.Core.status.Interfacce
{
	public class Statistica
	{
		private float _val;
		public string Name;
		public string NotificationText;
		public string StatName;
		public float ChangeVal;
		[JsonIgnore]
		public Delegate TickCallback;
		public float Val
		{
			get { return _val; }
			set
			{
				float var = value - _val;
				if (var > StatsNeeds.StatusMax)
					_val = StatsNeeds.StatusMax;
				else if (var < 0)
					_val = 0;
				else
					_val += var;
			}
		}

		public Statistica(string name, string statName, string notif, Delegate callback = null)
		{
			Name = name;
			StatName = statName;
			NotificationText = notif;
			_val = 0;
			ChangeVal = 0;
			TickCallback = callback;
		}

		public void OnTick(Ped ped, Player player) => TickCallback.DynamicInvoke(ped, player, this);
		public float GetPercent(float max) => _val / max * 100;
		public float GetPercent() => _val / StatsNeeds.StatusMax * 100;

		public void ShowStatNotification() => HUD.ShowStatNotification((int)Val, NotificationText);
	}
}
