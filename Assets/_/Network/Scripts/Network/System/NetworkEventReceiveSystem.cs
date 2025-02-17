﻿using Quantum;
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
				// 게임 결과
				case QCommandGameStatus qCommand:
					OnGameStatus(frame, evt.Player, qCommand);
					break;
				
				// 게임 다음 턴
				case QCommandGameNextTurn qCommand:
					OnGameNextTurn(frame, evt.Player, qCommand);
					break;
				
				// 게임 투표
				case QCommandGameVote qCommand:
					OnGameVote(frame, evt.Player, qCommand);
					break;
			}
		}
		
#region Event Method
		
		private void OnGameStatus(Frame frame, PlayerRef player, QCommandGameStatus command)
		{
			switch ((GameStatus)command.Type)
			{
				case GameStatus.Start:
				{
					frame.Signals.OnGameVote(new QEventGameVote
					{
						Type = GameVote.Ready
					});
					break;
				}

				case GameStatus.End:
				{
					frame.Signals.OnGameStatus(new QEventGameStatus
					{
						Type = GameStatus.End,
						ActorId = command.TargetPlayer
					});
					break;
				}
			}
		}
		
		private void OnGameNextTurn(Frame frame, PlayerRef player, QCommandGameNextTurn command)
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
						frame.Signals.OnGameVote(new QEventGameVote
						{
							Type = GameVote.Retry
						});

					break;
				}
			}
		}

#endregion
	}
}