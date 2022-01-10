﻿using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScaleformUI
{
    public class ScaleformUI : BaseScript
    {
        public static PauseMenuScaleform PauseMenu { get; set; }
        public static MediumMessageHandler MedMessageInstance { get; set; }
        public static InstructionalButtonsScaleform InstructionalButtons { get; set; }
        public static BigMessageHandler BigMessageInstance { get; set; }
        public static PopupWarning Warning { get; set; }
        public static PlayerListHandler PlayerListInstance { get; set; }
        public static MissionSelectorHandler JobMissionSelection { get; set; }
        internal static Scaleform _ui { get; set; }
        public ScaleformUI()
        {
            Warning = new();
            MedMessageInstance = new();
            BigMessageInstance = new();
            PlayerListInstance = new();
            JobMissionSelection = new();
            PauseMenu = new();
            _ui = new("scaleformui");
            InstructionalButtons = new();
            InstructionalButtons.Load();
            Tick += NativeUIThread_Tick;
        }

        private async Task NativeUIThread_Tick()
        {
            Warning.Update();
            MedMessageInstance.Update();
            BigMessageInstance.Update();
            PlayerListInstance.Update();
            JobMissionSelection.Update();
            InstructionalButtons.HandleScaleform();

            if (_ui is null)
                _ui = new Scaleform("scaleformui");

            if (!PauseMenu.Loaded)
                PauseMenu.Load();

            await Task.FromResult(0);
        }
    }
}
