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
				.Where(_ => _.Type is GameStatus.NextTurn)
				.Subscribe(_ =>
				{
					myTurn.SetActive(_.ActorId == NetworkPlayer.LocalPlayerId);
					otherTurn.SetActive(_.ActorId != NetworkPlayer.LocalPlayerId);
				}).AddTo(this);
			
			RxGame.OnGameStatus
				.Subscribe(_ =>
				{
					myTurn.SetActive(_.Type is not GameStatus.Start);
					otherTurn.SetActive(_.Type is not GameStatus.Start);
				}).AddTo(this);
			
			RxGame.OnGameVote
				.Subscribe(_ =>
				{
					myTurn.SetActive(false);
					otherTurn.SetActive(false);
				}).AddTo(this);
		}
	}
}