using CitizenFX.Core;
using System.Collections.Generic;

namespace TheLastPlanet.Client.RolePlay.Negozi
{
	internal class ConfigBarbieri
	{
		public static List<NegozioBarbiere> Kuts = new List<NegozioBarbiere> { new NegozioBarbiere(new Vector3(-279.154f, 6226.192f, 31.705f), new BarberModel("a_m_y_stbla_02", "S_M_M_HAIRDRESSER_01_BLACK_MINI_01", new Vector4(-278.205f, 6230.279f, 31.696f, 49.216f)), new Vector3(-278.4f, 6225.8f, 31.58f), new Vector3(-279.59f, 6226.81f, 31.58f), new Vector3(-280.61f, 6227.88f, 31.58f), 133.69f, new Vector3(0, -1.8391f, -0.1795f), 313.69f), new NegozioBarbiere(new Vector3(1211.0759277344f, -475.00064086914f, 66.218032836914f), new BarberModel("a_m_y_stbla_02", "S_M_M_HAIRDRESSER_01_BLACK_MINI_01", new Vector4(1211.521f, -470.704f, 66.208f, 79.543f)), new Vector3(1213.38f, -474.84f, 66.0f), new Vector3(1211.9f, -474.54f, 66.0f), new Vector3(1210.45f, -474.14f, 66.0f), 163.6f, new Vector3(0, -1.8391f, -0.1795f), 343.6f), new NegozioBarbiere(new Vector3(139.21583557129f, -1708.9689941406f, 29.301620483398f), new BarberModel("s_f_m_fembarber", "S_F_M_FEMBARBER_BLACK_MINI_01", new Vector4(-278.205f, 6230.279f, 31.696f, 49.216f)), new Vector3(-278.4f, 6225.8f, 31.58f), new Vector3(-279.59f, 6226.81f, 31.58f), new Vector3(-280.61f, 6227.88f, 31.58f), 226.24f, new Vector3(1.0109f, -0.8391f, -0.1795f), 0) };
		public static NegozioBarbiere Hawick = new NegozioBarbiere(new Vector3(-34.97777557373f, -150.9037322998f, 57.086517333984f), new BarberModel("a_m_y_stbla_02", "S_M_M_HAIRDRESSER_01_BLACK_MINI_01", new Vector4(-30.804f, -151.648f, 57.077f, 349.238f)), new Vector3(-35.31f, -153.19f, 56.89f), new Vector3(-34.67f, -151.79f, 56.89f), new Vector3(-34.17f, -150.40f, 56.89f), 68.48f, new Vector3(0, -1.8391f, -0.1795f), 248.48f);
		public static NegozioBarbiere Osheas = new NegozioBarbiere(new Vector3(1934.115234375f, 3730.7399902344f, 32.854434967041f), new BarberModel("s_f_m_fembarber", "S_F_M_FEMBARBER_BLACK_MINI_01", new Vector4(1930.855f, 3728.141f, 32.844f, 220.243f)), new Vector3(1932.51f, 3732.48f, 32.75f), new Vector3(1933.23f, 3731.19f, 33.21f), new Vector3(1933.93f, 3729.87f, 33.21f), 301.54f, new Vector3(0, -1.8391f, -0.1795f), 0);
		public static NegozioBarbiere Combo = new NegozioBarbiere(new Vector3(-1281.9802246094f, -1119.6861572266f, 7.0001249313354f), new BarberModel("s_f_m_fembarber", "S_F_M_FEMBARBER_BLACK_MINI_01", new Vector4(-1284.038f, -1115.635f, 6.990f, 85.177f)), new Vector3(-1281.26f, -1119.01f, 6.85f), new Vector3(-1282.81f, -1118.99f, 7.0f), new Vector3(-1284.3f, -1118.95f, 7.0f), -180.0f, new Vector3(0, -1.8391f, -0.1795f), 0);
		public static NegozioBarbiere Mulet = new NegozioBarbiere(new Vector3(-815.59008789063f, -182.16806030273f, 37.568920135498f), new BarberModel("s_f_m_fembarber", "S_F_M_FEMBARBER_BLACK_MINI_01", new Vector4(-822.9f, -183.87f, 37.569f, 225.98f)), new Vector3(-817.92f, -184.05f, 37.0f), new Vector3(-816.21f, -183.06f, 37.89f), new Vector3(-814.42f, -183.03f, 37.89f), 27.5f, new Vector3(0.186f, -1.4006f, 0.12f), 207.5f);
	}

	public class NegozioBarbiere
	{
		public Vector3 Coord;
		public BarberModel Model;
		public Vector3 Sedia1;
		public Vector3 Sedia2;
		public Vector3 Sedia3;
		public float Heading;
		public Vector3 CXYZ;
		public float Cam;

		public NegozioBarbiere(Vector3 coord, BarberModel model, Vector3 sedia1, Vector3 sedia2, Vector3 sedia3, float heading, Vector3 cxyz, float cam)
		{
			Coord = coord;
			Model = model;
			Sedia1 = sedia1;
			Sedia2 = sedia2;
			Sedia3 = sedia3;
			Heading = heading;
			CXYZ = cxyz;
			Cam = cam;
		}
	}

	public class BarberModel
	{
		public string Model;
		public string Voice;
		public Vector4 Coords;

		public BarberModel(string model, string voice, Vector4 coords)
		{
			Model = model;
			Voice = voice;
			Coords = coords;
		}
	}
}