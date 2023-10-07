using System;

namespace TheLastPlanet.Client
{
    public class InputController
    {
        public Position Position = Position.Zero;
        public string InputMessage = null;
        public MarkerEx Marker = null;
        public Control Control;
        public PadCheck Check;
        public ControlModifier Modifier;
        public ServerMode Modalita;
        public Delegate Action;
        public object[] parameters;

        public InputController(Control control, ServerMode modalita, PadCheck check = PadCheck.Any, ControlModifier modifier = ControlModifier.None, Delegate action = null, params object[] args)
        {
            Control = control;
            Check = check;
            Modifier = modifier;
            Action = action;
            parameters = args;
            Modalita = modalita;
        }

        public InputController(Control control, Position position, string message, MarkerEx marker, ServerMode modalita, PadCheck check = PadCheck.Any, ControlModifier modifier = ControlModifier.None, Delegate action = null, params object[] args)
        {
            Control = control;
            Check = check;
            Modifier = modifier;
            Action = action;
            Position = position;
            InputMessage = message;
            Marker = marker;
            parameters = args;
            Modalita = modalita;
        }
    }
}