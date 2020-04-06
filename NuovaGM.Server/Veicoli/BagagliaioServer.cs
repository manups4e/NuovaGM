﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Server.Veicoli
{
	static class BagagliaioServer
	{
		public static ConcurrentDictionary<string, string> BagagliaiGenerici = new ConcurrentDictionary<string, string>();
		public static void Init()
		{
			Server.Instance.RegisterEventHandler("lprp:bagagliaio:getTrunksContents", new Action<Player, string>(GestisciBagagliaio));
		}

		private static async void GestisciBagagliaio([FromSource] Player player, string plate)
		{

		}
	}
}
