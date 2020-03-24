﻿using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Client
{
    /// <summary>
    /// Pagina di riferimento: https://wiki.fivem.net/wiki/Controls
    /// Oppure /dev keycodetester in gioro e oremere un tasto
    /// Occhio.. i tasti premono piu comandi per volta
    /// </summary>
    public static class Input
    {
        // da far diventare costanti
        const int defaultControlGroup = 0;
        const int controllerControlGroup = 2;
        public static Dictionary<ControlModifier, int> ModifierFlagToKeyCode = new Dictionary<ControlModifier, int>()
        {
            [ControlModifier.Ctrl] = 36,
            [ControlModifier.Alt] = 19,
            [ControlModifier.Shift] = 21
        };

        /// <summary>
        /// Vero se a premere è stato il controller
        /// </summary>
        /// <returns></returns>
        public static bool WasLastInputFromController()
        {
            return !IsInputDisabled(controllerControlGroup);
        }

        /// <summary>
        /// Tiene conto se un modifier (alt, ctrl, shift) è stato premuto
        /// </summary>
        /// <param name="modifier">Si può specificare 1 solo modifier o più di uno (con |)</param>
        /// <returns></returns>
        public static bool IsControlModifierPressed(ControlModifier modifier)
        {
 //           if (Phone.PhoneKeyboardOpen) return false;

            if (modifier == ControlModifier.Any)
                return true;
            else
            {
                ControlModifier BitMask = 0;
                ModifierFlagToKeyCode.ToList().ForEach(w =>
                {
                    if (Game.IsControlPressed(defaultControlGroup, (Control)w.Value))
                        BitMask |= w.Key;
                });
                if (BitMask == modifier)
                    return true;
                else
                    return false;
            }
        }

        public static bool IsControlJustPressed(Control control, bool keyboardOnly = true, ControlModifier modifier = ControlModifier.None)
        {
            return Game.IsControlJustPressed(0, control) && ((!keyboardOnly && WasLastInputFromController()) || (keyboardOnly && !WasLastInputFromController())) && IsControlModifierPressed(modifier);
        }

        public static bool IsControlPressed(Control control, bool keyboardOnly = true, ControlModifier modifier = ControlModifier.None)
        {
            return Game.IsControlPressed(0, control) && ((!keyboardOnly && WasLastInputFromController()) || (keyboardOnly && !WasLastInputFromController())) && IsControlModifierPressed(modifier);
        }

        public static bool IsControlJustReleased(Control control, bool keyboardOnly = true, ControlModifier modifier = ControlModifier.None)
        {
            return Game.IsControlJustReleased(0, control) && ((!keyboardOnly && WasLastInputFromController()) || (keyboardOnly && !WasLastInputFromController())) && IsControlModifierPressed(modifier);
        }

        public static bool IsDisabledControlJustPressed(Control control, bool keyboardOnly = true, ControlModifier modifier = ControlModifier.None)
        {
            return Game.IsDisabledControlJustPressed(0, control) && ((!keyboardOnly && WasLastInputFromController()) || (keyboardOnly && !WasLastInputFromController())) && IsControlModifierPressed(modifier);
        }

        public static bool IsDisabledControlJustReleased(Control control, bool keyboardOnly = true, ControlModifier modifier = ControlModifier.None)
        {
            return Game.IsDisabledControlJustReleased(0, control) && ((!keyboardOnly && WasLastInputFromController()) || (keyboardOnly && !WasLastInputFromController())) && IsControlModifierPressed(modifier);
        }

        public static bool IsDisabledControlPressed(Control control, bool keyboardOnly = true, ControlModifier modifier = ControlModifier.None)
        {
            return Game.IsDisabledControlPressed(0, control) && ((!keyboardOnly && WasLastInputFromController()) || (keyboardOnly && !WasLastInputFromController())) && IsControlModifierPressed(modifier);
        }

        public static bool IsEnabledControlJustPressed(Control control, bool keyboardOnly = true, ControlModifier modifier = ControlModifier.None)
        {
            return Game.IsEnabledControlJustPressed(0, control) && ((!keyboardOnly && WasLastInputFromController()) || (keyboardOnly && !WasLastInputFromController())) && IsControlModifierPressed(modifier);
        }

        public static bool IsEnabledControlJustReleased(Control control, bool keyboardOnly = true, ControlModifier modifier = ControlModifier.None)
        {
            return Game.IsEnabledControlJustPressed(0, control) && ((!keyboardOnly && WasLastInputFromController()) || (keyboardOnly && !WasLastInputFromController())) && IsControlModifierPressed(modifier);
        }

        public static bool IsEnabledControlPressed(Control control, bool keyboardOnly = true, ControlModifier modifier = ControlModifier.None)
        {
            return Game.IsEnabledControlPressed(0, control) && ((!keyboardOnly && WasLastInputFromController()) || (keyboardOnly && !WasLastInputFromController())) && IsControlModifierPressed(modifier);
        }

        /// <summary>
        /// Aspetta finchè un tasto non è stato rilasciato
        /// </summary>
        /// <param name="control">Il <see cref="Control"/> che si vuole aspettare</param>
        /// <param name="modifier">Il <see cref="ControlModifier"/> aggiuntivo</param>
        /// <param name="timeout">Quanto aspettare prima che il controllo cominci a verificare in millisecondi</param>
        /// <returns>Ritorna se il player ha tenuto premuto più del tempo specificato</returns>
        public static async Task<bool> WaitForKeyRelease(Control control, ControlModifier modifier = ControlModifier.None, int timeout = 1000)
        {
            var currentTicks = Game.GameTime + 1;

            await BaseScript.Delay(0);

            while (IsControlPressed(control, !WasLastInputFromController(), modifier) && Game.GameTime - currentTicks < timeout)
                await BaseScript.Delay(0);

            return Game.GameTime - currentTicks >= timeout;
        }
    }
}