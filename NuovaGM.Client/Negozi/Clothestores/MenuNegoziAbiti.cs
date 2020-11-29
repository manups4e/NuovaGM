using CitizenFX.Core;
using Newtonsoft.Json;
using TheLastPlanet.Client.Core;
using TheLastPlanet.Client.Core.Personaggio;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.MenuNativo;
using TheLastPlanet.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.Negozi
{
	static class MenuNegoziAbiti
	{
		static private MenuPool pool = HUD.MenuPool;
		static private bool AccessoriAttivo = false;
		static List<UIMenu> _menuVestiti = new List<UIMenu>();

		#region Utili
		static private async void StartAnim(string lib, string anim)
		{
			Game.PlayerPed.BlockPermanentEvents = true;
			await Game.PlayerPed.Task.PlayAnimation(lib, anim, 8f, -8f, -1, AnimationFlags.Loop, 0);
		}

		static private async void StartAnimN(string lib, string anim)
		{
			Game.PlayerPed.BlockPermanentEvents = true;
			await Game.PlayerPed.Task.PlayAnimation(lib, anim, 4.0f, -2.0f, -1, AnimationFlags.None, 0);
		}

		static private void StartScenario(string anim)
		{
			Game.PlayerPed.Task.StartScenario(anim, Game.Player.GetPlayerData().posizione.ToVector3());
		}

		public static async Task UpdateDress(dynamic dress)
		{
			int id = PlayerPedId();
			for (int i = 0; i < 9; i++)
			{
				ClearPedProp(id, i);
			}

			SetPedComponentVariation(id, (int)DrawableIndexes.Faccia, dress.ComponentDrawables.Faccia, dress.ComponentTextures.Faccia, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Maschera, dress.ComponentDrawables.Maschera, dress.ComponentTextures.Maschera, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Torso, dress.ComponentDrawables.Torso, dress.ComponentTextures.Torso, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Pantaloni, dress.ComponentDrawables.Pantaloni, dress.ComponentTextures.Pantaloni, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Borsa_Paracadute, dress.ComponentDrawables.Borsa_Paracadute, dress.ComponentTextures.Borsa_Paracadute, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Scarpe, dress.ComponentDrawables.Scarpe, dress.ComponentTextures.Scarpe, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Accessori, dress.ComponentDrawables.Accessori, dress.ComponentTextures.Accessori, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Sottomaglia, dress.ComponentDrawables.Sottomaglia, dress.ComponentTextures.Sottomaglia, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Kevlar, dress.ComponentDrawables.Kevlar, dress.ComponentTextures.Kevlar, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Badge, dress.ComponentDrawables.Badge, dress.ComponentTextures.Badge, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Torso_2, dress.ComponentDrawables.Torso_2, dress.ComponentTextures.Torso_2, 2);
			SetPedPropIndex(id, (int)PropIndexes.Cappelli_Maschere, dress.PropIndices.Cappelli_Maschere, dress.PropTextures.Cappelli_Maschere, true);
			SetPedPropIndex(id, (int)PropIndexes.Orecchie, dress.PropIndices.Orecchie, dress.PropTextures.Orecchie, true);
			SetPedPropIndex(id, (int)PropIndexes.Occhiali_Occhi, dress.PropIndices.Occhiali_Occhi, dress.PropTextures.Occhiali_Occhi, true);
			SetPedPropIndex(id, (int)PropIndexes.Unk_3, dress.PropIndices.Unk_3, dress.PropTextures.Unk_3, true);
			SetPedPropIndex(id, (int)PropIndexes.Unk_4, dress.PropIndices.Unk_4, dress.PropTextures.Unk_4, true);
			SetPedPropIndex(id, (int)PropIndexes.Unk_5, dress.PropIndices.Unk_5, dress.PropTextures.Unk_5, true);
			SetPedPropIndex(id, (int)PropIndexes.Orologi, dress.PropIndices.Orologi, dress.PropTextures.Orologi, true);
			SetPedPropIndex(id, (int)PropIndexes.Bracciali, dress.PropIndices.Bracciali, dress.PropTextures.Bracciali, true);
			SetPedPropIndex(id, (int)PropIndexes.Unk_8, dress.PropIndices.Unk_8, dress.PropTextures.Unk_8, true);
			await Task.FromResult(0);
		}

		public static string GetRandomAnim(string lib, bool toggle)
		{
			int a;
			int b;
			a = GetRandomIntInRange(0, 3);
			if (lib == "clothingshirt")
			{
				if (a == 0)
				{
					b = GetRandomIntInRange(0, 4);
					if (b == 0)
					{
						return "try_shirt_negative_a";
					}
					else if (b == 1)
					{
						return "try_shirt_negative_b";
					}
					else if (b == 2)
					{
						return "try_shirt_negative_c";
					}
					else if (b == 3)
					{
						return "try_shirt_negative_d";
					}
				}
				else if (a == 1)
				{
					b = GetRandomIntInRange(0, 3);
					if (b == 0)
					{
						return "try_shirt_neutral_a";
					}
					else if (b == 1)
					{
						return "try_shirt_neutral_b";
					}
					else if (b == 2)
					{
						return "try_shirt_neutral_c";
					}
				}
				else if (a == 2)
				{
					b = GetRandomIntInRange(0, 3);
					if (b == 0)
					{
						return "try_shirt_positive_a";
					}
					else if (b == 1)
					{
						return "try_shirt_positive_b";
					}
					else if (b == 2)
					{
						return "try_shirt_positive_c";
					}
					else if (b == 3)
					{
						return "try_shirt_positive_d";
					}
				}
			}
			else if (lib == "clothingshoes")
			{
				if (a == 0)
				{
					b = GetRandomIntInRange(0, 4);
					if (b == 0)
					{
						return "try_shoes_negative_a";
					}
					else if (b == 1)
					{
						return "try_shoes_negative_b";
					}
					else if (b == 2)
					{
						return "try_shoes_negative_c";
					}
					else if (b == 3)
					{
						return "try_shoes_negative_d";
					}
				}
				else if (a == 1)
				{
					b = GetRandomIntInRange(0, 4);
					if (b == 0)
					{
						return "try_shoes_neutral_a";
					}
					else if (b == 1)
					{
						return "try_shoes_neutral_b";
					}
					else if (b == 2)
					{
						return "try_shoes_neutral_c";
					}
					else if (b == 3)
					{
						return "try_shoes_neutral_d";
					}
				}
				else if (a == 2)
				{
					b = GetRandomIntInRange(0, 4);
					if (b == 0)
					{
						return "try_shoes_positive_a";
					}
					else if (b == 1)
					{
						return "try_shoes_positive_b";
					}
					else if (b == 2)
					{
						return "try_shoes_positive_c";
					}
					else if (b == 3)
					{
						return "try_shoes_positive_d";
					}
				}
			}
			else if (lib == "clothingspecs")
			{
				if (toggle)
				{
					if (a == 0)
					{
						b = GetRandomIntInRange(0, 1);
						if (b == 0)
						{
							return "try_glasses_negative_a";
						}
						else if (b == 1)
						{
							return "try_glasses_negative_c";
						}
						else if (a == 1)
						{
							return "try_glasses_neutral_a";
						}
						else if (a == 2)
						{
							return "try_glasses_positive_c";
						}
					}
				}
				else
				{
					b = GetRandomIntInRange(0, 3);
					if (a == 0)
					{
						if (b == 0)
						{
							return "try_glasses_negative_a";
						}
						else if (b == 1)
						{
							return "try_glasses_negative_b";
						}
						else if (b == 2)
						{
							return "try_glasses_negative_c";
						}
					}
					else if (a == 1)
					{
						if (b == 0)
						{
							return "try_glasses_neutral_a";
						}
						else if (b == 1)
						{
							return "try_glasses_neutral_b";
						}
						else if (b == 2)
						{
							return "try_glasses_neutral_c";
						}
					}
					else if (a == 2)
					{
						if (b == 0)
						{
							return "try_glasses_positive_a";
						}
						else if (b == 1)
						{
							return "try_glasses_positive_b";
						}
						else if (b == 2)
						{
							return "try_glasses_positive_c";
						}
					}
				}
			}
			else if (lib == "clothingtie")
			{
				b = GetRandomIntInRange(0, 4);
				if (a == 0)
				{
					b = GetRandomIntInRange(0, 4);
					if (b == 0)
					{
						return "try_tie_negative_a";
					}
					else if (b == 1)
					{
						return "try_tie_negative_b";
					}
					else if (b == 2)
					{
						return "try_tie_negative_c";
					}
					else if (b == 3)
					{
						return "try_tie_negative_d";
					}
				}
				else if (a == 1)
				{
					b = GetRandomIntInRange(0, 4);
					if (b == 0)
					{
						return "try_tie_neutral_a";
					}
					else if (b == 1)
					{
						return "try_tie_neutral_b";
					}
					else if (b == 2)
					{
						return "try_tie_neutral_c";
					}
					else if (b == 3)
					{
						return "try_tie_neutral_d";
					}
				}
				else if (a == 2)
				{
					b = GetRandomIntInRange(0, 4);
					if (b == 0)
					{
						return "try_tie_positive_a";
					}
					else if (b == 1)
					{
						return "try_tie_positive_b";
					}
					else if (b == 2)
					{
						return "try_tie_positive_c";
					}
					else if (b == 3)
					{
						return "try_tie_positive_d";
					}
				}
			}
			else if (lib == "clothingtrousers")
			{
				if (a == 0)
				{
					b = GetRandomIntInRange(0, 4);
					if (b == 0)
					{
						return "try_trousers_negative_a";
					}
					else if (b == 1)
					{
						return "try_trousers_negative_b";
					}
					else if (b == 2)
					{
						return "try_trousers_negative_c";
					}
					else if (b == 3)
					{
						return "try_trousers_negative_d";
					}
				}
				else if (a == 1)
				{
					b = GetRandomIntInRange(0, 3);
					if (b == 0)
					{
						return "try_trousers_neutral_a";
					}
					else if (b == 1)
					{
						return "try_trousers_neutral_b";
					}
					else if (b == 2)
					{
						return "try_trousers_neutral_c";
					}
					else if (b == 3)
					{
						return "try_trousers_neutral_d";
					}
				}
				else if (a == 2)
				{
					b = GetRandomIntInRange(0, 4);
					if (b == 0)
					{
						return "try_trousers_positive_a";
					}
					else if (b == 1)
					{
						return "try_trousers_positive_b";
					}
					else if (b == 2)
					{
						return "try_trousers_positive_c";
					}
					else if (b == 3)
					{
						return "try_trousers_positive_d";
					}
				}
			}
			else if (lib == "mp_clothing@female@shirt")
			{
				b = GetRandomIntInRange(0, 2);
				if (b == 0)
				{
					return "try_shirt_negative_a";
				}
				else if (b == 1)
				{
					return "try_shirt_neutral_a";
				}
				else if (b == 2)
				{
					return "try_shirt_positive_a";
				}
			}
			else if (lib == "mp_clothing@female@trousers")
			{
				b = GetRandomIntInRange(0, 2);
				if (b == 0)
				{
					return "try_trousers_negative_a";
				}
				else if (b == 1)
				{
					return "try_trousers_neutral_a";
				}
				else if (b == 2)
				{
					return "try_trousers_positive_a";
				}
			}
			else if (lib == "mp_clothing@female@shoes")
			{
				b = GetRandomIntInRange(0, 2);
				if (b == 0)
				{
					return "try_shoes_negative_a";
				}
				else if (b == 1)
				{
					return "try_shoes_neutral_a";
				}
				else if (b == 2)
				{
					return "try_shoes_positive_a";
				}
			}
			else if (lib == "mp_clothing@female@glasses")
			{
				b = GetRandomIntInRange(0, 2);
				if (b == 0)
				{
					return "try_glasses_negative_a";
				}
				else if (b == 1)
				{
					return "try_glasses_neutral_a";
				}
				else if (b == 2)
				{
					return "try_glasses_positive_a";
				}
			}
			//  else if (lib == "anim@random@shop_clothes@watches" )
			//      return
			return "";
		}

		async static void TaskLookLeft(Ped p, string anim)
		{
			int sequence = 0;
			OpenSequenceTask(ref sequence);
			TaskPlayAnim(p.Handle, anim, "Profile_L_Intro", 4.0f, -4.0f, -1, 512, 0, false, false, false);
			TaskPlayAnim(p.Handle, anim, "Profile_L_Loop", 4.0f, -4.0f, -1, 513, 0, false, false, false);
			CloseSequenceTask(sequence);
			TaskPerformSequence(p.Handle, sequence);
			ClearSequenceTask(ref sequence);
			await Task.FromResult(0);
		}

		async static void TaskStopLookLeft(Ped p, string anim)
		{
			int sequence = 0;
			OpenSequenceTask(ref sequence);
			TaskPlayAnim(p.Handle, anim, "Profile_L_Outro", 4.0f, -4.0f, -1, 512, 0, false, false, false);
			TaskPlayAnim(p.Handle, anim, "Loop", 4.0f, -4.0f, -1, 513, 0, false, false, false);
			CloseSequenceTask(sequence);
			TaskPerformSequence(p.Handle, sequence);
			ClearSequenceTask(ref sequence);
			await Task.FromResult(0);
		}
		#endregion

		#region MenuVest
		public static async void MenuVest(List<Completo> Completi, string anim, string nome)
		{
			_menuVestiti.Clear();
			UIMenuItem ciao = new UIMenuItem("");
			UIMenu MenuVest = new UIMenu(" ", "~y~Benvenuti da " + nome + "!", new System.Drawing.Point(0, 0), Main.Textures[nome].Key, Main.Textures[nome].Value);
			pool.Add(MenuVest);
			MenuVest.AddInstructionalButton(new InstructionalButton(Control.FrontendLt, "Zoom"));
			Game.PlayerPed.BlockPermanentEvents = true;
			SetPedAlternateMovementAnim(Game.PlayerPed.Handle, 0, anim, "try_shirt_base", 4.0f, true);
			await Game.PlayerPed.Task.PlayAnimation(anim, "try_shirt_base", 8f, -8f, -1, AnimationFlags.Loop, 0);
			var completi = Completi.OrderBy(x => x.Price).ToList();
			_menuVestiti.Add(MenuVest);
			for (int i = 0; i < completi.Count; i++)
			{
				UIMenuItem vest = new UIMenuItem(completi[i].Name, completi[i].Description);
				MenuVest.AddItem(vest);
				if (Game.Player.GetPlayerData().Money >= completi[i].Price)
				{
					vest.SetRightLabel("~g~$" + completi[i].Price);
				}
				else
				{
					if (Game.Player.GetPlayerData().Bank >= completi[i].Price)
					{
						vest.SetRightLabel("~g~$" + completi[i].Price);
					}
					else
					{
						vest.SetRightLabel("~r~$" + completi[i].Price);
					}
				}
				if (completi[i].Name == Game.Player.GetPlayerData().CurrentChar.dressing.Name)
				{
					vest.SetRightBadge(UIMenuItem.BadgeStyle.Clothes); // cambiare con la collezione di abiti
					ciao = vest;
				}
			}
			MenuVest.OnIndexChange += async (sender, index) =>
			{
				string random = GetRandomAnim(anim, false);
				await UpdateDress(completi[index]);
				await Game.PlayerPed.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
			};
			MenuVest.OnItemSelect += async (_menu, _item, _index) =>
			{
				if (Game.Player.GetPlayerData().CurrentChar.dressing.Name == completi[_index].Name && Game.Player.GetPlayerData().CurrentChar.dressing.Description == completi[_index].Description)
				{
					HUD.ShowNotification("Possiedi già quest'abito!", true);
				}
				else
				{
					if (Game.Player.GetPlayerData().Money >= completi[_index].Price)
					{
						BaseScript.TriggerServerEvent("lprp:abiti:compra", completi[_index].Price, 1);
						BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", completi[_index].Serialize());
						Game.Player.GetPlayerData().CurrentChar.dressing = new Shared.Dressing(completi[_index].Name, completi[_index].Description, completi[_index].ComponentDrawables, completi[_index].ComponentTextures, completi[_index].PropIndices, completi[_index].PropTextures);
						ciao.SetRightBadge(UIMenuItem.BadgeStyle.None);
						ciao = _item;
						ciao.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
						HUD.ShowNotification("Hai speso ~g~" + completi[_index].Price + "$~w~, in contanti");
					}
					else
					{
						if (Game.Player.GetPlayerData().Bank >= completi[_index].Price)
						{
							BaseScript.TriggerServerEvent("lprp:abiti:compra", completi[_index].Price, 2);
							BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", completi[_index].Serialize());
							Game.Player.GetPlayerData().CurrentChar.dressing = new Shared.Dressing(completi[_index].Name, completi[_index].Description, completi[_index].ComponentDrawables, completi[_index].ComponentTextures, completi[_index].PropIndices, completi[_index].PropTextures);
							ciao.SetRightBadge(UIMenuItem.BadgeStyle.None);
							ciao = _item;
							ciao.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
							HUD.ShowNotification("Hai speso ~g~" + completi[_index].Price + "$~w~, con carta di credito");
						}
						else
						{
							HUD.ShowNotification("Non hai i soldi per questo abito!", NotificationColor.Red, true);
						}
					}
				}
			};
			MenuVest.OnMenuClose += async (_menu) =>
			{
				await UpdateDress(Game.Player.GetPlayerData().CurrentChar.dressing);
				NegozioAbitiClient.Esci();
				Client.Instance.RemoveTick(CameraVest);
			};

			MenuVest.Visible = true;
			Client.Instance.AddTick(CameraVest);
		}
		#endregion

		#region PantMenu
		public static async void MenuPant(List<Singolo> Completi, string anim, string nome)
		{
			_menuVestiti.Clear();
			UIMenuItem ciao = new UIMenuItem("");
			UIMenuItem ciaone = new UIMenuItem("");
			UIMenu MenuPant = new UIMenu(" ", "~y~Benvenuti da " + nome + "!", new System.Drawing.Point(0, 0), Main.Textures[nome].Key, Main.Textures[nome].Value);
			pool.Add(MenuPant);
			MenuPant.AddInstructionalButton(new InstructionalButton(Control.FrontendLt, "Zoom"));
			_menuVestiti.Add(MenuPant);
			Game.PlayerPed.BlockPermanentEvents = true;
			SetPedAlternateMovementAnim(Game.PlayerPed.Handle, 0, anim, "try_trousers_base", 4.0f, true);
			await Game.PlayerPed.Task.PlayAnimation(anim, "try_trousers_base", 8f, -8f, -1, AnimationFlags.Loop, 0);
			var completi = Completi.OrderBy(x => x.Price).ToList();
			int money = 0;
			int mod = 0;
			int text = 0;
			foreach (var v in completi)
			{
				UIMenu Pant = pool.AddSubMenu(MenuPant, v.Title, v.Description);
				Pant.AddInstructionalButton(new InstructionalButton(Control.FrontendLt, "Zoom"));
				_menuVestiti.Add(Pant);
				foreach (var texture in v.Text)
				{
					UIMenuItem pant = new UIMenuItem("Modello " + v.Text.IndexOf(texture));
					Pant.AddItem(pant);
					if (nome == "Binco")
					{
						money = v.Price + (v.Text.IndexOf(texture) * 47);
					}
					else if (nome == "Discount")
					{
						money = v.Price + (v.Text.IndexOf(texture) * 62);
					}
					else if (nome == "Suburban")
					{
						money = v.Price + (v.Text.IndexOf(texture) * 75);
					}
					else if (nome == "Ponsombys")
					{
						money = v.Price + (v.Text.IndexOf(texture) * 89);
					}

					if (Game.Player.GetPlayerData().Money >= money)
					{
						pant.SetRightLabel("~g~$" + money);
					}
					else
					{
						if (Game.Player.GetPlayerData().Bank >= money)
						{
							pant.SetRightLabel("~g~$" + money);
						}
						else
						{
							pant.SetRightLabel("~r~$" + money);
						}
					}
					if (Game.Player.GetPlayerData().CurrentChar.dressing.ComponentDrawables.Pantaloni == v.Modello && Game.Player.GetPlayerData().CurrentChar.dressing.ComponentTextures.Pantaloni == texture)
					{
						Pant.ParentItem.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
						ciao = Pant.ParentItem;
						pant.SetRightBadge(UIMenuItem.BadgeStyle.Clothes); // cambiare con la collezione di abiti
						ciaone = pant;
					}
				}
				Pant.OnMenuOpen += async (menu) =>
				{
					SetPedComponentVariation(PlayerPedId(), 4, v.Modello, v.Text[0], 2);
					string random = GetRandomAnim(anim, false);
					SetPedComponentVariation(PlayerPedId(), 4, v.Modello, v.Text[0], 2);
					await Game.PlayerPed.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
					mod = v.Modello;
					text = v.Text[0];
				};
				Pant.OnIndexChange += async (sender, index) =>
				{
					string random = GetRandomAnim(anim, false);
					SetPedComponentVariation(PlayerPedId(), 4, v.Modello, v.Text[index], 2);
					await Game.PlayerPed.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
					mod = v.Modello;
					text = v.Text[index];
				};
				Pant.OnMenuClose += async (_menu) =>
				{
					await UpdateDress(Game.Player.GetPlayerData().CurrentChar.dressing);
				};
				Pant.OnItemSelect += async (_menu, _item, _index) =>
				{
					if (Game.Player.GetPlayerData().CurrentChar.dressing.ComponentDrawables.Pantaloni == mod && Game.Player.GetPlayerData().CurrentChar.dressing.ComponentTextures.Pantaloni == text)
					{
						HUD.ShowNotification("Hai già acquistato questo pantalone!!", true);
						return;
					}
					else
					{

						string m = string.Empty;
						int val = 0;
						for (int i = 0; i < _item.RightLabel.Length; i++)
						{
							if (Char.IsDigit(_item.RightLabel[i]))
							{
								m += _item.RightLabel[i];
							}
						}

						if (m.Length > 0)
						{
							val = int.Parse(m);
						}

						if (Game.Player.GetPlayerData().Money >= val)
						{
							BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
							Game.Player.GetPlayerData().CurrentChar.dressing.ComponentDrawables.Pantaloni = mod;
							Game.Player.GetPlayerData().CurrentChar.dressing.ComponentTextures.Pantaloni = text;
							Game.Player.GetPlayerData().CurrentChar.dressing.Name = null;
							Game.Player.GetPlayerData().CurrentChar.dressing.Description = null;
							BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Game.Player.GetPlayerData().CurrentChar.dressing.Serialize());
							ciao.SetRightBadge(UIMenuItem.BadgeStyle.None);
							ciao = MenuPant.MenuItems[MenuPant.CurrentSelection];
							ciao.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
							ciaone.SetRightBadge(UIMenuItem.BadgeStyle.None);
							ciaone = _item;
							ciaone.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
							HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
						}
						else
						{
							if (Game.Player.GetPlayerData().Bank >= val)
							{
								BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
								Game.Player.GetPlayerData().CurrentChar.dressing.ComponentDrawables.Pantaloni = mod;
								Game.Player.GetPlayerData().CurrentChar.dressing.ComponentTextures.Pantaloni = text;
								Game.Player.GetPlayerData().CurrentChar.dressing.Name = null;
								Game.Player.GetPlayerData().CurrentChar.dressing.Description = null;
								BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Game.Player.GetPlayerData().CurrentChar.dressing.Serialize());
								ciao.SetRightBadge(UIMenuItem.BadgeStyle.None);
								ciao = MenuPant.MenuItems[MenuPant.CurrentSelection];
								ciao.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
								ciaone.SetRightBadge(UIMenuItem.BadgeStyle.None);
								ciaone = _item;
								ciaone.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
								HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, con carta di credito");
							}
							else
							{
								HUD.ShowNotification("Non hai i soldi per questo abito!", NotificationColor.Red, true);
							}
						}
					}
				};
			}
			MenuPant.OnMenuClose += async (_menu) =>
			{
				await BaseScript.Delay(100);
				for (int i = 0; i < _menuVestiti.Count; i++) if (_menuVestiti[i].Visible) return;
				await UpdateDress(Game.Player.GetPlayerData().CurrentChar.dressing);
				NegozioAbitiClient.Esci();
				Client.Instance.RemoveTick(CameraVest);
			};
			MenuPant.Visible = true;
			Client.Instance.AddTick(CameraVest);
		}

		#endregion

		#region ScarpeMenu

		public static async void MenuScarpe(List<Singolo> Completi, string anim, string nome)
		{
			_menuVestiti.Clear();
			UIMenuItem ciao = new UIMenuItem("");
			UIMenuItem ciaone = new UIMenuItem("");
			UIMenu MenuScarpe = new UIMenu(" ", "~y~Benvenuti da " + nome + "!", new System.Drawing.Point(0, 0), Main.Textures[nome].Key, Main.Textures[nome].Value);
			pool.Add(MenuScarpe);
			_menuVestiti.Add(MenuScarpe);
			MenuScarpe.AddInstructionalButton(new InstructionalButton(Control.FrontendLt, "Zoom"));
			Game.PlayerPed.BlockPermanentEvents = true;
			SetPedAlternateMovementAnim(Game.PlayerPed.Handle, 0, anim, "try_shoes_base", 4.0f, true);
			await Game.PlayerPed.Task.PlayAnimation(anim, "try_shoes_base", 8f, -8f, -1, AnimationFlags.Loop, 0);
			var completi = Completi.OrderBy(x => x.Price).ToList();
			int money = 0;
			int mod = 0;
			int text = 0;
			foreach (var v in completi)
			{
				UIMenu Scarp = pool.AddSubMenu(MenuScarpe, v.Title, v.Description);
				Scarp.AddInstructionalButton(new InstructionalButton(Control.FrontendLt, "Zoom"));
				_menuVestiti.Add(Scarp);
				foreach (var texture in v.Text)
				{
					UIMenuItem pant = new UIMenuItem("Modello " + v.Text.IndexOf(texture));
					Scarp.AddItem(pant);

					if (nome == "Binco")
					{
						money = v.Price + (v.Text.IndexOf(texture) * 47);
					}
					else if (nome == "Discount")
					{
						money = v.Price + (v.Text.IndexOf(texture) * 62);
					}
					else if (nome == "Suburban")
					{
						money = v.Price + (v.Text.IndexOf(texture) * 75);
					}
					else if (nome == "Ponsombys")
					{
						money = v.Price + (v.Text.IndexOf(texture) * 89);
					}

					if (Game.Player.GetPlayerData().Money >= money)
					{
						pant.SetRightLabel("~g~$" + money);
					}
					else
					{
						if (Game.Player.GetPlayerData().Bank >= money)
						{
							pant.SetRightLabel("~g~$" + money);
						}
						else
						{
							pant.SetRightLabel("~r~$" + money);
						}
					}
					if (Game.Player.GetPlayerData().CurrentChar.dressing.ComponentDrawables.Scarpe == v.Modello && Game.Player.GetPlayerData().CurrentChar.dressing.ComponentTextures.Scarpe == texture)
					{
						Scarp.ParentItem.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
						ciao = Scarp.ParentItem;
						pant.SetRightBadge(UIMenuItem.BadgeStyle.Clothes); // cambiare con la collezione di abiti
						ciaone = pant;
					}
				}
				Scarp.OnMenuOpen += async (menu) =>
				{
					string random = GetRandomAnim(anim, false);
					SetPedComponentVariation(PlayerPedId(), 6, v.Modello, v.Text[0], 2);
					await Game.PlayerPed.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
					mod = v.Modello;
					text = v.Text[0];
				};
				Scarp.OnIndexChange += async (sender, index) =>
				{
					string random = GetRandomAnim(anim, false);
					SetPedComponentVariation(PlayerPedId(), 6, v.Modello, v.Text[index], 2);
					await Game.PlayerPed.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
					mod = v.Modello;
					text = v.Text[index];
				};
				Scarp.OnMenuClose += async (_menu) =>
				{
					await UpdateDress(Game.Player.GetPlayerData().CurrentChar.dressing);
				};
				Scarp.OnItemSelect += async (_menu, _item, _index) =>
				{
					if (Game.Player.GetPlayerData().CurrentChar.dressing.ComponentDrawables.Scarpe == mod && Game.Player.GetPlayerData().CurrentChar.dressing.ComponentTextures.Scarpe == text)
					{
						HUD.ShowNotification("Hai già acquistato queste scarpe!!", true);
						return;
					}
					else
					{

						string m = string.Empty;
						int val = 0;
						for (int i = 0; i < _item.RightLabel.Length; i++)
						{
							if (Char.IsDigit(_item.RightLabel[i]))
							{
								m += _item.RightLabel[i];
							}
						}

						if (m.Length > 0)
						{
							val = int.Parse(m);
						}

						if (Game.Player.GetPlayerData().Money >= val)
						{
							BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
							Game.Player.GetPlayerData().CurrentChar.dressing.ComponentDrawables.Scarpe = mod;
							Game.Player.GetPlayerData().CurrentChar.dressing.ComponentTextures.Scarpe = text;
							Game.Player.GetPlayerData().CurrentChar.dressing.Name = null;
							Game.Player.GetPlayerData().CurrentChar.dressing.Description = null;
							BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Game.Player.GetPlayerData().CurrentChar.dressing.Serialize());
							ciao.SetRightBadge(UIMenuItem.BadgeStyle.None);
							ciao = MenuScarpe.MenuItems[MenuScarpe.CurrentSelection];
							ciao.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
							ciaone.SetRightBadge(UIMenuItem.BadgeStyle.None);
							ciaone = _item;
							ciaone.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
							HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
						}
						else
						{
							if (Game.Player.GetPlayerData().Bank >= val)
							{
								BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
								Game.Player.GetPlayerData().CurrentChar.dressing.ComponentDrawables.Scarpe = mod;
								Game.Player.GetPlayerData().CurrentChar.dressing.ComponentTextures.Scarpe = text;
								Game.Player.GetPlayerData().CurrentChar.dressing.Name = null;
								Game.Player.GetPlayerData().CurrentChar.dressing.Description = null;
								BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Game.Player.GetPlayerData().CurrentChar.dressing.Serialize());
								ciao.SetRightBadge(UIMenuItem.BadgeStyle.None);
								ciao = MenuScarpe.MenuItems[MenuScarpe.CurrentSelection];
								ciao.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
								ciaone.SetRightBadge(UIMenuItem.BadgeStyle.None);
								ciaone = _item;
								ciaone.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
								HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, con carta di credito");
							}
							else
							{
								HUD.ShowNotification("Non hai i soldi per questo abito!", NotificationColor.Red, true);
							}
						}
					}
				};
			}
			MenuScarpe.OnMenuClose += async (_menu) =>
			{
				await BaseScript.Delay(100);
				for (int i = 0; i < _menuVestiti.Count; i++) if (_menuVestiti[i].Visible) return;
				await UpdateDress(Game.Player.GetPlayerData().CurrentChar.dressing);
				NegozioAbitiClient.Esci();
				Client.Instance.RemoveTick(CameraVest);
			};

			MenuScarpe.Visible = true;
			Client.Instance.AddTick(CameraVest);
		}

		#endregion

		#region OcchialiMenu
		public static async void MenuOcchiali(List<Singolo> Completi, string anim, string nome)
		{
			_menuVestiti.Clear();
			UIMenuItem ciao = new UIMenuItem("");
			UIMenuItem ciaone = new UIMenuItem("");
			UIMenu MenuOcchiali = new UIMenu(" ", "~y~Benvenuti da " + nome + "!", new System.Drawing.Point(0, 0), Main.Textures[nome].Key, Main.Textures[nome].Value);
			pool.Add(MenuOcchiali);
			_menuVestiti.Add(MenuOcchiali);
			MenuOcchiali.AddInstructionalButton(new InstructionalButton(Control.FrontendLt, "Zoom"));
			Game.PlayerPed.BlockPermanentEvents = true;
			SetPedAlternateMovementAnim(Game.PlayerPed.Handle, 0, anim, "Try_Glasses_Base", 4.0f, true);
			await Game.PlayerPed.Task.PlayAnimation(anim, "Try_Glasses_Base", 8f, -8f, -1, AnimationFlags.Loop, 0);
			var completi = Completi.OrderBy(x => x.Price).ToList();
			int money = 0;
			int mod = 0;
			int text = 0;
			foreach (var v in completi)
			{
				UIMenu Scarp = pool.AddSubMenu(MenuOcchiali, v.Title, v.Description);
				Scarp.AddInstructionalButton(new InstructionalButton(Control.FrontendLt, "Zoom"));
				_menuVestiti.Add(Scarp);
				foreach (var texture in v.Text)
				{
					UIMenuItem pant = new UIMenuItem("Colore " + v.Text.IndexOf(texture));
					Scarp.AddItem(pant);

					if (nome == "Binco")
					{
						money = v.Price + (v.Text.IndexOf(texture) * 47);
					}
					else if (nome == "Discount")
					{
						money = v.Price + (v.Text.IndexOf(texture) * 62);
					}
					else if (nome == "Suburban")
					{
						money = v.Price + (v.Text.IndexOf(texture) * 75);
					}
					else if (nome == "Ponsombys")
					{
						money = v.Price + (v.Text.IndexOf(texture) * 89);
					}

					if (Game.Player.GetPlayerData().Money >= money)
					{
						pant.SetRightLabel("~g~$" + money);
					}
					else
					{
						if (Game.Player.GetPlayerData().Bank >= money)
						{
							pant.SetRightLabel("~g~$" + money);
						}
						else
						{
							pant.SetRightLabel("~r~$" + money);
						}
					}
					if (Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Orecchie == v.Modello && Game.Player.GetPlayerData().CurrentChar.dressing.PropTextures.Orecchie == texture)
					{
						Scarp.ParentItem.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
						ciao = Scarp.ParentItem;
						pant.SetRightBadge(UIMenuItem.BadgeStyle.Clothes); // cambiare con la collezione di abiti
						ciaone = pant;
					}
				}
				Scarp.OnMenuOpen += async (menu) =>
				{
					string random = GetRandomAnim(anim, false);
					SetPedPropIndex(PlayerPedId(), 1, v.Modello, v.Text[0], false);
					await Game.PlayerPed.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
					mod = v.Modello;
					text = v.Text[0];
				};
				Scarp.OnIndexChange += async (menu, index) =>
				{
					string random = GetRandomAnim(anim, false);
					SetPedPropIndex(PlayerPedId(), 1, v.Modello, v.Text[index], false);
					await Game.PlayerPed.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
					mod = v.Modello;
					text = v.Text[index];
				};
				Scarp.OnMenuClose += async (_menu) =>
				{
					await UpdateDress(Game.Player.GetPlayerData().CurrentChar.dressing);
				};
				Scarp.OnItemSelect += async (_menu, _item, _index) =>
				{
					if (Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Orecchie == mod && Game.Player.GetPlayerData().CurrentChar.dressing.PropTextures.Orecchie == text)
					{
						HUD.ShowNotification("Hai già acquistato questi occhiali!!", true);
						return;
					}
					else
					{

						string m = string.Empty;
						int val = 0;
						for (int i = 0; i < _item.RightLabel.Length; i++)
						{
							if (Char.IsDigit(_item.RightLabel[i]))
							{
								m += _item.RightLabel[i];
							}
						}

						if (m.Length > 0)
						{
							val = int.Parse(m);
						}

						if (Game.Player.GetPlayerData().Money >= val)
						{
							BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
							Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Orecchie = mod;
							Game.Player.GetPlayerData().CurrentChar.dressing.PropTextures.Orecchie = text;
							Game.Player.GetPlayerData().CurrentChar.dressing.Name = null;
							Game.Player.GetPlayerData().CurrentChar.dressing.Description = null;
							BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Game.Player.GetPlayerData().CurrentChar.dressing.Serialize());
							ciao.SetRightBadge(UIMenuItem.BadgeStyle.None);
							ciao = MenuOcchiali.MenuItems[MenuOcchiali.CurrentSelection];
							ciao.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
							ciaone.SetRightBadge(UIMenuItem.BadgeStyle.None);
							ciaone = _item;
							ciaone.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
							HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
						}
						else
						{
							if (Game.Player.GetPlayerData().Bank >= val)
							{
								BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
								Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Orecchie = mod;
								Game.Player.GetPlayerData().CurrentChar.dressing.PropTextures.Orecchie = text;
								Game.Player.GetPlayerData().CurrentChar.dressing.Name = null;
								Game.Player.GetPlayerData().CurrentChar.dressing.Description = null;
								BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Game.Player.GetPlayerData().CurrentChar.dressing.Serialize());
								ciao.SetRightBadge(UIMenuItem.BadgeStyle.None);
								ciao = MenuOcchiali.MenuItems[MenuOcchiali.CurrentSelection];
								ciao.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
								ciaone.SetRightBadge(UIMenuItem.BadgeStyle.None);
								ciaone = _item;
								ciaone.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
								HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, con carta di credito");
							}
							else
							{
								HUD.ShowNotification("Non hai i soldi per questo abito!", NotificationColor.Red, true);
							}
						}
					}
				};
			}
			MenuOcchiali.OnMenuClose += async (_menu) =>
			{
				await BaseScript.Delay(100);
				for (int i = 0; i < _menuVestiti.Count; i++) if (_menuVestiti[i].Visible) return;
				await UpdateDress(Game.Player.GetPlayerData().CurrentChar.dressing);
				NegozioAbitiClient.Esci();
				Client.Instance.RemoveTick(CameraVest);
			};

			MenuOcchiali.Visible = true;
			Client.Instance.AddTick(CameraVest);
		}

		#endregion

		#region AccessoriMenu

		static UIMenu Borse = new UIMenu(" ", "Borse");
		static UIMenu Orecc = new UIMenu(" ", "Orecchini e Auricolari");
		static UIMenu Capp = new UIMenu(" ", "Orologi");
		static UIMenu Polso = new UIMenu(" ", "Orologi e Braccialetti");
		static UIMenu Orol = new UIMenu(" ", "Orologi");
		static UIMenu Brac = new UIMenu(" ", "Orologi");
		static UIMenu Orologino = new UIMenu(" ", " ");
		static List<UIMenu> SubMenusCapp = new List<UIMenu>();
		static List<UIMenu> SubMenusPolso = new List<UIMenu>();

		static float fov = 0;
		public static async void MenuAccessori(Accessori Accessorio, string anim, string nome)
		{
			AccessoriAttivo = true;
			UIMenuItem orecAtt = new UIMenuItem("");
			UIMenuItem coloOreccAtt = new UIMenuItem("");
			UIMenuItem OrolAtt = new UIMenuItem("");
			UIMenuItem OrolMod = new UIMenuItem("");
			UIMenuItem BraccAtt = new UIMenuItem("");
			UIMenuItem CappAtt = new UIMenuItem("");
			UIMenuItem CappAttMod = new UIMenuItem("");
			UIMenuItem BorsAtt = new UIMenuItem("");
			UIMenuItem CappRim = new UIMenuItem("");

			UIMenu MenuAccessori = new UIMenu(" ", "~y~Benvenuti da " + nome + "!", new System.Drawing.Point(0, 0), Main.Textures[nome].Key, Main.Textures[nome].Value);
			pool.Add(MenuAccessori);
			MenuAccessori.AddInstructionalButton(new InstructionalButton(Control.FrontendLt, "Zoom"));
			Game.PlayerPed.BlockPermanentEvents = true;
			SetPedAlternateMovementAnim(Game.PlayerPed.Handle, 0, anim, "try_shirt_base", 4.0f, true);
			await Game.PlayerPed.Task.PlayAnimation(anim, "try_shirt_base", 8f, -8f, -1, AnimationFlags.Loop, 0);
			int money = 0;
			int IntorecAtt = Game.Player.GetPlayerData().CurrentChar.skin.ears.style;
			int IntcoloOreccAtt = Game.Player.GetPlayerData().CurrentChar.skin.ears.color;
			int IntOrolAtt = GetPedPropIndex(PlayerPedId(), 6);
			int IntOrolMod = GetPedPropTextureIndex(PlayerPedId(), 6);
			int IntBraccAtt = GetPedPropIndex(PlayerPedId(), 7);
			int IntCappAtt = GetPedPropIndex(PlayerPedId(), 0);
			int IntCappAttMod = GetPedPropTextureIndex(PlayerPedId(), 0);
			int IntBors = GetPedDrawableVariation(PlayerPedId(), 5);
			int mod = 0;
			int text = 0;

			#region sottomenu
			Borse = pool.AddSubMenu(MenuAccessori, "Borse", "Scegli qui tra le borse disponibili!");
			Borse.AddInstructionalButton(new InstructionalButton(Control.FrontendLt, "Zoom"));
			Capp = pool.AddSubMenu(MenuAccessori, "Cappellini", "Scegli qui i tuoi accessori preferiti!");
			Capp.AddInstructionalButton(new InstructionalButton(Control.FrontendLt, "Zoom"));
			Orecc = pool.AddSubMenu(MenuAccessori, "Orecchini e Auricolari", "Scegli qui i tuoi pendagli preferiti!");
			Orecc.AddInstructionalButton(new InstructionalButton(Control.FrontendLt, "Zoom"));
			Polso = pool.AddSubMenu(MenuAccessori, "Orologi e Braccialetti", "Scegli qui tra orologi e braccialetti!");
			Polso.AddInstructionalButton(new InstructionalButton(Control.FrontendLt, "Zoom"));

			Orol = pool.AddSubMenu(Polso, "Orologi", "Scegli qui i tuoi orologi!");
			Orol.AddInstructionalButton(new InstructionalButton(Control.FrontendLt, "Zoom"));
			Brac = pool.AddSubMenu(Polso, "Braccialetti", "Scegli qui i tuoi braccialetti!");
			Brac.AddInstructionalButton(new InstructionalButton(Control.FrontendLt, "Zoom"));
			SubMenusPolso.Add(Orol);
			SubMenusPolso.Add(Brac);
			#endregion
			List<dynamic> ore = new List<dynamic>();
			List<dynamic> bra = new List<dynamic>();
			foreach (string s in Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList().Select(x => x.Title).ToList())
			{
				ore.Add(s);
			}

			foreach (string s in Accessorio.Bracciali.OrderBy(x => x.Price).ToList().Select(x => x.Title).ToList())
			{
				bra.Add(s);
			}

			UIMenuListItem orecchini = new UIMenuListItem("Orecchini", ore, 0, "Scegli qui i tuoi orecchini preferiti");
			Orecc.AddItem(orecchini);
			UIMenuListItem braccialetti = new UIMenuListItem("Braccialetti", bra, 0, "Scegli qui il tuo bracciale preferito");
			Brac.AddItem(braccialetti);

			foreach (var borsa in Accessorio.Borse)
			{
				money = borsa.Price;
				UIMenuItem bors = new UIMenuItem(borsa.Title, borsa.Description);
				if (Game.Player.GetPlayerData().Money >= money)
				{
					bors.SetRightLabel("~g~$" + money);
				}
				else
				{
					if (Game.Player.GetPlayerData().Bank >= money)
					{
						bors.SetRightLabel("~g~$" + money);
					}
					else
					{
						bors.SetRightLabel("~r~$" + money);
					}
				}
				Borse.AddItem(bors);
				if (Game.Player.GetPlayerData().CurrentChar.dressing.ComponentDrawables.Borsa_Paracadute == borsa.Modello)
				{
					bors.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
					BorsAtt = bors;
				}
			}
			Borse.OnIndexChange += async (_menu, index) =>
			{
				IntBors = Accessorio.Borse[index].Modello;
				string random = GetRandomAnim(anim, false);
				SetPedComponentVariation(PlayerPedId(), 5, IntBors, 0, 2);
				await Game.PlayerPed.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
			};
			Borse.OnMenuClose += async (menu) => { await UpdateDress(Game.Player.GetPlayerData().CurrentChar.dressing); };
			Borse.OnItemSelect += async (_menu, _item, _index) =>
			{
				if (Game.Player.GetPlayerData().CurrentChar.dressing.ComponentDrawables.Borsa_Paracadute == IntBors)
				{
					HUD.ShowNotification("Hai già acquistato questa borsa!!", true);
					return;
				}
				else
				{

					string m = string.Empty;
					int val = 0;
					for (int i = 0; i < _item.RightLabel.Length; i++)
					{
						if (Char.IsDigit(_item.RightLabel[i]))
						{
							m += _item.RightLabel[i];
						}
					}

					if (m.Length > 0)
					{
						val = int.Parse(m);
					}

					if (Game.Player.GetPlayerData().Money >= val)
					{
						BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
						Game.Player.GetPlayerData().CurrentChar.dressing.ComponentDrawables.Borsa_Paracadute = IntBors;
						Game.Player.GetPlayerData().CurrentChar.dressing.ComponentTextures.Borsa_Paracadute = 0;
						Game.Player.GetPlayerData().CurrentChar.dressing.Name = null;
						Game.Player.GetPlayerData().CurrentChar.dressing.Description = null;
						BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Game.Player.GetPlayerData().CurrentChar.dressing.Serialize());
						BorsAtt.SetRightBadge(UIMenuItem.BadgeStyle.None);
						BorsAtt = _menu.MenuItems[_menu.CurrentSelection];
						BorsAtt.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
						HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
					}
					else
					{
						if (Game.Player.GetPlayerData().Bank >= val)
						{
							BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
							Game.Player.GetPlayerData().CurrentChar.dressing.ComponentDrawables.Borsa_Paracadute = IntBors;
							Game.Player.GetPlayerData().CurrentChar.dressing.ComponentTextures.Borsa_Paracadute = 0;
							Game.Player.GetPlayerData().CurrentChar.dressing.Name = null;
							Game.Player.GetPlayerData().CurrentChar.dressing.Description = null;
							BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Game.Player.GetPlayerData().CurrentChar.dressing.Serialize());
							BorsAtt.SetRightBadge(UIMenuItem.BadgeStyle.None);
							BorsAtt = _menu.MenuItems[_menu.CurrentSelection];
							BorsAtt.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
							HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, con carta di credito");
						}
						else
						{
							HUD.ShowNotification("Non hai i soldi per questo abito!", NotificationColor.Red, true);
						}
					}
				}

			};

			UIMenuItem CappAtt1 = new UIMenuItem("");
			UIMenuItem newCap = new UIMenuItem("");
			UIMenuItem capelino = new UIMenuItem("");
			UIMenu Capelino = new UIMenu("", "");
			foreach (var cappellino in Accessorio.Testa.Cappellini)
			{
				if (Accessorio.Testa.Cappellini.IndexOf(cappellino) != 0)
				{
					Capelino = pool.AddSubMenu(Capp, cappellino.Title + " " + Accessorio.Testa.Cappellini.IndexOf(cappellino), cappellino.Description);
					Capelino.AddInstructionalButton(new InstructionalButton(Control.FrontendLt, "Zoom"));
					SubMenusCapp.Add(Capelino);
				}
				if (cappellino.Modello == -1)
				{
					CappRim = new UIMenuItem(cappellino.Title, cappellino.Description);
					Capp.AddItem(CappRim);
					CappRim.SetRightLabel("~g~$0");
				}
				if (Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Cappelli_Maschere == -1)
				{
					CappRim.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
					CappAtt = CappRim;
				}
				if (Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Cappelli_Maschere != -1 && Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Cappelli_Maschere == cappellino.Modello)
				{
					capelino.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
					CappAtt = CappRim;
				}
				foreach (var texture in cappellino.Text)
				{
					newCap = new UIMenuItem("Modello " + cappellino.Text.IndexOf(texture), cappellino.Description);
					Capelino.AddItem(newCap);
					if (nome == "Binco")
					{
						money = cappellino.Price + (cappellino.Text.IndexOf(texture) * 47);
					}
					else if (nome == "Discount")
					{
						money = cappellino.Price + (cappellino.Text.IndexOf(texture) * 62);
					}
					else if (nome == "Suburban")
					{
						money = cappellino.Price + (cappellino.Text.IndexOf(texture) * 75);
					}
					else if (nome == "Ponsombys")
					{
						money = cappellino.Price + (cappellino.Text.IndexOf(texture) * 89);
					}

					if (Game.Player.GetPlayerData().Money >= money)
					{
						newCap.SetRightLabel("~g~$" + money);
					}
					else
					{
						if (Game.Player.GetPlayerData().Bank >= money)
						{
							newCap.SetRightLabel("~g~$" + money);
						}
						else
						{
							newCap.SetRightLabel("~r~$" + money);
						}
					}
					if (Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Cappelli_Maschere == cappellino.Modello && Game.Player.GetPlayerData().CurrentChar.dressing.PropTextures.Cappelli_Maschere == texture)
					{
						newCap.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
						CappAtt1 = newCap;
					}
				}
				Capelino.OnIndexChange += async (_menu, _newIndex) =>
				{
					string random = GetRandomAnim(anim, false);
					SetPedPropIndex(PlayerPedId(), 0, cappellino.Modello, cappellino.Text[_newIndex], false);
					IntCappAtt = cappellino.Text[_newIndex];
					IntCappAttMod = cappellino.Text[_newIndex];
					await Game.PlayerPed.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
				};
				Capelino.OnItemSelect += async (_menu, _item, _index) =>
				{
					if (Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Cappelli_Maschere == IntCappAtt && Game.Player.GetPlayerData().CurrentChar.dressing.PropTextures.Cappelli_Maschere == IntCappAttMod)
					{
						HUD.ShowNotification("Non puoi acquistare lo stesso cappello che hai già! Prova a cambiare modello!");
					}
					else
					{
						string m = string.Empty;
						int val = 0;
						for (int i = 0; i < _item.RightLabel.Length; i++)
						{
							if (Char.IsDigit(_item.RightLabel[i]))
							{
								m += _item.RightLabel[i];
							}
						}

						if (m.Length > 0)
						{
							val = int.Parse(m);
						}

						if (Game.Player.GetPlayerData().Money >= val)
						{
							BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
							Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Cappelli_Maschere = IntCappAtt;
							Game.Player.GetPlayerData().CurrentChar.dressing.PropTextures.Cappelli_Maschere = IntCappAttMod;
							Game.Player.GetPlayerData().CurrentChar.dressing.Name = null;
							Game.Player.GetPlayerData().CurrentChar.dressing.Description = null;
							BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Game.Player.GetPlayerData().CurrentChar.dressing.Serialize());
							CappAtt.SetRightBadge(UIMenuItem.BadgeStyle.None);
							CappAtt = Capp.MenuItems[Capp.CurrentSelection];
							CappAtt.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
							CappAtt1.SetRightBadge(UIMenuItem.BadgeStyle.None);
							CappAtt1 = _menu.MenuItems[_menu.CurrentSelection];
							CappAtt1.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
							HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
						}
						else
						{
							if (Game.Player.GetPlayerData().Bank >= val)
							{
								BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
								Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Cappelli_Maschere = IntCappAtt;
								Game.Player.GetPlayerData().CurrentChar.dressing.PropTextures.Cappelli_Maschere = IntCappAttMod;
								Game.Player.GetPlayerData().CurrentChar.dressing.Name = null;
								Game.Player.GetPlayerData().CurrentChar.dressing.Description = null;
								BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Game.Player.GetPlayerData().CurrentChar.dressing.Serialize());
								CappAtt.SetRightBadge(UIMenuItem.BadgeStyle.None);
								CappAtt = Capp.MenuItems[Capp.CurrentSelection];
								CappAtt.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
								CappAtt1.SetRightBadge(UIMenuItem.BadgeStyle.None);
								CappAtt1 = _menu.MenuItems[_menu.CurrentSelection];
								CappAtt1.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
								HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, con carta di credito");
							}
							else
							{
								HUD.ShowNotification("Non hai i soldi per questo abito!", NotificationColor.Red, true);
							}
						}

					}
				};
				Capelino.OnMenuClose += async (menu) =>
				{
					PointCamAtPedBone(NegozioAbitiClient.camm.Handle, PlayerPedId(), 24818, 0.0f, 0.0f, 0.0f, true);
					await UpdateDress(Game.Player.GetPlayerData().CurrentChar.dressing);
				};
			}
			Capp.OnItemSelect += async (_menu, _item, _index) =>
			{
				if (_item == CappRim)
				{
					if (Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Cappelli_Maschere == -1)
					{
						HUD.ShowNotification("Non puoi rimuovere 2 volte un Cappello!!", true);
					}
					else
					{
						string m = string.Empty;
						int val = 0;
						for (int i = 0; i < _item.RightLabel.Length; i++)
						{
							if (Char.IsDigit(_item.RightLabel[i]))
							{
								m += _item.RightLabel[i];
							}
						}

						if (m.Length > 0)
						{
							val = int.Parse(m);
						}

						if (Game.Player.GetPlayerData().Money >= val)
						{
							BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
							Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Cappelli_Maschere = -1;
							Game.Player.GetPlayerData().CurrentChar.dressing.PropTextures.Cappelli_Maschere = -1;
							Game.Player.GetPlayerData().CurrentChar.dressing.Name = null;
							Game.Player.GetPlayerData().CurrentChar.dressing.Description = null;
							BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Game.Player.GetPlayerData().CurrentChar.dressing.Serialize());
							CappAtt.SetRightBadge(UIMenuItem.BadgeStyle.None);
							CappAtt = _menu.MenuItems[_menu.CurrentSelection];
							CappAtt.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
							HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
						}
						else
						{
							if (Game.Player.GetPlayerData().Bank >= val)
							{
								BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
								Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Cappelli_Maschere = -1;
								Game.Player.GetPlayerData().CurrentChar.dressing.PropTextures.Cappelli_Maschere = -1;
								Game.Player.GetPlayerData().CurrentChar.dressing.Name = null;
								Game.Player.GetPlayerData().CurrentChar.dressing.Description = null;
								BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Game.Player.GetPlayerData().CurrentChar.dressing.Serialize());
								CappAtt.SetRightBadge(UIMenuItem.BadgeStyle.None);
								CappAtt = _menu.MenuItems[_menu.CurrentSelection];
								CappAtt.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
								HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, con carta di credito");
							}
							else
							{
								HUD.ShowNotification("Non hai i soldi per questo abito!", NotificationColor.Red, true);
							}
						}
					}
				}
			};

			Orecc.OnListChange += (_menu, _listItem, _newIndex) =>
			{
				string ActiveItem = _listItem.Items[_newIndex].ToString();
				if (Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Orecchie == Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_newIndex].Modello)
				{
//					_listItem.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
				}
				else
				{
//					_listItem.SetRightBadge(UIMenuItem.BadgeStyle.None);
				}

				_listItem.Description = Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_newIndex].Description + ", Prezzo: $" + Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_newIndex].Price;
				SetPedPropIndex(PlayerPedId(), 2, Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_newIndex].Modello, 0, false);
				IntorecAtt = Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_newIndex].Modello;
				_menu.UpdateDescription();
			};
			Orecc.OnItemSelect += (_menu, _listItem, _listIndex) =>
			{
				if (Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Orecchie == Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_listIndex].Modello && Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Orecchie == -1)
				{
					HUD.ShowNotification("Non puoi rimuovere 2 volte gli orecchini!!", true);
				}
				else if (Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Orecchie == Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_listIndex].Modello && Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Orecchie != -1)
				{
					HUD.ShowNotification("Non puoi acquistare di nuovo gli orecchini che hai già!");
				}
				else
				{
					int val = Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_listIndex].Price;
					if (Game.Player.GetPlayerData().Money >= val)
					{
						BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
						Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Orecchie = Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_listIndex].Modello;
						Game.Player.GetPlayerData().CurrentChar.dressing.PropTextures.Orecchie = 0;
						Game.Player.GetPlayerData().CurrentChar.skin.ears.style = Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_listIndex].Modello;
						Game.Player.GetPlayerData().CurrentChar.skin.ears.color = 0;
						Game.Player.GetPlayerData().CurrentChar.dressing.Name = null;
						Game.Player.GetPlayerData().CurrentChar.dressing.Description = null;
						BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Game.Player.GetPlayerData().CurrentChar.dressing.Serialize());
						HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
					}
					else
					{
						if (Game.Player.GetPlayerData().Bank >= val)
						{
							BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
							Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Orecchie = Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_listIndex].Modello;
							Game.Player.GetPlayerData().CurrentChar.dressing.PropTextures.Orecchie = 0;
							Game.Player.GetPlayerData().CurrentChar.skin.ears.style = Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_listIndex].Modello;
							Game.Player.GetPlayerData().CurrentChar.skin.ears.color = 0;
							Game.Player.GetPlayerData().CurrentChar.dressing.Name = null;
							Game.Player.GetPlayerData().CurrentChar.dressing.Description = null;
							BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Game.Player.GetPlayerData().CurrentChar.dressing.Serialize());
							HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, con carta di credito");
						}
						else
						{
							HUD.ShowNotification("Non hai i soldi per questo abito!", NotificationColor.Red, true);
						}
					}
				}
			};

			foreach (var aurico in Accessorio.Testa.Auricolari.OrderBy(x => x.Price).ToList())
			{
				UIMenuItem auricolari = new UIMenuItem(aurico.Title, aurico.Description);
				if (Game.Player.GetPlayerData().Money >= aurico.Price)
				{
					auricolari.SetRightLabel("~g~$" + aurico.Price);
				}
				else
				{
					if (Game.Player.GetPlayerData().Bank >= aurico.Price)
					{
						auricolari.SetRightLabel("~g~$" + aurico.Price);
					}
					else
					{
						auricolari.SetRightLabel("~r~$" + aurico.Price);
					}
				}
				Orecc.AddItem(auricolari);
			}
			Orecc.OnIndexChange += async (_menu, _newIndex) =>
			{
				if (_newIndex != 0)
				{
					if (Accessorio.Testa.Auricolari.OrderBy(x => x.Price).ToList()[_newIndex - 1].Modello == -1)
					{
						ClearPedProp(PlayerPedId(), 2);
					}
					else
					{
						SetPedPropIndex(PlayerPedId(), 2, Accessorio.Testa.Auricolari.OrderBy(x => x.Price).ToList()[_newIndex - 1].Modello, 0, false);
					}
				}
			};
			Orecc.OnItemSelect += async (_menu, _item, _index) =>
			{
				if (_index != 0)
				{
					if (Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Orecchie == Accessorio.Testa.Auricolari.OrderBy(x => x.Price).ToList()[_index - 1].Modello && Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Orecchie == -1)
						HUD.ShowNotification("Non puoi rimuovere 2 volte l'auricolare!!", true);
					else if (Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Orecchie == Accessorio.Testa.Auricolari.OrderBy(x => x.Price).ToList()[_index - 1].Modello && Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Orecchie != -1)
						HUD.ShowNotification("Non puoi acquistare l'auricolare che hai già!");
					else
					{
						int val = Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_index - 1].Price;
						if (Game.Player.GetPlayerData().Money >= val)
						{
							BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
							Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Orecchie = Accessorio.Testa.Auricolari.OrderBy(x => x.Price).ToList()[_index - 1].Modello;
							Game.Player.GetPlayerData().CurrentChar.dressing.PropTextures.Orecchie = 0;
							Game.Player.GetPlayerData().CurrentChar.skin.ears.style = Accessorio.Testa.Auricolari.OrderBy(x => x.Price).ToList()[_index - 1].Modello;
							Game.Player.GetPlayerData().CurrentChar.skin.ears.color = 0;
							Game.Player.GetPlayerData().CurrentChar.dressing.Name = null;
							Game.Player.GetPlayerData().CurrentChar.dressing.Description = null;
							BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Game.Player.GetPlayerData().CurrentChar.dressing.Serialize());
							HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
						}
						else
						{
							if (Game.Player.GetPlayerData().Bank >= val)
							{
								BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
								Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Orecchie = Accessorio.Testa.Auricolari.OrderBy(x => x.Price).ToList()[_index - 1].Modello;
								Game.Player.GetPlayerData().CurrentChar.dressing.PropTextures.Orecchie = 0;
								Game.Player.GetPlayerData().CurrentChar.skin.ears.style = Accessorio.Testa.Auricolari.OrderBy(x => x.Price).ToList()[_index - 1].Modello;
								Game.Player.GetPlayerData().CurrentChar.skin.ears.color = 0;
								Game.Player.GetPlayerData().CurrentChar.dressing.Name = null;
								Game.Player.GetPlayerData().CurrentChar.dressing.Description = null;
								BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Game.Player.GetPlayerData().CurrentChar.dressing.Serialize());
								HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, con carta di credito");
							}
							else
								HUD.ShowNotification("Non hai i soldi per questo abito!", NotificationColor.Red, true);
						}
					}
				}
			};

			UIMenuItem OrologinoItem = new UIMenuItem("");
			UIMenuItem NoOrol = new UIMenuItem("");
			UIMenuItem NewOrol = new UIMenuItem("");
			foreach (var orologio in Accessorio.Orologi)
			{
				if (Accessorio.Orologi.IndexOf(orologio) != 0)
				{
					Orologino = pool.AddSubMenu(Orol, orologio.Title, orologio.Description);
					Orologino.AddInstructionalButton(new InstructionalButton(Control.FrontendLt, "Zoom"));
					SubMenusPolso.Add(Orologino);
				}
				if (orologio.Modello == -1)
				{
					NoOrol = new UIMenuItem(orologio.Title, orologio.Description);
					Orol.AddItem(NoOrol);
					if (Game.Player.GetPlayerData().Money >= orologio.Price)
					{
						NoOrol.SetRightLabel("~g~$" + orologio.Price);
					}
					else
					{
						if (Game.Player.GetPlayerData().Bank >= orologio.Price)
						{
							NoOrol.SetRightLabel("~g~$" + orologio.Price);
						}
						else
						{
							NoOrol.SetRightLabel("~r~$" + orologio.Price);
						}
					}
				}

				foreach (var v in orologio.Text)
				{
					NewOrol = new UIMenuItem("Modello " + orologio.Text.IndexOf(v), orologio.Description);
					Orologino.AddItem(NewOrol);
					if (nome == "Binco")
					{
						money = orologio.Price + (orologio.Text.IndexOf(v) * 47);
					}
					else if (nome == "Discount")
					{
						money = orologio.Price + (orologio.Text.IndexOf(v) * 62);
					}
					else if (nome == "Suburban")
					{
						money = orologio.Price + (orologio.Text.IndexOf(v) * 75);
					}
					else if (nome == "Ponsombys")
					{
						money = orologio.Price + (orologio.Text.IndexOf(v) * 89);
					}

					if (Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Orologi == orologio.Modello && Game.Player.GetPlayerData().CurrentChar.dressing.PropTextures.Orologi == v)
					{
						if (orologio.Modello == -1)
						{
							NoOrol.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
							OrolAtt = NoOrol;
						}
						else
						{
							OrologinoItem.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
							OrolAtt = OrologinoItem;
						}
						NewOrol.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
						OrolMod = NewOrol;
					}

					if (Game.Player.GetPlayerData().Money >= money)
					{
						NewOrol.SetRightLabel("~g~$" + money);
					}
					else
					{
						if (Game.Player.GetPlayerData().Bank >= money)
						{
							NewOrol.SetRightLabel("~g~$" + money);
						}
						else
						{
							NewOrol.SetRightLabel("~r~$" + money);
						}
					}
					Orologino.OnMenuOpen += (menu) =>
					{
						SetPedPropIndex(PlayerPedId(), 6, orologio.Modello, 0, false);
						IntOrolAtt = orologio.Modello;
						IntOrolMod = 0;
					};
					Orologino.OnIndexChange += async (_menu, _newIndex) =>
					{
						SetPedPropIndex(PlayerPedId(), 6, orologio.Modello, orologio.Text[_newIndex], false);
						IntOrolAtt = orologio.Modello;
						IntOrolMod = orologio.Text[_newIndex];
					};
				}
				Orologino.OnItemSelect += async (_menu, _item, _index) =>
				{
					if (Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Orologi == IntOrolAtt && Game.Player.GetPlayerData().CurrentChar.dressing.PropTextures.Orologi == IntOrolMod)
					{
						HUD.ShowNotification("Non puoi acquistare lo stesso orologio che hai già! Prova a cambiare modello!", true);
					}
					else
					{
						string m = string.Empty;
						int val = 0;
						for (int i = 0; i < _item.RightLabel.Length; i++)
						{
							if (Char.IsDigit(_item.RightLabel[i]))
							{
								m += _item.RightLabel[i];
							}
						}

						if (m.Length > 0)
						{
							val = int.Parse(m);
						}

						if (Game.Player.GetPlayerData().Money >= val)
						{
							BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
							Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Orologi = IntOrolAtt;
							Game.Player.GetPlayerData().CurrentChar.dressing.PropTextures.Orologi = IntOrolMod;
							Game.Player.GetPlayerData().CurrentChar.dressing.Description = null;
							OrolAtt.SetRightBadge(UIMenuItem.BadgeStyle.None);
							OrolAtt = Orol.MenuItems[Orol.CurrentSelection];
							OrolAtt.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
							OrolMod.SetRightBadge(UIMenuItem.BadgeStyle.None);
							OrolMod = _menu.MenuItems[_menu.CurrentSelection];
							OrolMod.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
							BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Game.Player.GetPlayerData().CurrentChar.dressing.Serialize());
							if (val > 0)
							{
								HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
							}
						}
						else
						{
							if (Game.Player.GetPlayerData().Bank >= val)
							{
								BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
								Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Orologi = IntOrolAtt;
								Game.Player.GetPlayerData().CurrentChar.dressing.PropTextures.Orologi = IntOrolMod;
								OrolAtt.SetRightBadge(UIMenuItem.BadgeStyle.None);
								OrolAtt = Orol.MenuItems[Orol.CurrentSelection];
								OrolAtt.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
								OrolMod.SetRightBadge(UIMenuItem.BadgeStyle.None);
								OrolMod = _menu.MenuItems[_menu.CurrentSelection];
								OrolMod.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
								Game.Player.GetPlayerData().CurrentChar.dressing.Name = null;
								Game.Player.GetPlayerData().CurrentChar.dressing.Description = null;
								BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Game.Player.GetPlayerData().CurrentChar.dressing.Serialize());
								if (val > 0)
								{
									HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, con carta di credito");
								}
							}
							else
							{
								HUD.ShowNotification("Non hai i soldi per questo abito!", NotificationColor.Red, true);
							}
						}
					}
				};
			}
			Orol.OnIndexChange += async (_menu, _newIndex) =>
			{
				if (_menu.MenuItems[_newIndex] == NoOrol)
				{
					ClearPedProp(PlayerPedId(), 6);
				}
			};
			Orol.OnItemSelect += async (_menu, _item, _index) =>
			{
				if (_item == NoOrol)
				{
					if (Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Orologi == -1 && IntOrolAtt == -1)
					{
						HUD.ShowNotification("Non puoi rimuovere 2 volte un orologio!!", true);
					}
					else
					{
						string m = string.Empty;
						int val = 0;
						for (int i = 0; i < _item.RightLabel.Length; i++)
						{
							if (Char.IsDigit(_item.RightLabel[i]))
							{
								m += _item.RightLabel[i];
							}
						}

						if (m.Length > 0)
						{
							val = int.Parse(m);
						}

						if (Game.Player.GetPlayerData().Money >= val)
						{
							BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
							Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Orologi = -1;
							Game.Player.GetPlayerData().CurrentChar.dressing.PropTextures.Orologi = -1;
							OrolAtt.SetRightBadge(UIMenuItem.BadgeStyle.None);
							OrolAtt = _menu.MenuItems[_menu.CurrentSelection];
							OrolAtt.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
							Game.Player.GetPlayerData().CurrentChar.skin.ears.style = Accessorio.Orologi.OrderBy(x => x.Price).ToList()[_index].Modello;
							Game.Player.GetPlayerData().CurrentChar.dressing.Description = null;
							BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Game.Player.GetPlayerData().CurrentChar.dressing.Serialize());
							if (val > 0)
							{
								HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
							}
						}
						else
						{
							if (Game.Player.GetPlayerData().Bank >= val)
							{
								BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
								Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Orologi = -1;
								Game.Player.GetPlayerData().CurrentChar.dressing.PropTextures.Orologi = -1;
								OrolAtt.SetRightBadge(UIMenuItem.BadgeStyle.None);
								OrolAtt = _menu.MenuItems[_menu.CurrentSelection];
								OrolAtt.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
								Game.Player.GetPlayerData().CurrentChar.dressing.Name = null;
								Game.Player.GetPlayerData().CurrentChar.dressing.Description = null;
								BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Game.Player.GetPlayerData().CurrentChar.dressing.Serialize());
								if (val > 0)
								{
									HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, con carta di credito");
								}
							}
							else
							{
								HUD.ShowNotification("Non hai i soldi per questo abito!", NotificationColor.Red, true);
							}
						}
					}
				}
			};
			Brac.OnListChange += (_menu, _listItem, _newIndex) =>
			{
				if (_listItem == braccialetti)
				{
					string ActiveItem = _listItem.Items[_newIndex].ToString();
					_listItem.Description = Accessorio.Bracciali.OrderBy(x => x.Price).ToList()[_newIndex].Description + ", Prezzo: $" + Accessorio.Bracciali.OrderBy(x => x.Price).ToList()[_newIndex].Price;
					if (Accessorio.Bracciali.OrderBy(x => x.Price).ToList()[_newIndex].Modello != -1)
					{
						SetPedPropIndex(PlayerPedId(), 7, Accessorio.Bracciali.OrderBy(x => x.Price).ToList()[_newIndex].Modello, 0, false);
					}
					else
					{
						ClearPedProp(PlayerPedId(), 7);
					}

					IntorecAtt = Accessorio.Bracciali.OrderBy(x => x.Price).ToList()[_newIndex].Modello;
					_menu.UpdateDescription();
				}
			};
			Brac.OnItemSelect += (_menu, _listItem, _listIndex) =>
			{
				if (Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Bracciali == Accessorio.Bracciali.OrderBy(x => x.Price).ToList()[_listIndex].Modello && Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Bracciali == -1)
				{
					HUD.ShowNotification("Non puoi rimuovere 2 volte un bracciale!!", true);
				}
				else if (Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Bracciali == Accessorio.Bracciali.OrderBy(x => x.Price).ToList()[_listIndex].Modello && Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Bracciali != -1)
				{
					HUD.ShowNotification("Non puoi acquistare di nuovo il braccialetto che hai già!");
				}
				else
				{
					int val = Accessorio.Bracciali.OrderBy(x => x.Price).ToList()[_listIndex].Price;
					if (Game.Player.GetPlayerData().Money >= val)
					{
						BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
						Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Bracciali = Accessorio.Bracciali.OrderBy(x => x.Price).ToList()[_listIndex].Modello;
						Game.Player.GetPlayerData().CurrentChar.dressing.PropTextures.Bracciali = 0;
						Game.Player.GetPlayerData().CurrentChar.dressing.Name = null;
						Game.Player.GetPlayerData().CurrentChar.dressing.Description = null;
						BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Game.Player.GetPlayerData().CurrentChar.dressing.Serialize());
						HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
					}
					else
					{
						if (Game.Player.GetPlayerData().Bank >= val)
						{
							BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
							Game.Player.GetPlayerData().CurrentChar.dressing.PropIndices.Bracciali = Accessorio.Bracciali.OrderBy(x => x.Price).ToList()[_listIndex].Modello;
							Game.Player.GetPlayerData().CurrentChar.dressing.PropTextures.Bracciali = 0;
							Game.Player.GetPlayerData().CurrentChar.dressing.Name = null;
							Game.Player.GetPlayerData().CurrentChar.dressing.Description = null;
							BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Game.Player.GetPlayerData().CurrentChar.dressing.Serialize());
							HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, con carta di credito");
						}
						else
						{
							HUD.ShowNotification("Non hai i soldi per questo abito!", NotificationColor.Red, true);
						}
					}
				}

			};

			Borse.OnMenuOpen += async (menu) =>
			{
				float newheading = Game.PlayerPed.Heading - 180f;
				Game.PlayerPed.Task.AchieveHeading(newheading, 1000);
				PointCamAtPedBone(NegozioAbitiClient.camm.Handle, PlayerPedId(), 24818, 0.0f, 0.0f, 0.0f, true);
			};
			Borse.OnMenuClose += async (menu) =>
			{
				PointCamAtPedBone(NegozioAbitiClient.camm.Handle, PlayerPedId(), 24818, 0.0f, 0.0f, 0.0f, true);
				float newheading = Game.PlayerPed.Heading + 180f;
				Game.PlayerPed.Task.AchieveHeading(newheading, 1000);
				await UpdateDress(Game.Player.GetPlayerData().CurrentChar.dressing);
			};
			Orecc.OnMenuOpen += async (menu) =>
			{
				ClearPedProp(PlayerPedId(), 2);
				PointCamAtPedBone(NegozioAbitiClient.camm.Handle, PlayerPedId(), 19336, 0.0f, 0.0f, 0.0f, true);
				do
				{
					await BaseScript.Delay(0);
					fov -= .7f;
					if (fov < 15f)
					{
						fov = 15f;
					}

					NegozioAbitiClient.camm.FieldOfView = fov;
				} while (fov > 15f);
				Game.PlayerPed.Task.LookAt(new Vector3(NegozioAbitiClient.camm.Position.X + 5f, NegozioAbitiClient.camm.Position.Y, NegozioAbitiClient.camm.Position.Z));
			};
			Orecc.OnMenuClose += async (menu) =>
			{
				PointCamAtPedBone(NegozioAbitiClient.camm.Handle, PlayerPedId(), 24818, 0.0f, 0.0f, 0.0f, true);
				do
				{
					await BaseScript.Delay(0);
					fov += .7f;
					if (fov > 45f)
					{
						fov = 45f;
					}

					NegozioAbitiClient.camm.FieldOfView = fov;
				} while (fov < 45f);
				await UpdateDress(Game.Player.GetPlayerData().CurrentChar.dressing);
				Game.PlayerPed.Task.LookAt(NegozioAbitiClient.camm.Position);
			};

			/*			Polso.OnMenuOpen += async (menu) =>
						{
							PointCamAtPedBone(NegozioAbitiClient.camm.Handle, PlayerPedId(), 24818, 0.0f, 0.0f, 0.0f, true);
							do
							{
								await BaseScript.Delay(0);
								fov -= .7f;
								if (fov < 15f)
									fov = 15f;
								NegozioAbitiClient.camm.FieldOfView = fov;
							} while (fov > 15f);
						};

						Orol.OnMenuOpen += async (menu) =>
						{
							StartAnim("anim@random@shop_clothes@watches", "base");
							PointCamAtPedBone(NegozioAbitiClient.camm.Handle, PlayerPedId(), 60309, 0.0f, 0.0f, 0.0f, true);
						};
			*/
			Orol.OnMenuOpen += async (menu) =>
			{
				StartAnim("anim@random@shop_clothes@watches", "base");
				PointCamAtPedBone(NegozioAbitiClient.camm.Handle, PlayerPedId(), 60309, 0.0f, 0.0f, 0.0f, true);
				do
				{
					await BaseScript.Delay(0);
					fov -= .7f;
					if (fov < 15f)
						fov = 15f;
					NegozioAbitiClient.camm.FieldOfView = fov;
				} while (fov > 15f);
			};

			Orol.OnMenuClose += async (menu) =>
			{
				await BaseScript.Delay(200);
				for (int i = 0; i < SubMenusPolso.Count; i++)
				{
					if (SubMenusPolso[i].Visible)
						return;
				}
				PointCamAtPedBone(NegozioAbitiClient.camm.Handle, PlayerPedId(), 24818, 0.0f, 0.0f, 0.0f, true);
				await UpdateDress(Game.Player.GetPlayerData().CurrentChar.dressing);
				StartAnim(anim, "try_shirt_base");
			};
			Brac.OnMenuOpen += async (menu) =>
			{
				PointCamAtPedBone(NegozioAbitiClient.camm.Handle, PlayerPedId(), 28422, 0.0f, 0.0f, 0.0f, true);
			};
			Brac.OnMenuClose += async (menu) =>
			{
				PointCamAtPedBone(NegozioAbitiClient.camm.Handle, PlayerPedId(), 24818, 0.0f, 0.0f, 0.0f, true);
				await UpdateDress(Game.Player.GetPlayerData().CurrentChar.dressing);
				StartAnim(anim, "try_shirt_base");
			};
			Polso.OnMenuClose += async (menu) =>
			{
				PointCamAtPedBone(NegozioAbitiClient.camm.Handle, PlayerPedId(), 24818, 0.0f, 0.0f, 0.0f, true);
				await BaseScript.Delay(100);
				for (int i = 0; i < SubMenusPolso.Count; i++) if (SubMenusPolso[i].Visible) return;
				do
				{
					await BaseScript.Delay(0);
					fov += .7f;
					if (fov > 45f)
						fov = 45f;
					NegozioAbitiClient.camm.FieldOfView = fov;
				} while (fov < 45f);
				StartAnim(anim, "try_shirt_base");
				await UpdateDress(Game.Player.GetPlayerData().CurrentChar.dressing);
			};
			MenuAccessori.OnMenuClose += async (_menu) =>
			{
				await BaseScript.Delay(200);
				for (int i = 0; i < SubMenusCapp.Count; i++) if (SubMenusCapp[i].Visible) return;
				for (int i = 0; i < SubMenusPolso.Count; i++) if (SubMenusPolso[i].Visible) return;
				if (Borse.Visible || Orecc.Visible || Brac.Visible || Polso.Visible || Orol.Visible || Capp.Visible || Orologino.Visible)
					return;
				await UpdateDress(Game.Player.GetPlayerData().CurrentChar.dressing);
				NegozioAbitiClient.Esci();
				AccessoriAttivo = false;
				Client.Instance.RemoveTick(CameraAcc);
			};
			MenuAccessori.Visible = true;
			Client.Instance.AddTick(CameraAcc);
		}
		#endregion

		static private async Task CameraVest()
		{
			if (_menuVestiti.Any(o => o.Visible))
			{
				if (Input.IsControlPressed(Control.FrontendLt))
				{
					fov -= .7f;
					if (fov <= 23f)
						fov = 23f;
					NegozioAbitiClient.camm.FieldOfView = fov;
				}
				else if (Input.IsControlJustReleased(Control.FrontendLt))
				{
					do
					{
						await BaseScript.Delay(0);
						fov += .7f;
						if (fov >= 45f)
							fov = 45f;
						NegozioAbitiClient.camm.FieldOfView = fov;
					} while ((fov != 23f) && (!Input.IsControlPressed(Control.FrontendLt)));
				}
			}
		}

		static private async Task CameraAcc()
		{
			if (Orecc.Visible || SubMenusPolso.Any(o => o.Visible))
			{
				if (Input.IsControlPressed(Control.FrontendLt))
				{
					fov -= .7f;
					if (fov <= 5.0f)
						fov = 5.0f;
					NegozioAbitiClient.camm.FieldOfView = fov;
				}
				else if (Input.IsControlJustReleased(Control.FrontendLt))
				{
					do
					{
						await BaseScript.Delay(0);
						fov += .7f;
						if (fov >= 15f)
							fov = 15f;
						NegozioAbitiClient.camm.FieldOfView = fov;
					} while ((fov != 15f) && (!Input.IsControlPressed(Control.FrontendLt)));
				}
			}
			else if (Borse.Visible)
			{
				if (Input.IsControlPressed(Control.FrontendLt))
				{
					fov -= .7f;
					if (fov <= 23f)
						fov = 23f;
					NegozioAbitiClient.camm.FieldOfView = fov;
				}
				else if (Input.IsControlJustReleased(Control.FrontendLt))
				{
					do
					{
						await BaseScript.Delay(0);
						fov += .7f;
						if (fov >= 45f)
							fov = 45f;
						NegozioAbitiClient.camm.FieldOfView = fov;
					} while ((fov != 23f) && (!Input.IsControlPressed(Control.FrontendLt)));
				}
			}
			else if (NegozioAbitiClient.camm.IsActive)
			{
				if (Input.IsControlPressed(Control.FrontendLt))
				{
					fov -= 0.7f;
					if (fov <= 23.0f)
						fov = 23.0f;
					NegozioAbitiClient.camm.FieldOfView = fov;
				}
				else if (Input.IsControlJustReleased(Control.FrontendLt))
				{
					do
					{
						await BaseScript.Delay(0);
						fov += 0.7f;
						if (fov >= 45.0f)
							fov = 45.0f;
						NegozioAbitiClient.camm.FieldOfView = fov;
					} while ((fov != 45.0f) && (!Input.IsControlPressed(Control.FrontendLt)));
				}
			}
			await Task.FromResult(0);
		}
	}
}
