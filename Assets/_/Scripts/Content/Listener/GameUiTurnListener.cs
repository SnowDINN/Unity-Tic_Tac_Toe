using R3;
using Redbean.Network;
using UnityEngine;

namespace Redbean.Content
{
	public class GameUiTurnListener : MonoBehaviour
	{
		[SerializeField]
		private GameObject myTurn;

		[SerializeField]
		private GameObject otherTurn;
		
		private void Awake()
		{
			GameSubscriber.OnGameStatus
				.Where(_ => _.Type == GameStatus.Next)
				.Subscribe(_ =>
				{
					myTurn.SetActive(_.ActorId == NetworkSetting.LocalPlayerId);
					otherTurn.SetActive(_.ActorId != NetworkSetting.LocalPlayerId);
				}).AddTo(this);
			
			GameSubscriber.OnGameStatus
				.Where(_ => _.Type is GameStatus.Start or GameStatus.Ready)
				.Subscribe(_ =>
				{
					myTurn.SetActive(false);
					otherTurn.SetActive(false);
				}).AddTo(this);
		}
	}
}