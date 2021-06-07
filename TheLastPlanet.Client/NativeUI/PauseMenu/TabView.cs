﻿using System;
using System.Collections.Generic;
using System.Drawing;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using TheLastPlanet.Client.Core;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.RolePlay.Core;
using TheLastPlanet.Client.SessionCache;
using Font = CitizenFX.Core.UI.Font;

namespace TheLastPlanet.Client.NativeUI.PauseMenu
{
    public class TabView
    {
        public delegate void PauseMenuClosing();
        public TabView(string title)
        {
            Title = title;
            SubTitle = "";
            SideStringTop = "";
            SideStringMiddle = "";
            SideStringBottom = "";
            Tabs = new List<TabItem>();
            Index = 0;
            Name = Cache.MyPlayer.Player.Name;
            TemporarilyHidden = false;
            CanLeave = true;
            if(!HUD.MenuPool.PauseMenus.Contains(this))
                HUD.MenuPool.PauseMenus.Add(this);
        }
        public TabView(string title, string subtitle)
        {
            Title = title;
            SubTitle = subtitle;
            SideStringTop = "";
            SideStringMiddle = "";
            SideStringBottom = "";
            Tabs = new List<TabItem>();
            Index = 0;
            Name = Cache.MyPlayer.Player.Name;
            TemporarilyHidden = false;
            CanLeave = true;
            if (!HUD.MenuPool.PauseMenus.Contains(this))
                HUD.MenuPool.PauseMenus.Add(this);
        }

        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string SideStringTop { get; set; }
        public string SideStringMiddle { get; set; }
        public string SideStringBottom { get; set; }
        public Tuple<string, string> HeaderPicture { internal get; set; }
        public Sprite Photo { get; set; }
        public string Name { get; set; }
        public string Money { get; set; }
        public string MoneySubtitle { get; set; }
        public List<TabItem> Tabs { get; set; }
        public int FocusLevel { get; set; }
        public bool TemporarilyHidden { get; set; }
        public bool CanLeave { get; set; }
        public bool HideTabs { get; set; }
        public bool DisplayHeader = true;

        protected readonly SizeF Resolution = ScreenTools.ResolutionMaintainRatio;
        internal bool _loaded;
        internal readonly static string _browseTextLocalized = Game.GetGXTEntry("HUD_INPUT1C");

        public event PauseMenuClosing OnMenuClose;

        public bool Visible
        {
	        get => _visible;
	        set
	        {
		        if (value)
		        {
			        API.SetPauseMenuActive(true);
			        Screen.Effects.Start(ScreenEffect.FocusOut, 800);
			        API.TransitionToBlurred(700);
                    InstructionalButtonsHandler.InstructionalButtons.Enabled = true;
                    List<InstructionalButton> kaka = new List<InstructionalButton>()
                    {
                        new InstructionalButton(Control.PhoneSelect, UIMenu._selectTextLocalized),
                        new InstructionalButton(Control.PhoneCancel, UIMenu._backTextLocalized),
                        new InstructionalButton(Control.FrontendRb, ""),
                        new InstructionalButton(Control.FrontendLb, _browseTextLocalized),
                    };
                    InstructionalButtonsHandler.InstructionalButtons.ControlButtons = kaka;
		        }
		        else
		        {
			        API.SetPauseMenuActive(false);
			        Screen.Effects.Start(ScreenEffect.FocusOut, 500);
			        API.TransitionFromBlurred(400);
		        }

		        _visible = value;
	        }
        }
        public void AddTab(TabItem item)
        {
            Tabs.Add(item);
            item.Parent = this;
        }

        public int Index;
        private bool _visible;

        private Scaleform _sc;
        private Scaleform _header;

