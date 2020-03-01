using CitizenFX.Core;
using Newtonsoft.Json;
using NuovaGM.Server.gmPrincipale;
using NuovaGM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuovaGM.Server.Telefoni
{
	public class Phone
	{
		dynamic data;
		public List<Phone_data> phone_data = new List<Phone_data>();
		public Phone() { }
		[JsonIgnore]
		public Player p;
		public Phone(Player player, dynamic result)
		{
			p = player;
			data = JsonConvert.DeserializeObject(result.phone_data);
			for (int i = 0; i < data.Count; i++)
				phone_data.Add(new Phone_data(data[i]));
		}

		public Phone_data getCurrentCharPhone()
		{
			for (int i = 0; i < phone_data.Count; i++)
			{
				if ((ServerEntrance.PlayerList[p.Handle].char_current - 1) == phone_data[i].id - 1)
					return phone_data[i];
			}
			return null;
		}
	}
}
