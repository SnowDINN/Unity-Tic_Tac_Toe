using Quantum;
using R3;

namespace Redbean.Content
{
	public class GameSubscriber
	{
		private static readonly Subject<EVT_GameStatus> onGameStatus = new();
		public static Observable<EVT_GameStatus> OnGameStatus => onGameStatus.Share();
		
		private static readonly Subject<EVT_GameRetry> onGameRetry = new();
		public static Observable<EVT_GameRetry> OnGameRetry => onGameRetry.Share();

		private static readonly Subject<EVT_Position> onBoardSelect = new();
		public static Observable<EVT_Position> OnBoardSelect => onBoardSelect.Share();

		private static readonly Subject<EVT_PositionAndOwner> onStoneCreate = new();
		public static Observable<EVT_PositionAndOwner> OnStoneCreate => onStoneCreate.Share();

		private static readonly Subject<EVT_Position> onStoneDestroy = new();
		public static Observable<EVT_Position> OnStoneDestroy => onStoneDestroy.Share();
		
		private static readonly Subject<QComponentStone> onStoneHighlight = new();
		public static Observable<QComponentStone> OnStoneHighlight => onStoneHighlight.Share();
		
		private static readonly Subject<EVT_Position> onStoneMatchValidation = new();
		public static Observable<EVT_Position> OnStoneMatchValidation => onStoneMatchValidation.Share();
		
		public static void SetGameStatus(EVT_GameStatus evt) => onGameStatus.OnNext(evt);
		public static void SetGameRetry(EVT_GameRetry evt) => onGameRetry.OnNext(evt);
		public static void SetBoardSelect(EVT_Position evt) => onBoardSelect.OnNext(evt);
		public static void SetStoneCreate(EVT_PositionAndOwner evt) => onStoneCreate.OnNext(evt);
		public static void SetStoneDestroy(EVT_Position evt) => onStoneDestroy.OnNext(evt);
		public static void SetStoneHighlight(QComponentStone evt) => onStoneHighlight.OnNext(evt);
		public static void SetStoneMatchValidation(EVT_Position evt) => onStoneMatchValidation.OnNext(evt);
	}
}