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
					ready.SetActive(false);
				}).AddTo(this);
			
			RxGame.OnGameVote
				.Subscribe(_ =>
				{
					ready.SetActive(_.Type is GameVote.Ready);
					
					if (_.Type is GameVote.Ready)
						this.NetworkEventPublish(new QCommandGameVote
						{
							VoteType = (int)GameVote.Ready,
							VotePlayer = NetworkPlayer.LocalPlayerId
						});
				}).AddTo(this);
		}
	}
}