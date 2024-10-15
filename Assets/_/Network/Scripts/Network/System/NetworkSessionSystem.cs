using Quantum;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public class NetworkQuickSessionSystem : SystemSignalsOnly, ISignalOnPlayerConnected, ISignalOnPlayerDisconnected
	{
		// 플레이어 접속
		public void OnPlayerConnected(Frame frame, PlayerRef player)
		{
			frame.SessionReset();
			frame.AddPlayer(player);
			
			if (frame.PlayerConnectedCount < frame.PlayerCount)
				return;
			
			frame.Signals.OnGameVote(new QEventGameVote
			{
				Type = GameVote.Ready
			});
		}
		
		// 플레이어 접속 해제
		public void OnPlayerDisconnected(Frame frame, PlayerRef player)
		{
			frame.SessionReset();
			frame.RemovePlayer(player);
			
			frame.Signals.OnGameVote(new QEventGameVote
			{
				Type = GameVote.Ready
			});
		}
	}
	
	[Preserve]
	public class NetworkPartySessionSystem : SystemSignalsOnly, ISignalOnPlayerConnected, ISignalOnPlayerDisconnected
	{
		// 플레이어 접속
		public void OnPlayerConnected(Frame frame, PlayerRef player)
		{
			frame.SessionReset();
			frame.AddPlayer(player);
			
			frame.Signals.OnPlayers(new QEventPlayers
			{
				Type = ConnectionType.Connect,
				Players = frame.GetPlayers()
			});
		}
		
		// 플레이어 접속 해제
		public void OnPlayerDisconnected(Frame frame, PlayerRef player)
		{
			frame.SessionReset();
			frame.RemovePlayer(player);
			
			frame.Signals.OnPlayers(new QEventPlayers
			{
				Type = ConnectionType.Disconnect,
				Players = frame.GetPlayers()
			});
			
			frame.Signals.OnGameVote(new QEventGameVote
			{
				Type = GameVote.Ready
			});
		}
	}
}