using R3;
using Redbean.Network;
using UnityEngine;

namespace Redbean.Content
{
	public class GameStartListener : MonoBehaviour
	{
		[SerializeField]
		private GameObject myTurn;

		[SerializeField]
		private GameObject otherTurn;
		
		private void Awake()
		{
			GameSubscriber.OnGameStatus
				.Where(_ => _.Status == GameStatus.Start)
				.Subscribe(_ =>
				{
					myTurn.SetActive(_.ActorId == NetworkSetting.LocalPlayerId);
					otherTurn.SetActive(_.ActorId != NetworkSetting.LocalPlayerId);
				}).AddTo(this);
		}
	}
}