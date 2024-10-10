using System.Linq;
using Quantum;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public unsafe class NetworkGameStatusSystem : SystemSignalsOnly, ISignalOnGameStatus
	{
		public override void OnInit(Frame frame)
		{
			Clear(frame);
		}
		
		public void OnGameStatus(Frame frame, QEventGameStatus evt)
		{
			switch (evt.Type)
			{
				case GameStatus.Start:
				{
					OnGameStart(frame);
					break;
				}

				case GameStatus.End:
				{
					OnGameEnd(frame, evt.ActorId);
					break;
				}
						
				case GameStatus.NextTurn:
				{
					OnGameNextTurn(frame, evt.Args);
					break;
				}
			}
		}

		private void Clear(Frame frame)
		{
			// Game Data Reset
			var qSystem = frame.Unsafe.GetPointerSingleton<QComponentSystem>();
			qSystem->Players = frame.AllocateList<PlayerRef>();
			qSystem->ReadyPlayers = frame.AllocateList<PlayerRef>();
			qSystem->RetryPlayers = frame.AllocateList<PlayerRef>();
			qSystem->CurrentPlayerTurn = default;
			qSystem->CurrentTurn = default;
			
			var qStone = frame.Filter<QComponentStone>();
			while (qStone.Next(out var entity, out var stone))
			{
				frame.Events.OnStoneDestroyed(stone);
				frame.Destroy(entity);
			}
		}

		private void OnGameStart(Frame frame)
		{
			Clear(frame);
							
			var random = frame.RNG->Next(0, frame.PlayerCount);
			var qPlayers = frame.AllocateList<PlayerRef>();

			var qPlayersFilter = frame.Filter<QComponentPlayer>();
			while (qPlayersFilter.Next(out var entity, out var qPlayer))
				qPlayers.Add(qPlayer.Player);

			var nextTurn = frame.PlayerToActorId(qPlayers[random]).Value;
			var qSystem = frame.Unsafe.GetPointerSingleton<QComponentSystem>();
			qSystem->Players = qPlayers;
			qSystem->ReadyPlayers = frame.AllocateList<PlayerRef>();
			qSystem->RetryPlayers = frame.AllocateList<PlayerRef>();
			qSystem->CurrentPlayerTurn = frame.PlayerToActorId(qPlayers[random]).Value;

			frame.Events.OnGameStatus(new QEventGameStatus
			{
				Type = GameStatus.Start
			});
			frame.Events.OnGameStatus(new QEventGameStatus
			{
				Type = GameStatus.NextTurn,
				ActorId = nextTurn
			});
		}

		private void OnGameEnd(Frame frame, int actorId)
		{
			frame.Events.OnGameStatus(new QEventGameStatus
			{
				Type = GameStatus.End,
				ActorId = actorId
			});
		}

		private void OnGameNextTurn(Frame frame, object[] args)
		{
			var player = (PlayerRef)args[0];
			var x = (int)args[1];
			var y = (int)args[2];
							
			var qSystem = frame.Unsafe.GetPointerSingleton<QComponentSystem>();
			var nextPlayer = frame.ResolveList(qSystem->Players)
				.FirstOrDefault(_ => frame.PlayerToActorId(_).Value != qSystem->CurrentPlayerTurn);
			qSystem->CurrentPlayerTurn = frame.PlayerToActorId(nextPlayer).Value;
			qSystem->CurrentTurn += 1;

			var entity = frame.Create();
			var qStone = new QComponentStone
			{
				X = x,
				Y = y,
				OwnerId = frame.PlayerToActorId(player).Value,
				DestroyTurn = qSystem->CurrentTurn + NetworkConst.StoneDestroyTurn
			};
			frame.Set(entity, qStone);
			frame.Events.OnStoneCreated(qStone);
			
			var qStoneFilter = frame.Filter<QComponentStone>();
			while (qStoneFilter.Next(out var destroyedEntity, out var destroyedStone))
			{
				switch (destroyedStone.DestroyTurn - qSystem->CurrentTurn)
				{
					case 1:
						frame.Events.OnStoneHighlighted(destroyedStone);
						break;

					case <= 0:
						frame.Events.OnStoneDestroyed(destroyedStone);
						frame.Destroy(destroyedEntity);
						break;
				}
			}
			
			frame.Events.OnStoneMatchValidation(x, y);
			frame.Events.OnGameStatus(new QEventGameStatus
			{
				Type = GameStatus.NextTurn,
				ActorId = qSystem->CurrentPlayerTurn
			});
		}
	}
}