using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.MenuNativo
{
	public class UITimerBarProgressItem : UITimerBarItem
	{
		public UIResRectangle BackgroundProgressBar;
		public UIResRectangle ProgressBar;
		protected float _max;
		protected float _value;
		protected int _multiplier = 5;
		public float Percentage
		{
			get
			{
				return _value;
			}
			set
			{
				if (value > _max)
					_value = _max;
				else if (value < 0)
					_value = 0;
				else
					_value = value;
				ProgressBar.Size = new SizeF(150f / _max * _value, ProgressBar.Size.Height);
			}
		}

		public UITimerBarProgressItem(string text, float max, float starting, Color color) : base(text, color)
		{
			_max = max;
			_value = starting;
			Background = new Sprite("timerbars", "all_black_bg", new PointF(0, 0), new SizeF(350, 35), 0, color);
			Text = new UIResText(text != string.Empty ? text : "N/A", new PointF(0, 0), 0.35f, Color.FromArgb(255, 255, 255, 255), CitizenFX.Core.UI.Font.ChaletLondon, CitizenFX.Core.UI.Alignment.Right);
			BackgroundProgressBar = new UIResRectangle(new PointF(0, 0), new SizeF(150, 17), Color.FromArgb(100, 255, 0, 0));
			ProgressBar = new UIResRectangle(new PointF(0, 0), new SizeF(0, 17), Color.FromArgb(255, 255, 0, 0));
//			Position = new PointF(1540, 1060);
			Position = new PointF(1580, 1082);
		}

		public override async void Draw(float offset)
		{
			if (!Enabled) return;
			base.Draw(offset);
			BackgroundProgressBar.Position= new PointF(Position.X + 190.0f, Position.Y - offset + 10.0f);
			ProgressBar.Position = new PointF(Position.X + 190.0f, Position.Y - offset + 10.0f);
			BackgroundProgressBar.Draw();
			ProgressBar.Draw();
		}
	}
}
