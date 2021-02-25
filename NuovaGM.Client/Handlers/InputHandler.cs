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
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Client.Core;

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
				foreach(var input in ListaInput)
				{
					if (input.Position != Vector3.Zero || input.Marker != null || input.InputMessage != null)
					{
						if (Cache.PlayerPed.IsInRangeOf(input.Position, input.Radius.MarkerDistance)) // big range personalizzato sennò default 50f
						{
							if (input.Marker != null)
								input.Marker.Draw();
							if (Cache.PlayerPed.IsInRangeOf(input.Position, input.Radius.MinInputDistance) && !HUD.MenuPool.IsAnyMenuOpen) // radius personalizzato sennò default 1.375f
							{
								if (!string.IsNullOrWhiteSpace(input.InputMessage))
									HUD.ShowHelp(input.InputMessage);
								if (Input.IsControlJustPressed(input.Control))
								{
									if (Input.IsControlModifierPressed(input.Modifier))
									{
										switch (input.Check)
										{
											case PadCheck.Any:
												input.Action.DynamicInvoke(Cache.PlayerPed, input.parameters);
												break;
											case PadCheck.Controller:
												if (Input.WasLastInputFromController())
													input.Action.DynamicInvoke(Cache.PlayerPed, input.parameters);
												break;
											case PadCheck.Keyboard:
												if (!Input.WasLastInputFromController())
													input.Action.DynamicInvoke(Cache.PlayerPed, input.parameters);
												break;
										}
									}
								}
							}
						}
					}
					else
					{
						if (Input.IsControlJustPressed(input.Control) && !HUD.MenuPool.IsAnyMenuOpen)
						{
							if (Input.IsControlModifierPressed(input.Modifier))
							{
								switch (input.Check)
								{
									case PadCheck.Any:
										input.Action.DynamicInvoke(Cache.PlayerPed, input.parameters);
										break;
									case PadCheck.Controller:
										if (Input.WasLastInputFromController())
											input.Action.DynamicInvoke(Cache.PlayerPed, input.parameters);
										break;
									case PadCheck.Keyboard:
										if (!Input.WasLastInputFromController())
											input.Action.DynamicInvoke(Cache.PlayerPed, input.parameters);
										break;
								}
							}
						}
					}
				};
				await Task.FromResult(0);
			}
			catch(Exception e)
			{
				Printa(LogType.Warning, e.ToString());
			}
		}
	}
}
