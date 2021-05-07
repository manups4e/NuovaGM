﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Logger;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Client.SessionCache;

namespace TheLastPlanet.Client.Handlers
{
	public enum InputCallingModifier
	{
		OnFoot,
		InVehicle
	}

	internal static class InputHandler
	{
		public static List<InputController> ListaInput = new();

		public static void Init()
		{
			Client.Instance.AddTick(InputHandling);
		}

		public static void Stop()
		{
			ListaInput.Clear();
			Client.Instance.RemoveTick(InputHandling);
		}

		public static async Task InputHandling()
		{
			try
			{
				Ped p = Cache.MyPlayer.Ped;

				foreach (InputController input in ListaInput)
					if (input.Position != Vector3.Zero || input.Marker != null || input.InputMessage != null)
					{
						if (p.IsInRangeOf(input.Position, input.Radius.MarkerDistance)) // big range personalizzato sennò default 50f
						{
							if (input.Marker != null) input.Marker.Draw();

							if ((p.IsInRangeOf(input.Position, input.Radius.MinInputDistance) || input.Marker.IsInMarker) && !HUD.MenuPool.IsAnyMenuOpen) // radius personalizzato sennò default 1.375f
							{
								if (!string.IsNullOrWhiteSpace(input.InputMessage)) HUD.ShowHelp(input.InputMessage);

								if (Game.IsControlJustPressed(0, input.Control))
									if (Input.IsControlModifierPressed(input.Modifier))
										switch (input.Check)
										{
											case PadCheck.Any:
												input.Action.DynamicInvoke(p, input.parameters);

												break;
											case PadCheck.Controller:
												if (Input.WasLastInputFromController()) input.Action.DynamicInvoke(p, input.parameters);

												break;
											case PadCheck.Keyboard:
												if (!Input.WasLastInputFromController()) input.Action.DynamicInvoke(p, input.parameters);

												break;
										}
							}
						}
					}
					else
					{
						if (Game.IsControlJustPressed(0, input.Control) && !HUD.MenuPool.IsAnyMenuOpen)
							if (Input.IsControlModifierPressed(input.Modifier))
								switch (input.Check)
								{
									case PadCheck.Any:
										input.Action.DynamicInvoke(p, input.parameters);

										break;
									case PadCheck.Controller:
										if (Input.WasLastInputFromController()) input.Action.DynamicInvoke(p, input.parameters);

										break;
									case PadCheck.Keyboard:
										if (!Input.WasLastInputFromController()) input.Action.DynamicInvoke(p, input.parameters);

										break;
								}
					}

				;
				await Task.FromResult(0);
			}
			catch (Exception e)
			{
				Client.Logger.Error(e.ToString());
			}
		}
	}
}