﻿using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScaleformUI
{
    public class NativeUIScaleform : BaseScript
    {
        public static MediumMessageHandler MedMessageInstance { get; set; }
        public static InstructionalButtonsScaleform InstructionalButtons { get; set; }
        public static BigMessageHandler BigMessageInstance { get; set; }
        public static PopupWarning Warning { get; set; }
        internal static Scaleform _nativeui { get; set; }
        public NativeUIScaleform()
        {
            Warning = new PopupWarning();
            MedMessageInstance = new MediumMessageHandler();
            BigMessageInstance = new BigMessageHandler();
            InstructionalButtons = new InstructionalButtonsScaleform();
            InstructionalButtons.Load();
            _nativeui = new Scaleform("nativeui");
            Tick += NativeUIThread_Tick;
        }

        private async Task NativeUIThread_Tick()
        {
            Warning.Update();
            MedMessageInstance.Update();
            BigMessageInstance.Update();
            InstructionalButtons.HandleScaleform();

            if (_nativeui is null)
                _nativeui = new Scaleform("nativeui");

            await Task.FromResult(0);
        }
    }
}
