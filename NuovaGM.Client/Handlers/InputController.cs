using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Shared;

namespace TheLastPlanet.Client
{
	public class InputController
	{
		public Vector3 Position = Vector3.Zero;
		public float W = -1f;
		public Radius Radius;
		public string InputMessage = null;
		public Marker Marker = null;
		public Control Control;
		public PadCheck Check;
		public ControlModifier Modifier;
		public Delegate Action;
		public object[] parameters;
		public InputController(Control control, PadCheck check = PadCheck.Any, ControlModifier modifier = ControlModifier.None, Delegate action = null, params object[] args)
		{
			Control = control;
			Check = check;
			Modifier = modifier;
			Action = action;
			parameters = args;
		}
		public InputController(Control control, Vector3 position, Radius radius, string message, Marker marker, PadCheck check = PadCheck.Any, ControlModifier modifier = ControlModifier.None, Delegate action = null, params object[] args)
		{
			Control = control;
			Check = check;
			Modifier = modifier;
			Action = action;
			Position = position;
			Radius = radius;
			InputMessage = message;
			Marker = marker;
			parameters = args;
		}
		public InputController(Control control, Vector4 position, Radius radius, string message, Marker marker, PadCheck check = PadCheck.Any, ControlModifier modifier = ControlModifier.None, Delegate action = null, params object[] args)
		{
			Control = control;
			Check = check;
			Modifier = modifier;
			Action = action;
			Position = position.ToVector3();
			W = position.W;
			Radius = radius;
			InputMessage = message;
			Marker = marker;
			parameters = args;
		}
	}

	public class Radius
	{
		public float Min;
		public float Max;
		public Radius(float minimo, float markerDistance)
		{
			Min = minimo;
			Max = markerDistance;
		}
	}
}
