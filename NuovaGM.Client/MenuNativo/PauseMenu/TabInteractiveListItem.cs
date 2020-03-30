﻿using System;
using System.Collections.Generic;
using System.Drawing;
using Font = CitizenFX.Core.UI.Font;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;

namespace NuovaGM.Client.MenuNativo.PauseMenu
{
    public class TabInteractiveListItem : TabItem
    {
        public TabInteractiveListItem(string name, IEnumerable<UIMenuItem> items) : base(name)
        {
            DrawBg = false;
            CanBeFocused = true;
            Items = new List<UIMenuItem>(items);
            IsInList = true;
            _maxItem = MaxItemsPerView;
            _minItem = 0;
        }

        public List<UIMenuItem> Items { get; set; }
        public int Index { get; set; }
        public bool IsInList { get; set; }
        protected const int MaxItemsPerView = 15;
        protected int _minItem;
        protected int _maxItem;

        public void MoveDown()
        {
            Index = (1000 - (1000 % Items.Count) + Index + 1) % Items.Count;

            if (Items.Count <= MaxItemsPerView) return;

            if (Index >= _maxItem)
            {
                _maxItem++;
                _minItem++;
            }

            if (Index == 0)
            {
                _minItem = 0;
                _maxItem = MaxItemsPerView;
            }
        }

        public void MoveUp()
        {
            Index = (1000 - (1000 % Items.Count) + Index - 1) % Items.Count;

            if (Items.Count <= MaxItemsPerView) return;

            if (Index < _minItem)
            {
                _minItem--;
                _maxItem--;
            }

            if (Index == Items.Count - 1)
            {
                _minItem = Items.Count - MaxItemsPerView;
                _maxItem = Items.Count;
            }
        }

        public void RefreshIndex()
        {
            Index = 0;
            _maxItem = MaxItemsPerView;
            _minItem = 0;
        }


