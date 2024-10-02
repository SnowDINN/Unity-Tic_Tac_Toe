using Quantum;
using R3;
using Redbean.Network;
using UnityEngine;

namespace Redbean.Content
{
	public class GameEndListener : MonoBehaviour
	{
		[SerializeField]
		private GameObject victory;

		[SerializeField]
		private GameObject defeat;
		
		private void Awake()
		{
			GameSubscriber.OnGameStatus
				.Where(_ => _.Status == GameStatus.End)
				.Subscribe(_ =>
				{
					victory.SetActive(_.ActorId == NetworkSetting.LocalPlayerId);
					defeat.SetActive(_.ActorId != NetworkSetting.LocalPlayerId);
				}).AddTo(this);
		}
	}
}