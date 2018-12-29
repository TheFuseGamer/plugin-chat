using CitizenFX.Core;
using CitizenFX.Core.Native;
using JetBrains.Annotations;
using NFive.Chat.Client.Overlays;
using NFive.SDK.Client.Commands;
using NFive.SDK.Client.Events;
using NFive.SDK.Client.Interface;
using NFive.SDK.Client.Rpc;
using NFive.SDK.Client.Services;
using NFive.SDK.Core.Chat;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Core.Models.Player;
using NFive.SDK.Core.Rpc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFive.Chat.Client
{
	[PublicAPI]
	public class ChatService : Service
	{
		private ChatOverlay overlay;

		public ChatService(ILogger logger, ITickManager ticks, IEventManager events, IRpcHandler rpc, ICommandManager commands, OverlayManager overlay, User user) : base(logger, ticks, events, rpc, commands, overlay, user) { }

		public override Task Started()
		{
			API.SetTextChatEnabled(false);

			this.overlay = new ChatOverlay(this.OverlayManager);

			this.overlay.Focus += (s, a) =>
			{
				API.SetNuiFocus(a.Focus, false);
			};

			this.overlay.Message += (s, a) =>
			{
				this.Logger.Debug($"Sending message: {a.Message}");
				var message = new ChatMessage
				{
					Content = a.Message
				};
				this.Rpc.Event(RpcEvents.ChatSendMessage).Trigger(message);
			};

			this.Rpc.Event(RpcEvents.ChatSendMessage).On(new Action<IRpcEvent, ChatMessage>((e, message) =>
			{
				this.Logger.Debug($"Got message: {message.Content}");

				this.overlay.Manager.Send("message", message.Content);
			}));

			this.Commands.Register("test1", () => this.Logger.Debug("test1"));
			this.Commands.Register("test2", (string s) => this.Logger.Debug($"test2: {s}"));
			this.Commands.Register("test3", (IEnumerable<string> a) => this.Logger.Debug($"test3: {string.Join("|", a)}"));
			//this.Commands.Register<TestArgs>("test4", a => this.Logger.Debug($"test4: {a.Test}"));

			this.Ticks.Attach(Tick);

			return base.Started();
		}

		private Task Tick()
		{
			if (Game.IsControlJustPressed(2, Control.MpTextChatAll)) // T
			{
				API.SetNuiFocus(true, false);
				this.overlay.Manager.Send("open");
			}

			return Task.FromResult(0);
		}
	}
}
