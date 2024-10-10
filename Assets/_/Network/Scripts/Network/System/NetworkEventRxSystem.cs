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
			QuantumEvent.SubscribeManual<EventOnGameStatus>(async _ =>
			{
				while (!GameManager.IsReady)
					await Task.Yield();
				
				RxGame.SetGameStatus(new EVT_GameStatus
				{
					Type = _.Status.Type,
					ActorId = _.Status.ActorId
				});
			}).AddTo(disposables);
			
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
			
			QuantumEvent.SubscribeManual<EventOnStoneDestroyed>(_ =>
			{
				RxGame.SetStoneCreateOrDestroy(new EVT_PositionAndOwner
				{
					Type = CreateOrDestroyType.Destroy,
					X = _.Stone.X,
					Y = _.Stone.Y
				});
			}).AddTo(disposables);
			
			QuantumEvent.SubscribeManual<EventOnStoneHighlighted>(this, _ =>
			{
				RxGame.SetStoneHighlight(_.Stone);
			}).AddTo(disposables);
			
			QuantumEvent.SubscribeManual<EventOnStoneMatchValidation>(this, _ =>
			{
				RxGame.SetStoneMatchValidation(new EVT_Position
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