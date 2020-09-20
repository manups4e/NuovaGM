using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Logger;
using Newtonsoft.Json;
using NuovaGM.Client.gmPrincipale;
using NuovaGM.Client.gmPrincipale.NuovoIngresso;
using NuovaGM.Client.gmPrincipale.Personaggio;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.MenuNativo;
using NuovaGM.Client.MenuNativo.PauseMenu;
using NuovaGM.Shared;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Client
{
	static class ClasseDiTest
	{
		public static void Init()
		{
			Client.Instance.AddTick(TabsPauseMenu);
		}

		/*
		 		/// <summary>
		/// Get the first <see cref="Vehicle"/> in front of this <see cref="Entity"/> 
		/// </summary>
		/// <param name="distance">Max distance of the Raycast</param>
		/// <returns>Returns the first <see cref="Vehicle"/> encountered in a distance specified</returns>
		public Vehicle GetVehicleInFront(float distance = 5f)
		{
			return GetEntityInFront<Vehicle>(distance);
		}

		/// <summary>
		/// Get the first <see cref="Ped"/> in front of this <see cref="Entity"/> 
		/// </summary>
		/// <param name="distance">Max distance of the Raycast</param>
		/// <returns>Returns the first <see cref="Ped"/> encountered in a distance specified</returns>
		public Ped GetPedInFront(float distance = 5f)
		{
			return GetEntityInFront<Ped>(distance);
		}

		/// <summary>
		/// Get the first <see cref="Prop"/> in front of this <see cref="Entity"/> 
		/// </summary>
		/// <param name="distance">Max distance of the Raycast</param>
		/// <returns>Returns the first <see cref="Prop"/> encountered in a distance specified</returns>
		public Prop GetPropInFront(float distance = 5f)
		{
			return GetEntityInFront<Prop>(distance);
		}

		/// <summary>
		/// Get the first generic <see cref="Entity"/> in front of this <see cref="Entity"/> 
		/// </summary>
		/// <param name="distance">Max distance of the Raycast</param>
		/// <returns>Returns the first <see cref="Entity"/> encountered in a distance specified</returns>
		public Entity GetPropInFront(float distance = 5f)
		{
			return GetEntityInFront<Entity>(distance);
		}

		private T GetEntityInFront<T>(float distance) where T : Entity
		{
			RaycastResult raycast = World.Raycast(Position, Position + 5f * ForwardVector), IntersectOptions.Everything);
			if (raycast.DitHitEntity && raycast.HitEntity.Handle != Handle)
				return (T)raycast.HitEntity;
			return null;
		}

		/// <summary>
		/// Looks for the closest <see cref="Vehicle"/> to this <see cref="Entity"/>
		/// </summary>
		/// <returns>The closest <see cref="Vehicle"/> to this <see cref="Entity"/> </returns>
		public Vehicle GetClosestVehicle()
		{
			return World.GetClosest<Vehicle>(Position, World.GetAllVehicles());
		}

		/// <summary>
		/// Looks for the closest <see cref="Ped"/> to this <see cref="Entity"/>
		/// </summary>
		/// <returns>The closest <see cref="Ped"/> to this <see cref="Entity"/> </returns>
		public Ped GetClosestPed()
		{
			return World.GetClosest<Ped>(Position, World.GetAllPeds());
		}

		/// <summary>
		/// Looks for the closest <see cref="Prop"/> to this <see cref="Entity"/>
		/// </summary>
		/// <returns>The closest <see cref="Prop"/> to this <see cref="Entity"/> </returns>

		public Prop GetClosestProp()
		{
			return World.GetClosest<Prop>(Position, World.GetAllProps());
		}

		/// <summary>
		/// Looks for the closest <see cref="Blip"/> to this <see cref="Entity"/>
		/// </summary>
		/// <returns>The closest <see cref="Blip"/> to this <see cref="Entity"/> </returns>
		public Blip GetClosestBlip()
		{
			return World.GetClosest<Blip>(Position, World.GetAllBlips());
		}
		 */

		private static async void AttivaMenu()
		{
			UIMenu Test = new UIMenu("Test", "test", new System.Drawing.PointF(700, 300));
			HUD.MenuPool.Add(Test);
			UIMenuItem m = new UIMenuItem("dettagli veicolo");
			Test.AddItem(m);
			Model mod = new Model();
			List<string> models = new List<string>();
			List<int> hashes = models.ConvertAll(x => GetHashKey(x));
			World.GetClosest<Vehicle>(Game.PlayerPed.Position, World.GetAllVehicles().Where(x => hashes.Contains(x.Model.Hash)).ToArray());

			/*
						UIMenuItem b = new UIMenuItem("ShowColoredShard");
						UIMenuItem c = new UIMenuItem("ShowOldMessage");
						UIMenuItem d = new UIMenuItem("ShowSimpleShard");
						UIMenuItem e = new UIMenuItem("ShowRankupMessage");
						UIMenuItem f = new UIMenuItem("ShowWeaponPurchasedMessage");
						UIMenuItem g = new UIMenuItem("ShowMpMessageLarge");
						UIMenuItem h = new UIMenuItem("ShowMpWastedMessage");
						Test.AddItem(b);
						Test.AddItem(c);
						Test.AddItem(d);
						Test.AddItem(e);
						Test.AddItem(f);
						Test.AddItem(g);
						Test.AddItem(h);
			*/

			Test.OnItemSelect += async (menu, item, index) =>
			{
				if(item == m)
				{
				}
				/*				if (item == b)
									BigMessageThread.MessageInstance.ShowColoredShard("Test1", "Test2", HudColor.HUD_COLOUR_BLUELIGHT, HudColor.HUD_COLOUR_MENU_YELLOW);
								else if (item == c)
									BigMessageThread.MessageInstance.ShowOldMessage("Test1");
								else if (item == d)
									BigMessageThread.MessageInstance.ShowSimpleShard("Test1", "Test2");
								else if (item == e)
									BigMessageThread.MessageInstance.ShowRankupMessage("Test1", "Test2", 15);
								else if (item == f)
									BigMessageThread.MessageInstance.ShowWeaponPurchasedMessage("Test1", "WEAPON_PISTOL", WeaponHash.Pistol);
								else if (item == g)
									BigMessageThread.MessageInstance.ShowMpMessageLarge("~g~MISSIONE COMPIUTA", "Veicolo recuperato!");
								else if (item == h)
									BigMessageThread.MessageInstance.ShowMpWastedMessage("Test 1", "Test 2");
				*/

			};
			Test.Visible = true;
		}

		/*
		static TabView b = new TabView("New");
		static List<UIMenuItem> Players = new List<UIMenuItem>()
				{
					new UIMenuItem(Game.Player.Name)
				};
		static TabInteractiveListItem item2 = new TabInteractiveListItem("Item 2", Players);
		*/
		public static async Task TabsPauseMenu()
		{
			/*
			b.ProcessControls();
			b.Update();
			item2.ProcessControls();
			*/
			RaycastResult raycast = World.Raycast(Game.PlayerPed.Position, Game.PlayerPed.Position + 5f * Game.PlayerPed.ForwardVector, IntersectOptions.Everything);
			if (raycast.DitHitEntity)
			{
				Log.Printa(LogType.Debug, $"Handle {raycast.HitEntity.Handle}, Model {raycast.HitEntity.Model.Hash}");
				Log.Printa(LogType.Debug, $"Player Handle {Game.PlayerPed.Handle}, Model {Game.PlayerPed.Model.Hash}");
			}

			if (Input.IsControlJustPressed(Control.DropWeapon, PadCheck.Any, ControlModifier.Shift))
			{
				AttivaMenu();
				/*
				b.Tabs.Clear();
				TabItem item1 = new TabItem("Item 1");
				List<MissionInformation> missions = new List<MissionInformation>()
				{
					new MissionInformation("Mission Info", new List<Tuple<string, string>>()
					{
						new Tuple<string, string>("Mission title", "Mission subtitle")
					})
				};
				TabMissionSelectItem item3 = new TabMissionSelectItem("Mission control to Major Tom", missions);
				b.AddTab(item1);
				b.AddTab(item2);
				b.AddTab(item3);
				b.Visible = true;
				*/
			}
		}
	}
}
