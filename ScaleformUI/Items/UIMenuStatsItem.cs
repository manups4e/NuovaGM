﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScaleformUI
{
    public delegate void StatChanged(int value);
    public class UIMenuStatsItem : UIMenuItem
    {
        private int _value;

        public int Value { get => _value; set
            {
               _value = value;
                SetValue(_value);
            }
        }
        public int Type { get; set; }
        public HudColor Color { get; set; }
        public event StatChanged OnStatChanged;

        public UIMenuStatsItem(string text) : this(text, "", 0, HudColor.HUD_COLOUR_FREEMODE)
        {
        }

        public UIMenuStatsItem(string text, string subtitle, int value, HudColor color) : base(text, subtitle)
        {
            Type = 0;
            _value = value;
            Color = color;
        }

        public void SetValue(int value)
        {
            ScaleformUI._ui.CallFunction("SET_ITEM_VALUE", Parent.MenuItems.IndexOf(this), value);
            OnStatChanged?.Invoke(value);
        }
    }
}
