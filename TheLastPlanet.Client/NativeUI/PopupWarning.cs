using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Logger;
using TheLastPlanet.Client.Core.PlayerChar;
using TheLastPlanet.Client.SessionCache;
using TheLastPlanet.Shared;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.NativeUI
{

	public enum WarningPopupType
	{
		Classico,
		Serio
	}
	public delegate void PopupWarningEvent(InstructionalButton button);

	public class PopupWarning
	{
		private Scaleform _warning = new("POPUP_WARNING");
		private bool _disableControls;
		private List<InstructionalButton> _buttonList;
		private bool _callInstr;
		public bool IsShowing
		{
			get => _warning != null;
		}

		public event PopupWarningEvent OnButtonPressed;

		public async Task Load()
		{
			if (_warning != null) return;
			_warning = new Scaleform("POPUP_WARNING");
			int timeout = 1000;
			DateTime start = DateTime.Now;
			while (!_warning.IsLoaded && DateTime.Now.Subtract(start).TotalMilliseconds < timeout) await BaseScript.Delay(0);
		}

		public void Dispose()
		{
			_disableControls = false;
			_warning.CallFunction("HIDE_POPUP_WARNING", 1000);
			_warning.Dispose();
			_warning = null;
			_callInstr = false;
			Cache.MyPlayer.Player.CanControlCharacter = true;
		}

		public async void ShowWarning(string title, string subtitle, string prompt = "", WarningPopupType type = WarningPopupType.Classico, int timer = 1000)
		{
			Cache.MyPlayer.Player.CanControlCharacter = false;
			await Load();
			_warning.CallFunction("SHOW_POPUP_WARNING", timer, title, subtitle, prompt, true, (int)type, $"The Last Planet V. {Assembly.GetExecutingAssembly().GetName().Version}");
		}

		public void UpdateWarning(string title, string subtitle, string prompt = "", WarningPopupType type = WarningPopupType.Classico, int timer = 1000)
		{
			_warning.CallFunction("SHOW_POPUP_WARNING", timer, title, subtitle, prompt, true, (int)type, $"The Last Planet V. {Assembly.GetExecutingAssembly().GetName().Version}");
		}

		public async void ShowWarningWithButtons(string title, string subtitle, string prompt, List<InstructionalButton> buttons, WarningPopupType type = WarningPopupType.Classico)
		{
			Cache.MyPlayer.Player.CanControlCharacter = false;
			await Load();
			_disableControls = true;
			_buttonList = buttons;
			if (buttons == null || buttons.Count == 0)
			{
				Cache.MyPlayer.Player.CanControlCharacter = true;
				return;
			}
			InstructionalButtonsHandler.InstructionalButtons.ControlButtons = _buttonList;
			InstructionalButtonsHandler.InstructionalButtons.UseMouseButtons = true;
			_warning.CallFunction("SHOW_POPUP_WARNING", 1000, title, subtitle, prompt, true, (int)type, $"The Last Planet - Versione: {Assembly.GetExecutingAssembly().GetName().Version}");
			InstructionalButtonsHandler.InstructionalButtons.Enabled = true;
			_callInstr = true;
		}

		internal async Task Update()
		{
			if (_warning == null) return;
			_warning.Render2D();
			if (_callInstr)
				InstructionalButtonsHandler.InstructionalButtons.Draw();
			if (_disableControls)
			{
				foreach(var b in _buttonList)
				{
					if (Input.IsControlJustPressed(b._controllerButtonControl) || Input.IsControlJustPressed(b._keyboardButtonControl)) 
					{
						OnButtonPressed?.Invoke(b);
						Dispose();
						InstructionalButtonsHandler.InstructionalButtons.Enabled = false;
						InstructionalButtonsHandler.InstructionalButtons.UseMouseButtons = false;
						return;
					}
				}
			}
		}
	}

	public class PopupWarningThread : BaseScript
	{
		public static PopupWarning Warning { get; set; }

		public PopupWarningThread()
		{
			Warning = new PopupWarning();
			Tick += PopupWarningThread_Tick;
		}

		private async Task PopupWarningThread_Tick()
		{
			await Warning.Update();
			await Task.FromResult(0);
		}
	}

}
