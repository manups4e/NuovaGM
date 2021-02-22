using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
#if SERVER
using TheLastPlanet.Server.Core;
#endif
// ReSharper disable All

namespace TheLastPlanet.Shared
{
	public class ChatMessage
	{

	}
	public class ChatSuggestion
	{
		public string help;
		public SuggestionParam[] @params;

		public ChatSuggestion(string help, SuggestionParam[] args)
		{
			this.help = help;
			@params = args;
		}
		public ChatSuggestion(string help)
		{
			this.help = help;
			@params = new SuggestionParam[0];
		}
	}
	public class SuggestionParam
	{
		public string name;
		public string help;
		public SuggestionParam(string name, string help)
		{
			this.name = name;
			this.help = help;
		}
	}
	public class ChatCommand
	{
		public string CommandName;
		public Delegate Action;
		public Player Source;
		public string rawCommand;
		public List<string> Args;
		public UserGroup Restriction;
		public ChatCommand(string commandName, UserGroup minGroupAllowed, Delegate handler)
		{
			CommandName = commandName;
			Restriction = minGroupAllowed;
			Action = handler;
		}
	}
}
