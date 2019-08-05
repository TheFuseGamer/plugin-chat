using NFive.Chat.Shared.Models;
using NFive.SDK.Core.Chat;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NFive.Chat.Server.Models
{
	public class ColoredMessage: IColoredMessage
	{
		public ChatMessage Message { get; set; }
		public Dictionary<string, string> MessageSections { get; set; }

		public ColoredMessage(ChatMessage message)
		{
			this.Message = message;
			this.MessageSections = new Dictionary<string, string>();
			this.Colorize();
		}

		private void Colorize()
		{
			string[] sections = Regex.Split(this.Message.Content, @"({[A-Fa-f0-9]{6}})");
			for (int i = 0; i < sections.Length; i++)
			{
				Match match = Regex.Match(sections[i], @"{[A-Fa-f0-9]{6}}");
				while (match.Success && i < sections.Length)
				{
					i++;
					if (i >= sections.Length)
						break;
					match = Regex.Match(sections[i], @"{[A-Fa-f0-9]{6}}");
				}

				if (i < sections.Length && !string.IsNullOrEmpty(sections[i])) // we found text
				{
					this.MessageSections.Add(sections[i], (i - 1) >= 0 ? sections[i - 1].Substring(1, 6) : "");
				}
			}
		}

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
