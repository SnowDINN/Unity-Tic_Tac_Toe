using Quantum;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public unsafe class OnEventSystem : SystemMainThreadFilter<OnEventSystem.Filter>
	{
		public struct Filter
		{
			public EntityRef Entity;
			public LocalPlayer* LocalPlayer;
		}

		public override void Update(Frame frame, ref Filter filter)
		{
			if (frame.GetPlayerCommand(filter.LocalPlayer->Player) is QPrototypeNextTurn)
			{
				var currentTurn = frame.Unsafe.GetPointerSingleton<Game>()->NextTurn();
			
				var stones = frame.Filter<Stone>();
				while (stones.Next(out var entity, out var stone))
				{
					switch (stone.DestroyTurn - currentTurn)
					{
						case 1:
						{
							break;
						}

						case <= 0:
						{
							if (QuantumRunner.Default.NetworkClient.LocalPlayer.IsMasterClient)
								QuantumRunner.DefaultGame.SendCommand(new QCommandStoneDestroy
								{
									Entity = entity
								});

							break;
						}
					}
				}
			}

			if (frame.GetPlayerCommand(filter.LocalPlayer->Player) is QCommandStoneCreated qCommandStoneCreated)
				qCommandStoneCreated.StoneCreate(frame, 
				                                 filter.LocalPlayer->Player,
				                                 NetworkSetting.StoneDestroyTurn);
			
			if (frame.GetPlayerCommand(filter.LocalPlayer->Player) is QCommandStoneDestroy qCommandStoneDestroy)
				qCommandStoneDestroy.StoneDestroy(frame);
			
			if (frame.GetPlayerCommand(filter.LocalPlayer->Player) is QPrototypeMatch qPrototypeMatch)
				qPrototypeMatch.Match(frame,
				                      filter.LocalPlayer->Player);
		}
	}
}