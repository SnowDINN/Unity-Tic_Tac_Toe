using Quantum;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public unsafe class NetworkGameVoteSystem : SystemSignalsOnly, ISignalOnGameVote
	{
		public void OnGameVote(Frame frame, QEventGameVote evt)
		{
			switch (evt.Type)
			{
				// 게임 준비 투표
				case GameVote.Ready:
				{
					OnGameReady(frame);
					break;
				}
				
				// 게임 재시작 투표
				case GameVote.Retry:
				{
					OnGameRetry(frame);
					break;
				}
			}
		}
		
		private void OnGameReady(Frame frame)
		{
			frame.MapAssetRef = NetworkAsset.Map;
			frame.Events.OnGameVote(new QEventGameVote
			{
				Type = GameVote.Ready
			});
		}

		private void OnGameRetry(Frame frame)
		{
			var qSystem = frame.Unsafe.GetPointerSingleton<QComponentSystem>();
			frame.Events.OnGameVote(new QEventGameVote
			{
				Type = GameVote.Retry,
				CurrentCount = frame.ResolveList(qSystem->RetryPlayers).Count,
				TotalCount = frame.PlayerCount
			});
		}
	}
}