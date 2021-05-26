using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.NativeUI
{
	public class UITimerBarItem
	{

		public UIResText Text;
		public UIResText TextTimerBar;
		public PointF Position;
		public Sprite Background;
		public bool Enabled;

		public UITimerBarItem(string text) : this(text, Color.FromArgb(200, 255, 255, 255))
		{
			Text = new UIResText(text != string.Empty? text : "N/A", new PointF(0, 0), 0.35f, Color.FromArgb(255, 255, 255, 255), CitizenFX.Core.UI.Font.ChaletLondon, CitizenFX.Core.UI.Alignment.Right);
		}

		public UITimerBarItem(string text, Color color) : this(text, "timerbars", "all_black_bg", color)
		{
			Background = new Sprite("timerbars", "all_black_bg", new PointF(0, 0), new SizeF(350, 35), 0, color);
			Text = new UIResText(text != string.Empty ? text : "N/A", new PointF(0, 0), 0.35f, Color.FromArgb(255, 255, 255, 255), CitizenFX.Core.UI.Font.ChaletLondon, CitizenFX.Core.UI.Alignment.Right);
		}

		public UITimerBarItem(string text, string txtDictionary, string txtName, Color color)
		{
			Background = new Sprite("timerbars", "all_black_bg", new PointF(0, 0), new SizeF(350, 35), 0, color);
			Text = new UIResText(text != string.Empty ? text : "N/A", new PointF(0, 0), 0.35f, Color.FromArgb(255, 255, 255, 255), CitizenFX.Core.UI.Font.ChaletLondon, CitizenFX.Core.UI.Alignment.Right);
			TextTimerBar = new UIResText("", new PointF(0, 0), 0.35f, Color.FromArgb(255, 255, 255, 255), CitizenFX.Core.UI.Font.ChaletLondon, CitizenFX.Core.UI.Alignment.Right);
//			Position = new PointF(1540, 1060);
			Position = new PointF(1580, 1082);
			Enabled = true;
		}

		public async virtual void Draw(float offset)
		{
			if (!Enabled) return;
			Background.Position = new PointF(Position.X, Position.Y - offset);
			Background.Draw();
			Text.Position = new PointF(Position.X + 170.0f, Position.Y - offset);
			Text.Draw();
			if (TextTimerBar.Caption != "")
			{
				TextTimerBar.Position = new PointF(Position.X + 340.0f, Position.Y - offset);
				TextTimerBar.Draw();
			}
			for (int i = 6; i < 10; i++)
				API.HideHudComponentThisFrame(i);
		}
	}
}
