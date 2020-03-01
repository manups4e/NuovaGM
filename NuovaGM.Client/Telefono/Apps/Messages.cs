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
	public class Messages : App
	{
		private int SelectedItem { get; set; } = 0;
		private static bool FirstTick = true;
		private Phone phone;
		private int messageCount=0;
		public Messages(Phone phone) : base("Messaggi", 2, phone)   // 8
		{
			Client.GetInstance.RegisterEventHandler("lprp:riceviMessaggio", new Action<int, string>(RiceviMessaggio));
			this.phone = phone;
		}

		private async void RiceviMessaggio(int sender, string messaggio)
		{
			Tuple<int, string> mugshot = await Funzioni.GetPedMugshotAsync(new Ped(GetPlayerPed(GetPlayerFromServerId(sender))));
			HUD.ShowAdvancedNotification("Messaggio Privato", Funzioni.GetPlayerCharFromPlayerId(sender).FullName, messaggio, mugshot.Item2, IconType.ChatBox);
			AddMessage(phone.Scaleform, messageCount, sender, messaggio, false);
			messageCount += 1;
		}

		private static async void AddMessage(Scaleform scaleform, int index, int sender, string messageTopic, bool sending)
		{
			PushScaleformMovieFunction(scaleform.Handle, "SET_DATA_SLOT");
			PushScaleformMovieFunctionParameterInt(8);
			PushScaleformMovieFunctionParameterInt(index);
			if (!sending)
				PushScaleformMovieFunctionParameterInt(0);
			else
				PushScaleformMovieFunctionParameterInt(4);
			PushScaleformMovieFunctionParameterInt(0);
			BeginTextComponent("STRING");
			AddTextComponentSubstringPlayerName("~l~" + messageTopic);
			EndTextComponent();
			BeginTextComponent("STRING");
			AddTextComponentSubstringPlayerName("~l~" + Funzioni.GetPlayerCharFromPlayerId(sender).FullName);
			EndTextComponent();
			PopScaleformMovieFunctionVoid();
		}

		public override async Task Tick()
		{
			if (FirstTick)
			{
				FirstTick = false;
				await BaseScript.Delay(100);
				return;
			}
			var appName = "Messaggi";
			Phone.Scaleform.CallFunction("SET_HEADER", appName);

		}

		public override void Initialize(Phone phone)
		{
			Phone = phone;
			FirstTick = true;
			SelectedItem = 0;
		}

		public override void Kill()
		{
		}

	}
}
