using NFive.SDK.Core.Controllers;
using System;

namespace NFive.Chat.Shared
{
	public class Configuration : ControllerConfiguration
	{
		public TimeSpan Fadeout { get; set; } = TimeSpan.FromSeconds(5);

		public bool SpellCheck { get; set; } = false;

		public string WelcomeMessage { get; set; } = "Welcome to the server";

		public int HotKey { get; set; } = 245; // T
	}
}
