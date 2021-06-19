using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using TheLastPlanet.Client.RolePlay.Core;
using TheLastPlanet.Shared;

namespace TheLastPlanet.Client.NativeUI
{
    public delegate void OnInstructionControlSelected(Control control);

    public class InstructionalButton
    {
        public event OnInstructionControlSelected OnControlSelected;
        public string Text { get; set; }
        public bool IsUsingController => !API.IsUsingKeyboard(2);

        public UIMenuItem ItemBind { get; private set; }

        public Control GamepadButton { get; private set; }
        public Control KeyboardButton { get; private set; }
        public List<Control> ControllerButtons { get; private set; }
        public List<Control> KeyboardButtons { get; private set; }
        public PadCheck PadCheck { get; private set; }
   
        /// <summary>
        /// Add a dynamic button to the instructional buttons array.
        /// Changes whether the controller is being used and changes depending on keybinds.
        /// </summary>
        /// <param name="control">CitizenFX.Core.Control that gets converted into a button.</param>
        /// <param name="text">Help text that goes with the button.</param>
        public InstructionalButton(Control gamepadControl, Control keyboardControl, string text)
        {
            Text = text;
            GamepadButton = gamepadControl;
            KeyboardButton = keyboardControl;
            PadCheck = PadCheck.Any;
        }

        public InstructionalButton(List<Control> gamepadControls, List<Control> keyboardControls, string text)
        {
            Text = text;
            ControllerButtons = gamepadControls;
            KeyboardButtons = keyboardControls;
            PadCheck = PadCheck.Any;
        }


        public InstructionalButton(Control control, string text, PadCheck padFilter = PadCheck.Any)
        {
            if (padFilter == PadCheck.Controller)
                GamepadButton = control;
            else if (padFilter == PadCheck.Keyboard)
                KeyboardButton = control;
            else if (padFilter == PadCheck.Any)
            {
                GamepadButton = control;
                KeyboardButton = control;
            }
            Text = text;
            PadCheck = padFilter;
        }

        public InstructionalButton(List<Control> controls, string text, PadCheck padFilter = PadCheck.Any)
        {
            if (padFilter == PadCheck.Controller)
                ControllerButtons = controls;
            else if (padFilter == PadCheck.Keyboard)
                KeyboardButtons = controls;
            else if (padFilter == PadCheck.Any)
            {
                ControllerButtons = controls;
                KeyboardButtons = controls;
            }
            Text = text;
            PadCheck = padFilter;
        }


        /// <summary>
        /// Bind this button to an item, so it's only shown when that item is selected.
        /// </summary>
        /// <param name="item">Item to bind to.</param>
        public void BindToItem(UIMenuItem item)
        {
            ItemBind = item;
        }

        public string GetButtonId()
        {
            if(KeyboardButtons!= null || ControllerButtons != null)
			{
                string retVal = "";
                if (IsUsingController)
                {
                    for (int i = ControllerButtons.Count - 1; i > -1; i--) 
                    {
                        if (i == 0)
                            retVal += API.GetControlInstructionalButton(2, (int)ControllerButtons[i], 1);
                        else
                            retVal += API.GetControlInstructionalButton(2, (int)ControllerButtons[i], 1) + "%";
                    }
                }
				else
				{
                    for (int i = KeyboardButtons.Count-1; i > -1; i--)
                    {
                        if (i == 0)
                            retVal += API.GetControlInstructionalButton(2, (int)KeyboardButtons[i], 1);
                        else
                            retVal += API.GetControlInstructionalButton(2, (int)KeyboardButtons[i], 1) + "%";
                    }
                }
                return retVal;
            }
            return IsUsingController ? API.GetControlInstructionalButton(2, (int)GamepadButton, 1) : API.GetControlInstructionalButton(2, (int)KeyboardButton, 1);
        }


        public void InvokeEvent(Control control)
		{
            OnControlSelected?.Invoke(control);
        }

    }

