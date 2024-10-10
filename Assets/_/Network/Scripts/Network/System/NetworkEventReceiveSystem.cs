using Quantum;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public unsafe class NetworkEventReceiveSystem : SystemSignalsOnly, ISignalOnEvent
	{
		public void OnEvent(Frame frame, QEventCommand evt)
		{
			switch (evt.Command)
			{
				case QCommandGameResult qCommand:
					OnGameResult(frame, evt.Player, qCommand);
					break;
						
				case QCommandGameTurn qCommand:
					OnGameTurn(frame, evt.Player, qCommand);
					break;
				
				case QCommandGameVote qCommand:
					OnGameVote(frame, evt.Player, qCommand);
					break;
			}
		}
		
#region Event Method
		
		private void OnGameResult(Frame frame, PlayerRef player, QCommandGameResult command)
		{
			frame.Signals.OnGameStatus(new QEventGameStatus
			{
				Type = GameStatus.End,
				ActorId = command.WinnerPlayer
			});
		}
		
		private void OnGameTurn(Frame frame, PlayerRef player, QCommandGameTurn command)
		{
			frame.Signals.OnGameStatus(new QEventGameStatus
			{
				Type = GameStatus.NextTurn,
				Args = new object[]
				{
					player,
					command.X,
					command.Y,
				}
			});
		}

		private void OnGameVote(Frame frame, PlayerRef player, QCommandGameVote command)
		{
			switch ((GameVote)command.VoteType)
			{
				case GameVote.Ready:
				{
					var qSystem = frame.Unsafe.GetPointerSingleton<QComponentSystem>();
					var readyPlayers = frame.ResolveList(qSystem->ReadyPlayers);
					if (!readyPlayers.Contains(command.VotePlayer))
						readyPlayers.Add(command.VotePlayer);
					
					qSystem->ReadyPlayers = readyPlayers;

					if (frame.PlayerCount == readyPlayers.Count)
						frame.Signals.OnGameStatus(new QEventGameStatus
						{
							Type = GameStatus.Start
						});
					
					break;
				}

				case GameVote.Retry:
				{
					var qSystem = frame.Unsafe.GetPointerSingleton<QComponentSystem>();
					var retryPlayers = frame.ResolveList(qSystem->RetryPlayers);
					if (!retryPlayers.Contains(command.VotePlayer))
						retryPlayers.Add(command.VotePlayer);

					qSystem->RetryPlayers = retryPlayers;

					if (frame.PlayerCount == retryPlayers.Count)
						frame.Signals.OnGameStatus(new QEventGameStatus
						{
							Type = GameStatus.Start
						});
					else
						frame.Signals.OnGameStatus(new QEventGameStatus
						{
							Type = GameStatus.Retry
						});

					break;
				}
			}
		}

#endregion
	}
}