        public async void ShowHeader()
        {
            if (_header == null)
                _header = new Scaleform("pause_menu_header");
            while (!_header.IsLoaded) await BaseScript.Delay(0);
            if (String.IsNullOrEmpty(SubTitle) || String.IsNullOrWhiteSpace(SubTitle))
                _header.CallFunction("SET_HEADER_TITLE", Title);
            else
            {
                _header.CallFunction("SET_HEADER_TITLE", Title, false, SubTitle);
                _header.CallFunction("SHIFT_CORONA_DESC", true);
            }
            if (HeaderPicture != null)
                _header.CallFunction("SET_CHAR_IMG", HeaderPicture.Item1, HeaderPicture.Item2, true);
            else
            {
                int mugshot = API.RegisterPedheadshot(API.PlayerPedId());
                while (!API.IsPedheadshotReady(mugshot)) await BaseScript.Delay(1);
                string Txd = API.GetPedheadshotTxdString(mugshot);
                HeaderPicture = new Tuple<string, string>(Txd, Txd);
                API.ReleasePedheadshotImgUpload(mugshot);
                _header.CallFunction("SET_CHAR_IMG", HeaderPicture.Item1, HeaderPicture.Item2, true);
            }
            _header.CallFunction("SET_HEADING_DETAILS", SideStringTop, SideStringMiddle, SideStringBottom, false);
            _header.CallFunction("BUILD_MENU");
            _header.CallFunction("adjustHeaderPositions");
            _header.CallFunction("SHOW_HEADING_DETAILS", true);
            _header.CallFunction("SHOW_MENU", true);
            _loaded = true;
        }

        public void DrawInstructionalButton(int slot, Control control, string text)
        {
            _sc.CallFunction("SET_DATA_SLOT", slot, API.GetControlInstructionalButton(2, (int)control, 0), text);
        }

