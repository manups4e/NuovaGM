using TheLastPlanet.Shared.Internal.Events.Attributes;

namespace TheLastPlanet.Shared
{
	[Serialization]
	public partial class FRBlipsInfo
	{
		public string Name { get; set; }
		public Position Pos { get; set; }
		public int Sprite { get; set; } = 1;
		public int NetId { get; set; }
		public int ServerId {  get; set; }

		public FRBlipsInfo(string name, Position pos, int netId, int handle)
		{
			Name = name;
			Pos = pos;
			NetId = netId;
			ServerId = handle;
		}
	}
}