using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;
using TheLastPlanet.Client.Core.Status;

namespace TheLastPlanet.Client.Core.status.Interfacce
{
	public class Necessità
	{
		private float _val;
		public string Name;
		public float ChangeVal;
		[JsonIgnore] public Delegate TickCallback;
		public float Val
		{
			get => _val;
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

		public Necessità(string name, float defalt, float changeVal, Delegate callback = null)
		{
			Name = name;
			_val = defalt;
			ChangeVal = changeVal;
			TickCallback = callback;
		}

		public void OnTick(Ped ped, Player me) { TickCallback.DynamicInvoke(ped, me, this); }

		public float GetPercent(float max) { return _val / max * 100; }

		public float GetPercent() { return _val / StatsNeeds.StatusMax * 100; }
	}
}