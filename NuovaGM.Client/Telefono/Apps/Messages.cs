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
		private bool MessaggioAperto = false;
		public Messages(Phone phone) : base("Messaggi", 2, phone)   // 8
		{
			Client.GetInstance.RegisterEventHandler("lprp:riceviMessaggio", new Action<int, string>(RiceviMessaggio));
			this.phone = phone;
		}

		private async void RiceviMessaggio(int sender, string messaggio)
		{
			Tuple<int, string> mugshot = await Funzioni.GetPedMugshotAsync(new Ped(GetPlayerPed(GetPlayerFromServerId(sender))));
			HUD.ShowAdvancedNotification("Messaggio Privato", Funzioni.GetPlayerCharFromPlayerId(sender).FullName, messaggio, mugshot.Item2, IconType.ChatBox);
			AddMessage(messageCount, messaggio, Funzioni.GetPlayerCharFromPlayerId(sender).FullName, false);
			messageCount += 1;
		}

		private async void AddMessage(int index, string sender, string messageTopic, bool sending)
		{
			PushScaleformMovieFunction(phone.Scaleform.Handle, "SET_DATA_SLOT");
			PushScaleformMovieFunctionParameterInt(8);
			PushScaleformMovieFunctionParameterInt(index);
			if (sending)
				PushScaleformMovieFunctionParameterInt(4);
			else
				PushScaleformMovieFunctionParameterInt(0);
			PushScaleformMovieFunctionParameterInt(0);
			BeginTextCommandScaleformString("CELL_2000");
			AddTextComponentSubstringPlayerName("~l~" + messageTopic);
			EndTextComponent();
			BeginTextCommandScaleformString("CELL_EMAIL_SUBJ");
			AddTextComponentSubstringPlayerName("~l~" + sender);
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
			
			Phone.Scaleform.CallFunction("SET_DATA_SLOT_EMPTY", 8);

			var appName = "Messaggi";

			foreach (var messaggio in Phone.getCurrentCharPhone().messaggi)
			{
				AddMessage(Phone.getCurrentCharPhone().messaggi.IndexOf(messaggio), messaggio.From, messaggio.Messaggio, false);
				messageCount += 1;
			}
			Phone.Scaleform.CallFunction("SET_HEADER", appName);
			Phone.Scaleform.CallFunction("DISPLAY_VIEW", 8, SelectedItem);

			var navigated = true;
			if (Game.IsControlJustPressed(0, Control.PhoneUp))
			{
				MoveFinger(1);
				if (SelectedItem > 0)
					SelectedItem -= 1;
			}
			else if (Game.IsControlJustPressed(0, Control.PhoneDown))
			{
				MoveFinger(2);
				if (SelectedItem < Phone.getCurrentCharPhone().messaggi.Count - 1)
					SelectedItem += 1;
				else
					SelectedItem = 0;
			}
			else if (Game.IsControlJustPressed(0, Control.FrontendAccept))
			{
				MoveFinger(5);
			}
			else if (Game.IsControlJustPressed(0, Control.FrontendCancel))
			{
				MoveFinger(5);
				if (MessaggioAperto)
				{

				}
				else
				{
					navigated = false;
					BaseScript.TriggerEvent("lprp:phone_start", "Main");
				}
			}
			else
			{
				navigated = false;
			}
			if (navigated)
			{
				Game.PlaySound("Menu_Navigate", "Phone_SoundSet_Default");
			}
			await Task.FromResult(0);
		}

		public override void Initialize(Phone phone)
		{
			Phone = phone;
			FirstTick = true;
			SelectedItem = 0;
			SetMobilePhoneRotation(-90.0f, 0.0f, 90.0f, 0);
			SetPhoneLean(true);
		}

		public override void Kill()
		{
		}

	}
}
