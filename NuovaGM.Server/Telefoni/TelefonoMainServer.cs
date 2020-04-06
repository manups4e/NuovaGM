﻿using CitizenFX.Core;
using Newtonsoft.Json;
using NuovaGM.Server.gmPrincipale;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuovaGM.Server.Telefoni
{
	static class TelefonoMainServer
	{
		public static ConcurrentDictionary<string, Phone> Phones = new ConcurrentDictionary<string, Phone>();

		public static void Init()
		{
			//Server.Instance.RegisterEventHandler("lprp:setupUser", new Action<Player>(SetupPhone));
			Server.Instance.RegisterEventHandler("lprp:onPlayerSpawn", new Action<Player>(SetupPhone));
		}

		private static async void SetupPhone([FromSource] Player player)
		{
			await BaseScript.Delay(0);
			try
			{
				dynamic result = await Server.Instance.Query("SELECT * FROM telefoni WHERE discord = @disc", new { disc = License.GetLicense(player, Identifier.Discord) });
				string valore = JsonConvert.SerializeObject(result);
				if (valore != "[]" && valore != "{}" && valore != null)
				{
					Phones[player.Handle] = new Phone(player, result[0]);
					string datiphone = JsonConvert.SerializeObject(Phones[player.Handle]);
					player.TriggerEvent("lprp:setupPhoneClientUser", datiphone);
				}
				else
				{
					await Server.Instance.Execute("INSERT INTO telefoni (discord, playerName, phone_data) VALUES (@disc, @name, @data)", new
					{
						disc = License.GetLicense(player, Identifier.Discord),
						name = player.Name,
						data = "{}"
					});
					dynamic Newresult = await Server.Instance.Query("SELECT * FROM telefoni WHERE discord = @disc", new { disc = License.GetLicense(player, Identifier.Discord) });
					Phones[player.Handle] = new Phone(player, Newresult[0]);
					string datiphone = JsonConvert.SerializeObject(Phones[player.Handle]);
					player.TriggerEvent("lprp:setupPhoneClientUser", datiphone);
				}
			}
			catch( Exception e)
			{
				Debug.WriteLine(e.ToString());
				Debug.WriteLine(e.StackTrace);
			}
		}
	}
}
