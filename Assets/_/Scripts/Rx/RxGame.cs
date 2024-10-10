using Quantum;
using R3;

namespace Redbean
{
	public class RxGame
	{
		public static readonly Subject<EVT_GameStatus> OnGameStatus = new();
		public static readonly Subject<EVT_GameVote> OnGameVote = new();
		public static readonly Subject<EVT_PositionAndOwner> OnStoneCreateOrDestroy = new();
		public static readonly Subject<EVT_Position> OnMatchValidation = new();
		public static readonly Subject<EVT_Position> OnPositionSelect = new();
		public static readonly Subject<QComponentStone> OnStoneHighlight = new();
		
		public static void SetGameStatus(EVT_GameStatus evt) => OnGameStatus.OnNext(evt);
		public static void SetGameVote(EVT_GameVote evt) => OnGameVote.OnNext(evt);
		public static void SetStoneCreateOrDestroy(EVT_PositionAndOwner evt) => OnStoneCreateOrDestroy.OnNext(evt);
		public static void SetMatchValidation(EVT_Position evt) => OnMatchValidation.OnNext(evt);
		public static void SetBoardSelect(EVT_Position evt) => OnPositionSelect.OnNext(evt);
		public static void SetStoneHighlight(QComponentStone evt) => OnStoneHighlight.OnNext(evt);
	}
}