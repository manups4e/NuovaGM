﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NuovaGM.Server.Discord.GuildData
{
	public class Guild
	{
		public List<Ruolo> Roles = new List<Ruolo>();
		public List<Emoji> emojis = new List<Emoji>();
		public List<VoiceState> voice_states = new List<VoiceState>();
		public List<Channel> channels = new List<Channel>();
		public string name;
		public string icon;
		public string preferred_locale;
		public long owner_id;
		public string region;
		public long afk_channel_id;
		public int afk_timeout;
		public bool embed_enabled;
		public long embed_channel_id;
		public int verification_level;
		public int default_message_notifications;
		public int explicit_content_filter;
		public long system_channel_id;
		public int system_channel_flags;
		public dynamic rules_channel_id; // da capire
		public dynamic public_updates_channel_id; // da capire
		public dynamic application_id; // da capire
		public List<dynamic> features = new List<dynamic>(); // da capire
		public int mfa_level;
		public DateTime joined_at;
		public bool large;
		public bool unavailable;
		public int member_count;
		public dynamic max_members; // da capire
		public dynamic max_presences; // da capire
		public bool is_owner;
		public dynamic vanity_url_code; // da capire
		public dynamic description; // da capire
		public dynamic banner; // da capire
		public long premium_tier = 0;
		public int premium_subscription_count;
		public long id;
	}
}
