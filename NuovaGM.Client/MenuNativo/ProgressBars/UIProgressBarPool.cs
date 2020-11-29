﻿using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.MenuNativo
{
	public class UIProgressBarPool
	{
		public List<UIProgressBar> ProgressBars = new List<UIProgressBar>();

		public void Add(UIProgressBar bar)
		{
			ProgressBars.Add(bar);
		}

		public void Remove(UIProgressBar bar)
		{
			ProgressBars.Remove(bar);
		}

		public void Remove(int id)
		{
			ProgressBars.RemoveAt(id);
		}

		public async void Draw()
		{
			for (int i=0; i<ProgressBars.Count; i++)
			{
				ProgressBars[i].Draw();
			}
		}
	}
}