    public class InstructionalButtonsScaleform
	{
        private Scaleform _sc;
        private bool _useMouseButtons;
        private bool _enabled;
        internal bool _isUsingKeyboard;
        internal bool _changed = true;
        internal int savingTimer = 0;
        private bool _isSaving;
        
        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (!value)
                {
                    _sc.CallFunction("CLEAR_ALL");
                    _sc.CallFunction("CLEAR_RENDER");
                }
                _enabled = value;
                _changed = true;
            }
        }

        public bool UseMouseButtons
        {
            get => _useMouseButtons;
            set => _useMouseButtons = value;
        }

        public bool IsSaving
		{
            get => _isSaving;
            set => _isSaving = value;
        }

        public List<InstructionalButton> ControlButtons { get; private set; }

		public async void Load()
		{
            if (_sc != null) return;
            _sc = new Scaleform("INSTRUCTIONAL_BUTTONS");
            int timeout = 1000;
            DateTime start = DateTime.Now;
            while (!API.HasScaleformMovieLoaded(_sc.Handle) && DateTime.Now.Subtract(start).TotalMilliseconds < timeout) await BaseScript.Delay(0);
        }

        public void SetInstructionalButtons(List<InstructionalButton> buttons)
		{
            ControlButtons = buttons;
            _changed = true;
		}

        public void AddInstructionalButton(InstructionalButton button)
        {
            ControlButtons.Add(button);
            _changed = true;
        }

        public void RemoveInstructionalButton(InstructionalButton button)
        {
            ControlButtons.Remove(button);
            _changed = true;
        }

        public void RemoveInstructionalButton(int button)
        {
            ControlButtons.RemoveAt(button);
            _changed = true;
        }

        public async void AddSavingText(int value, string text, int time)
		{
            _isSaving = true;
            _changed = true;
            savingTimer = Game.GameTime;
            Screen.LoadingPrompt.Show(text, (LoadingSpinnerType)value);
            while(Game.GameTime - savingTimer < time) await BaseScript.Delay(100);
            Screen.LoadingPrompt.Hide();
            _isSaving = false;
        }

        public void UpdateButtons()
		{
            if (!_changed) return;
            _sc.CallFunction("CLEAR_ALL");
            _sc.CallFunction("TOGGLE_MOUSE_BUTTONS", _useMouseButtons);
            int count = 0;

            foreach (InstructionalButton button in ControlButtons)
            {
				if (button.IsUsingController)
				{
                    if (button.PadCheck == PadCheck.Keyboard) continue;
                    if(PopupWarningThread.Warning.IsShowing)
                        _sc.CallFunction("SET_DATA_SLOT", count, button.GetButtonId(), button.Text, 0, -1);
                    else
                        _sc.CallFunction("SET_DATA_SLOT", count, button.GetButtonId(), button.Text);
                }
                else
                {
                    if (button.PadCheck == PadCheck.Controller) continue;
                    if (_useMouseButtons)
                        _sc.CallFunction("SET_DATA_SLOT", count, button.GetButtonId(), button.Text, 1, (int)button.KeyboardButton);
                    else
                    {
                        if(PopupWarningThread.Warning.IsShowing)
                            _sc.CallFunction("SET_DATA_SLOT", count, button.GetButtonId(), button.Text, 0, -1);
                        else
                            _sc.CallFunction("SET_DATA_SLOT", count, button.GetButtonId(), button.Text);

                    }

                }
                count++;
            }
            _sc.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
            _changed = false;
        }

        public void Draw()
		{
            if (Main.ImpostazioniClient != null)
            {
                if (!Main.ImpostazioniClient.ModCinema)
                    _sc.Render2D();
                else
                    API.DrawScaleformMovie(_sc.Handle, 0.5f, 0.5f - Main.ImpostazioniClient.LetterBox / 1000, 1f, 1f, 255, 255, 255, 255, 0);
            }
            else
                _sc.Render2D();
        }

        public void HandleScaleform()
		{
            if (_sc == null || !_enabled) return;
            if ((ControlButtons == null || ControlButtons.Count == 0) && !_isSaving) return;
            if (API.IsUsingKeyboard(2))
            {
                if (!_isUsingKeyboard)
                {
                    _isUsingKeyboard = true;
                    _changed = true;
                }
            }
            else
            {
                if (_isUsingKeyboard)
                {
                    _isUsingKeyboard = false;
                    _changed = true;
                }
            }
            UpdateButtons();

            if (!PopupWarningThread.Warning.IsShowing) Draw(); 

            foreach (InstructionalButton button in ControlButtons)
			{
                if (Input.IsControlJustPressed(button.GamepadButton, button.PadCheck) || (button.ControllerButtons != null && button.ControllerButtons.Any(x=>Input.IsControlJustPressed(x, button.PadCheck))))
                    button.InvokeEvent(button.GamepadButton);
                else if (Input.IsControlJustPressed(button.KeyboardButton, button.PadCheck) || (button.KeyboardButtons != null && button.KeyboardButtons.Any(x => Input.IsControlJustPressed(x, button.PadCheck))))
                    button.InvokeEvent(button.KeyboardButton);
            }
            if (_useMouseButtons)
                Screen.Hud.ShowCursorThisFrame();
            Screen.Hud.HideComponentThisFrame(HudComponent.VehicleName);
            Screen.Hud.HideComponentThisFrame(HudComponent.AreaName);
            Screen.Hud.HideComponentThisFrame(HudComponent.StreetName);
        }
    }

    public class InstructionalButtonsHandler : BaseScript
	{
        public static InstructionalButtonsScaleform InstructionalButtons;
        public InstructionalButtonsHandler()
		{
            InstructionalButtons = new InstructionalButtonsScaleform();
            InstructionalButtons.Load();
            Tick += InstructionalButtons_Tick;
		}

        private async Task InstructionalButtons_Tick()
		{
            InstructionalButtons.HandleScaleform();
            await Task.FromResult(0);
        }
    }
}