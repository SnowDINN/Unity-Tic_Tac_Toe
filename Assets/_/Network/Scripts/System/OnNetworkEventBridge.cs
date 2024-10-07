﻿using Quantum;
using R3;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public class OnNetworkEventBridge : SystemSignalsOnly
	{
		private readonly CompositeDisposable disposables = new();
		
		public override void OnEnabled(Frame frame)
		{
			QuantumEvent.SubscribeManual<EventOnStoneCreated>(_ =>
			{
				GameSubscriber.SetStoneCreate(new EVT_PositionAndOwner
				{
					OwnerId = _.Stone.OwnerId,
					Position = new EVT_Position
					{
						X = _.Stone.X,
						Y = _.Stone.Y
					}
				});
			}).AddTo(disposables);
			
			QuantumEvent.SubscribeManual<EventOnStoneDestroyed>(_ =>
			{
				GameSubscriber.SetStoneDestroy(new EVT_Position
				{
					X = _.Stone.X,
					Y = _.Stone.Y
				});
			}).AddTo(disposables);
			
			QuantumEvent.SubscribeManual<EventOnStoneHighlighted>(this, _ =>
			{
				GameSubscriber.SetStoneHighlight(_.Stone);
			}).AddTo(disposables);
			
			QuantumEvent.SubscribeManual<EventOnStoneMatchValidation>(this, _ =>
			{
				GameSubscriber.SetStoneMatchValidation(new EVT_Position
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