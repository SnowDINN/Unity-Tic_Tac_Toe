using R3;
using UnityEngine.SceneManagement;

namespace Redbean
{
	public class LobbySubscriber
	{
		private static readonly Subject<EVT_ConnectStatus> onConnect = new();
		public static Observable<EVT_ConnectStatus> OnConnect => onConnect.Share();
		
		private static readonly Subject<EVT_ConnectStatus> onDisconnect = new();
		public static Observable<EVT_ConnectStatus> OnDisconnect => onDisconnect.Share();
		
		private static readonly Subject<Scene> onSceneChanged = new();
		public static Observable<Scene> OnSceneChanged => onSceneChanged.Share();

		public static void SetConnect(EVT_ConnectStatus evt) => onConnect.OnNext(evt);
		public static void SetDisconnect(EVT_ConnectStatus evt) => onDisconnect.OnNext(evt);
		public static void SetSceneChanged(Scene evt) => onSceneChanged.OnNext(evt);
	}
}