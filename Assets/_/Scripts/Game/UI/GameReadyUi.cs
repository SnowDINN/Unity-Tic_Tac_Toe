using Quantum;
using R3;
using UnityEngine;

namespace Redbean.Game
{
	public class GameReadyUi : MonoBehaviour
	{
		[SerializeField] private GameObject ready;
		
		private void Awake()
		{
			RxGame.OnGameStatus
				.Subscribe(_ =>
				{
					ready.SetActive(_.Type == GameStatus.Ready);
				}).AddTo(this);
			
			RxGame.OnGameStatus
				.Where(_ => _.Type == GameStatus.Ready)
				.Subscribe(_ =>
				{
					this.NetworkEventPublish(new QCommandGameVote
					{
						VoteType = (int)GameVote.Ready,
						VotePlayer = NetworkPlayer.LocalPlayerId
					});
				}).AddTo(this);
		}
	}
}