using Quantum;
using R3;
using UnityEngine;

namespace Redbean.Game
{
	public class GameTurnUi : MonoBehaviour
	{
		[SerializeField]
		private GameObject myTurn;

		[SerializeField]
		private GameObject otherTurn;
		
		private void Awake()
		{
			RxGame.OnGameStatus
				.Where(_ => _.Type == GameStatus.NextTurn)
				.Subscribe(_ =>
				{
					myTurn.SetActive(_.ActorId == NetworkPlayer.LocalPlayerId);
					otherTurn.SetActive(_.ActorId != NetworkPlayer.LocalPlayerId);
				}).AddTo(this);
			
			RxGame.OnGameStatus
				.Where(_ => _.Type is GameStatus.Start)
				.Subscribe(_ =>
				{
					myTurn.SetActive(false);
					otherTurn.SetActive(false);
				}).AddTo(this);
			
			RxGame.OnGameVote
				.Where(_ => _.Type is GameVote.Ready)
				.Subscribe(_ =>
				{
					myTurn.SetActive(false);
					otherTurn.SetActive(false);
				}).AddTo(this);
		}
	}
}