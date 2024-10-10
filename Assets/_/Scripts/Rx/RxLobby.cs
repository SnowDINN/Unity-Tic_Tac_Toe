using R3;
using UnityEngine.SceneManagement;

namespace Redbean
{
	public class RxLobby
	{
		public static readonly Subject<EVT_ConnectionStatus> OnConnect = new();
		public static readonly Subject<EVT_ConnectionStatus> OnDisconnect = new();
		public static readonly Subject<string> OnProgress = new();
		public static readonly Subject<Scene> OnSceneChanged = new();

		public static void SetConnect(EVT_ConnectionStatus evt) => OnConnect.OnNext(evt);
		public static void SetDisconnect(EVT_ConnectionStatus evt) => OnDisconnect.OnNext(evt);
		public static void SetProgress(string evt) => OnProgress.OnNext(evt);
		public static void SetSceneChanged(Scene evt) => OnSceneChanged.OnNext(evt);
	}
}