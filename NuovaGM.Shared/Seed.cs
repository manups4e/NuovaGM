using System;
using System.Security.Cryptography;

namespace TheLastPlanet.Shared
{
	public class Seed
	{
		public static string Generate()
		{
			var randomBytes = new byte[10];

			using (var rng = new RNGCryptoServiceProvider())
			{
				rng.GetBytes(randomBytes);
			}

			var timestampBytes = BitConverter.GetBytes(DateTime.UtcNow.Ticks / 10000L);

			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(timestampBytes);
			}

			var guidBytes = new byte[16];

			Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);
			Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);

			if (!BitConverter.IsLittleEndian) return new Guid(guidBytes).ToString();

			Array.Reverse(guidBytes, 0, 4);
			Array.Reverse(guidBytes, 4, 2);

			return new Guid(guidBytes).ToString();
		}
	}
}