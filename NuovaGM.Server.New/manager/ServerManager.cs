using CitizenFX.Core;
using System;

namespace NuovaGM.Server.manager
{
	static class ServerManager
	{
		public static void Init()
		{
			Server.GetInstance.RegisterEventHandler("lprp:getDecor", new Action<Player>(getDecorandSendBack));
		}
		public static void getDecorandSendBack([FromSource] Player p)
		{
			BaseScript.TriggerClientEvent(p, "lprp:setDecor", "NuovaGM2019fighissimo!yeah", 146254368);
		}
	}
}