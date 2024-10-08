using Quantum;
using R3;
using Redbean.Network;
using UnityEngine;

namespace Redbean.Game
{
	public class GameReadyUi : MonoBehaviour
	{
		[SerializeField] private GameObject ready;
		
		private void Awake()
		{
			GameSubscriber.OnGameStatus
				.Subscribe(_ =>
				{
					ready.SetActive(_.Type == GameStatus.Ready);
					
					if (_.Type == GameStatus.Ready)
						QuantumRunner.DefaultGame.SendCommand(new QCommandGameVote
						{
							VoteType = (int)GameVote.Ready,
							ActorId = NetworkSetting.LocalPlayerId
						});
				}).AddTo(this);
		}
	}
}