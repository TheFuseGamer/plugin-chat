using NFive.SDK.Client.Interface;
using System;
using CitizenFX.Core.Native;
using NFive.SDK.Core.Chat;
using NFive.Chat.Client.Models;

namespace NFive.Chat.Client.Overlays
{
	public class ChatOverlay : Overlay
	{
		public event EventHandler<MessageEventArgs> Message;
		public event EventHandler<TypingEventArgs> Typing;
		public event EventHandler<VisibleEventArgs> Visible;

		public ChatOverlay(OverlayManager manager) : base("ChatOverlay.html", manager)
		{
			Attach("visible", (visible, callback) => this.Visible?.Invoke(this, new VisibleEventArgs(this, visible)));
			Attach("message", (message, callback) => this.Message?.Invoke(this, new MessageEventArgs(this, message)));
			Attach("typing", (typing, callback) =>
			{
				if(!typing)
					API.SetNuiFocus(false, false);
				this.Typing?.Invoke(this, new TypingEventArgs(this, typing));
			});
		}

		public void Open()
		{
			API.SetNuiFocus(true, false);
			this.Manager.Send("open");
		}

		public void AddMessage(ColoredMessage coloredMessage)
		{
			this.Manager.Send("message", coloredMessage.MessageSections);
		}

		public class MessageEventArgs : OverlayEventArgs
		{
			public string Message { get; }

			public MessageEventArgs(Overlay overlay, string message) : base(overlay)
			{
				this.Message = message;
			}
		}

		public class VisibleEventArgs : OverlayEventArgs
		{
			public bool Visible { get; }

			public VisibleEventArgs(Overlay overlay, bool visible) : base(overlay)
			{
				this.Visible = visible;
			}
		}

		public class TypingEventArgs : OverlayEventArgs
		{
			public bool Typing { get; }

			public TypingEventArgs(Overlay overlay, bool typing) : base(overlay)
			{
				this.Typing = typing;
			}
		}
	}
}
