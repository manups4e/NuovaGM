using Settings.Shared.Config.Generic;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;


namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Interactions
{
    internal static class PickupsClient
    {
        // raccogliere pickup random@domestic", "pickup_low"
        public static List<PickupObject> Pickups = new List<PickupObject>();

        public static void Init()
        {
            AccessingEvents.OnRoleplaySpawn += Spawnato;
            AccessingEvents.OnRoleplayLeave += onPlayerLeft;
        }
        public static void Spawnato(PlayerClient client)
        {
            Client.Instance.AddEventHandler("lprp:createPickup", new Action<string, string>(CreatePickup));
            Client.Instance.AddEventHandler("lprp:removePickup", new Action<int>(RimuoviPickup));
            Client.Instance.AddEventHandler("lprp:createMissingPickups", new Action<string>(CreaMissingPickups));
        }
        public static void onPlayerLeft(PlayerClient client)
        {
            Client.Instance.RemoveEventHandler("lprp:createPickup", new Action<string, string>(CreatePickup));
            Client.Instance.RemoveEventHandler("lprp:removePickup", new Action<int>(RimuoviPickup));
            Client.Instance.RemoveEventHandler("lprp:createMissingPickups", new Action<string>(CreaMissingPickups));
        }

        public static async Task PickupsMain()
        {
            bool letSleep = true;
            Tuple<Player, float> closest = Funzioni.GetClosestPlayer();

            foreach (PickupObject pickup in Pickups)
                if (pickup != null)
                {
                    Prop pick = new Prop(pickup.PropObj);

                    if (pick.HasDecor("PickupOggetto") || pick.HasDecor("PickupArma") || pick.HasDecor("PickupAccount"))
                    {
                        float dist = Cache.PlayerCache.MyPlayer.Posizione.Distance(pick.Position);

                        if (dist < 5)
                        {
                            string label = pickup.Label;
                            letSleep = false;

                            if (dist < 1.5)
                                if (Cache.PlayerCache.MyPlayer.Ped.IsOnFoot && !MenuHandler.IsAnyMenuOpen)
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
                                            if (!pickup.InRange)
                                            {
                                                pickup.InRange = true;
                                                Cache.PlayerCache.MyPlayer.Ped.Task.PlayAnimation("pickup_object", "pickup_low");
                                                await BaseScript.Delay(1000);
                                                BaseScript.TriggerServerEvent("lprp:onPickup", pickup.Id);
                                                Game.PlaySound("PICK_UP", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                                            }
                                        }
                                    }
                                }

                            if (!pick.HasDecor("PickupArma"))
                                HUD.DrawText3D((pick.Position + new Vector3(0, 0, 1f)).ToPosition(), Colors.Cyan, ConfigShared.SharedConfig.Main.Generics.ItemList[pickup.Name].label, CitizenFX.Core.UI.Font.HouseScript);
                            else
                                HUD.DrawText3D((pick.Position + new Vector3(0, 0, 1f)).ToPosition(), Colors.Cyan, $"{GetLabelText(label)} [{pickup.Amount}]", CitizenFX.Core.UI.Font.HouseScript);
                        }
                        else if (pickup.InRange)
                        {
                            pickup.InRange = false;
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
            PickupObject oggetto = jsonOggetto.FromJson<PickupObject>();
            Position entityCoords = playerPed.Position.ToPosition();
            Position forward = playerPed.ForwardVector.ToPosition();
            Position objectCoords = entityCoords + forward * 1.0f;
            Model model = new((int)oggetto.ObjectHash);
            model.Request();
            Entity pickupObject = null;

            switch (oggetto.Type)
            {
                case "item":
                    if (model.Hash == (int)ObjectHash.a_c_fish)
                        pickupObject = await Funzioni.SpawnPed((int)oggetto.ObjectHash, objectCoords, PedTypes.Animal);
                    else
                        pickupObject = await Funzioni.CreateProp(model.Hash, objectCoords.ToVector3, new Vector3(0), true);
                    pickupObject.SetDecor("PickupOggetto", oggetto.Amount);

                    break;
                case "weapon":
                    RequestWeaponAsset(Funzioni.HashUint(oggetto.Name), 31, 0);
                    while (!HasWeaponAssetLoaded(Funzioni.HashUint(oggetto.Name))) await BaseScript.Delay(0);
                    pickupObject = new Prop(CreateWeaponObject(Funzioni.HashUint(oggetto.Name), 50, objectCoords.X, objectCoords.Y, objectCoords.Z, true, 1.0f, 0));
                    pickupObject.SetDecor("PickupArma", oggetto.Amount);
                    oggetto.PropObj = pickupObject.Handle;
                    SetWeaponObjectTintIndex(pickupObject.Handle, oggetto.TintIndex);

                    foreach (Components comp in oggetto.Components)
                    {
                        GiveWeaponComponentToWeaponObject(pickupObject.Handle, Funzioni.HashUint(comp.name));
                        if (comp.name.Contains("FLSH")) SetCreateWeaponObjectLightSource(pickupObject.Handle, true);
                    }

                    break;
                case "account":
                    pickupObject = await Funzioni.CreateProp(model.Hash, objectCoords.ToVector3, new Vector3(0), true);
                    pickupObject.SetDecor("PickupAccount", oggetto.Amount);

                    break;
            }

            pickupObject.IsPersistent = true;
            PlaceObjectOnGroundProperly(pickupObject.Handle);
            SetActivateObjectPhysicsAsSoonAsItIsUnfrozen(pickupObject.Handle, true);
            oggetto.PropObj = pickupObject.Handle;
            oggetto.InRange = false;
            oggetto.Coords = objectCoords;
            Pickups.Add(oggetto);
        }

        private static void RimuoviPickup(int id)
        {
            PickupObject pickup = Pickups[id];

            if (pickup != null && pickup.PropObj != 0)
            {
                if (pickup.ObjectHash == ObjectHash.a_c_fish)
                    new Ped(pickup.PropObj).Delete();
                else
                    new Prop(pickup.PropObj).Delete();
                Pickups[id] = null;
            }
        }

        private static async void CreaMissingPickups(string jsonPickups)
        {
            Pickups = jsonPickups.FromJson<List<PickupObject>>();

            if (Pickups.Count > 0)
                foreach (PickupObject pickup in Pickups)
                    if (pickup != null)
                    {
                        Entity pickupObject = null;

                        if (pickup.Type == "item" || pickup.Type == "account")
                        {
                            Model model = new Model((int)pickup.ObjectHash);
                            model.Request();
                            if (model.Hash == (int)ObjectHash.a_c_fish)
                                pickupObject = await World.CreatePed(model, pickup.Coords.ToVector3);
                            else
                                pickupObject = new Prop(CreateObject(model.Hash, pickup.Coords.X, pickup.Coords.Y, pickup.Coords.Z, false, false, true));
                            if (pickup.Type == "item")
                                pickupObject.SetDecor("PickupOggetto", pickup.Amount);
                            else if (pickup.Type == "account") pickupObject.SetDecor("PickupAccount", pickup.Amount);
                        }
                        else if (pickup.Type == "weapon")
                        {
                            RequestWeaponAsset(Funzioni.HashUint(pickup.Name), 31, 0);
                            while (!HasWeaponAssetLoaded(Funzioni.HashUint(pickup.Name))) await BaseScript.Delay(0);
                            pickupObject = new Prop(CreateWeaponObject(Funzioni.HashUint(pickup.Name), 50, pickup.Coords.X, pickup.Coords.Y, pickup.Coords.Z, true, 1.0f, 0));
                            pickupObject.SetDecor("PickupArma", pickup.Amount);
                            SetWeaponObjectTintIndex(pickupObject.Handle, pickup.TintIndex);

                            foreach (Components comp in pickup.Components)
                            {
                                GiveWeaponComponentToWeaponObject(pickupObject.Handle, Funzioni.HashUint(comp.name));
                                if (comp.name.EndsWith("flsh")) SetCreateWeaponObjectLightSource(pickupObject.Handle, true);
                            }
                        }

                        pickup.PropObj = pickupObject.Handle;
                        pickupObject.IsPersistent = true;
                        PlaceObjectOnGroundProperly(pickupObject.Handle);
                        pickupObject.IsPositionFrozen = true;
                        pickup.PropObj = pickupObject.Handle;
                        pickup.InRange = false;
                        pickup.Coords = pickupObject.Position.ToPosition();
                    }
        }
    }
}