using NFive.SDK.Client.Interface;
using System;
using CitizenFX.Core.Native;
using NFive.SDK.Core.Chat;

namespace NFive.Chat.Client.Overlays
{
	public class ChatOverlay : Overlay
	{
		public event EventHandler<MessageEventArgs> Message;
		public event EventHandler<EventArgs> Shown;

		public ChatOverlay(OverlayManager manager) : base("ChatOverlay.html", manager)
		{
			Attach("focus", (focus, callback) => API.SetNuiFocus(focus, false));
			Attach("message", (message, callback) => this.Message?.Invoke(this, new MessageEventArgs(this, message)));
			Attach("shown", (message, callback) => this.Shown?.Invoke(this, EventArgs.Empty));
		}

		public void Open()
		{
			API.SetNuiFocus(true, false);
			this.Manager.Send("open");
		}

		public void AddMessage(ChatMessage message)
		{
			this.Manager.Send("message", message.Content);
		}

		public class MessageEventArgs : OverlayEventArgs
		{
			public string Message { get; }

			public MessageEventArgs(Overlay overlay, string message) : base(overlay)
			{
				this.Message = message;
			}
		}
	}
}
