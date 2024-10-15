using R3;
using UnityEngine.SceneManagement;

namespace Redbean
{
	public class RxLobby
	{
		/// <summary>
		/// Room 접속 스트림
		/// </summary>
		public static Observable<EVT_RoomStatus> OnConnect => onConnect.Share();
		private static readonly Subject<EVT_RoomStatus> onConnect = new();
		
		/// <summary>
		/// Room 접속 해제 스트림
		/// </summary>
		public static Observable<EVT_RoomStatus> OnDisconnect => onDisconnect.Share();
		private static readonly Subject<EVT_RoomStatus> onDisconnect = new();
		
		public static Observable<EVT_Players> OnPlayers => onPlayers.Share();
		private static readonly Subject<EVT_Players> onPlayers = new();
		
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

		public static void SetConnect(EVT_RoomStatus evt) => onConnect.OnNext(evt);
		public static void SetDisconnect(EVT_RoomStatus evt) => onDisconnect.OnNext(evt);
		public static void SetPlayers(EVT_Players evt) => onPlayers.OnNext(evt);
		public static void SetProgress(string evt) => onProgress.OnNext(evt);
		public static void SetSceneChanged(Scene evt) => onSceneChanged.OnNext(evt);
	}
}