﻿using System.Collections.Generic;
using CitizenFX.Core;

namespace Impostazioni.Client.Configurazione.Lavori.Generici
{
    public class Tassisti
    {
        public Vector3 PosAccettazione = new (894.88f, -180.23f, 74.5f);
        public Vector3 PosRitiroVeicolo = new Vector3(915.039f, -162.187f, 74.5f);
        public Vector3 PosDepositoVeicolo = new (908.317f, -183.070f, 74.201f);
        public Vector4 PosSpawnVeicolo = new(911.108f, -177.867f, 74.283f, 225.0f);
        public float PrezzoModifier = 0.45f;
        public float pickupRange = 280f;

        public List<Vector3> jobCoords = new List<Vector3>()
        {
	        new Vector3(293.476f, -590.163f, 42.7371f),
	        new Vector3(253.404f, -375.86f, 44.0819f),
	        new Vector3(120.808f, -300.416f, 45.1399f),
	        new Vector3(-38.4132f, -381.576f, 38.3456f),
	        new Vector3(-107.442f, -614.377f, 35.6703f),
	        new Vector3(-252.292f, -856.474f, 30.5626f),
	        new Vector3(-236.138f, -988.382f, 28.7749f),
	        new Vector3(-276.989f, -1061.18f, 25.6853f),
	        new Vector3(-576.451f, -998.989f, 21.785f),
	        new Vector3(-602.798f, -952.63f, 21.6353f),
	        new Vector3(-790.653f, -961.855f, 14.8932f),
	        new Vector3(-912.588f, -864.756f, 15.0057f),
	        new Vector3(-1069.77f, -792.513f, 18.8075f),
	        new Vector3(-1306.94f, -854.085f, 15.0959f),
	        new Vector3(-1468.51f, -681.363f, 26.178f),
	        new Vector3(-1380.89f, -452.7f, 34.0843f),
	        new Vector3(-1326.35f, -394.81f, 36.0632f),
	        new Vector3(-1383.68f, -269.985f, 42.4878f),
	        new Vector3(-1679.61f, -457.339f, 39.4048f),
	        new Vector3(-1812.45f, -416.917f, 43.6734f),
	        new Vector3(-2043.64f, -268.291f, 22.9927f),
	        new Vector3(-2186.45f, -421.595f, 12.6776f),
	        new Vector3(-1862.08f, -586.528f, 11.1747f),
	        new Vector3(-1859.5f, -617.563f, 10.8788f),
	        new Vector3(-1634.95f, -988.302f, 12.6241f),
	        new Vector3(-1283.99f, -1154.16f, 5.30998f),
	        new Vector3(-1126.47f, -1338.08f, 4.63434f),
	        new Vector3(-867.907f, -1159.67f, 5.00795f),
	        new Vector3(-847.55f, -1141.38f, 6.27591f),
	        new Vector3(-722.625f, -1144.6f, 10.2176f),
	        new Vector3(-575.503f, -318.446f, 34.5273f),
	        new Vector3(-592.309f, -224.853f, 36.1209f),
	        new Vector3(-559.594f, -162.873f, 37.7547f),
	        new Vector3(-534.992f, -65.6695f, 40.634f),
	        new Vector3(-758.234f, -36.6906f, 37.2911f),
	        new Vector3(-1375.88f, 20.9516f, 53.2255f),
	        new Vector3(-1320.25f, -128.018f, 48.097f),
	        new Vector3(-1285.71f, 294.287f, 64.4619f),
	        new Vector3(-1245.67f, 386.533f, 75.0908f),
	        new Vector3(-760.355f, 285.015f, 85.0667f),
	        new Vector3(-626.786f, 254.146f, 81.0964f),
	        new Vector3(-563.609f, 267.962f, 82.5116f),
	        new Vector3(-486.806f, 271.977f, 82.8187f),
	        new Vector3(88.295f, 250.867f, 108.188f),
	        new Vector3(234.087f, 344.678f, 105.018f),
	        new Vector3(434.963f, 96.707f, 99.1713f),
	        new Vector3(482.617f, -142.533f, 58.1936f),
	        new Vector3(762.651f, -786.55f, 25.8915f),
	        new Vector3(809.06f, -1290.8f, 25.7946f),
	        new Vector3(490.819f, -1751.37f, 28.0987f),
	        new Vector3(432.351f, -1856.11f, 27.0352f),
	        new Vector3(164.348f, -1734.54f, 28.8982f),
	        new Vector3(-57.6909f, -1501.4f, 31.1084f),
	        new Vector3(52.2241f, -1566.65f, 29.006f),
	        new Vector3(310.222f, -1376.76f, 31.4442f),
	        new Vector3(181.967f, -1332.79f, 28.8773f),
	        new Vector3(-74.6091f, -1100.64f, 25.738f),
	        new Vector3(-887.045f, -2187.46f, 8.13248f),
	        new Vector3(-749.584f, -2296.59f, 12.4627f),
	        new Vector3(-1064.83f, -2560.66f, 19.6811f),
	        new Vector3(-1033.44f, -2730.24f, 19.6868f),
	        new Vector3(-1018.67f, -2732f, 13.2687f),
	        new Vector3(797.4f, -174.4f, 72.7f),
	        new Vector3(508.2f, -117.9f, 60.8f),
	        new Vector3(159.5f, -27.6f, 67.4f),
	        new Vector3(-36.4f, -106.9f, 57.0f),
	        new Vector3(-355.8f, -270.4f, 33.0f),
	        new Vector3(-831.2f, -76.9f, 37.3f),
	        new Vector3(-1038.7f, -214.6f, 37.0f),
	        new Vector3(1918.4f, 3691.4f, 32.3f),
	        new Vector3(1820.2f, 3697.1f, 33.5f),
	        new Vector3(1619.3f, 3827.2f, 34.5f),
	        new Vector3(1418.6f, 3602.2f, 34.5f),
	        new Vector3(1944.9f, 3856.3f, 31.7f),
	        new Vector3(2285.3f, 3839.4f, 34.0f),
	        new Vector3(2760.9f, 3387.8f, 55.7f),
	        new Vector3(1952.8f, 2627.7f, 45.4f),
	        new Vector3(1051.4f, 474.8f, 93.7f),
	        new Vector3(866.4f, 17.6f, 78.7f),
	        new Vector3(319.0f, 167.4f, 103.3f),
	        new Vector3(88.8f, 254.1f, 108.2f),
	        new Vector3(-44.9f, 70.4f, 72.4f),
	        new Vector3(-115.5f, 84.3f, 70.8f),
	        new Vector3(-384.8f, 226.9f, 83.5f),
	        new Vector3(-578.7f, 139.1f, 61.3f),
	        new Vector3(-651.3f, -584.9f, 34.1f),
	        new Vector3(-571.8f, -1195.6f, 17.9f),
	        new Vector3(-1513.3f, -670.0f, 28.4f),
	        new Vector3(-1297.5f, -654.9f, 26.1f),
	        new Vector3(-1645.5f, 144.6f, 61.7f),
	        new Vector3(-1160.6f, 744.4f, 154.6f),
	        new Vector3(-798.1f, 831.7f, 204.4f)
        };
    }
}