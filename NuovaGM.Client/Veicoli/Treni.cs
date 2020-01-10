using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NuovaGM.Shared.Veicoli;

namespace NuovaGM.Client.Veicoli
{
	static class Treni
	{
		static List<Vector3> MetroTrainStations = new List<Vector3>
		{
			new Vector3(-547.34057617188f, -1286.1752929688f, 25.3059978411511f),
			new Vector3(-892.66284179688f, -2322.5168457031f, -13.246466636658f),
			new Vector3(-1100.2299804688f, -2724.037109375f, -8.3086919784546f),
			new Vector3(-1071.4924316406f, -2713.189453125f, -8.9240007400513f),
			new Vector3(-875.61907958984f, -2319.8686523438f, -13.241264343262f),
			new Vector3(-536.62890625f, -1285.0009765625f, 25.301458358765f),
			new Vector3(270.09558105469f, -1209.9177246094f, 37.465930938721f),
			new Vector3(-287.13568115234f, -327.40936279297f, 8.5491418838501f),
			new Vector3(-821.34295654297f, -132.45257568359f, 18.436864852905f),
			new Vector3(-1359.9794921875f, -465.32354736328f, 13.531299591064f),
			new Vector3(-498.96591186523f, -680.65930175781f, 10.295949935913f),
			new Vector3(-217.97073364258f, -1032.1605224609f, 28.724565505981f),
			new Vector3(113.90325164795f, -1729.9976806641f, 28.453630447388f),
			new Vector3(117.33223724365f, -1721.9318847656f, 28.527353286743f),
			new Vector3(-209.84713745117f, -1037.2414550781f, 28.722997665405f),
			new Vector3(-499.3971862793f, -665.58514404297f, 10.295639038086f),
			new Vector3(-1344.5224609375f, -462.10494995117f, 13.531820297241f),
			new Vector3(-806.85192871094f, -141.39852905273f, 18.436403274536f),
			new Vector3(-302.21514892578f, -327.28854370117f, 8.5495929718018f),
			new Vector3(262.01733398438f, -1198.6135253906f, 37.448017120361f)
		};

		static List<Vector3> train1platforms = new List<Vector3>
		{
			new Vector3(-543.84686279297f, -1287.7620849609f, 26.901607513428f),
			new Vector3(-883.95007324219f, -2318.7321777344f, -11.732789993286f),
			new Vector3(-1089.8125f, -2721.6364746094f, -7.410135269165f),
			new Vector3(273.87789916992f, -1204.3369140625f, 38.899459838867f),
			new Vector3(-294.69540405273f, -327.75311279297f, 10.063080787659f),
			new Vector3(-845.41217041016f, -155.26510620117f, 19.950351715088f),
			new Vector3(-1354.9700927734f, -459.91235351563f, 15.045303344727f),
			new Vector3(-498.40017700195f, -673.51654052734f, 11.809022903442f),
			new Vector3(-213.34342956543f, -1029.0593261719f, 30.140535354614f),
			new Vector3(118.84923553467f, -1730.1706542969f, 30.111122131348f),
			new Vector3(-212.40980529785f, -1035.8253173828f, 30.139507293701f)
		};

		static List<Vector3> trainstations2 = new List<Vector3>
		{
//			new Vector3(2072.4086914063f, 1569.0856933594f, 76.712524414063f),
			new Vector3(664.93090820313f, -997.59942626953f, 22.261747360229f),
			new Vector3(190.62687683105f, -1956.8131103516f, 19.520135879517f),
//		    new Vector3(2611.0278320313f, 1675.3806152344f, 26.578210830688f),
			new Vector3(2615.3901367188f, 2934.8666992188f, 39.312232971191f),
			new Vector3(2885.5346679688f, 4862.0146484375f, 62.551517486572f),
			new Vector3(47.061096191406f, 6280.8969726563f, 31.580261230469f),
			new Vector3(2002.3624267578f, 3619.8029785156f, 38.568252563477f),
			new Vector3(2609.7016601563f, 2937.11328125f, 39.418235778809f)
		};

		static string[] trainmodels = { "freight", "freightcar", "freightgrain", "freightcont1", "freightcont2", "freighttrailer", "tankercar", "metrotrain", "s_m_m_lsmetro_01" };
		static List<Treno> Trains = new List<Treno>();
		static List<Vector3> trainLocations = new List<Vector3>()
		{
			new Vector3(2606.0f, 2927.0f, 40.0f),
			new Vector3(2463.0f, 3872.0f, 38.8f),
			new Vector3(1164.0f, 6433.0f, 32.0f),
			new Vector3(537.0f, -1324.1f, 29.1f),
			new Vector3(219.1f, -2487.7f, 6.0f)
		};



		public static void Init()
		{
			Client.GetInstance.RegisterEventHandler("lprp:spawntrain", new Action(SpawnTrain));
			Client.GetInstance.RegisterEventHandler("lprp:onPlayerSpawn", new Action(Spawnato));
		}

		private static async void Spawnato()
		{
			foreach (var v in MetroTrainStations)
			{
				Blip p = new Blip(AddBlipForCoord(v.X, v.Y, v.Z))
				{
					Color = BlipColor.Green,
					Sprite = BlipSprite.Lift,
					Scale = 0.75f,
					Name = "Metropolitana"
				};
				SetBlipAsShortRange(p.Handle, true);
			}
		}

		private static async void SpawnTrain()
		{
			int traincount = 0;
			foreach (string p in trainmodels)
			{
				Model m = new Model(p);
				m.Request();
				while (!m.IsLoaded) await BaseScript.Delay(10);
			}
			Model n = new Model("s_m_m_lsmetro_01");
			n.Request();
			while (!n.IsLoaded) await BaseScript.Delay(10);
			foreach (Vehicle veh in World.GetAllVehicles())
			{
				int model = GetEntityModel(veh.Handle);
				bool istrain = IsThisModelATrain((uint)model);
				if (istrain) ++traincount;
			}

			if (traincount == 0)
			{
				for (int i = 0; i < trainLocations.Count; i++)
				{
					int tr1 = GetRandomIntInRange(0, 22);
					Treno train = new Treno();
					train.entity = CreateMissionTrain(tr1, trainLocations[i].X, trainLocations[i].Y, trainLocations[i].Z, true);
					SetEntityAsMissionEntity(train.entity, true, true);
					Trains.Add(train);
				}
			}
			foreach (Treno v in Trains)
			{
				CreatePedInsideVehicle(v.entity, 26, (uint)GetHashKey("s_m_m_lsmetro_01"), -1, true, true);
				SetVehicleDoorsLocked(v.entity, 0);
			}

			for (int i = 0; i < 4; i++)
			{
				int metroAvanti = CreateMissionTrain(24, 40.2f, -1201.3f, 31.0f, true);
				int metroIndietro = CreateMissionTrain(24, -618.0f, -1476.8f, 16.2f, true);
				CreatePedInsideVehicle(metroAvanti, 26, (uint)GetHashKey("s_m_m_lsmetro_01"), -1, true, true);
				SetVehicleDoorsLocked(metroAvanti, 0);
				CreatePedInsideVehicle(metroIndietro, 26, (uint)GetHashKey("s_m_m_lsmetro_01"), -1, true, true);
				SetVehicleDoorsLocked(metroIndietro, 0);
				SetEntityAsMissionEntity(metroAvanti, true, true);
				SetEntityAsMissionEntity(metroIndietro, true, true);
				await BaseScript.Delay(300000);
			}
			SetRandomTrains(true);
		}


		private static async Task Metropolitana()
		{

		}

	}
}
