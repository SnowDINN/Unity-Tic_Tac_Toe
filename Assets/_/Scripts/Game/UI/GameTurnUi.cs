using R3;
using Redbean.Network;
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
				.Where(_ => _.Type == GameStatus.Next)
				.Subscribe(_ =>
				{
					myTurn.SetActive(_.ActorId == NetworkPlayer.LocalPlayerId);
					otherTurn.SetActive(_.ActorId != NetworkPlayer.LocalPlayerId);
				}).AddTo(this);
			
			RxGame.OnGameStatus
				.Where(_ => _.Type is GameStatus.Start or GameStatus.Ready)
				.Subscribe(_ =>
				{
					myTurn.SetActive(false);
					otherTurn.SetActive(false);
				}).AddTo(this);
		}
	}
}