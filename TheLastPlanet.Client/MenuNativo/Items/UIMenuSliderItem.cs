﻿using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.MenuNativo
{
	public class UIMenuSliderItem : UIMenuItem
	{
		protected internal Sprite _arrowLeft;
		protected internal Sprite _arrowRight;

		protected internal UIResRectangle _rectangleBackground;
		protected internal UIResRectangle _rectangleSlider;
		protected internal UIResRectangle _rectangleDivider;

		protected internal int _value = 0;
		protected internal int _max = 100;
		protected internal int _multiplier = 5;
		protected internal bool Divider;


		/// <summary>
		/// Triggered when the slider is changed.
		/// </summary>
		public event ItemSliderEvent OnSliderChanged;


		/// <summary>
		/// The maximum value of the slider.
		/// </summary>
		public int Maximum
		{
			get
			{
				return _max;
			}
			set
			{
				_max = value;
				if (_value > value)
				{
					_value = value;
				}
			}
		}
		/// <summary>
		/// Curent value of the slider.
		/// </summary>
		public int Value
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
				SliderChanged(_value);
			}
		}
		/// <summary>
		/// The multiplier of the left and right navigation movements.
		/// </summary>
		public int Multiplier
		{
			get
			{
				return _multiplier;
			}
			set
			{
				_multiplier = value;
			}
		}


		/// <summary>
		/// List item, with slider.
		/// </summary>
		/// <param name="text">Item label.</param>
		/// <param name="items">List that contains your items.</param>
		/// <param name="index">Index in the list. If unsure user 0.</param>
		public UIMenuSliderItem(string text) : this(text, "", false)
		{
		}

		/// <summary>
		/// List item, with slider.
		/// </summary>
		/// <param name="text">Item label.</param>
		/// <param name="items">List that contains your items.</param>
		/// <param name="index">Index in the list. If unsure user 0.</param>
		/// <param name="description">Description for this item.</param>
		public UIMenuSliderItem(string text, string description) : this(text, description, false)
		{
		}

		/// <summary>
		/// List item, with slider.
		/// </summary>
		/// <param name="text">Item label.</param>
		/// <param name="items">List that contains your items.</param>
		/// <param name="index">Index in the list. If unsure user 0.</param>
		/// <param name="description">Description for this item.</param>
		/// /// <param name="divider">Put a divider in the center of the slider</param>
		public UIMenuSliderItem(string text, string description, bool divider) : base(text, description)
		{
			const int y = 0;
			_arrowLeft = new Sprite("commonmenutu", "arrowleft", new Point(0, 105 + y), new Size(15, 15));
			_arrowRight = new Sprite("commonmenutu", "arrowright", new Point(0, 105 + y), new Size(15, 15));
			_rectangleBackground = new UIResRectangle(new Point(0, 0), new Size(150, 9), Color.FromArgb(255, 4, 32, 57));
			_rectangleSlider = new UIResRectangle(new Point(0, 0), new Size(75, 9), Color.FromArgb(255, 57, 116, 200));
			Divider = divider;
			if (divider)
			{
				_rectangleDivider = new UIResRectangle(new Point(0, 0), new Size(2, 20), Colors.WhiteSmoke);
			}
			else
			{
				_rectangleDivider = new UIResRectangle(new Point(0, 0), new Size(2, 20), Color.Transparent);
			}
		}

		/// <summary>
		/// Change item's position.
		/// </summary>
		/// <param name="y">New Y position.</param>
		public override void Position(int y)
		{
			_rectangleBackground.Position = new PointF(250 + Offset.X, y + 158 + Offset.Y);
			_rectangleSlider.Position = new PointF(250 + Offset.X, y + 158 + Offset.Y);
			_rectangleDivider.Position = new PointF(323 + Offset.X, y + 153 + Offset.Y);
			_arrowLeft.Position = new PointF(235 + Offset.X + Parent.WidthOffset, 155 + y + Offset.Y);
			_arrowRight.Position = new PointF(400 + Offset.X + Parent.WidthOffset, 155 + y + Offset.Y);
			base.Position(y);
		}

		/// <summary>
		/// Draw item.
		/// </summary>
		public override async Task Draw()
		{
			base.Draw();

			_arrowLeft.Color = Enabled ? Selected ? Colors.Black : Colors.WhiteSmoke : Color.FromArgb(163, 159, 148);
			_arrowRight.Color = Enabled ? Selected ? Colors.Black : Colors.WhiteSmoke : Color.FromArgb(163, 159, 148);
			float offset = 176 + Offset.X + _rectangleBackground.Size.Width - _rectangleSlider.Size.Width;
			_rectangleSlider.Position = new PointF((int)(offset + (_value / (float)_max * 73)), _rectangleSlider.Position.Y);
			_arrowLeft.Draw();
			_arrowRight.Draw();
			_rectangleBackground.Draw();
			_rectangleSlider.Draw();
			_rectangleDivider.Draw();
		}

		internal virtual void SliderChanged(int Value)
		{
			OnSliderChanged?.Invoke(this, Value);
		}
	}
}