using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Newtonsoft.Json;
using TheLastPlanet.Client.Core.PlayerChar;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.Core.Utility.HUD
{
	public class NuiManager
	{
		private bool _hasFocus;
		public bool IsNuiFocusOn => _hasFocus;
		public Point NuiCursorPosition
		{
			get
			{
				int x = 0, y = 0;
				GetNuiCursorPosition(ref x, ref y);
				return new Point(x, y);
			} 
		}

		public void SetFocus(bool hasFocus, bool showCursor = true)
		{
			SetNuiFocus(hasFocus, showCursor);
			_hasFocus = hasFocus;
		}

		public void SetFocusKeepInput(bool keepInput)
		{
			SetNuiFocusKeepInput(keepInput);
			if (!_hasFocus) _hasFocus = true;
		}

		public void Emit(object data)
		{
			SendNuiMessage(JsonConvert.SerializeObject(data));
		}

		public void Emit(string data)
		{
			SendNuiMessage(data);
		}

		public void RegisterCallback(string @event, Action action)
		{
			RegisterNuiCallbackType(@event);

			Client.Instance.AddEventHandler($"__cfx_nui:{@event}", new Action<dynamic, CallbackDelegate>((data, callback) =>
			{
				action();
				callback("ok");
			}));
		}

		public void RegisterCallback<T>(string @event, Action<T> action)
		{
			RegisterNuiCallbackType(@event);

			Client.Instance.AddEventHandler($"__cfx_nui:{@event}", new Action<dynamic, CallbackDelegate>((data, callback) =>
			{
				var typedData = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(data));
				action(typedData);
				callback("ok");
			}));
		}

		public void RegisterCallback<TReturn>(string @event, Func<TReturn> action)
		{
			RegisterNuiCallbackType(@event);

			Client.Instance.AddEventHandler($"__cfx_nui:{@event}", new Action<dynamic, CallbackDelegate>((data, callback) =>
			{
				var result = action();
				callback(JsonConvert.SerializeObject(result));
			}));
		}

		public void RegisterCallback<T, TReturn>(string @event, Func<T, TReturn> action)
		{
			RegisterNuiCallbackType(@event);

			Client.Instance.AddEventHandler($"__cfx_nui:{@event}", new Action<dynamic, CallbackDelegate>((data, callback) =>
			{
				var typedData = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(data));
				var result = action(typedData);
				callback(JsonConvert.SerializeObject(result));
			}));
		}
	}
}
