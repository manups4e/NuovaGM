using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Shared;
using TheLastPlanet.Client.MenuNativo;
using TheLastPlanet.Client.Core;
using TheLastPlanet.Client.Core.PlayerChar;

namespace TheLastPlanet.Client.Interactions
{
	internal static class LootingDead
	{
		private static int _checkTimer;

		public static void Init()
		{
			// Loot da tutti o solo chi mi ha ucciso?
		}

		public static async Task Looting()
		{
			// alleggerire carico peso
			//Ped playerPed = Cache.PlayerPed;
			Tuple<Player, float> closest = new Tuple<Player, float>(new Player(0), -1);

			if (Game.GameTime - _checkTimer > 1000)
			{
				closest = Funzioni.GetClosestPlayer();
				_checkTimer = Game.GameTime;
			}

			if (!Cache.Char.StatiPlayer.InServizio)
				if (closest.Item2 > -1 && closest.Item2 < 3f)
					if (closest.Item1.GetPlayerData().StatiPlayer.Svenuto || closest.Item1.GetPlayerData().StatiPlayer.FinDiVita)
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per lootare");
						if (Input.IsControlJustPressed(Control.Context)) LootMenu(closest.Item1);
					}

			await Task.FromResult(0);
		}

		private static async void LootMenu(Player target)
		{
			Ped playerPed = Cache.PlayerPed;
			PlayerChar targetData = target.GetPlayerData();
			UIMenu loot = new UIMenu(targetData.FullName, "Looting Menu");
			HUD.MenuPool.Add(loot);
			UIMenu soldi = loot.AddSubMenu("Portafoglio");
			UIMenu Inventario = loot.AddSubMenu("Inventario");
			UIMenu Armi = loot.AddSubMenu("Armi");
			HUD.MenuPool.OnMenuStateChanged += (_old, _new, _state) =>
			{
				switch (_state)
				{
					case MenuState.Opened:
						playerPed.Task.PlayAnimation("amb@medic@standing@kneel@base", "base");
						playerPed.Task.PlayAnimation("anim@gangops@facility@servers@bodysearch@", "player_search", 8f, -1, AnimationFlags.Loop);

						break;
					case MenuState.Closed:
						if (IsEntityPlayingAnim(playerPed.Handle, "anim@gangops@facility@servers@bodysearch@", "player_search", 3)) StopAnimTask(playerPed.Handle, "anim@gangops@facility@servers@bodysearch@", "player_search", 1f);

						break;
					case MenuState.ChangeForward:
						if (_new == soldi)
						{
							if (targetData.Money > 0)
							{
								UIMenuItem cash = new UIMenuItem("Soldi");
								cash.SetRightLabel($"${targetData.Money}");
								_new.AddItem(cash);
								cash.Activated += async (a, b) =>
								{
									string val = await HUD.GetUserInput("Inserisci quantità", "", 10);

									if (val.All(x => char.IsDigit(x)))
									{
										int qt = Convert.ToInt32(val);

										if (qt <= targetData.Money)
										{
											if (qt >= 1)
											{
												BaseScript.TriggerServerEvent("lprp:removemoneytochar", target.ServerId, 0, qt);
												BaseScript.TriggerServerEvent("lprp:givemoneytochar", PlayerId(), 0, qt);
											}
											else
											{
												HUD.ShowNotification("Non puoi inserire 0 o meno!");

												return;
											}
										}
										else
										{
											HUD.ShowNotification("Non puoi superare la cifra della vittima!");

											return;
										}
									}
									else
									{
										HUD.ShowNotification("Devi inserire solo cifre!");

										return;
									}
								};
							}

							if (targetData.DirtyMoney > 0)
							{
								UIMenuItem dirty = new UIMenuItem("Soldi Sporchi");
								dirty.SetRightLabel($"${targetData.DirtyMoney}");
								_new.AddItem(dirty);
								dirty.Activated += async (a, b) =>
								{
									string val = await HUD.GetUserInput("Inserisci quantità", "", 10);

									if (val.All(x => char.IsDigit(x)))
									{
										int qt = Convert.ToInt32(val);

										if (qt <= targetData.Money)
										{
											if (qt >= 1)
											{
												BaseScript.TriggerServerEvent("lprp:removedirtytochar", target.ServerId, 0, qt);
												BaseScript.TriggerServerEvent("lprp:givedirtytochar", PlayerId(), 0, qt);
											}
											else
											{
												HUD.ShowNotification("Non puoi inserire 0 o meno!");

												return;
											}
										}
										else
										{
											HUD.ShowNotification("Non puoi superare la cifra della vittima!");

											return;
										}
									}
									else
									{
										HUD.ShowNotification("Devi inserire solo cifre!");

										return;
									}
								};
							}
						}
						else if (_new == Inventario)
						{
						}
						else if (_new == Armi)
						{
						}

						break;
				}
			};
		}
	}
}