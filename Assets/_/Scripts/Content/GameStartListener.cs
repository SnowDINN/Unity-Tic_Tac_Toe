using Quantum;
using R3;
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
					myTurn.SetActive(_.ActorId == QuantumRunner.Default.NetworkClient.LocalPlayer.ActorNumber);
					otherTurn.SetActive(_.ActorId != QuantumRunner.Default.NetworkClient.LocalPlayer.ActorNumber);
				}).AddTo(this);
		}
	}
}