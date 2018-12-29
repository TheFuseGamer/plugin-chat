using JetBrains.Annotations;
using NFive.SDK.Core.Chat;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Core.Rpc;
using NFive.SDK.Server.Controllers;
using NFive.SDK.Server.Events;
using NFive.SDK.Server.Rcon;
using NFive.SDK.Server.Rpc;

namespace NFive.Chat.Server
{
	[PublicAPI]
	public class ChatController : ConfigurableController<Configuration>
	{
		public ChatController(ILogger logger, IEventManager events, IRpcHandler rpc, IRconManager rcon, Configuration configuration) : base(logger, events, rpc, rcon, configuration)
		{
			this.Rpc.Event(RpcEvents.ChatSendMessage).On<ChatMessage>((e, message) =>
			{
				this.Logger.Debug($"Got message: {message.Content}");

				this.Rpc.Event(RpcEvents.ChatSendMessage).Trigger(message);
			});
		}
	}
}
