using System.Collections.Generic;

namespace NuovaGM.Shared
{
	public class GasStation
	{
		public float[] pos = new float[3];
		public List<float[]> pumps = new List<float[]>();
		public float[] ppos = new float[3];
		public int sellprice;
		public GasStation() { }
		public GasStation(dynamic data)
		{
			pos = new float[3] { (float)data["pos"][0].Value, (float)data["pos"][1].Value, (float)data["pos"][2].Value };
			ppos = new float[3] { (float)data["ppos"][0].Value, (float)data["ppos"][1].Value, (float)data["ppos"][2].Value };
			for (int i = 0; i < data["pumps"].Count; i++)
			{
				pumps.Add(new float[3] { (float)data["pumps"][i][0].Value, (float)data["pumps"][i][1].Value, (float)data["pumps"][i][2].Value });
			}

			sellprice = (int)data["sellprice"].Value;
		}
	}
}
