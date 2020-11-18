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
		public static async void Init()
		{
			Client.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action(Eccolo));
			//Client.Instance.AddTick(TabsPauseMenu);
			RequestStreamedTextureDict("default", true);
			while (!HasStreamedTextureDictLoaded("default")) await BaseScript.Delay(0);
		}

		private static void Eccolo()
		{
			Log.Printa(LogType.Debug, Game.Player.GetPlayerData().CurrentChar.Proprietà.Serialize());
			Log.Printa(LogType.Debug, Game.Player.GetPlayerData().CurrentChar.Veicoli.Serialize(includeEverything: true));
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
		public Vehicle GetClosestVehicleWithDistance()
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

			UIMenuItem b = new UIMenuItem("ResetTime");
			UIMenuItem c = new UIMenuItem("SetTime1");
			UIMenuItem d = new UIMenuItem("SetTime2");
			UIMenuItem e = new UIMenuItem("SetTime3");
			Test.AddItem(b);
			Test.AddItem(c);
			Test.AddItem(d);
			Test.AddItem(e);

			Test.OnItemSelect += async (menu, item, index) =>
			{
				if (item == b)
					NetworkClearClockTimeOverride();
				else if (item == c)
					AdvanceClockTimeTo(23, 0, 0);
				else if (item == d)
					SetClockTime(15, 0, 0);
				else if (item == e)
					NetworkOverrideClockTime(12, 0, 0);

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
			if (Input.IsControlJustPressed(Control.DropWeapon, PadCheck.Any, ControlModifier.Shift))
			{
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
