﻿using CitizenFX.Core;
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
			Cache.PlayerPed.BlockPermanentEvents = true;
			await Cache.PlayerPed.Task.PlayAnimation(lib, anim, 8f, -8f, -1, AnimationFlags.Loop, 0);
		}

		static private async void StartAnimN(string lib, string anim)
		{
			Cache.PlayerPed.BlockPermanentEvents = true;
			await Cache.PlayerPed.Task.PlayAnimation(lib, anim, 4.0f, -2.0f, -1, AnimationFlags.None, 0);
		}

		static private void StartScenario(string anim)
		{
			Cache.PlayerPed.Task.StartScenario(anim, Cache.Char.posizione.ToVector3());
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
			Cache.PlayerPed.BlockPermanentEvents = true;
			SetPedAlternateMovementAnim(Cache.PlayerPed.Handle, 0, anim, "try_shirt_base", 4.0f, true);
			await Cache.PlayerPed.Task.PlayAnimation(anim, "try_shirt_base", 8f, -8f, -1, AnimationFlags.Loop, 0);
			var completi = Completi.OrderBy(x => x.Price).ToList();
			_menuVestiti.Add(MenuVest);
			for (int i = 0; i < completi.Count; i++)
			{
				UIMenuItem vest = new UIMenuItem(completi[i].Name, completi[i].Description);
				MenuVest.AddItem(vest);
				if (Cache.Char.Money >= completi[i].Price)
				{
					vest.SetRightLabel("~g~$" + completi[i].Price);
				}
				else
				{
					if (Cache.Char.Bank >= completi[i].Price)
					{
						vest.SetRightLabel("~g~$" + completi[i].Price);
					}
					else
					{
						vest.SetRightLabel("~r~$" + completi[i].Price);
					}
				}
				if (completi[i].Name == Cache.Char.CurrentChar.dressing.Name)
				{
					vest.SetRightBadge(BadgeStyle.Clothes); // cambiare con la collezione di abiti
					ciao = vest;
				}
			}
			MenuVest.OnIndexChange += async (sender, index) =>
			{
				string random = GetRandomAnim(anim, false);
				await UpdateDress(completi[index]);
				await Cache.PlayerPed.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
			};
			MenuVest.OnItemSelect += async (_menu, _item, _index) =>
			{
				if (Cache.Char.CurrentChar.dressing.Name == completi[_index].Name && Cache.Char.CurrentChar.dressing.Description == completi[_index].Description)
				{
					HUD.ShowNotification("Possiedi già quest'abito!", true);
				}
				else
				{
					if (Cache.Char.Money >= completi[_index].Price)
					{
						BaseScript.TriggerServerEvent("lprp:abiti:compra", completi[_index].Price, 1);
						BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", completi[_index].Serialize());
						Cache.Char.CurrentChar.dressing = new Shared.Dressing(completi[_index].Name, completi[_index].Description, completi[_index].ComponentDrawables, completi[_index].ComponentTextures, completi[_index].PropIndices, completi[_index].PropTextures);
						ciao.SetRightBadge(BadgeStyle.None);
						ciao = _item;
						ciao.SetRightBadge(BadgeStyle.Clothes);
						HUD.ShowNotification("Hai speso ~g~" + completi[_index].Price + "$~w~, in contanti");
					}
					else
					{
						if (Cache.Char.Bank >= completi[_index].Price)
						{
							BaseScript.TriggerServerEvent("lprp:abiti:compra", completi[_index].Price, 2);
							BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", completi[_index].Serialize());
							Cache.Char.CurrentChar.dressing = new Shared.Dressing(completi[_index].Name, completi[_index].Description, completi[_index].ComponentDrawables, completi[_index].ComponentTextures, completi[_index].PropIndices, completi[_index].PropTextures);
							ciao.SetRightBadge(BadgeStyle.None);
							ciao = _item;
							ciao.SetRightBadge(BadgeStyle.Clothes);
							HUD.ShowNotification("Hai speso ~g~" + completi[_index].Price + "$~w~, con carta di credito");
						}
						else
						{
							HUD.ShowNotification("Non hai i soldi per questo abito!", NotificationColor.Red, true);
						}
					}
				}
			};
			MenuVest.OnMenuStateChanged += async (oldmenu, _menu, state) =>
			{
				if (state == MenuState.Closed)
				{
					await UpdateDress(Cache.Char.CurrentChar.dressing);
					NegozioAbitiClient.Esci();
					Client.Instance.RemoveTick(CameraVest);
				}
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
			Cache.PlayerPed.BlockPermanentEvents = true;
			SetPedAlternateMovementAnim(Cache.PlayerPed.Handle, 0, anim, "try_trousers_base", 4.0f, true);
			await Cache.PlayerPed.Task.PlayAnimation(anim, "try_trousers_base", 8f, -8f, -1, AnimationFlags.Loop, 0);
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

					if (Cache.Char.Money >= money)
					{
						pant.SetRightLabel("~g~$" + money);
					}
					else
					{
						if (Cache.Char.Bank >= money)
						{
							pant.SetRightLabel("~g~$" + money);
						}
						else
						{
							pant.SetRightLabel("~r~$" + money);
						}
					}
					if (Cache.Char.CurrentChar.dressing.ComponentDrawables.Pantaloni == v.Modello && Cache.Char.CurrentChar.dressing.ComponentTextures.Pantaloni == texture)
					{
						Pant.ParentItem.SetRightBadge(BadgeStyle.Clothes);
						ciao = Pant.ParentItem;
						pant.SetRightBadge(BadgeStyle.Clothes); // cambiare con la collezione di abiti
						ciaone = pant;
					}
				}
				Pant.OnMenuStateChanged += async (oldmenu, newmenu, state) =>
				{
					if (state == MenuState.ChangeForward)
					{
						SetPedComponentVariation(PlayerPedId(), 4, v.Modello, v.Text[0], 2);
						string random = GetRandomAnim(anim, false);
						SetPedComponentVariation(PlayerPedId(), 4, v.Modello, v.Text[0], 2);
						await Cache.PlayerPed.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
						mod = v.Modello;
						text = v.Text[0];
					}
					else if (state == MenuState.ChangeBackward)
					{
						await UpdateDress(Cache.Char.CurrentChar.dressing);
					}
				};
				Pant.OnIndexChange += async (sender, index) =>
				{
					string random = GetRandomAnim(anim, false);
					SetPedComponentVariation(PlayerPedId(), 4, v.Modello, v.Text[index], 2);
					await Cache.PlayerPed.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
					mod = v.Modello;
					text = v.Text[index];
				};
				Pant.OnItemSelect += async (_menu, _item, _index) =>
				{
					if (Cache.Char.CurrentChar.dressing.ComponentDrawables.Pantaloni == mod && Cache.Char.CurrentChar.dressing.ComponentTextures.Pantaloni == text)
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

						if (Cache.Char.Money >= val)
						{
							BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
							Cache.Char.CurrentChar.dressing.ComponentDrawables.Pantaloni = mod;
							Cache.Char.CurrentChar.dressing.ComponentTextures.Pantaloni = text;
							Cache.Char.CurrentChar.dressing.Name = null;
							Cache.Char.CurrentChar.dressing.Description = null;
							BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Cache.Char.CurrentChar.dressing.Serialize());
							ciao.SetRightBadge(BadgeStyle.None);
							ciao = MenuPant.MenuItems[MenuPant.CurrentSelection];
							ciao.SetRightBadge(BadgeStyle.Clothes);
							ciaone.SetRightBadge(BadgeStyle.None);
							ciaone = _item;
							ciaone.SetRightBadge(BadgeStyle.Clothes);
							HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
						}
						else
						{
							if (Cache.Char.Bank >= val)
							{
								BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
								Cache.Char.CurrentChar.dressing.ComponentDrawables.Pantaloni = mod;
								Cache.Char.CurrentChar.dressing.ComponentTextures.Pantaloni = text;
								Cache.Char.CurrentChar.dressing.Name = null;
								Cache.Char.CurrentChar.dressing.Description = null;
								BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Cache.Char.CurrentChar.dressing.Serialize());
								ciao.SetRightBadge(BadgeStyle.None);
								ciao = MenuPant.MenuItems[MenuPant.CurrentSelection];
								ciao.SetRightBadge(BadgeStyle.Clothes);
								ciaone.SetRightBadge(BadgeStyle.None);
								ciaone = _item;
								ciaone.SetRightBadge(BadgeStyle.Clothes);
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
			MenuPant.OnMenuStateChanged += async (oldmenu, _menu, state) =>
			{
				if (state == MenuState.Closed)
				{
					await BaseScript.Delay(100);
					for (int i = 0; i < _menuVestiti.Count; i++) if (_menuVestiti[i].Visible) return;
					await UpdateDress(Cache.Char.CurrentChar.dressing);
					NegozioAbitiClient.Esci();
					Client.Instance.RemoveTick(CameraVest);
				}
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
			Cache.PlayerPed.BlockPermanentEvents = true;
			SetPedAlternateMovementAnim(Cache.PlayerPed.Handle, 0, anim, "try_shoes_base", 4.0f, true);
			await Cache.PlayerPed.Task.PlayAnimation(anim, "try_shoes_base", 8f, -8f, -1, AnimationFlags.Loop, 0);
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

					if (Cache.Char.Money >= money)
					{
						pant.SetRightLabel("~g~$" + money);
					}
					else
					{
						if (Cache.Char.Bank >= money)
						{
							pant.SetRightLabel("~g~$" + money);
						}
						else
						{
							pant.SetRightLabel("~r~$" + money);
						}
					}
					if (Cache.Char.CurrentChar.dressing.ComponentDrawables.Scarpe == v.Modello && Cache.Char.CurrentChar.dressing.ComponentTextures.Scarpe == texture)
					{
						Scarp.ParentItem.SetRightBadge(BadgeStyle.Clothes);
						ciao = Scarp.ParentItem;
						pant.SetRightBadge(BadgeStyle.Clothes); // cambiare con la collezione di abiti
						ciaone = pant;
					}
				}
				Scarp.OnMenuStateChanged += async (oldmenu, newmenu, state) =>
				{
					if (state == MenuState.ChangeForward)
					{
						string random = GetRandomAnim(anim, false);
						SetPedComponentVariation(PlayerPedId(), 6, v.Modello, v.Text[0], 2);
						await Cache.PlayerPed.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
						mod = v.Modello;
						text = v.Text[0];
					}
					else if (state == MenuState.ChangeBackward)
						await UpdateDress(Cache.Char.CurrentChar.dressing);
				};
				Scarp.OnIndexChange += async (sender, index) =>
				{
					string random = GetRandomAnim(anim, false);
					SetPedComponentVariation(PlayerPedId(), 6, v.Modello, v.Text[index], 2);
					await Cache.PlayerPed.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
					mod = v.Modello;
					text = v.Text[index];
				};
				Scarp.OnItemSelect += async (_menu, _item, _index) =>
				{
					if (Cache.Char.CurrentChar.dressing.ComponentDrawables.Scarpe == mod && Cache.Char.CurrentChar.dressing.ComponentTextures.Scarpe == text)
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

						if (Cache.Char.Money >= val)
						{
							BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
							Cache.Char.CurrentChar.dressing.ComponentDrawables.Scarpe = mod;
							Cache.Char.CurrentChar.dressing.ComponentTextures.Scarpe = text;
							Cache.Char.CurrentChar.dressing.Name = null;
							Cache.Char.CurrentChar.dressing.Description = null;
							BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Cache.Char.CurrentChar.dressing.Serialize());
							ciao.SetRightBadge(BadgeStyle.None);
							ciao = MenuScarpe.MenuItems[MenuScarpe.CurrentSelection];
							ciao.SetRightBadge(BadgeStyle.Clothes);
							ciaone.SetRightBadge(BadgeStyle.None);
							ciaone = _item;
							ciaone.SetRightBadge(BadgeStyle.Clothes);
							HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
						}
						else
						{
							if (Cache.Char.Bank >= val)
							{
								BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
								Cache.Char.CurrentChar.dressing.ComponentDrawables.Scarpe = mod;
								Cache.Char.CurrentChar.dressing.ComponentTextures.Scarpe = text;
								Cache.Char.CurrentChar.dressing.Name = null;
								Cache.Char.CurrentChar.dressing.Description = null;
								BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Cache.Char.CurrentChar.dressing.Serialize());
								ciao.SetRightBadge(BadgeStyle.None);
								ciao = MenuScarpe.MenuItems[MenuScarpe.CurrentSelection];
								ciao.SetRightBadge(BadgeStyle.Clothes);
								ciaone.SetRightBadge(BadgeStyle.None);
								ciaone = _item;
								ciaone.SetRightBadge(BadgeStyle.Clothes);
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
			MenuScarpe.OnMenuStateChanged += async (oldmenu, _menu, state) =>
			{
				if (state == MenuState.Closed)
				{
					await BaseScript.Delay(100);
					for (int i = 0; i < _menuVestiti.Count; i++) if (_menuVestiti[i].Visible) return;
					await UpdateDress(Cache.Char.CurrentChar.dressing);
					NegozioAbitiClient.Esci();
					Client.Instance.RemoveTick(CameraVest);
				}
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
			Cache.PlayerPed.BlockPermanentEvents = true;
			SetPedAlternateMovementAnim(Cache.PlayerPed.Handle, 0, anim, "Try_Glasses_Base", 4.0f, true);
			await Cache.PlayerPed.Task.PlayAnimation(anim, "Try_Glasses_Base", 8f, -8f, -1, AnimationFlags.Loop, 0);
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

					if (Cache.Char.Money >= money)
					{
						pant.SetRightLabel("~g~$" + money);
					}
					else
					{
						if (Cache.Char.Bank >= money)
						{
							pant.SetRightLabel("~g~$" + money);
						}
						else
						{
							pant.SetRightLabel("~r~$" + money);
						}
					}
					if (Cache.Char.CurrentChar.dressing.PropIndices.Orecchie == v.Modello && Cache.Char.CurrentChar.dressing.PropTextures.Orecchie == texture)
					{
						Scarp.ParentItem.SetRightBadge(BadgeStyle.Clothes);
						ciao = Scarp.ParentItem;
						pant.SetRightBadge(BadgeStyle.Clothes); // cambiare con la collezione di abiti
						ciaone = pant;
					}
				}
				Scarp.OnMenuStateChanged += async (oldmenu, newmenu, state) =>
				{
					if (state == MenuState.ChangeForward)
					{
						string random = GetRandomAnim(anim, false);
						SetPedPropIndex(PlayerPedId(), 1, v.Modello, v.Text[0], false);
						await Cache.PlayerPed.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
						mod = v.Modello;
						text = v.Text[0];
					}
					else if (state == MenuState.ChangeBackward)
						await UpdateDress(Cache.Char.CurrentChar.dressing);
				};
				Scarp.OnIndexChange += async (menu, index) =>
				{
					string random = GetRandomAnim(anim, false);
					SetPedPropIndex(PlayerPedId(), 1, v.Modello, v.Text[index], false);
					await Cache.PlayerPed.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
					mod = v.Modello;
					text = v.Text[index];
				};
				Scarp.OnItemSelect += async (_menu, _item, _index) =>
				{
					if (Cache.Char.CurrentChar.dressing.PropIndices.Orecchie == mod && Cache.Char.CurrentChar.dressing.PropTextures.Orecchie == text)
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

						if (Cache.Char.Money >= val)
						{
							BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
							Cache.Char.CurrentChar.dressing.PropIndices.Orecchie = mod;
							Cache.Char.CurrentChar.dressing.PropTextures.Orecchie = text;
							Cache.Char.CurrentChar.dressing.Name = null;
							Cache.Char.CurrentChar.dressing.Description = null;
							BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Cache.Char.CurrentChar.dressing.Serialize());
							ciao.SetRightBadge(BadgeStyle.None);
							ciao = MenuOcchiali.MenuItems[MenuOcchiali.CurrentSelection];
							ciao.SetRightBadge(BadgeStyle.Clothes);
							ciaone.SetRightBadge(BadgeStyle.None);
							ciaone = _item;
							ciaone.SetRightBadge(BadgeStyle.Clothes);
							HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
						}
						else
						{
							if (Cache.Char.Bank >= val)
							{
								BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
								Cache.Char.CurrentChar.dressing.PropIndices.Orecchie = mod;
								Cache.Char.CurrentChar.dressing.PropTextures.Orecchie = text;
								Cache.Char.CurrentChar.dressing.Name = null;
								Cache.Char.CurrentChar.dressing.Description = null;
								BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Cache.Char.CurrentChar.dressing.Serialize());
								ciao.SetRightBadge(BadgeStyle.None);
								ciao = MenuOcchiali.MenuItems[MenuOcchiali.CurrentSelection];
								ciao.SetRightBadge(BadgeStyle.Clothes);
								ciaone.SetRightBadge(BadgeStyle.None);
								ciaone = _item;
								ciaone.SetRightBadge(BadgeStyle.Clothes);
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
			MenuOcchiali.OnMenuStateChanged += async (oldmenu, _menu, state) =>
			{
				if (state == MenuState.Closed)
				{
					await BaseScript.Delay(100);
					for (int i = 0; i < _menuVestiti.Count; i++) if (_menuVestiti[i].Visible) return;
					await UpdateDress(Cache.Char.CurrentChar.dressing);
					NegozioAbitiClient.Esci();
					Client.Instance.RemoveTick(CameraVest);
				}
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
			Cache.PlayerPed.BlockPermanentEvents = true;
			SetPedAlternateMovementAnim(Cache.PlayerPed.Handle, 0, anim, "try_shirt_base", 4.0f, true);
			await Cache.PlayerPed.Task.PlayAnimation(anim, "try_shirt_base", 8f, -8f, -1, AnimationFlags.Loop, 0);
			int money = 0;
			int IntorecAtt = Cache.Char.CurrentChar.skin.ears.style;
			int IntcoloOreccAtt = Cache.Char.CurrentChar.skin.ears.color;
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
				if (Cache.Char.Money >= money)
				{
					bors.SetRightLabel("~g~$" + money);
				}
				else
				{
					if (Cache.Char.Bank >= money)
					{
						bors.SetRightLabel("~g~$" + money);
					}
					else
					{
						bors.SetRightLabel("~r~$" + money);
					}
				}
				Borse.AddItem(bors);
				if (Cache.Char.CurrentChar.dressing.ComponentDrawables.Borsa_Paracadute == borsa.Modello)
				{
					bors.SetRightBadge(BadgeStyle.Clothes);
					BorsAtt = bors;
				}
			}
			Borse.OnIndexChange += async (_menu, index) =>
			{
				IntBors = Accessorio.Borse[index].Modello;
				string random = GetRandomAnim(anim, false);
				SetPedComponentVariation(PlayerPedId(), 5, IntBors, 0, 2);
				await Cache.PlayerPed.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
			};
			Borse.OnItemSelect += async (_menu, _item, _index) =>
			{
				if (Cache.Char.CurrentChar.dressing.ComponentDrawables.Borsa_Paracadute == IntBors)
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

					if (Cache.Char.Money >= val)
					{
						BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
						Cache.Char.CurrentChar.dressing.ComponentDrawables.Borsa_Paracadute = IntBors;
						Cache.Char.CurrentChar.dressing.ComponentTextures.Borsa_Paracadute = 0;
						Cache.Char.CurrentChar.dressing.Name = null;
						Cache.Char.CurrentChar.dressing.Description = null;
						BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Cache.Char.CurrentChar.dressing.Serialize());
						BorsAtt.SetRightBadge(BadgeStyle.None);
						BorsAtt = _menu.MenuItems[_menu.CurrentSelection];
						BorsAtt.SetRightBadge(BadgeStyle.Clothes);
						HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
					}
					else
					{
						if (Cache.Char.Bank >= val)
						{
							BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
							Cache.Char.CurrentChar.dressing.ComponentDrawables.Borsa_Paracadute = IntBors;
							Cache.Char.CurrentChar.dressing.ComponentTextures.Borsa_Paracadute = 0;
							Cache.Char.CurrentChar.dressing.Name = null;
							Cache.Char.CurrentChar.dressing.Description = null;
							BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Cache.Char.CurrentChar.dressing.Serialize());
							BorsAtt.SetRightBadge(BadgeStyle.None);
							BorsAtt = _menu.MenuItems[_menu.CurrentSelection];
							BorsAtt.SetRightBadge(BadgeStyle.Clothes);
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
				if (Cache.Char.CurrentChar.dressing.PropIndices.Cappelli_Maschere == -1)
				{
					CappRim.SetRightBadge(BadgeStyle.Clothes);
					CappAtt = CappRim;
				}
				if (Cache.Char.CurrentChar.dressing.PropIndices.Cappelli_Maschere != -1 && Cache.Char.CurrentChar.dressing.PropIndices.Cappelli_Maschere == cappellino.Modello)
				{
					capelino.SetRightBadge(BadgeStyle.Clothes);
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

					if (Cache.Char.Money >= money)
					{
						newCap.SetRightLabel("~g~$" + money);
					}
					else
					{
						if (Cache.Char.Bank >= money)
						{
							newCap.SetRightLabel("~g~$" + money);
						}
						else
						{
							newCap.SetRightLabel("~r~$" + money);
						}
					}
					if (Cache.Char.CurrentChar.dressing.PropIndices.Cappelli_Maschere == cappellino.Modello && Cache.Char.CurrentChar.dressing.PropTextures.Cappelli_Maschere == texture)
					{
						newCap.SetRightBadge(BadgeStyle.Clothes);
						CappAtt1 = newCap;
					}
				}
				Capelino.OnIndexChange += async (_menu, _newIndex) =>
				{
					string random = GetRandomAnim(anim, false);
					SetPedPropIndex(PlayerPedId(), 0, cappellino.Modello, cappellino.Text[_newIndex], false);
					IntCappAtt = cappellino.Text[_newIndex];
					IntCappAttMod = cappellino.Text[_newIndex];
					await Cache.PlayerPed.Task.PlayAnimation(anim, random, 4f, -2f, -1, AnimationFlags.None, 0);
				};
				Capelino.OnItemSelect += async (_menu, _item, _index) =>
				{
					if (Cache.Char.CurrentChar.dressing.PropIndices.Cappelli_Maschere == IntCappAtt && Cache.Char.CurrentChar.dressing.PropTextures.Cappelli_Maschere == IntCappAttMod)
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

						if (Cache.Char.Money >= val)
						{
							BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
							Cache.Char.CurrentChar.dressing.PropIndices.Cappelli_Maschere = IntCappAtt;
							Cache.Char.CurrentChar.dressing.PropTextures.Cappelli_Maschere = IntCappAttMod;
							Cache.Char.CurrentChar.dressing.Name = null;
							Cache.Char.CurrentChar.dressing.Description = null;
							BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Cache.Char.CurrentChar.dressing.Serialize());
							CappAtt.SetRightBadge(BadgeStyle.None);
							CappAtt = Capp.MenuItems[Capp.CurrentSelection];
							CappAtt.SetRightBadge(BadgeStyle.Clothes);
							CappAtt1.SetRightBadge(BadgeStyle.None);
							CappAtt1 = _menu.MenuItems[_menu.CurrentSelection];
							CappAtt1.SetRightBadge(BadgeStyle.Clothes);
							HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
						}
						else
						{
							if (Cache.Char.Bank >= val)
							{
								BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
								Cache.Char.CurrentChar.dressing.PropIndices.Cappelli_Maschere = IntCappAtt;
								Cache.Char.CurrentChar.dressing.PropTextures.Cappelli_Maschere = IntCappAttMod;
								Cache.Char.CurrentChar.dressing.Name = null;
								Cache.Char.CurrentChar.dressing.Description = null;
								BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Cache.Char.CurrentChar.dressing.Serialize());
								CappAtt.SetRightBadge(BadgeStyle.None);
								CappAtt = Capp.MenuItems[Capp.CurrentSelection];
								CappAtt.SetRightBadge(BadgeStyle.Clothes);
								CappAtt1.SetRightBadge(BadgeStyle.None);
								CappAtt1 = _menu.MenuItems[_menu.CurrentSelection];
								CappAtt1.SetRightBadge(BadgeStyle.Clothes);
								HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, con carta di credito");
							}
							else
							{
								HUD.ShowNotification("Non hai i soldi per questo abito!", NotificationColor.Red, true);
							}
						}

					}
				};
				Capelino.OnMenuStateChanged += async (oldmenu, newmenu, state) =>
				{
					if (state == MenuState.ChangeBackward && newmenu == Capp)
					{
						PointCamAtPedBone(NegozioAbitiClient.camm.Handle, PlayerPedId(), 24818, 0.0f, 0.0f, 0.0f, true);
						await UpdateDress(Cache.Char.CurrentChar.dressing);
					}
				};
			}
			Capp.OnItemSelect += async (_menu, _item, _index) =>
			{
				if (_item == CappRim)
				{
					if (Cache.Char.CurrentChar.dressing.PropIndices.Cappelli_Maschere == -1)
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

						if (Cache.Char.Money >= val)
						{
							BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
							Cache.Char.CurrentChar.dressing.PropIndices.Cappelli_Maschere = -1;
							Cache.Char.CurrentChar.dressing.PropTextures.Cappelli_Maschere = -1;
							Cache.Char.CurrentChar.dressing.Name = null;
							Cache.Char.CurrentChar.dressing.Description = null;
							BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Cache.Char.CurrentChar.dressing.Serialize());
							CappAtt.SetRightBadge(BadgeStyle.None);
							CappAtt = _menu.MenuItems[_menu.CurrentSelection];
							CappAtt.SetRightBadge(BadgeStyle.Clothes);
							HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
						}
						else
						{
							if (Cache.Char.Bank >= val)
							{
								BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
								Cache.Char.CurrentChar.dressing.PropIndices.Cappelli_Maschere = -1;
								Cache.Char.CurrentChar.dressing.PropTextures.Cappelli_Maschere = -1;
								Cache.Char.CurrentChar.dressing.Name = null;
								Cache.Char.CurrentChar.dressing.Description = null;
								BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Cache.Char.CurrentChar.dressing.Serialize());
								CappAtt.SetRightBadge(BadgeStyle.None);
								CappAtt = _menu.MenuItems[_menu.CurrentSelection];
								CappAtt.SetRightBadge(BadgeStyle.Clothes);
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
				if (Cache.Char.CurrentChar.dressing.PropIndices.Orecchie == Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_newIndex].Modello)
				{
//					_listItem.SetRightBadge(BadgeStyle.Clothes);
				}
				else
				{
//					_listItem.SetRightBadge(BadgeStyle.None);
				}

				_listItem.Description = Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_newIndex].Description + ", Prezzo: $" + Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_newIndex].Price;
				SetPedPropIndex(PlayerPedId(), 2, Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_newIndex].Modello, 0, false);
				IntorecAtt = Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_newIndex].Modello;
				_menu.UpdateDescription();
			};
			Orecc.OnItemSelect += (_menu, _listItem, _listIndex) =>
			{
				if (Cache.Char.CurrentChar.dressing.PropIndices.Orecchie == Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_listIndex].Modello && Cache.Char.CurrentChar.dressing.PropIndices.Orecchie == -1)
				{
					HUD.ShowNotification("Non puoi rimuovere 2 volte gli orecchini!!", true);
				}
				else if (Cache.Char.CurrentChar.dressing.PropIndices.Orecchie == Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_listIndex].Modello && Cache.Char.CurrentChar.dressing.PropIndices.Orecchie != -1)
				{
					HUD.ShowNotification("Non puoi acquistare di nuovo gli orecchini che hai già!");
				}
				else
				{
					int val = Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_listIndex].Price;
					if (Cache.Char.Money >= val)
					{
						BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
						Cache.Char.CurrentChar.dressing.PropIndices.Orecchie = Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_listIndex].Modello;
						Cache.Char.CurrentChar.dressing.PropTextures.Orecchie = 0;
						Cache.Char.CurrentChar.skin.ears.style = Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_listIndex].Modello;
						Cache.Char.CurrentChar.skin.ears.color = 0;
						Cache.Char.CurrentChar.dressing.Name = null;
						Cache.Char.CurrentChar.dressing.Description = null;
						BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Cache.Char.CurrentChar.dressing.Serialize());
						HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
					}
					else
					{
						if (Cache.Char.Bank >= val)
						{
							BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
							Cache.Char.CurrentChar.dressing.PropIndices.Orecchie = Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_listIndex].Modello;
							Cache.Char.CurrentChar.dressing.PropTextures.Orecchie = 0;
							Cache.Char.CurrentChar.skin.ears.style = Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_listIndex].Modello;
							Cache.Char.CurrentChar.skin.ears.color = 0;
							Cache.Char.CurrentChar.dressing.Name = null;
							Cache.Char.CurrentChar.dressing.Description = null;
							BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Cache.Char.CurrentChar.dressing.Serialize());
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
				if (Cache.Char.Money >= aurico.Price)
				{
					auricolari.SetRightLabel("~g~$" + aurico.Price);
				}
				else
				{
					if (Cache.Char.Bank >= aurico.Price)
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
					if (Cache.Char.CurrentChar.dressing.PropIndices.Orecchie == Accessorio.Testa.Auricolari.OrderBy(x => x.Price).ToList()[_index - 1].Modello && Cache.Char.CurrentChar.dressing.PropIndices.Orecchie == -1)
						HUD.ShowNotification("Non puoi rimuovere 2 volte l'auricolare!!", true);
					else if (Cache.Char.CurrentChar.dressing.PropIndices.Orecchie == Accessorio.Testa.Auricolari.OrderBy(x => x.Price).ToList()[_index - 1].Modello && Cache.Char.CurrentChar.dressing.PropIndices.Orecchie != -1)
						HUD.ShowNotification("Non puoi acquistare l'auricolare che hai già!");
					else
					{
						int val = Accessorio.Testa.Orecchini.OrderBy(x => x.Price).ToList()[_index - 1].Price;
						if (Cache.Char.Money >= val)
						{
							BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
							Cache.Char.CurrentChar.dressing.PropIndices.Orecchie = Accessorio.Testa.Auricolari.OrderBy(x => x.Price).ToList()[_index - 1].Modello;
							Cache.Char.CurrentChar.dressing.PropTextures.Orecchie = 0;
							Cache.Char.CurrentChar.skin.ears.style = Accessorio.Testa.Auricolari.OrderBy(x => x.Price).ToList()[_index - 1].Modello;
							Cache.Char.CurrentChar.skin.ears.color = 0;
							Cache.Char.CurrentChar.dressing.Name = null;
							Cache.Char.CurrentChar.dressing.Description = null;
							BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Cache.Char.CurrentChar.dressing.Serialize());
							HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
						}
						else
						{
							if (Cache.Char.Bank >= val)
							{
								BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
								Cache.Char.CurrentChar.dressing.PropIndices.Orecchie = Accessorio.Testa.Auricolari.OrderBy(x => x.Price).ToList()[_index - 1].Modello;
								Cache.Char.CurrentChar.dressing.PropTextures.Orecchie = 0;
								Cache.Char.CurrentChar.skin.ears.style = Accessorio.Testa.Auricolari.OrderBy(x => x.Price).ToList()[_index - 1].Modello;
								Cache.Char.CurrentChar.skin.ears.color = 0;
								Cache.Char.CurrentChar.dressing.Name = null;
								Cache.Char.CurrentChar.dressing.Description = null;
								BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Cache.Char.CurrentChar.dressing.Serialize());
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
					if (Cache.Char.Money >= orologio.Price)
					{
						NoOrol.SetRightLabel("~g~$" + orologio.Price);
					}
					else
					{
						if (Cache.Char.Bank >= orologio.Price)
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

					if (Cache.Char.CurrentChar.dressing.PropIndices.Orologi == orologio.Modello && Cache.Char.CurrentChar.dressing.PropTextures.Orologi == v)
					{
						if (orologio.Modello == -1)
						{
							NoOrol.SetRightBadge(BadgeStyle.Clothes);
							OrolAtt = NoOrol;
						}
						else
						{
							OrologinoItem.SetRightBadge(BadgeStyle.Clothes);
							OrolAtt = OrologinoItem;
						}
						NewOrol.SetRightBadge(BadgeStyle.Clothes);
						OrolMod = NewOrol;
					}

					if (Cache.Char.Money >= money)
					{
						NewOrol.SetRightLabel("~g~$" + money);
					}
					else
					{
						if (Cache.Char.Bank >= money)
						{
							NewOrol.SetRightLabel("~g~$" + money);
						}
						else
						{
							NewOrol.SetRightLabel("~r~$" + money);
						}
					}
					Orologino.OnMenuStateChanged += (oldmenu, newmenu, state) =>
					{
						if (state == MenuState.ChangeForward)
						{
							SetPedPropIndex(PlayerPedId(), 6, orologio.Modello, 0, false);
							IntOrolAtt = orologio.Modello;
							IntOrolMod = 0;
						}
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
					if (Cache.Char.CurrentChar.dressing.PropIndices.Orologi == IntOrolAtt && Cache.Char.CurrentChar.dressing.PropTextures.Orologi == IntOrolMod)
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

						if (Cache.Char.Money >= val)
						{
							BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
							Cache.Char.CurrentChar.dressing.PropIndices.Orologi = IntOrolAtt;
							Cache.Char.CurrentChar.dressing.PropTextures.Orologi = IntOrolMod;
							Cache.Char.CurrentChar.dressing.Description = null;
							OrolAtt.SetRightBadge(BadgeStyle.None);
							OrolAtt = Orol.MenuItems[Orol.CurrentSelection];
							OrolAtt.SetRightBadge(BadgeStyle.Clothes);
							OrolMod.SetRightBadge(BadgeStyle.None);
							OrolMod = _menu.MenuItems[_menu.CurrentSelection];
							OrolMod.SetRightBadge(BadgeStyle.Clothes);
							BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Cache.Char.CurrentChar.dressing.Serialize());
							if (val > 0)
							{
								HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
							}
						}
						else
						{
							if (Cache.Char.Bank >= val)
							{
								BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
								Cache.Char.CurrentChar.dressing.PropIndices.Orologi = IntOrolAtt;
								Cache.Char.CurrentChar.dressing.PropTextures.Orologi = IntOrolMod;
								OrolAtt.SetRightBadge(BadgeStyle.None);
								OrolAtt = Orol.MenuItems[Orol.CurrentSelection];
								OrolAtt.SetRightBadge(BadgeStyle.Clothes);
								OrolMod.SetRightBadge(BadgeStyle.None);
								OrolMod = _menu.MenuItems[_menu.CurrentSelection];
								OrolMod.SetRightBadge(BadgeStyle.Clothes);
								Cache.Char.CurrentChar.dressing.Name = null;
								Cache.Char.CurrentChar.dressing.Description = null;
								BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Cache.Char.CurrentChar.dressing.Serialize());
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
					if (Cache.Char.CurrentChar.dressing.PropIndices.Orologi == -1 && IntOrolAtt == -1)
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

						if (Cache.Char.Money >= val)
						{
							BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
							Cache.Char.CurrentChar.dressing.PropIndices.Orologi = -1;
							Cache.Char.CurrentChar.dressing.PropTextures.Orologi = -1;
							OrolAtt.SetRightBadge(BadgeStyle.None);
							OrolAtt = _menu.MenuItems[_menu.CurrentSelection];
							OrolAtt.SetRightBadge(BadgeStyle.Clothes);
							Cache.Char.CurrentChar.skin.ears.style = Accessorio.Orologi.OrderBy(x => x.Price).ToList()[_index].Modello;
							Cache.Char.CurrentChar.dressing.Description = null;
							BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Cache.Char.CurrentChar.dressing.Serialize());
							if (val > 0)
							{
								HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
							}
						}
						else
						{
							if (Cache.Char.Bank >= val)
							{
								BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
								Cache.Char.CurrentChar.dressing.PropIndices.Orologi = -1;
								Cache.Char.CurrentChar.dressing.PropTextures.Orologi = -1;
								OrolAtt.SetRightBadge(BadgeStyle.None);
								OrolAtt = _menu.MenuItems[_menu.CurrentSelection];
								OrolAtt.SetRightBadge(BadgeStyle.Clothes);
								Cache.Char.CurrentChar.dressing.Name = null;
								Cache.Char.CurrentChar.dressing.Description = null;
								BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Cache.Char.CurrentChar.dressing.Serialize());
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
				if (Cache.Char.CurrentChar.dressing.PropIndices.Bracciali == Accessorio.Bracciali.OrderBy(x => x.Price).ToList()[_listIndex].Modello && Cache.Char.CurrentChar.dressing.PropIndices.Bracciali == -1)
				{
					HUD.ShowNotification("Non puoi rimuovere 2 volte un bracciale!!", true);
				}
				else if (Cache.Char.CurrentChar.dressing.PropIndices.Bracciali == Accessorio.Bracciali.OrderBy(x => x.Price).ToList()[_listIndex].Modello && Cache.Char.CurrentChar.dressing.PropIndices.Bracciali != -1)
				{
					HUD.ShowNotification("Non puoi acquistare di nuovo il braccialetto che hai già!");
				}
				else
				{
					int val = Accessorio.Bracciali.OrderBy(x => x.Price).ToList()[_listIndex].Price;
					if (Cache.Char.Money >= val)
					{
						BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 1);
						Cache.Char.CurrentChar.dressing.PropIndices.Bracciali = Accessorio.Bracciali.OrderBy(x => x.Price).ToList()[_listIndex].Modello;
						Cache.Char.CurrentChar.dressing.PropTextures.Bracciali = 0;
						Cache.Char.CurrentChar.dressing.Name = null;
						Cache.Char.CurrentChar.dressing.Description = null;
						BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Cache.Char.CurrentChar.dressing.Serialize());
						HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, in contanti");
					}
					else
					{
						if (Cache.Char.Bank >= val)
						{
							BaseScript.TriggerServerEvent("lprp:abiti:compra", val, 2);
							Cache.Char.CurrentChar.dressing.PropIndices.Bracciali = Accessorio.Bracciali.OrderBy(x => x.Price).ToList()[_listIndex].Modello;
							Cache.Char.CurrentChar.dressing.PropTextures.Bracciali = 0;
							Cache.Char.CurrentChar.dressing.Name = null;
							Cache.Char.CurrentChar.dressing.Description = null;
							BaseScript.TriggerServerEvent("lprp:updateCurChar", "chardressing", Cache.Char.CurrentChar.dressing.Serialize());
							HUD.ShowNotification("Hai speso ~g~" + val + "$~w~, con carta di credito");
						}
						else
						{
							HUD.ShowNotification("Non hai i soldi per questo abito!", NotificationColor.Red, true);
						}
					}
				}

			};

			HUD.MenuPool.OnMenuStateChanged += async (oldmenu, newmenu, state) =>
			{
				if(state == MenuState.Closed && oldmenu == MenuAccessori)
				{
					await BaseScript.Delay(200);
					for (int i = 0; i < SubMenusCapp.Count; i++) if (SubMenusCapp[i].Visible) return;
					for (int i = 0; i < SubMenusPolso.Count; i++) if (SubMenusPolso[i].Visible) return;
					if (Borse.Visible || Orecc.Visible || Brac.Visible || Polso.Visible || Orol.Visible || Capp.Visible || Orologino.Visible)
						return;
					await UpdateDress(Cache.Char.CurrentChar.dressing);
					NegozioAbitiClient.Esci();
					AccessoriAttivo = false;
					Client.Instance.RemoveTick(CameraAcc);
				}
				else if (state == MenuState.ChangeForward)
				{
					if (newmenu == Brac)
						PointCamAtPedBone(NegozioAbitiClient.camm.Handle, PlayerPedId(), 28422, 0.0f, 0.0f, 0.0f, true);
					else if (newmenu == Borse)
					{
						float newheading = Cache.PlayerPed.Heading - 180f;
						Cache.PlayerPed.Task.AchieveHeading(newheading, 1000);
						PointCamAtPedBone(NegozioAbitiClient.camm.Handle, PlayerPedId(), 24818, 0.0f, 0.0f, 0.0f, true);
					}
					else if (newmenu == Orecc)
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
						Cache.PlayerPed.Task.LookAt(new Vector3(NegozioAbitiClient.camm.Position.X + 5f, NegozioAbitiClient.camm.Position.Y, NegozioAbitiClient.camm.Position.Z));
					}
					else if (newmenu == Orol)
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
					}
				}

				else if (state == MenuState.ChangeBackward)
				{
					if (oldmenu == Borse)
					{
						PointCamAtPedBone(NegozioAbitiClient.camm.Handle, PlayerPedId(), 24818, 0.0f, 0.0f, 0.0f, true);
						float newheading = Cache.PlayerPed.Heading + 180f;
						Cache.PlayerPed.Task.AchieveHeading(newheading, 1000);
						await UpdateDress(Cache.Char.CurrentChar.dressing);
					}
					else if (oldmenu == Orecc)
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
						await UpdateDress(Cache.Char.CurrentChar.dressing);
						Cache.PlayerPed.Task.LookAt(NegozioAbitiClient.camm.Position);
					}
					else if (oldmenu == Brac)
					{
						PointCamAtPedBone(NegozioAbitiClient.camm.Handle, PlayerPedId(), 24818, 0.0f, 0.0f, 0.0f, true);
						await UpdateDress(Cache.Char.CurrentChar.dressing);
						StartAnim(anim, "try_shirt_base");
					}
					else if (oldmenu == Orol)
					{
						await BaseScript.Delay(200);
						for (int i = 0; i < SubMenusPolso.Count; i++)
						{
							if (SubMenusPolso[i].Visible)
								return;
						}
						PointCamAtPedBone(NegozioAbitiClient.camm.Handle, PlayerPedId(), 24818, 0.0f, 0.0f, 0.0f, true);
						await UpdateDress(Cache.Char.CurrentChar.dressing);
						StartAnim(anim, "try_shirt_base");
					}
					else if (oldmenu == Polso)
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
						await UpdateDress(Cache.Char.CurrentChar.dressing);
					}
				}
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
