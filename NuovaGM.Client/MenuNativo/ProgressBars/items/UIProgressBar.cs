using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuovaGM.Client.MenuNativo
{
	public class UIProgressBar
	{
		public UIResRectangle Background;
		public UIResText Text;
		public UIResRectangle ProgressBar;
		public PointF Position;

		public int Percentage;

		public UIProgressBar(string text) : this(text, new PointF(800, 1030), Color.FromArgb(100, 255, 255, 255))
		{
			Text = new UIResText(text != "" ? text : "N/A", new PointF(0, 0), 0.35f, Color.FromArgb(255, 255, 255, 255), CitizenFX.Core.UI.Font.ChaletLondon, CitizenFX.Core.UI.Alignment.Center);
		}

		public UIProgressBar(string text, PointF position, Color color)
		{
			Text = new UIResText(text != "" ? text : "N/A", new PointF(0, 0), 0.35f, Color.FromArgb(255, 255, 255, 255), CitizenFX.Core.UI.Font.ChaletLondon, CitizenFX.Core.UI.Alignment.Center);
			Background = new UIResRectangle(new PointF(0, 0), new SizeF(350, 40), Color.FromArgb(100, 0, 0, 0));
			ProgressBar = new UIResRectangle(new PointF(0, 0), new SizeF(0, 30), color);
			Position = position;
		}

		public async void Draw()
		{
			Background.Position = Position;
			Text.Position = new PointF(Position.X + 170f, Position.Y + 5f);
			ProgressBar.Position = new PointF(Position.X + 5f, Position.Y + 5f);
			Background.Draw();
			Text.Draw();
			ProgressBar.Draw();
		}

	}
}
