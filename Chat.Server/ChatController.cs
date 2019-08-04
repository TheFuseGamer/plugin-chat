using JetBrains.Annotations;
using NFive.Chat.Shared;
using NFive.SDK.Core.Chat;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Core.Rpc;
using NFive.SDK.Server.Controllers;
using NFive.SDK.Server.Events;
using NFive.SDK.Server.Rcon;
using NFive.SDK.Server.Rpc;
using System.Threading.Tasks;

namespace NFive.Chat.Server
{
	[PublicAPI]
	public class ChatController : ConfigurableController<Configuration>
	{
		public ChatController(ILogger logger, IEventManager events, IRpcHandler rpc, IRconManager rcon, Configuration configuration) : base(logger, events, rpc, rcon, configuration) { }

		public override Task Started()
		{
			// Send configuration when requested
			this.Rpc.Event(ChatEvents.Configuration).On(e => e.Reply(this.Configuration));

			// Send chat messages
			this.Rpc.Event(RpcEvents.ChatSendMessage).On<ChatMessage>((e, message) =>
			{
				this.Logger.Debug($"Sending message: {message.Content}");

				this.Rpc.Event(RpcEvents.ChatSendMessage).Trigger(message);
			});

			return base.Started();
		}

		public override void Reload(Configuration configuration)
		{
			// Update local configuration
			base.Reload(configuration);

			// Send out new configuration
			this.Rpc.Event(ChatEvents.Configuration).Trigger(this.Configuration);
		}
	}
}
