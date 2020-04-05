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
			Client.GetInstance.RegisterEventHandler("lprp:createPickup", new Action<string,string>(CreatePickup));
			Client.GetInstance.RegisterEventHandler("lprp:removePickup", new Action<int>(RimuoviPickup));
			Client.GetInstance.RegisterEventHandler("lprp:createMissingPickups", new Action<string>(CreaMissingPickups));
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
					if (pick.HasDecor("PickupOggetto") || pick.HasDecor("PickupArma") || pick.HasDecor("PickupAccount"))
					{
						float dist = World.GetDistance(Game.PlayerPed.Position, pick.Position);
						if (dist < 5)
						{
							string label = pickup.label;
							letSleep = false;
							if (dist < 1.5)
							{
								if (Game.PlayerPed.IsOnFoot && !HUD.MenuPool.IsAnyMenuOpen())
								{
									HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per raccogliere");
									if (Input.IsControlJustPressed(Control.Context))
									{
										if (closest.Item2 != -1 && closest.Item2 <= 3)
											HUD.ShowNotification("Non puoi con qualcuno nelle vicinanze");
										else
										{
											if (!pickup.inRange)
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
							}
							if (!pick.HasDecor("PickupArma"))
								HUD.DrawText3D(pick.Position + new Vector3(0, 0, 1f), Colors.Cyan, label, 1);
							else 
								HUD.DrawText3D(pick.Position + new Vector3(0, 0, 1f), Colors.Cyan, $"{GetLabelText(label)} [{pickup.amount}]", 1);
						}
						else if (pickup.inRange) pickup.inRange = false;
					}
					else pick.Delete();
				}
			}
			if(letSleep)
				await BaseScript.Delay(500);
		}

		private static async void CreatePickup(string jsonOggetto, string userId)
		{
			int playerId = Convert.ToInt32(userId);
			Ped playerPed = new Ped(GetPlayerPed(GetPlayerFromServerId(playerId)));
			OggettoRaccoglibile oggetto = JsonConvert.DeserializeObject<OggettoRaccoglibile>(jsonOggetto);
			Vector3 entityCoords = playerPed.Position;
			Vector3 forward = playerPed.ForwardVector;
			Vector3 objectCoords = entityCoords + forward * 1.0f;
			Model model = new Model((int)oggetto.obj);
			model.Request();
			Entity pickupObject = null;
			switch (oggetto.type)
			{
				case "item":
					if (model.Hash == (int)ObjectHash.a_c_fish)
						pickupObject = await World.CreatePed(model, objectCoords);
					else
						pickupObject = new Prop(CreateObject(model.Hash, objectCoords.X, objectCoords.Y, objectCoords.Z, false, false, true));
					pickupObject.SetDecor("PickupOggetto", oggetto.amount);
					break;
				case "weapon":
					RequestWeaponAsset(Funzioni.HashUint(oggetto.name), 31, 0);
					while (!HasWeaponAssetLoaded(Funzioni.HashUint(oggetto.name))) await BaseScript.Delay(0);
					pickupObject = new Prop(CreateWeaponObject(Funzioni.HashUint(oggetto.name), 50, objectCoords.X, objectCoords.Y, objectCoords.Z, true, 1.0f, 0));
					pickupObject.SetDecor("PickupArma", oggetto.amount);
					oggetto.propObj = pickupObject.Handle;
					SetWeaponObjectTintIndex(pickupObject.Handle, oggetto.tintIndex);
					foreach (var comp in oggetto.componenti)
					{
						GiveWeaponComponentToWeaponObject(pickupObject.Handle, Funzioni.HashUint(comp.name));
						if (comp.name.Contains("FLSH")) SetCreateWeaponObjectLightSource(pickupObject.Handle, true);
					}
					break;
				case "account":
					pickupObject = new Prop(CreateObject(model.Hash, objectCoords.X, objectCoords.Y, objectCoords.Z, false, false, true));
					pickupObject.SetDecor("PickupAccount", oggetto.amount);
					break;
			}
			pickupObject.IsPersistent = true;
			PlaceObjectOnGroundProperly(pickupObject.Handle);
			SetActivateObjectPhysicsAsSoonAsItIsUnfrozen(pickupObject.Handle, true);
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

		private static async void CreaMissingPickups(string jsonPickups)
		{
			Pickups = JsonConvert.DeserializeObject<List<OggettoRaccoglibile>>(jsonPickups);
			if (Pickups.Count > 0)
			{
				foreach (var pickup in Pickups)
				{
					Entity pickupObject = null;
					if (pickup.type == "item" || pickup.type == "account")
					{
						Model model = new Model((int)pickup.obj);
						model.Request();
						if (model.Hash == (int)ObjectHash.a_c_fish)
							pickupObject = await World.CreatePed(model, pickup.coords.ToVector3());
						else
							pickupObject = new Prop(CreateObject(model.Hash, pickup.coords.ToVector3().X, pickup.coords.ToVector3().Y, pickup.coords.ToVector3().Z, false, false, true));
						if (pickup.type == "item") pickupObject.SetDecor("PickupOggetto", pickup.amount);
						else if (pickup.type == "account") pickupObject.SetDecor("PickupAccount", pickup.amount);
					}
					else if (pickup.type == "weapon")
					{
						RequestWeaponAsset(Funzioni.HashUint(pickup.name), 31, 0);
						while (!HasWeaponAssetLoaded(Funzioni.HashUint(pickup.name))) await BaseScript.Delay(0);
						pickupObject = new Prop(CreateWeaponObject(Funzioni.HashUint(pickup.name), 50, pickup.coords.ToVector3().X, pickup.coords.ToVector3().Y, pickup.coords.ToVector3().Z, true, 1.0f, 0));
						pickupObject.SetDecor("PickupArma", pickup.amount);
						SetWeaponObjectTintIndex(pickupObject.Handle, pickup.tintIndex);
						foreach (var comp in pickup.componenti)
						{
							GiveWeaponComponentToWeaponObject(pickupObject.Handle, Funzioni.HashUint(comp.name));
							if (comp.name.EndsWith("flsh")) SetCreateWeaponObjectLightSource(pickupObject.Handle, true);
						}
					}
					pickup.propObj = pickupObject.Handle;
					pickupObject.IsPersistent = true;
					PlaceObjectOnGroundProperly(pickupObject.Handle);
					pickupObject.IsPositionFrozen = true;
					pickup.propObj = pickupObject.Handle;
					pickup.inRange = false;
					pickup.coords = pickupObject.Position.ToArray();
				}
			}
		}
	}
}
