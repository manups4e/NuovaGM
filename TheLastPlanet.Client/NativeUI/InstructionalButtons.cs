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
        public bool IsUsingController => !API.IsInputDisabled(2);

        public UIMenuItem ItemBind { get; private set; }

        internal readonly Control _controllerButtonControl;
        internal readonly Control _keyboardButtonControl;
        internal readonly PadCheck padCheck;

        /// <summary>
        /// Add a dynamic button to the instructional buttons array.
        /// Changes whether the controller is being used and changes depending on keybinds.
        /// </summary>
        /// <param name="control">CitizenFX.Core.Control that gets converted into a button.</param>
        /// <param name="text">Help text that goes with the button.</param>
        public InstructionalButton(Control gamepadControl, Control keyboardControl, string text)
        {
            Text = text;
            _controllerButtonControl = gamepadControl;
            _keyboardButtonControl = keyboardControl;
            padCheck = PadCheck.Any;
        }

        public InstructionalButton(Control control, string text, PadCheck padcheck = PadCheck.Any)
        {
            if (padcheck == PadCheck.Controller)
                _controllerButtonControl = control;
            else if (padcheck == PadCheck.Keyboard)
                _keyboardButtonControl = control;
            else if (padcheck == PadCheck.Any)
            {
                _controllerButtonControl = control;
                _keyboardButtonControl = control;
            }
            Text = text;
            padCheck = padcheck;
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
            return IsUsingController ? API.GetControlInstructionalButton(2, (int)_controllerButtonControl, 0) : API.GetControlInstructionalButton(2, (int)_keyboardButtonControl, 0);
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
                    ControlButtons.Clear();
                    _sc.CallFunction("CLEAR_ALL");
                    _sc.CallFunction("CLEAR_RENDER");
                   _changed = true;
                }
                _enabled = value;
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

        public List<InstructionalButton> ControlButtons = new List<InstructionalButton>();

		public async void Load()
		{
            if (_sc != null) return;
            _sc = new Scaleform("INSTRUCTIONAL_BUTTONS");
            int timeout = 1000;
            DateTime start = DateTime.Now;
            while (!API.HasScaleformMovieLoaded(_sc.Handle) && DateTime.Now.Subtract(start).TotalMilliseconds < timeout) await BaseScript.Delay(0);
        }

        public void AddInstructionalButton(InstructionalButton button)
        {
            ControlButtons.Add(button);
        }

        public void RemoveInstructionalButton(InstructionalButton button)
        {
            ControlButtons.Remove(button);
        }
        public void RemoveInstructionalButton(int button)
        {
            ControlButtons.RemoveAt(button);
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
            _sc.CallFunction("TOGGLE_MOUSE_BUTTONS", _useMouseButtons);
            int count = 0;

            foreach (InstructionalButton button in ControlButtons)
            {
				if (button.IsUsingController)
				{
                    if(PopupWarningThread.Warning.IsShowing)
                        _sc.CallFunction("SET_DATA_SLOT", count, button.GetButtonId(), button.Text, 0, -1);
                    else
                        _sc.CallFunction("SET_DATA_SLOT", count, button.GetButtonId(), button.Text);
                }
                else
                {
                    if (_useMouseButtons)
                        _sc.CallFunction("SET_DATA_SLOT", count, button.GetButtonId(), button.Text, 1, (int)button._keyboardButtonControl);
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
            if (_sc == null) return;
            if (ControlButtons.Count == 0 && !_isSaving) return;
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
                if (Input.IsControlJustPressed(button._controllerButtonControl, button.padCheck))
                    button.InvokeEvent(button._controllerButtonControl);
                else if (Input.IsControlJustPressed(button._keyboardButtonControl, button.padCheck))
                    button.InvokeEvent(button._keyboardButtonControl);
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