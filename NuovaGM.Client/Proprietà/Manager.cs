using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using Newtonsoft.Json;

namespace NuovaGM.Client.Proprietà
{
	static class Manager
	{
		public static void Init()
		{
			// CREO OGGETTO NEI DATI DEL PERSONAGGIO DEL PLAYER... COSI POSSO GESTIRLO IN BASE AL PERSONAGGIO :)

//			Client.GetInstance.RegisterEventHandler("lprp:appartamenti:carica", new Action<string>(CaricaLeMieProprieta));
		}
	}
}
