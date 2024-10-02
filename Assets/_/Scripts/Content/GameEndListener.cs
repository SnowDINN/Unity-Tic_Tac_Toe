using Quantum;
using R3;
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
					victory.SetActive(_.ActorId == QuantumRunner.Default.NetworkClient.LocalPlayer.ActorNumber);
					defeat.SetActive(_.ActorId != QuantumRunner.Default.NetworkClient.LocalPlayer.ActorNumber);
				}).AddTo(this);
		}
	}
}