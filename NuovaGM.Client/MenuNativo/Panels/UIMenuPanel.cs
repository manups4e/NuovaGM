﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.MenuNativo
{
	public class UIMenuPanel
	{
		internal dynamic Background;
		public UIMenuPanel()
		{
		}

		public readonly SizeF res = ScreenTools.ResolutionMaintainRatio;

		public virtual bool Selected { get; set; }
		public virtual bool Enabled { get; set; }
		internal virtual void Position(float y)
		{
		}
		public virtual void UpdateParent()
		{
			ParentItem.Parent.ListChange(ParentItem, ParentItem.Index);
			ParentItem.ListChangedTrigger(ParentItem.Index);
		}
		internal async virtual Task Draw()
		{
		}

		public void SetParentItem(UIMenuListItem item)
		{
			ParentItem = item;
		}

		public UIMenuListItem ParentItem { get; set; }

	}
}
