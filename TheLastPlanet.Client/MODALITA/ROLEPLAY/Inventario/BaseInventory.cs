using System;
using System.Collections.Generic;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Handlers;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Inventario
{
    static class BaseInventory
    {
        private static bool inventarioAperto;
        public static bool InventarioAperto { get => inventarioAperto; set => inventarioAperto = value; }
        private static List<InputController> controlliInv = new()
        {
            new(Control.DropAmmo, ServerMode.Roleplay, PadCheck.Keyboard, action: new Action<Ped, object[]>(ApriMenuPed)),
            //PER ORA
            new(Control.FrontendPause, ServerMode.Roleplay, action: new Action<Ped, object[]>(CloseMenuPed)),
            new(Control.FrontendPauseAlternate, ServerMode.Roleplay, action: new Action<Ped, object[]>(CloseMenuPed)),
            new(Control.PhoneCancel, ServerMode.Roleplay, action: new Action<Ped, object[]>(CloseMenuPed)),
        };

        public static void Init()
        {
            InputHandler.AddInputList(controlliInv);
        }

        private static async void ApriMenuPed(Ped p, object[] _)
        {
            if (!inventarioAperto)
            {
                inventarioAperto = true;
                SetFrontendActive(true);
                ActivateFrontendMenu(Funzioni.HashUint("FE_MENU_VERSION_EMPTY_NO_BACKGROUND"), true, -1);
                SetMouseCursorVisibleInMenus(true);
                Ped cloned = await Funzioni.CreatePedLocally(PlayerCache.MyPlayer.Ped.Model.Hash, GetGameplayCamCoord() + new Vector3(0, 2, 0), 0f, PedTypes.Mission);
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
            if (inventarioAperto)
            {
                ClearPedInPauseMenu();
                SetFrontendActive(false);
                Screen.Effects.Start(ScreenEffect.FocusOut, 500);
                TransitionFromBlurred(400);
                inventarioAperto = false;
            }
        }

        private static void ApriInventario()
        {

        }
    }
}
