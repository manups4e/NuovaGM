using NuovaGM.Shared;
using System.Collections.Generic;

namespace NuovaGM.Client.Lavori.Whitelistati.Polizia
{
	public class InfoPolizia
	{
		public string discord;
		public string Name;
		public int char_current;
		public List<Char_data> Char_data = new List<Char_data>();

		public InfoPolizia(string disc, string name, int current, dynamic chardata)
		{
			discord = disc;
			Name = name;
			char_current = current;
			for (int i = 0; i < chardata.Count; i++)
			{
				Char_data.Add(new Char_data(chardata[i]));
			}
		}
	}
}
