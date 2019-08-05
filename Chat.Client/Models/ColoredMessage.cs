using NFive.Chat.Shared.Models;
using NFive.SDK.Core.Chat;
using System.Collections.Generic;
using System.Text;

namespace NFive.Chat.Client.Models
{
	public class ColoredMessage : IColoredMessage
	{
		public ChatMessage Message { get; set; }
		public Dictionary<string, string> MessageSections { get; set; }

		public ColoredMessage() {}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("======= Message sections ========");
			foreach (var pair in MessageSections)
			{
				sb.AppendLine($"Message: {pair.Key} | Color: {pair.Value}");
			}
			return sb.ToString();
		}
	}
}
