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
using TheLastPlanet.Shared;

namespace TheLastPlanet.Client.Handlers
{
	public enum InputCallingModifier
	{
		OnFoot,
		InVehicle
	}

	internal static class InputHandler
	{
		private static List<InputController> _listaInput = new();
		public static List<InputController> ToList => _listaInput.ToList();

		public static void AddInput(InputController input)
		{
			if (_listaInput.Contains(input)) return;
			_listaInput.Add(input);
		}

		public static void RemoveInput(InputController input)
		{
			if (!_listaInput.Contains(input)) return;
			_listaInput.Remove(input);
		}

		public static void AddInputList(List<InputController> inputs)
		{
			foreach (var input in inputs) 
			{ 
				if (_listaInput.Contains(input)) continue;
				_listaInput.Add(input);
			}
		}

		public static void RemoveInputList(List<InputController> inputs)
		{
			foreach (var input in inputs)
			{
				if (!_listaInput.Contains(input)) continue;
				_listaInput.Remove(input);
			}
		}


		public static void Clear()
		{
			_listaInput.Clear();
		}

		public static void Init()
		{
			Client.Instance.AddTick(InputHandling);
		}

		public static void Stop()
		{
			_listaInput.Clear();
			Client.Instance.RemoveTick(InputHandling);
		}

		public static async Task InputHandling()
		{
			try
			{
				await Cache.PlayerCache.Loaded();
				Ped p = Cache.PlayerCache.MyPlayer.Ped;

				foreach (InputController input in _listaInput)
				{
					if (input.Modalita != ModalitaServer.UNKNOWN && input.Modalita != Cache.PlayerCache.ModalitàAttuale) continue;
					if (input.Position != Position.Zero || input.Marker != null || input.InputMessage != null)
					{
						if (p.IsInRangeOf(input.Position.ToVector3, 100f)) // big range personalizzato sennò default 50f
						{
							if (input.Marker != null) input.Marker.Draw();

							if (input.Marker.IsInMarker && !HUD.MenuPool.IsAnyMenuOpen) // radius personalizzato sennò default 1.375f
							{
								if (!string.IsNullOrWhiteSpace(input.InputMessage)) HUD.ShowHelp(input.InputMessage);

								if (Input.IsControlJustPressed(input.Control, input.Check, input.Modifier))
								{
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
					}
					else
					{
						if (Input.IsControlJustPressed(input.Control, input.Check, input.Modifier) && !HUD.MenuPool.IsAnyMenuOpen)
						{
							switch (input.Check)
							{
								case PadCheck.Any:
									input.Action.DynamicInvoke(p, input.parameters);
									break;
								case PadCheck.Controller:
									if (Input.WasLastInputFromController())
										input.Action.DynamicInvoke(p, input.parameters);
									break;
								case PadCheck.Keyboard:
									if (!Input.WasLastInputFromController())
										input.Action.DynamicInvoke(p, input.parameters);
									break;
							}
						}
					}
					await Task.FromResult(0);
				}
			}
			catch (Exception e)
			{
				Client.Logger.Error(e.ToString());
			}
		}
	}
}