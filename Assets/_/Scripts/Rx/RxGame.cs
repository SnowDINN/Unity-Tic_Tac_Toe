using Quantum;
using R3;

namespace Redbean
{
	public class RxGame
	{
		/// <summary>
		/// 게임 상태 스트림
		/// </summary>
		public static Observable<EVT_GameStatus> OnGameStatus => onGameStatus.Share();
		private static readonly Subject<EVT_GameStatus> onGameStatus = new();
		
		/// <summary>
		/// 게임 투표 스트림
		/// </summary>
		public static Observable<EVT_GameVote> OnGameVote => onGameVote.Share();
		private static readonly Subject<EVT_GameVote> onGameVote = new();

		/// <summary>
		/// 플레이 턴 시간 초과 스트림
		/// </summary>
		public static Observable<Unit> OnGameTimeout => OnGameTimeout.Share();
		private static readonly Subject<Unit> onGameTimeout = new();
		
		/// <summary>
		/// 바둑알 생성 및 제거 스트림
		/// </summary>
		public static Observable<EVT_PositionAndOwner> OnStoneCreateOrDestroy => onStoneCreateOrDestroy.Share();
		private static readonly Subject<EVT_PositionAndOwner> onStoneCreateOrDestroy = new();
		
		/// <summary>
		/// 바둑알 매치 검증 스트림
		/// </summary>
		public static Observable<EVT_Position> OnMatchValidation => onMatchValidation.Share();
		private static readonly Subject<EVT_Position> onMatchValidation = new();
		
		/// <summary>
		/// 바둑알 강조 표시 스트림
		/// </summary>
		public static Observable<QComponentStone> OnStoneHighlight => onStoneHighlight.Share();
		private static readonly Subject<QComponentStone> onStoneHighlight = new();
		
		public static void SetGameStatus(EVT_GameStatus evt) => onGameStatus.OnNext(evt);
		public static void SetGameVote(EVT_GameVote evt) => onGameVote.OnNext(evt);
		public static void SetGameTimeout() => onGameTimeout.OnNext(Unit.Default);
		public static void SetStoneCreateOrDestroy(EVT_PositionAndOwner evt) => onStoneCreateOrDestroy.OnNext(evt);
		public static void SetMatchValidation(EVT_Position evt) => onMatchValidation.OnNext(evt);
		public static void SetStoneHighlight(QComponentStone evt) => onStoneHighlight.OnNext(evt);
	}
}