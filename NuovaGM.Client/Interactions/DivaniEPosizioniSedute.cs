﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using NuovaGM.Client.gmPrincipale.Utility.HUD;

namespace NuovaGM.Client.Interactions
{
	static class DivaniEPosizioniSedute
	{
		public static bool Seduto = false;
		private static bool stato = false;

		private static List<Vector3[]> Divani = new List<Vector3[]>()
		{
			new Vector3[6]{new Vector3(-797.691f, 339.522f, 205.693f), new Vector3(-797.671f, 338.612f, 205.693f), new Vector3(-797.671f, 337.652f, 205.693f), new Vector3(-798.831f, 336.392f, 205.684f), new Vector3(-799.761f, 336.402f, 205.684f), new Vector3(-800.721f, 336.402f, 205.684f) },
			new Vector3[6]{new Vector3(-758.3256f, 317.730469f, 174.8758f), new Vector3(-758.3456f, 318.640472f, 174.8758f), new Vector3(-758.3456f, 319.600464f, 174.8758f), new Vector3(-757.1856f, 320.860474f, 174.8668f), new Vector3(-756.2556f, 320.850464f, 174.8668f), new Vector3(-755.2956f, 320.850464f, 174.8668f)},
			new Vector3[6]{new Vector3(-758.4162f, 317.73407f, 221.329788f), new Vector3(-758.436157f, 318.644073f, 221.329788f), new Vector3(-758.436157f, 319.604065f, 221.329788f), new Vector3(-757.2762f, 320.864075f, 221.3208f), new Vector3(-756.3462f, 320.854065f, 221.3208f), new Vector3(-755.386169f, 320.854065f, 221.3208f)},
			new Vector3[6]{new Vector3(-798.1582f, 339.9142f, 158.0736f), new Vector3(-798.138245f, 339.0042f, 158.0736f), new Vector3(-798.138245f, 338.04422f, 158.0736f), new Vector3(-799.2982f, 336.7842f, 158.0646f), new Vector3(-800.2282f, 336.79422f, 158.0646f), new Vector3(-801.188232f, 336.79422f, 158.0646f)},
			new Vector3[6]{new Vector3(-257.3307f, -948.0441f, 75.3034f), new Vector3(-258.192627f, -947.751648f, 75.3034f), new Vector3(-259.094727f, -947.42334f, 75.3034f), new Vector3(-259.882019f, -945.902344f, 75.29441f), new Vector3(-259.554535f, -945.03186f, 75.29441f), new Vector3(-259.2262f, -944.129761f, 75.29441f)},
			new Vector3[6]{new Vector3(-285.8551f, -960.2964f, 90.58299f), new Vector3(-284.993164f, -960.588867f, 90.58299f), new Vector3(-284.091064f, -960.9172f, 90.58299f), new Vector3(-283.303772f, -962.4382f, 90.5740051f), new Vector3(-283.631256f, -963.308655f, 90.5740051f), new Vector3(-283.9596f, -964.210754f, 90.5740051f)},
			new Vector3[6]{new Vector3(-1474.09314f, -532.4244f, 67.62868f), new Vector3(-1473.55481f, -533.1583f, 67.62868f), new Vector3(-1473.00415f, -533.944763f, 67.62868f), new Vector3(-1473.23169f, -535.6422f, 67.61969f), new Vector3(-1473.99927f, -536.1674f, 67.61969f), new Vector3(-1474.78564f, -536.7181f, 67.61969f)},
			new Vector3[6]{new Vector3(-1474.09314f, -532.4244f, 55.0010567f), new Vector3(-1473.55481f, -533.1583f, 55.0010567f), new Vector3(-1473.00415f, -533.944763f, 55.0010567f), new Vector3(-1473.23169f, -535.6422f, 54.99207f), new Vector3(-1473.99927f, -536.1674f, 54.99207f), new Vector3(-1474.78564f, -536.7181f, 54.99207f)},
			new Vector3[6]{new Vector3(-882.9605f, -450.102142f, 124.6065f), new Vector3(-883.3992f, -449.3046f, 124.6065f), new Vector3(-883.8433f, -448.453522f, 124.6065f), new Vector3(-883.3978f, -446.799835f, 124.597511f), new Vector3(-882.568665f, -446.378479f, 124.597511f), new Vector3(-881.7176f, -445.934326f, 124.597511f)},
			new Vector3[6]{new Vector3(-915.4687f, -439.7672f, 119.679192f), new Vector3(-915.0421f, -440.5713f, 119.679192f), new Vector3(-914.6109f, -441.429f, 119.679192f), new Vector3(-915.0813f, -443.075775f, 119.670204f), new Vector3(-915.9167f, -443.484558f, 119.670204f), new Vector3(-916.7744f, -443.915771f, 119.670204f)},
			new Vector3[6]{new Vector3(-893.898438f, -428.0541f, 93.53317f), new Vector3(-894.7008f, -428.4839f, 93.53317f), new Vector3(-895.556763f, -428.918518f, 93.53317f), new Vector3(-897.2054f, -428.454681f, 93.5241852f), new Vector3(-897.6175f, -427.6209f, 93.5241852f), new Vector3(-898.0521f, -426.764923f, 93.5241852f)},
			new Vector3[6]{new Vector3(-37.6873436f, -575.467041f, 88.18688f), new Vector3(-37.97983f, -576.329f, 88.18688f), new Vector3(-38.3081665f, -577.2311f, 88.18688f), new Vector3(-39.8291321f, -578.0184f, 88.1778946f), new Vector3(-40.6996155f, -577.690857f, 88.1778946f), new Vector3(-41.60174f, -577.3625f, 88.1778946f)},
			new Vector3[6]{new Vector3(-9.528797f, -588.1019f, 98.30492f), new Vector3(-10.3907471f, -587.809448f, 98.30492f), new Vector3(-11.2928438f, -587.48114f, 98.30492f), new Vector3(-12.0801315f, -585.960144f, 98.29593f), new Vector3(-11.7526493f, -585.089661f, 98.29593f), new Vector3(-11.4243021f, -584.187561f, 98.29593f)},
			new Vector3[6]{new Vector3(-898.2065f, -375.3974f, 83.55256f), new Vector3(-898.636353f, -374.5951f, 83.55256f), new Vector3(-899.071045f, -373.739166f, 83.55256f), new Vector3(-898.6074f, -372.090485f, 83.54357f), new Vector3(-897.773743f, -371.678253f, 83.54357f), new Vector3(-896.9178f, -371.24353f, 83.54357f)},
			new Vector3[6]{new Vector3(-931.895264f, -375.881165f, 107.5124f), new Vector3(-931.4581f, -376.679535f, 107.5124f), new Vector3(-931.0156f, -377.5315f, 107.5124f), new Vector3(-931.464233f, -379.184326f, 107.50341f), new Vector3(-932.2942f, -379.6041f, 107.50341f), new Vector3(-933.1461f, -380.0466f, 107.50341f)},
			new Vector3[6]{new Vector3(-619.937256f, 64.49962f, 106.0991f), new Vector3(-619.9173f, 63.5896149f, 106.0991f), new Vector3(-619.9173f, 62.6296234f, 106.0991f), new Vector3(-621.0773f, 61.3696136f, 106.090111f), new Vector3(-622.007263f, 61.3796234f, 106.090111f), new Vector3(-622.9673f, 61.3796234f, 106.090111f)},
			new Vector3[6]{new Vector3(-581.4289f, 42.8178825f, 91.6982f), new Vector3(-581.448853f, 43.7278862f, 91.6982f), new Vector3(-581.448853f, 44.6878777f, 91.6982f), new Vector3(-580.2889f, 45.9478874f, 91.68921f), new Vector3(-579.3589f, 45.9378777f, 91.68921f), new Vector3(-578.398865f, 45.9378777f, 91.68921f)},
			new Vector3[6]{new Vector3(-1457.9978f, -560.0503f, 67.7401f), new Vector3(-1457.26392f, -559.511963f, 67.7401f), new Vector3(-1456.47754f, -558.961365f, 67.7401f), new Vector3(-1454.78f, -559.188843f, 67.73111f), new Vector3(-1454.25476f, -559.9564f, 67.73111f), new Vector3(-1453.7041f, -560.7428f, 67.73111f)},
			new Vector3[6]{new Vector3(-894.0826f, -373.474365f, 107.9705f), new Vector3(-894.5135f, -372.6726f, 107.9705f), new Vector3(-894.949341f, -371.817261f, 107.9705f), new Vector3(-894.4878f, -370.167969f, 107.96151f), new Vector3(-893.654663f, -369.754669f, 107.96151f), new Vector3(-892.799255f, -369.318848f, 107.96151f)},
			new Vector3[6]{new Vector3(-590.381165f, 43.0761948f, 91.896f), new Vector3(-590.4011f, 43.9862f, 91.896f), new Vector3(-590.4011f, 44.94619f, 91.896f), new Vector3(-589.24115f, 46.2062f, 91.88702f), new Vector3(-588.311157f, 46.19619f, 91.88702f), new Vector3(-587.351135f, 46.19619f, 91.88702f)},
			new Vector3[6]{new Vector3(-797.6909f, 339.522034f, 205.693f), new Vector3(-797.671f, 338.612f, 205.693f), new Vector3(-797.671f, 337.652f, 205.693f), new Vector3(-798.830933f, 336.392029f, 205.684f), new Vector3(-799.761f, 336.402039f, 205.684f), new Vector3(-800.721f, 336.402f, 205.684f)},
			new Vector3[6]{new Vector3(-781.31f, 335.92f, 210.65f), new Vector3(-782.17f, 335.91f, 210.65f), new Vector3(-783.07f, 335.91f, 210.65f), new Vector3(-783.82f, 336.97f, 210.66f), new Vector3(-783.82f, 337.83f, 210.66f), new Vector3(-783.81f, 338.68f, 210.66f)},
			new Vector3[6]{new Vector3(-1464.443f, -544.5658f, 72.6971f), new Vector3(-1463.94153f, -545.264465f, 72.6971f), new Vector3(-1463.42529f, -546.0018f, 72.6971f), new Vector3(-1463.8634f, -547.2241f, 72.70711f), new Vector3(-1464.56787f, -547.717346f, 72.70711f), new Vector3(-1465.2699f, -548.1967f, 72.70711f)},
			new Vector3[6]{new Vector3(-910.3134f, -377.701782f, 112.9275f), new Vector3(-909.5517f, -377.30246f, 112.9275f), new Vector3(-908.749756f, -376.89386f, 112.9275f), new Vector3(-907.6003f, -377.4978f, 112.937508f), new Vector3(-907.2099f, -378.264069f, 112.937508f), new Vector3(-906.8329f, -379.02597f, 112.937508f)},
			new Vector3[6]{new Vector3(-606.762146f, 46.6781845f, 96.8530045f), new Vector3(-605.902161f, 46.6881943f, 96.8530045f), new Vector3(-605.002136f, 46.6881943f, 96.8530045f), new Vector3(-604.252136f, 45.6281967f, 96.8630142f), new Vector3(-604.252136f, 44.76821f, 96.8630142f), new Vector3(-604.262146f, 43.9182053f, 96.8630142f)},
			new Vector3[6]{new Vector3(-23.6634979f, -584.855469f, 78.6837f), new Vector3(-24.4744453f, -584.5691f, 78.6837f), new Vector3(-25.3195457f, -584.25946f, 78.6837f), new Vector3(-25.6591511f, -583.006165f, 78.69371f), new Vector3(-25.363327f, -582.198669f, 78.69371f), new Vector3(-25.0615368f, -581.404f, 78.69371f)},
			new Vector3[6]{new Vector3(-781.309937f, 335.919922f, 210.65f), new Vector3(-782.170044f, 335.9099f, 210.65f), new Vector3(-783.069946f, 335.9099f, 210.65f), new Vector3(-783.819946f, 336.969971f, 210.66f), new Vector3(-783.819946f, 337.830078f, 210.66f), new Vector3(-783.809937f, 338.679932f, 210.66f)},
			new Vector3[6]{new Vector3(-600.517944f, -1060.37878f, 161.15f), new Vector3(-599.7778f, -1059.94092f, 161.15f), new Vector3(-598.997864f, -1059.49182f, 161.15f), new Vector3(-597.819f, -1060.036f, 161.16f), new Vector3(-597.3898f, -1060.78113f, 161.16f), new Vector3(-596.9741f, -1061.52283f, 161.16f)},
		};

