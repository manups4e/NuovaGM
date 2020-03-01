using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NuovaGM.Client.Telefono;
using NuovaGM.Client.Telefono.Models;
using NuovaGM.Shared;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.gmPrincipale.Utility;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.Native;


namespace NuovaGM.Client.Telefono.Apps
{
	class QuickSave : App
	{
		public QuickSave(Phone phone) : base("Salvataggio Rapido", 43, phone)
		{

		}

		private static bool FirstTick = true;
		public override async Task Tick()
		{
			if (FirstTick)
			{
				FirstTick = false;
				await BaseScript.Delay(100);
				return;
			}
		}

		public override async void Initialize(Phone phone)
		{
			Phone = phone;
			BaseScript.TriggerServerEvent("lprp:salvaPlayer");
			BaseScript.TriggerEvent("lprp:phone_start", "Main");
			await BaseScript.Delay(5000);
			HUD.ShowNotification("Salvataggio Completato", NotificationColor.GreenDark);

		}

		public override void Kill()
		{
		}

	}
}
