using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using TheLastPlanet.Client.Core.Utility.HUD;

namespace TheLastPlanet.Client
{
	public class InputController
	{
		public Vector3 Position = Vector3.Zero;
		public Radius Radius;
		public string InputMessage = null;
		public Marker Marker = null;
		public Control Control;
		public PadCheck Check;
		public ControlModifier Modifier;
		public Delegate Action;
		public InputController(Control control, PadCheck check = PadCheck.Any, ControlModifier modifier = ControlModifier.None, Delegate action = null)
		{
			Control = control;
			Check = check;
			Modifier = modifier;
			Action = action;
		}
		public InputController(Control control, Vector3 position, Radius radius, string message, Marker marker, PadCheck check = PadCheck.Any, ControlModifier modifier = ControlModifier.None, Delegate action = null)
		{
			Control = control;
			Check = check;
			Modifier = modifier;
			Action = action;
			Position = position;
			Radius = radius;
			InputMessage = message;
			Marker = marker;
		}
	}

	public class Radius
	{
		public float Min;
		public float Max;
		public Radius(float min, float max)
		{
			Min = min;
			Max = max;
		}
	}
}
