using CitizenFX.Core;
using System;

namespace NuovaGM.Server.manager
{
	static class ServerManager
	{
		public static void Init()
		{
			Server.Instance.RegisterEventHandler("lprp:getDecor", new Action<Player>(getDecorandSendBack));
		}
		public static void getDecorandSendBack([FromSource] Player p)
		{
			p.TriggerEvent("lprp:setDecor", "NuovaGM2019fighissimo!yeah", 146254368);
		}
	}
}