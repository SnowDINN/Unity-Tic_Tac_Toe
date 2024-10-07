using R3;

namespace Redbean
{
	public class LobbySubscriber
	{
		private static readonly Subject<EVT_ConnectStatus> onConnect = new();
		public static Observable<EVT_ConnectStatus> OnConnect => onConnect.Share();
		
		private static readonly Subject<EVT_ConnectStatus> onDisconnect = new();
		public static Observable<EVT_ConnectStatus> OnDisconnect => onDisconnect.Share();

		public static void SetConnect(EVT_ConnectStatus evt) => onConnect.OnNext(evt);
		public static void SetDisconnect(EVT_ConnectStatus evt) => onDisconnect.OnNext(evt);
	}
}