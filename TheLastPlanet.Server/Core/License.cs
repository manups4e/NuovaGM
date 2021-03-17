using CitizenFX.Core;

namespace TheLastPlanet.Server.Core
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
		public static string GetLicense(this Player player, Identifier identifier)
		{
			return identifier switch
			{
				Identifier.Steam   => player.Identifiers["steam"],
				Identifier.License => player.Identifiers["license2"],
				Identifier.Discord => player.Identifiers["discord"],
				Identifier.Fivem   => player.Identifiers["fivem"],
				Identifier.Ip      => player.Identifiers["ip"],
				_                  => null
			};
		}
	}
}