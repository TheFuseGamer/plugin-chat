using System;
using NFive.SDK.Client.Interface;

namespace NFive.Chat.Client.Overlays
{
	public class MessageEventArgs : OverlayEventArgs
	{
		public string Message { get; }

		public MessageEventArgs(Overlay overlay, string message) : base(overlay)
		{
			this.Message = message;
		}
	}

	public class FocusEventArgs : OverlayEventArgs
	{
		public bool Focus { get; }

		public FocusEventArgs(Overlay overlay, bool focus) : base(overlay)
		{
			this.Focus = focus;
		}
	}

	public class ChatOverlay : Overlay
	{
		public event EventHandler<FocusEventArgs> Focus;
		public event EventHandler<MessageEventArgs> Message;

		public ChatOverlay(OverlayManager manager) : base("ChatOverlay.html", manager)
		{
			this.Attach("focus", (focus, callback) => this.Focus?.Invoke(this, new FocusEventArgs(this, focus)));
			this.Attach("message", (message, callback) => this.Message?.Invoke(this, new MessageEventArgs(this, message)));
		}
	}
}
