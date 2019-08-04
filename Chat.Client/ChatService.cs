using CitizenFX.Core;
using CitizenFX.Core.Native;
using JetBrains.Annotations;
using NFive.Chat.Client.Overlays;
using NFive.Chat.Shared;
using NFive.SDK.Client.Commands;
using NFive.SDK.Client.Events;
using NFive.SDK.Client.Input;
using NFive.SDK.Client.Interface;
using NFive.SDK.Client.Rpc;
using NFive.SDK.Client.Services;
using NFive.SDK.Core.Chat;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Core.Models.Player;
using NFive.SDK.Core.Rpc;
using System;
using System.Threading.Tasks;

namespace NFive.Chat.Client
{
	[PublicAPI]
	public class ChatService : Service
	{
		private Configuration config;
		private ChatOverlay overlay;

		public ChatService(ILogger logger, ITickManager ticks, IEventManager events, IRpcHandler rpc, ICommandManager commands, OverlayManager overlay, User user) : base(logger, ticks, events, rpc, commands, overlay, user) { }

		public override async Task Started()
		{
			API.SetTextChatEnabled(false);

			// Request server configuration
			this.config = await this.Rpc.Event(ChatEvents.Configuration).Request<Configuration>();

			// Update local configuration on server configuration change
			this.Rpc.Event(ChatEvents.Configuration).On<Configuration>((e, c) => this.config = c);

			// Create overlay
			this.overlay = new ChatOverlay(this.OverlayManager);


			this.overlay.Shown += (s) => this.wait = false;


			// Send entered messages
			this.overlay.Message += (s, a) => this.Rpc.Event(RpcEvents.ChatSendMessage).Trigger(new ChatMessage
			{
				Sender = this.User,
				Content = a.Message
			});

			// Listen for new messages
			this.Rpc.Event(RpcEvents.ChatSendMessage).On(new Action<IRpcEvent, ChatMessage>((e, message) => this.overlay.AddMessage(message)));

			// Attach a tick handler
			this.Ticks.Attach(OnTick);
		}

		private bool wait = false;

		private async Task OnTick()
		{
			if (!this.wait && Input.IsControlPressed(Control.MpTextChatAll)) this.overlay.Open(); // T

			this.wait = true;

			await Delay(0);
		}
	}
}