        public void ProcessControls()
        {
            if (!Visible || TemporarilyHidden) return;
            API.DisableAllControlActions(0);

            if (Game.IsControlJustPressed(2, Control.PhoneLeft) && FocusLevel == 0)
            {
                Tabs[Index].Active = false;
                Tabs[Index].Focused = false;
                Tabs[Index].Visible = false;
                Index = (1000 - (1000 % Tabs.Count) + Index - 1) % Tabs.Count;
                Tabs[Index].Active = true;
                Tabs[Index].Focused = false;
                Tabs[Index].Visible = true;

                Game.PlaySound("NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET");
            }

            else if (Game.IsControlJustPressed(2, Control.PhoneRight) && FocusLevel == 0)
            {
                Tabs[Index].Active = false;
                Tabs[Index].Focused = false;
                Tabs[Index].Visible = false;
                Index = (1000 - (1000 % Tabs.Count) + Index + 1) % Tabs.Count;
                Tabs[Index].Active = true;
                Tabs[Index].Focused = false;
                Tabs[Index].Visible = true;
                Game.PlaySound("NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET");
            }

            else if (Game.IsControlJustPressed(2, Control.FrontendAccept) && FocusLevel == 0)
            {
                if (Tabs[Index].CanBeFocused)
                {
                    Tabs[Index].Focused = true;
                    Tabs[Index].JustOpened = true;
                    FocusLevel = 1;
                }
                else
                {
                    Tabs[Index].JustOpened = true;
                    Tabs[Index].OnActivated();
                }

                Game.PlaySound("SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET");

            }

            else if (Game.IsControlJustPressed(2, Control.PhoneCancel))
            {
                if (FocusLevel == 1)
                {
                    Tabs[Index].Focused = false;
                    FocusLevel = 0;
                    Game.PlaySound("BACK", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                }
                else if (FocusLevel == 0 && CanLeave)
                {
                    Visible = false;
                    Game.PlaySound("BACK", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                    OnMenuClose?.Invoke();
                    _loaded = false;
                    _header.CallFunction("REMOVE_MENU", true);
                    _header.Dispose();
                    _header = null;
                }
            }

            if (!HideTabs)
            {
                if (Game.IsControlJustPressed(0, Control.FrontendLb))
                {
                    Tabs[Index].Active = false;
                    Tabs[Index].Focused = false;
                    Tabs[Index].Visible = false;
                    Index = (1000 - (1000 % Tabs.Count) + Index - 1) % Tabs.Count;
                    Tabs[Index].Active = true;
                    Tabs[Index].Focused = false;
                    Tabs[Index].Visible = true;

                    FocusLevel = 0;

                    Game.PlaySound("NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                }

                else if (Game.IsControlJustPressed(0, Control.FrontendRb))
                {
                    Tabs[Index].Active = false;
                    Tabs[Index].Focused = false;
                    Tabs[Index].Visible = false;
                    Index = (1000 - (1000 % Tabs.Count) + Index + 1) % Tabs.Count;
                    Tabs[Index].Active = true;
                    Tabs[Index].Focused = false;
                    Tabs[Index].Visible = true;

                    FocusLevel = 0;

                    Game.PlaySound("NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                }
            }

            if (Tabs.Count > 0) Tabs[Index].ProcessControls();
        }

        public void RefreshIndex()
        {
            foreach (TabItem item in Tabs)
            {
                item.Focused = false;
                item.Active = false;
                item.Visible = false;
            }

            Index = (1000 - (1000 % Tabs.Count)) % Tabs.Count;
            Tabs[Index].Active = true;
            Tabs[Index].Focused = false;
            Tabs[Index].Visible = true;
            FocusLevel = 0;
        }

        public void Draw()
        {
            if (!Visible || TemporarilyHidden) return;
            API.HideHudAndRadarThisFrame();
            API.ShowCursorThisFrame();


            PointF safe = new PointF(300, SubTitle != null && SubTitle != "" ? 205 : 195);
            if (!HideTabs)
            {
                for (int i = 0; i < Tabs.Count; i++)
                {
                    float activeSize = Resolution.Width - 2 * safe.X;
                    activeSize -= 5;
                    float tabWidth = ((int)activeSize / Tabs.Count) - 1.95f;
                    Game.EnableControlThisFrame(0, Control.CursorX);
                    Game.EnableControlThisFrame(0, Control.CursorY);

                    bool hovering = ScreenTools.IsMouseInBounds(safe.AddPoints(new PointF((tabWidth + 5) * i, 0)),
                        new SizeF(tabWidth, 40));

                    Color tabColor = Tabs[i].Active ? Colors.White : hovering ? Color.FromArgb(100, 50, 50, 50) : Colors.Black;
                    new UIResRectangle(safe.AddPoints(new PointF((tabWidth + 5) * i, 0)), new SizeF(tabWidth, 40), Color.FromArgb(Tabs[i].Active ? 255 : 200, tabColor)).Draw();
                    if (Tabs[i].Active)
                        new UIResRectangle(safe.SubtractPoints(new PointF(-((tabWidth + 5) * i), 10)), new SizeF(tabWidth, 10), Colors.DodgerBlue).Draw();

                    new UIResText(Tabs[i].Title.ToUpper(), safe.AddPoints(new PointF((tabWidth / 2) + (tabWidth + 5) * i, 5)), 0.3f, Tabs[i].Active ? Colors.Black : Colors.White, Font.ChaletLondon, Alignment.Center).Draw();

                    if (hovering && Game.IsControlJustPressed(0, Control.CursorAccept) && !Tabs[i].Active)
                    {
                        Tabs[Index].Active = false;
                        Tabs[Index].Focused = false;
                        Tabs[Index].Visible = false;
                        Index = (1000 - (1000 % Tabs.Count) + i) % Tabs.Count;
                        Tabs[Index].Active = true;
                        Tabs[Index].Focused = true;
                        Tabs[Index].Visible = true;
                        Tabs[Index].JustOpened = true;

                        if (Tabs[Index].CanBeFocused)
                            FocusLevel = 1;
                        else
                            FocusLevel = 0;

                        Game.PlaySound("NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                    }
                }
            }
            Tabs[Index].Draw();

            _sc.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
            if (!Main.ImpostazioniClient.ModCinema)
                _sc.Render2D();
            else
                API.DrawScaleformMovie(_sc.Handle, 0.5f, 0.5f - (Main.ImpostazioniClient.LetterBox / 1000), 1f, 1f, 255, 255, 255, 255, 0);
            if (DisplayHeader)
            {
                if (!_loaded)
                    ShowHeader();
                API.DrawScaleformMovie(_header.Handle, 0.501f, 0.162f, 0.6782f, 0.145f, 255, 255, 255, 255, 0);
            }
        }
    }
}