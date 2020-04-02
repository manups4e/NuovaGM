using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.MenuNativo;
using NuovaGM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NuovaGM.Client.Interactions
{
	static class PickupsClient
	{

		// raccogliere pickup random@domestic", "pickup_low"

		public static List<OggettoRaccoglibile> Pickups = new List<OggettoRaccoglibile>();

		public static void Init()
		{
			Client.GetInstance.RegisterTickHandler(PickupsMain);
			Client.GetInstance.RegisterEventHandler("lprp:createPickupInventory", new Action<string,string>(CreatePickupInventory));
			Client.GetInstance.RegisterEventHandler("lprp:createPickupWeapon", new Action<string,string>(CreatePickupWeapon));
//			Client.GetInstance.RegisterEventHandler("lprp:createPickupMoney", new Action<string,string>(CreatePickupMoney));
			Client.GetInstance.RegisterEventHandler("lprp:removePickup", new Action<int>(RimuoviPickup));
		}

		public static async Task PickupsMain()
		{
			bool letSleep = true;
			var closest = Funzioni.GetClosestPlayer();
			foreach (var pickup in Pickups)
			{
				if (pickup != null)
				{
					Prop pick = new Prop(pickup.propObj);
					//				if (pick.HasDecor("PickupOggetto") || pick.HasDecor("PickupArma") || pick.HasDecor("PickupAccount"))
					//				{
					float dist = World.GetDistance(Game.PlayerPed.Position, pick.Position);
					if (dist < 5)
					{
						string label = pickup.label;
						letSleep = false;
						if (dist < 1)
						{
							if (Game.PlayerPed.IsOnFoot && !HUD.MenuPool.IsAnyMenuOpen())
							{
								HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per raccogliere");
								if (Input.IsControlJustPressed(Control.Context))
								{
									if ((closest.Item2 == -1 || closest.Item2 > 3) && !pickup.inRange)
									{
										pickup.inRange = true;
										Game.PlayerPed.Task.PlayAnimation("pickup_object", "pickup_low");
										await BaseScript.Delay(1000);
										BaseScript.TriggerServerEvent("lprp:onPickup", pickup.id);
										Game.PlaySound("PICK_UP", "HUD_FRONTEND_DEFAULT_SOUNDSET");
									}
								}
							}
						}
						HUD.DrawText3D(new Vector3(pickup.coords[0], pickup.coords[1], pickup.coords[2] + 0.25f), Colors.Cyan, label.StartsWith("wt") ? $"{GetLabelText(label)} [{(pickup as OggettoArmaRaccoglibile).ammo}]" : label, 1);
						//						else if (pick.HasDecor("PickupArma"))
						//							HUD.DrawText3D(new Vector3(pickup.coords[0], pickup.coords[1], pickup.coords[2] + 0.25f), Colors.Cyan, $"{GetLabelText(label)} [munizioni:{(pickup as OggettoArmaRaccoglibile).ammo}]" , 1);
						//						else if (pick.HasDecor("PickupAccount"))

					}
					else if (pickup.inRange)
						pickup.inRange = false;
					//				}
				}
			}
			if(letSleep)
				await BaseScript.Delay(500);
		}

		private static async void CreatePickupInventory(string jsonOggetto, string userId)
		{
			int playerId = Convert.ToInt32(userId);
			Ped playerPed = new Ped(GetPlayerPed(GetPlayerFromServerId(playerId)));
			OggettoRaccoglibile oggetto = JsonConvert.DeserializeObject<OggettoRaccoglibile>(jsonOggetto);
			Vector3 entityCoords = playerPed.Position;
			Vector3 forward = playerPed.ForwardVector;
			Vector3 objectCoords = entityCoords + forward * 1.0f;
			Model model = new Model((int)oggetto.obj);
			model.Request();
			Entity pickupObject;
			if (model.Hash == (int)ObjectHash.a_c_fish)
			{
				pickupObject = await World.CreatePed(model, objectCoords);
			}
			else 
			{
				pickupObject = new Prop(CreateObject(model.Hash, objectCoords.X, objectCoords.Y, objectCoords.Z, false, false, true));
			}
			pickupObject.SetDecor("PickupOggetto", oggetto.amount);
			pickupObject.IsPersistent = true;
			pickupObject.Rotation = new Vector3(0f, 90f, 0);
			PlaceObjectOnGroundProperly(pickupObject.Handle);
//			pickupObject.IsPositionFrozen = true;
			oggetto.propObj = pickupObject.Handle;
			oggetto.inRange = false;
			oggetto.coords = objectCoords.ToArray();
			Pickups.Add(oggetto);
		}

		private static async void CreatePickupWeapon(string jsonWeapon, string userId)
		{
			int playerId = Convert.ToInt32(userId);
			Ped playerPed = new Ped(GetPlayerPed(GetPlayerFromServerId(playerId)));
			OggettoArmaRaccoglibile oggetto = JsonConvert.DeserializeObject<OggettoArmaRaccoglibile>(jsonWeapon);
			Vector3 entityCoords = playerPed.Position;
			Vector3 forward = playerPed.ForwardVector;
			Vector3 objectCoords = entityCoords + forward * 1.0f;
			RequestWeaponAsset(Funzioni.HashUint(oggetto.name), 31, 0);
			while (!HasWeaponAssetLoaded(Funzioni.HashUint(oggetto.name))) await BaseScript.Delay(0);
			Prop pickupObject = new Prop(CreateWeaponObject(Funzioni.HashUint(oggetto.name), 50, objectCoords.X, objectCoords.Y, objectCoords.Z, true, 1.0f, 0));
			pickupObject.SetDecor("PickupArma", oggetto.ammo);
			oggetto.propObj = pickupObject.Handle;
			SetWeaponObjectTintIndex(pickupObject.Handle, oggetto.tintIndex);
			foreach (var comp in oggetto.componenti)
			{
				GiveWeaponComponentToWeaponObject(pickupObject.Handle, Funzioni.HashUint(comp.name));
				if (comp.name.EndsWith("flsh")) SetCreateWeaponObjectLightSource(pickupObject.Handle, true);
			}
			pickupObject.IsPersistent = true;
			PlaceObjectOnGroundProperly(pickupObject.Handle);
//			pickupObject.IsPositionFrozen = true;
			oggetto.propObj = pickupObject.Handle;
			oggetto.inRange = false;
			oggetto.coords = objectCoords.ToArray();
			Pickups.Add(oggetto);
		}

		private static void RimuoviPickup(int id)
		{
			var pickup = Pickups[id];
			if (pickup != null && pickup.propObj != 0)
			{
				if (pickup.obj == ObjectHash.a_c_fish)
					new Ped(pickup.propObj).Delete();
				else
					new Prop(pickup.propObj).Delete();
				Pickups[id] = null;
			}
		}
	}
}
