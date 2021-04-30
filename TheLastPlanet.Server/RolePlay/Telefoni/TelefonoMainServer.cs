using CitizenFX.Core;
using Logger;
using Newtonsoft.Json;
using TheLastPlanet.Server.Core;
using TheLastPlanet.Shared;
using System;
using System.Collections.Concurrent;

namespace TheLastPlanet.Server.Telefoni
{
	static class TelefonoMainServer
	{
		public static ConcurrentDictionary<string, Phone> Phones = new ConcurrentDictionary<string, Phone>();

		public static void Init()
		{
			//Server.Instance.AddEventHandler("lprp:setupUser", new Action<Player>(SetupPhone));
			Server.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action<Player>(SetupPhone));
		}

		private static async void SetupPhone([FromSource] Player player)
		{
			await BaseScript.Delay(0);
			try
			{
				dynamic result = await Server.Instance.Query("SELECT * FROM telefoni WHERE discord = @disc", new { disc = License.GetLicense(player, Identifier.Discord) });
				await BaseScript.Delay(0);
				string valore = JsonConvert.SerializeObject(result);
				if (valore != "[]" && valore != "{}" && valore != null)
				{
					Phones[player.Handle] = new Phone(player, result[0]);
					string datiphone = (Phones[player.Handle]).ToJson();
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
					await BaseScript.Delay(0);
					dynamic Newresult = await Server.Instance.Query("SELECT * FROM telefoni WHERE discord = @disc", new { disc = License.GetLicense(player, Identifier.Discord) });
					await BaseScript.Delay(0);
					Phones[player.Handle] = new Phone(player, Newresult[0]);
					string datiphone = (Phones[player.Handle]).ToJson();
					player.TriggerEvent("lprp:setupPhoneClientUser", datiphone);
				}
			}
			catch( Exception e)
			{
				Server.Logger.Error( e.ToString());
				Server.Logger.Error( e.StackTrace);
			}
		}
	}
}