        public override void ProcessControls()
        {
            if (!Visible) return;
            if (JustOpened)
            {
                JustOpened = false;
                return;
            }

            if (!Focused) return;

            if (Items.Count == 0) return;


            if (Input.IsControlJustPressed(Control.FrontendAccept) && Focused && Items[Index] is UIMenuCheckboxItem)
            {
                Game.PlaySound("SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                ((UIMenuCheckboxItem)Items[Index]).Checked = !((UIMenuCheckboxItem)Items[Index]).Checked;
                ((UIMenuCheckboxItem)Items[Index]).CheckboxEventTrigger();
            }
            else if (Input.IsControlJustPressed(Control.FrontendAccept) && Focused)
            {
                Game.PlaySound("SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                Items[Index].ItemActivate(null);
            }

            if (Input.IsControlJustPressed(Control.FrontendLeft) && Focused && Items[Index] is UIMenuListItem)
            {
                var it = (UIMenuListItem)Items[Index];
                it.Index--;
                Game.PlaySound("NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                it.ListChangedTrigger(it.Index);
            }

            if (Input.IsControlJustPressed(Control.FrontendRight) && Focused && Items[Index] is UIMenuListItem)
            {
                var it = (UIMenuListItem)Items[Index];
                it.Index++;
                Game.PlaySound("NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                it.ListChangedTrigger(it.Index);
            }

            if (Input.IsControlJustPressed(Control.FrontendUp) || Input.IsControlJustPressed(Control.MoveUpOnly) || Input.IsControlJustPressed(Control.CursorScrollUp))
            {
                Game.PlaySound("NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                MoveUp();
            }

            else if (Input.IsControlJustPressed(Control.FrontendDown) || Input.IsControlJustPressed(Control.MoveDownOnly) || Input.IsControlJustPressed(Control.CursorScrollDown))
            {
                Game.PlaySound("NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                MoveDown();
            }
        }

        public override void Draw()
        {
            if (!Visible) return;
            base.Draw();

            var res = ScreenTools.ResolutionMaintainRatio;

            var alpha = Focused ? 120 : 30;
            var blackAlpha = Focused ? 200 : 100;
            var fullAlpha = Focused ? 255 : 150;

            var submenuWidth = (BottomRight.X - TopLeft.X);
            var itemSize = new SizeF(submenuWidth, 40);

            int i = 0;
            for (int c = _minItem; c < Math.Min(Items.Count, _maxItem); c++)
            {
                var hovering = ScreenTools.IsMouseInBounds(SafeSize.AddPoints(new PointF(0, (itemSize.Height + 3) * i)),
                    itemSize);

                var hasLeftBadge = Items[c].LeftBadge != UIMenuItem.BadgeStyle.None;
                var hasRightBadge = Items[c].RightBadge != UIMenuItem.BadgeStyle.None;

                var hasBothBadges = hasRightBadge && hasLeftBadge;
                var hasAnyBadge = hasRightBadge || hasLeftBadge;

                new UIResRectangle(SafeSize.AddPoints(new PointF(0, (itemSize.Height + 3) * i)), itemSize, (Index == c && Focused) ? Color.FromArgb(fullAlpha, Colors.White) : Focused && hovering ? Color.FromArgb(100, 50, 50, 50) : Color.FromArgb(blackAlpha, Colors.Black)).Draw();
                new UIResText(Items[c].Text, SafeSize.AddPoints(new PointF((hasBothBadges ? 60 : hasAnyBadge ? 30 : 6), 5 + (itemSize.Height + 3) * i)), 0.35f, Color.FromArgb(fullAlpha, (Index == c && Focused) ? Colors.Black : Colors.White)).Draw();

                if (hasLeftBadge && !hasRightBadge)
                {
                    new Sprite(UIMenuItem.BadgeToSpriteLib(Items[c].LeftBadge),
                        UIMenuItem.BadgeToSpriteName(Items[c].LeftBadge, (Index == c && Focused)), SafeSize.AddPoints(new PointF(-2, 1 + (itemSize.Height + 3) * i)), new SizeF(40, 40), 0f,
                        UIMenuItem.BadgeToColor(Items[c].LeftBadge, (Index == c && Focused))).Draw();
                }

                if (!hasLeftBadge && hasRightBadge)
                {
                    new Sprite(UIMenuItem.BadgeToSpriteLib(Items[c].RightBadge),
                        UIMenuItem.BadgeToSpriteName(Items[c].RightBadge, (Index == c && Focused)), SafeSize.AddPoints(new PointF(-2, 1 + (itemSize.Height + 3) * i)), new SizeF(40, 40), 0f,
                        UIMenuItem.BadgeToColor(Items[c].RightBadge, (Index == c && Focused))).Draw();
                }

                if (hasLeftBadge && hasRightBadge)
                {
                    new Sprite(UIMenuItem.BadgeToSpriteLib(Items[c].LeftBadge),
                        UIMenuItem.BadgeToSpriteName(Items[c].LeftBadge, (Index == c && Focused)), SafeSize.AddPoints(new PointF(-2, 1 + (itemSize.Height + 3) * i)), new SizeF(40, 40), 0f,
                        UIMenuItem.BadgeToColor(Items[c].LeftBadge, (Index == c && Focused))).Draw();

                    new Sprite(UIMenuItem.BadgeToSpriteLib(Items[c].RightBadge),
                        UIMenuItem.BadgeToSpriteName(Items[c].RightBadge, (Index == c && Focused)), SafeSize.AddPoints(new PointF(25, 1 + (itemSize.Height + 3) * i)), new SizeF(40, 40), 0f,
                        UIMenuItem.BadgeToColor(Items[c].RightBadge, (Index == c && Focused))).Draw();
                }

                if (!string.IsNullOrEmpty(Items[c].RightLabel))
                {
                    new UIResText(Items[c].RightLabel,
                        SafeSize.AddPoints(new PointF(BottomRight.X - SafeSize.X - 5, 5 + (itemSize.Height + 3) * i)),
                        0.35f, Color.FromArgb(fullAlpha, (Index == c && Focused) ? Colors.Black : Colors.White),
                        Font.ChaletLondon, Alignment.Right).Draw();
                }

                if (Items[c] is UIMenuCheckboxItem)
                {
                    string textureName = "";
                    if (c == Index && Focused)
                    {
                        textureName = ((UIMenuCheckboxItem)Items[c]).Checked ? "shop_box_tickb" : "shop_box_blankb";
                    }
                    else
                    {
                        textureName = ((UIMenuCheckboxItem)Items[c]).Checked ? "shop_box_tick" : "shop_box_blank";
                    }
                    new Sprite("commonmenu", textureName, SafeSize.AddPoints(new PointF(BottomRight.X - SafeSize.X - 60, -5 + (itemSize.Height + 3) * i)), new SizeF(50, 50)).Draw();
                }
                else if (Items[c] is UIMenuListItem convItem)
                {
                    var yoffset = 5;
                    var basePos =
                        SafeSize.AddPoints(new PointF(BottomRight.X - SafeSize.X - 30, yoffset + (itemSize.Height + 3) * i));

                    var arrowLeft = new Sprite("commonmenu", "arrowleft", basePos, new SizeF(30, 30));
                    var arrowRight = new Sprite("commonmenu", "arrowright", basePos, new SizeF(30, 30));
                    var itemText = new UIResText("", basePos, 0.35f, Colors.White, Font.ChaletLondon,
                        Alignment.Left)
                    { TextAlignment = Alignment.Right };

                    string caption = convItem.Items[convItem.Index].ToString();
					float offset = ScreenTools.GetTextWidth(caption, itemText.Font, itemText.Scale);

                    var selected = c == Index && Focused;

                    itemText.Color = convItem.Enabled ? selected ? Colors.Black : Colors.WhiteSmoke : Color.FromArgb(163, 159, 148);

                    itemText.Caption = caption;

                    arrowLeft.Color = convItem.Enabled ? selected ? Colors.Black : Colors.WhiteSmoke : Color.FromArgb(163, 159, 148);
                    arrowRight.Color = convItem.Enabled ? selected ? Colors.Black : Colors.WhiteSmoke : Color.FromArgb(163, 159, 148);

                    arrowLeft.Position =
                        SafeSize.AddPoints(new PointF(BottomRight.X - SafeSize.X - 60 - (int)offset, yoffset + (itemSize.Height + 3) * i));
                    if (selected)
                    {
                        arrowLeft.Draw();
                        arrowRight.Draw();
                        itemText.Position = SafeSize.AddPoints(new PointF(BottomRight.X - SafeSize.X - 30, yoffset + (itemSize.Height + 3) * i));
                    }
                    else
                    {
                        itemText.Position = SafeSize.AddPoints(new PointF(BottomRight.X - SafeSize.X - 5, yoffset + (itemSize.Height + 3) * i));
                    }

                    itemText.Draw();
                }

                if (Focused && hovering && Input.IsControlJustPressed(Control.CursorAccept))
                {
                    bool open = Index == c;
                    Index = (1000 - (1000 % Items.Count) + c) % Items.Count;
                    if (!open)
                        Game.PlaySound("NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                    else
                    {
                        if (Items[Index] is UIMenuCheckboxItem)
                        {
                            Game.PlaySound("SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                            ((UIMenuCheckboxItem)Items[Index]).Checked = !((UIMenuCheckboxItem)Items[Index]).Checked;
                            ((UIMenuCheckboxItem)Items[Index]).CheckboxEventTrigger();
                        }
                        else
                        {
                            Game.PlaySound("SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                            Items[Index].ItemActivate(null);
                        }
                    }
                }

                i++;
            }
        }
    }
}