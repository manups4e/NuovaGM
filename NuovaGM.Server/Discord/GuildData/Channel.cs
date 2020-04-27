using System;
using System.Collections.Generic;
using System.Text;

namespace NuovaGM.Server.Discord.GuildData
{
	public class Channel
	{
		public List<PermissionOverwrite> permission_overwrites = new List<PermissionOverwrite>();
		public long guild_id;
		public long parent_id;
		public string name;
		public int type;
		public int position;
		public string topic;
		public long last_message_id;
		public int bitrate;
		public int user_limit;
		public int rate_limit_per_user;
		public bool nsfw;
		public long id;
	}

	public class PermissionOverwrite
	{
		public int type;
		public int allow;
		public int deny;
		public long id;
	}
}
