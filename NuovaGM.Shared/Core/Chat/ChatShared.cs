using CitizenFX.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
// ReSharper disable All

namespace TheLastPlanet.Shared
{
	public class ChatMessage
	{

	}
	public class ChatSuggestion
	{
		public string Name;
		public string Help;
		public List<string> Params = new List<string>();

		public ChatSuggestion(string name, string help, List<string> args)
		{
			Name = name;
			Help = help;
			Params = args;
		}
	}

	public class ChatCommand
	{
		public string Name;
		public Delegate Action;
		public Player Source;
		public string rawCommand;
		public object[] Args;
		UserGroup Allowance;
	}
}
