using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.NativeUI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Impostazioni.Shared.Configurazione.Generici;
using Newtonsoft.Json;
using TheLastPlanet.Shared;
using TheLastPlanet.Client.Core;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Interactions
{
	internal static class PickupsClient
	{
		// raccogliere pickup random@domestic", "pickup_low"
		public static List<OggettoRaccoglibile> Pickups = new List<OggettoRaccoglibile>();

		public static void Init()
		{
			Client.Instance.AddEventHandler("lprp:createPickup", new Action<string, string>(CreatePickup));
			Client.Instance.AddEventHandler("lprp:removePickup", new Action<int>(RimuoviPickup));
			Client.Instance.AddEventHandler("lprp:createMissingPickups", new Action<string>(CreaMissingPickups));
		}

		public static void Stop()
		{
			Client.Instance.RemoveEventHandler("lprp:createPickup", new Action<string, string>(CreatePickup));
			Client.Instance.RemoveEventHandler("lprp:removePickup", new Action<int>(RimuoviPickup));
			Client.Instance.RemoveEventHandler("lprp:createMissingPickups", new Action<string>(CreaMissingPickups));
		}

		public static async Task PickupsMain()
		{
			bool letSleep = true;
			Tuple<Player, float> closest = Funzioni.GetClosestPlayer();

			foreach (OggettoRaccoglibile pickup in Pickups)
				if (pickup != null)
				{
					Prop pick = new Prop(pickup.propObj);

					if (pick.HasDecor("PickupOggetto") || pick.HasDecor("PickupArma") || pick.HasDecor("PickupAccount"))
					{
						float dist = Cache.PlayerCache.MyPlayer.User.Posizione.Distance(pick.Position);

						if (dist < 5)
						{
							string label = pickup.label;
							letSleep = false;

							if (dist < 1.5)
								if (Cache.PlayerCache.MyPlayer.Ped.IsOnFoot && !HUD.MenuPool.IsAnyMenuOpen)
								{
									HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per raccogliere");

									if (Input.IsControlJustPressed(Control.Context))
									{
										if (closest.Item2 > -1 && closest.Item2 <= 3)
										{
											HUD.ShowNotification("Non puoi con qualcuno nelle vicinanze");
										}
										else
										{
											if (!pickup.inRange)
											{
												pickup.inRange = true;
												Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation("pickup_object", "pickup_low");
												await BaseScript.Delay(1000);
												BaseScript.TriggerServerEvent("lprp:onPickup", pickup.id);
												Game.PlaySound("PICK_UP", "HUD_FRONTEND_DEFAULT_SOUNDSET");
											}
										}
									}
								}

							if (!pick.HasDecor("PickupArma"))
								HUD.DrawText3D((pick.Position + new Vector3(0, 0, 1f)).ToPosition(), Colors.Cyan, ConfigShared.SharedConfig.Main.Generici.ItemList[pickup.name].label, CitizenFX.Core.UI.Font.HouseScript);
							else
								HUD.DrawText3D((pick.Position + new Vector3(0, 0, 1f)).ToPosition(), Colors.Cyan, $"{GetLabelText(label)} [{pickup.amount}]", CitizenFX.Core.UI.Font.HouseScript);
						}
						else if (pickup.inRange)
						{
							pickup.inRange = false;
						}
					}
					else
					{
						pick.Delete();
					}
				}

			if (letSleep) await BaseScript.Delay(500);
		}

		private static async void CreatePickup(string jsonOggetto, string userId)
		{
			int playerId = Convert.ToInt32(userId);
			Ped playerPed = new Ped(GetPlayerPed(GetPlayerFromServerId(playerId)));
			OggettoRaccoglibile oggetto = jsonOggetto.FromJson<OggettoRaccoglibile>();
			Position entityCoords = playerPed.Position.ToPosition();
			Position forward = playerPed.ForwardVector.ToPosition();
			Position objectCoords = entityCoords + forward * 1.0f;
			Model model = new((int)oggetto.obj);
			model.Request();
			Entity pickupObject = null;

			switch (oggetto.type)
			{
				case "item":
					if (model.Hash == (int)ObjectHash.a_c_fish)
						pickupObject = await Funzioni.SpawnPed((int)oggetto.obj, objectCoords, PedTypes.Animal);
					else
						pickupObject = await Funzioni.CreateProp(model.Hash, objectCoords.ToVector3, new Vector3(0), true);
					pickupObject.SetDecor("PickupOggetto", oggetto.amount);

					break;
				case "weapon":
					RequestWeaponAsset(Funzioni.HashUint(oggetto.name), 31, 0);
					while (!HasWeaponAssetLoaded(Funzioni.HashUint(oggetto.name))) await BaseScript.Delay(0);
					pickupObject = new Prop(CreateWeaponObject(Funzioni.HashUint(oggetto.name), 50, objectCoords.X, objectCoords.Y, objectCoords.Z, true, 1.0f, 0));
					pickupObject.SetDecor("PickupArma", oggetto.amount);
					oggetto.propObj = pickupObject.Handle;
					SetWeaponObjectTintIndex(pickupObject.Handle, oggetto.tintIndex);

					foreach (Components comp in oggetto.componenti)
					{
						GiveWeaponComponentToWeaponObject(pickupObject.Handle, Funzioni.HashUint(comp.name));
						if (comp.name.Contains("FLSH")) SetCreateWeaponObjectLightSource(pickupObject.Handle, true);
					}

					break;
				case "account":
					pickupObject = await Funzioni.CreateProp(model.Hash, objectCoords.ToVector3, new Vector3(0), true);
					pickupObject.SetDecor("PickupAccount", oggetto.amount);

					break;
			}

			pickupObject.IsPersistent = true;
			PlaceObjectOnGroundProperly(pickupObject.Handle);
			SetActivateObjectPhysicsAsSoonAsItIsUnfrozen(pickupObject.Handle, true);
			oggetto.propObj = pickupObject.Handle;
			oggetto.inRange = false;
			oggetto.coords = objectCoords;
			Pickups.Add(oggetto);
		}

		private static void RimuoviPickup(int id)
		{
			OggettoRaccoglibile pickup = Pickups[id];

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
			Pickups = jsonPickups.FromJson<List<OggettoRaccoglibile>>();

			if (Pickups.Count > 0)
				foreach (OggettoRaccoglibile pickup in Pickups)
					if (pickup != null)
					{
						Entity pickupObject = null;

						if (pickup.type == "item" || pickup.type == "account")
						{
							Model model = new Model((int)pickup.obj);
							model.Request();
							if (model.Hash == (int)ObjectHash.a_c_fish)
								pickupObject = await World.CreatePed(model, pickup.coords.ToVector3);
							else
								pickupObject = new Prop(CreateObject(model.Hash, pickup.coords.X, pickup.coords.Y, pickup.coords.Z, false, false, true));
							if (pickup.type == "item")
								pickupObject.SetDecor("PickupOggetto", pickup.amount);
							else if (pickup.type == "account") pickupObject.SetDecor("PickupAccount", pickup.amount);
						}
						else if (pickup.type == "weapon")
						{
							RequestWeaponAsset(Funzioni.HashUint(pickup.name), 31, 0);
							while (!HasWeaponAssetLoaded(Funzioni.HashUint(pickup.name))) await BaseScript.Delay(0);
							pickupObject = new Prop(CreateWeaponObject(Funzioni.HashUint(pickup.name), 50, pickup.coords.X, pickup.coords.Y, pickup.coords.Z, true, 1.0f, 0));
							pickupObject.SetDecor("PickupArma", pickup.amount);
							SetWeaponObjectTintIndex(pickupObject.Handle, pickup.tintIndex);

							foreach (Components comp in pickup.componenti)
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
						pickup.coords = pickupObject.Position.ToPosition();
					}
		}
	}
}