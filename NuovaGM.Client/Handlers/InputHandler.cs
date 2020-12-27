using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Logger;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using static Logger.Log;

namespace TheLastPlanet.Client.Handlers
{
	public enum InputCallingModifier
	{
		OnFoot,
		InVehicle,
	}

	static class InputHandler
	{
		public static List<InputController> ListaInput = new List<InputController>();
		public static void Init()
		{
			Client.Instance.AddTick(InputHandling);
		}
		public static async Task InputHandling()
		{
			try
			{
				Ped p = Game.PlayerPed;
				foreach (var input in ListaInput)
				{
					if (input.Position != Vector3.Zero || input.Marker != null || input.InputMessage != null)
					{
						if (p.IsInRangeOf(input.Position, input.Radius.Max)) // big range personalizzato sennò default 50f
						{
							if(input.Marker != null)
								input.Marker.Draw();
							if (p.IsInRangeOf(input.Position, input.Radius.Min) && !HUD.MenuPool.IsAnyMenuOpen) // radius personalizzato semmò default 1.375f
							{
								HUD.ShowHelp(input.InputMessage);
								if (Game.IsControlJustPressed(0, input.Control))
								{
									if (Input.IsControlModifierPressed(input.Modifier))
									{
										switch (input.Check)
										{
											case PadCheck.Any:
												input.Action.DynamicInvoke(Game.PlayerPed);
												break;
											case PadCheck.Controller:
												if (Input.WasLastInputFromController())
													input.Action.DynamicInvoke(Game.PlayerPed);
												break;
											case PadCheck.Keyboard:
												if (!Input.WasLastInputFromController())
													input.Action.DynamicInvoke(Game.PlayerPed);
												break;
										}
									}
								}
							}
						}
					}
					else
					{
						if (Game.IsControlJustPressed(0, input.Control))
						{
							if (Input.IsControlModifierPressed(input.Modifier))
							{
								switch (input.Check)
								{
									case PadCheck.Any:
										input.Action.DynamicInvoke(Game.PlayerPed);
										break;
									case PadCheck.Controller:
										if (Input.WasLastInputFromController())
											input.Action.DynamicInvoke(Game.PlayerPed);
										break;
									case PadCheck.Keyboard:
										if (!Input.WasLastInputFromController())
											input.Action.DynamicInvoke(Game.PlayerPed);
										break;
								}
							}
						}
					}
				}
			}
			catch(Exception e)
			{
				Printa(LogType.Warning, e.ToString());
			}
		}
	}
}
