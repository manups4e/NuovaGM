using CitizenFX.Core;

namespace NuovaGM.Server.gmPrincipale
{
	public enum Identifier
	{
		Steam,
		License,
		Discord,
		Fivem,
		Ip
	}
	public static class License
	{
		public static string GetLicense(Player player, Identifier identifier)
		{

			switch (identifier)
			{
				case Identifier.Steam:
					return "steam:" + player.Identifiers["steam"];
				case Identifier.License:
					return "license:" + player.Identifiers["license"];
				case Identifier.Discord:
					return "discord:" + player.Identifiers["discord"];
				case Identifier.Fivem:
					return "FiveM:" + player.Identifiers["fivem"];
				case Identifier.Ip:
					return "ip:" + player.Identifiers["ip"];
				default:
					return null;
			}
		}
	}
}