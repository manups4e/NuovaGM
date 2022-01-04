using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScaleformUI
{
    public class NativeUIScaleform : BaseScript
    {
        public static PauseMenuScaleform PauseMenu { get; set; }
        public static MediumMessageHandler MedMessageInstance { get; set; }
        public static InstructionalButtonsScaleform InstructionalButtons { get; set; }
        public static BigMessageHandler BigMessageInstance { get; set; }
        public static PopupWarning Warning { get; set; }
        public static PlayerListHandler PlayerListInstance { get; set; }
        public static MissionSelectorHandler JobMissionSelection { get; set; }
        internal static Scaleform _nativeui { get; set; }
        public NativeUIScaleform()
        {
            Warning = new();
            MedMessageInstance = new();
            BigMessageInstance = new();
            PlayerListInstance = new();
            JobMissionSelection = new();
            PauseMenu = new();
            _nativeui = new("nativeui");
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

            if (_nativeui is null)
                _nativeui = new Scaleform("nativeui");

            if (!PauseMenu.Loaded)
                PauseMenu.Load();

            await Task.FromResult(0);
        }
    }
}