		public static async Task DivaniCasa()
		{
			Vector3 pedpos = Game.PlayerPed.Position;
			if (!Seduto)
			{
				for (int i = 0; i < Divani.Count; i++)
				{
					for (int j = 0; j < Divani[i].Length; j++)
					{
						if (Game.PlayerPed.IsInRangeOf(Divani[i][j], 1.5f))
						{
							HUD.ShowHelp(GetLabelText("MPTV_WALK"));
							if (Input.IsControlJustPressed(Control.Context))
							{
								if (DoesScenarioExistInArea(Divani[i][j].X, Divani[i][j].Y, Divani[i][j].Z, 2f, true))
								{
									SetPedConfigFlag(PlayerPedId(), 414, true);
									TaskUseNearestScenarioToCoord(PlayerPedId(), Divani[i][j].X, Divani[i][j].Y, Divani[i][j].Z, 2f, 5000);
									Seduto = true;
									Client.Instance.AddTick(Televisione);
								}
							}
						}
					}
				}
			}
		}

		public static async Task Televisione()
		{
			if (Seduto /*&&controllo per casa o hotel*/)
			{
				if (!stato)
				{
					HUD.ShowHelp(GetLabelText("MPTV_SEAT"));
					if (Input.IsControlJustPressed(Control.Context))
					{
						Client.Instance.AddTick(Televisioni.ControllaTV);
						stato = true;
					}
					else if (IsControlJustPressed(0, IsInputDisabled(2) ? 177 : 202))
					{
						Game.PlayerPed.Task.ClearAll();
						Seduto = false;
						SetPedConfigFlag(PlayerPedId(), 414, false);
					}
				}
				else if (stato && IsControlJustPressed(0, IsInputDisabled(2) ? 177 : 202))
				{
					Client.Instance.RemoveTick(Televisioni.ControllaTV);
					stato = false;
					SetPedConfigFlag(PlayerPedId(), 414, false);
				}
			}
		}
	}
}
