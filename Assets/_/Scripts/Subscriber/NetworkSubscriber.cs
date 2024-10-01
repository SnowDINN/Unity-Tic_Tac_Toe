using Photon.Deterministic;
using Quantum;
using R3;

namespace Redbean.Content
{
	public class NetworkSubscriber
	{
		private static readonly Subject<NetworkEventStream> onNetworkEvent = new();

		public static Observable<NetworkEventStream> OnNetworkEvent => onNetworkEvent.Share();

		public static void Publish(NetworkEventStream stream) => onNetworkEvent.OnNext(stream);
	}

	public class NetworkEventStream
	{
		public Frame Frame;
		public PlayerRef Player;
		public DeterministicCommand Command;
	}
}