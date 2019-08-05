using NFive.SDK.Core.Chat;
using System.Collections.Generic;

namespace NFive.Chat.Shared.Models
{
	public interface IColoredMessage
	{
		ChatMessage Message { get; set; }
		Dictionary<string, string> MessageSections { get; set; }
	}
}
