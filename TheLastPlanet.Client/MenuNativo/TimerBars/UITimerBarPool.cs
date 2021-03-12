using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core.UI;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;

namespace TheLastPlanet.Client.MenuNativo
{
	public class UITimerBarPool
	{
		public List<UITimerBarItem> TimerBars;

		public UITimerBarPool()
		{
			TimerBars = new List<UITimerBarItem>();
		}

		public void Add(UITimerBarItem item)
		{
			TimerBars.Add(item);
		}

		public void Remove(int id)
		{
			TimerBars.RemoveAt(id);
		}

		public void Remove(UITimerBarItem item)
		{
			TimerBars.Remove(item);
		}

		public async Task Draw()
		{
			for (int i=0; i<TimerBars.Count; i++)
			{
				UITimerBarItem item = TimerBars[i];
				item.Draw(38 + 38 * i + (Screen.LoadingPrompt.IsActive || HUD.MenuPool.IsAnyMenuOpen ? 38 : 0));
			}
//			for (int i = 6; i < 10; i++)
//				HideHudComponentThisFrame(i);
		}
	}
}
