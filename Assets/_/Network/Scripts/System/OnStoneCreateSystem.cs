using Quantum;
using R3;
using Redbean.Content;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public unsafe class OnStoneCreateSystem : SystemMainThreadFilter<OnStoneCreateSystem.Filter>
	{
		public struct Filter
		{
			public EntityRef Entity;
			public LocalPlayer* LocalPlayer;
		}

		private readonly CompositeDisposable disposables = new();

		public override void OnEnabled(Frame f)
		{
			GameSubscriber.OnInteraction
				.Subscribe(_ =>
				{
					QuantumRunner.DefaultGame.SendCommand(new QCommandStoneCreated
					{
						Entity = NetworkAsset.Stone,
						X = _.X,
						Y = _.Y,
					});
				}).AddTo(disposables);
		}

		public override void OnDisabled(Frame f)
		{
			disposables?.Clear();
			disposables?.Dispose();
		}

		public override void Update(Frame f, ref Filter filter)
		{
			if (f.GetPlayerCommand(filter.LocalPlayer->Player) is not QCommandStoneCreated command)
				return;
			
			command.Spawn(f, f.PlayerToActorId(filter.LocalPlayer->Player).Value, NetworkSetting.StoneDestroyTurn);
		}
	}
}