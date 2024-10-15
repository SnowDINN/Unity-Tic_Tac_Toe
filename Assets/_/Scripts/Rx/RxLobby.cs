using R3;
using UnityEngine.SceneManagement;

namespace Redbean
{
	public class RxLobby
	{
		/// <summary>
		/// Room 접속 스트림
		/// </summary>
		public static Observable<EVT_ConnectionStatus> OnConnect => onConnect.Share();
		private static readonly Subject<EVT_ConnectionStatus> onConnect = new();
		
		/// <summary>
		/// Room 접속 해제 스트림
		/// </summary>
		public static Observable<EVT_ConnectionStatus> OnDisconnect => onDisconnect.Share();
		private static readonly Subject<EVT_ConnectionStatus> onDisconnect = new();
		
		/// <summary>
		/// Room 접속 진행 상황 스트림
		/// </summary>
		public static Observable<string> OnProgress => onProgress.Share();
		private static readonly Subject<string> onProgress = new();
		
		/// <summary>
		/// Scene 변경 스트림
		/// </summary>
		public static Observable<Scene> OnSceneChanged => onSceneChanged.Share();
		private static readonly Subject<Scene> onSceneChanged = new();

		public static void SetConnect(EVT_ConnectionStatus evt) => onConnect.OnNext(evt);
		public static void SetDisconnect(EVT_ConnectionStatus evt) => onDisconnect.OnNext(evt);
		public static void SetProgress(string evt) => onProgress.OnNext(evt);
		public static void SetSceneChanged(Scene evt) => onSceneChanged.OnNext(evt);
	}
}