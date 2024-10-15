using System.Threading.Tasks;
using Quantum;
using R3;
using Redbean.Game;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public class NetworkEventRxSystem : SystemSignalsOnly
	{
		private readonly CompositeDisposable disposables = new();
		
		public override void OnEnabled(Frame frame)
		{
			QuantumEvent.SubscribeManual<EventOnPlayers>(_ =>
			{
				RxLobby.SetPlayers(new EVT_Players
				{
					Type = _.Evt.Type,
					Players = _.Evt.Players
				});
			}).AddTo(disposables);
			
			// 게임 상태
			QuantumEvent.SubscribeManual<EventOnGameStatus>(async _ =>
			{
				while (!GameManager.IsReady)
					await Task.Yield();
				
				RxGame.SetGameStatus(new EVT_GameStatus
				{
					Type = _.Evt.Type,
					ActorId = _.Evt.ActorId
				});
			}).AddTo(disposables);
			
			// 게임 투표
			QuantumEvent.SubscribeManual<EventOnGameVote>(async _ =>
			{
				while (!GameManager.IsReady)
					await Task.Yield();
				
				RxGame.SetGameVote(new EVT_GameVote
				{
					Type = _.Evt.Type,
					CurrentCount = _.Evt.CurrentCount,
					TotalCount = _.Evt.TotalCount
				});
			}).AddTo(disposables);
			
			// 바둑알 생성
			QuantumEvent.SubscribeManual<EventOnStoneCreated>(_ =>
			{
				RxGame.SetStoneCreateOrDestroy(new EVT_PositionAndOwner
				{
					Type = CreateOrDestroyType.Create,
					OwnerId = _.Stone.OwnerId,
					X = _.Stone.X,
					Y = _.Stone.Y
				});
			}).AddTo(disposables);
			
			// 바둑알 제거
			QuantumEvent.SubscribeManual<EventOnStoneDestroyed>(_ =>
			{
				RxGame.SetStoneCreateOrDestroy(new EVT_PositionAndOwner
				{
					Type = CreateOrDestroyType.Destroy,
					X = _.Stone.X,
					Y = _.Stone.Y
				});
			}).AddTo(disposables);
			
			// 다음 턴 제거될 바둑알 강조
			QuantumEvent.SubscribeManual<EventOnStoneHighlighted>(_ =>
			{
				RxGame.SetStoneHighlight(_.Stone);
			}).AddTo(disposables);
			
			// 바둑알 매치 검증
			QuantumEvent.SubscribeManual<EventOnStoneMatchValidation>(_ =>
			{
				RxGame.SetMatchValidation(new EVT_Position
				{
					X = _.X,
					Y = _.Y
				});
			}).AddTo(disposables);
		}

		public override void OnDisabled(Frame frame)
		{
			disposables?.Clear();
			disposables?.Dispose();
		}
	}
}