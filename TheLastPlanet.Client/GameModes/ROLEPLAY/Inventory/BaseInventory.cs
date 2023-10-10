using System;
using System.Collections.Generic;
using TheLastPlanet.Client.Handlers;

namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Inventory
{
    //TODO: THIS CLASS WAS NEVER FINISHED
    static class BaseInventory
    {
        private static bool inventoryOpen;
        public static bool InventarioAperto { get => inventoryOpen; set => inventoryOpen = value; }
        private static List<InputController> controlliInv = new()
        {
            new(Control.DropAmmo, ServerMode.Roleplay, PadCheck.Keyboard, action: new Action<Ped, object[]>(OpenMenuPed)),
            //TODO: For Now
            new(Control.FrontendPause, ServerMode.Roleplay, action: new Action<Ped, object[]>(CloseMenuPed)),
            new(Control.FrontendPauseAlternate, ServerMode.Roleplay, action: new Action<Ped, object[]>(CloseMenuPed)),
            new(Control.PhoneCancel, ServerMode.Roleplay, action: new Action<Ped, object[]>(CloseMenuPed)),
        };

        public static void Init()
        {
            InputHandler.AddInputList(controlliInv);
        }

        private static async void OpenMenuPed(Ped p, object[] _)
        {
            if (!inventoryOpen)
            {
                inventoryOpen = true;
                SetFrontendActive(true);
                ActivateFrontendMenu(Functions.HashUint("FE_MENU_VERSION_EMPTY_NO_BACKGROUND"), true, -1);
                SetMouseCursorVisibleInMenus(true);
                Ped cloned = await Functions.CreatePedLocally(PlayerCache.MyPlayer.Ped.Model.Hash, GetGameplayCamCoord() + new Vector3(0, 2, 0), 0f, PedTypes.Mission);
                cloned.IsCollisionEnabled = false;
                ClonePedToTarget(PlayerCache.MyPlayer.Ped.Handle, cloned.Handle);
                cloned.IsCollisionEnabled = false;
                FinalizeHeadBlend(cloned.Handle);
                cloned.IsPositionFrozen = true;

                //if()
                /*
                SetFrontendActive(true);
                ActivateFrontendMenu(Funzioni.HashUint("FE_MENU_VERSION_EMPTY_NO_BACKGROUND"), true, -1);
                Screen.Effects.Start(ScreenEffect.FocusOut, 800);
                TransitionToBlurred(700);
                SetMouseCursorVisibleInMenus(false);
                await BaseScript.Delay(200);
                // NATIVO GIUSTO, IN R* SCRIPTS è UN VALORE FLOAT
                GivePedToPauseMenu(cloned.Handle, 1);
                SetPauseMenuPedLighting(true);
                SetPauseMenuPedSleepState(true);
                GiveWeaponObjectToPed((int)WeaponHash.Pistol, cloned.Handle);
                */
            }
        }
        private static void CloseMenuPed(Ped p, object[] _)
        {
            if (inventoryOpen)
            {
                ClearPedInPauseMenu();
                SetFrontendActive(false);
                Screen.Effects.Start(ScreenEffect.FocusOut, 500);
                TransitionFromBlurred(400);
                inventoryOpen = false;
            }
        }

        private static void ApriInventario()
        {

        }
    }
}
