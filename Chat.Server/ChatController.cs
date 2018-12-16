using JetBrains.Annotations;
using NFive.Chat.Shared;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Server.Controllers;
using NFive.SDK.Server.Events;
using NFive.SDK.Server.Rpc;

namespace NFive.Chat.Server
{
	[PublicAPI]
	public class ChatController : ConfigurableController<Configuration>
	{
		public ChatController(ILogger logger, IEventManager events, IRpcHandler rpc, Configuration configuration) : base(logger, events, rpc, configuration)
		{
			this.Rpc.Event(ChatEvents.Message).On<string>((e, message) =>
			{
				this.Logger.Debug($"Got message: {message}");

				this.Rpc.Event(ChatEvents.Message).Trigger(message);
			});
		}
	}
}
