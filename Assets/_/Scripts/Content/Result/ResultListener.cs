using Quantum;
using UnityEngine;

namespace Redbean.Content
{
	public class ResultListener : MonoBehaviour
	{
		[SerializeField]
		private GameObject victory;

		[SerializeField]
		private GameObject defeat;

		private DispatcherSubscription subscription;
		
		private void OnEnable()
		{
			subscription = QuantumEvent.Subscribe<EventOnGameEnd>(this, _ =>
			{
				victory.SetActive(_.WinnerId == QuantumRunner.Default.NetworkClient.LocalPlayer.ActorNumber);
				defeat.SetActive(_.WinnerId != QuantumRunner.Default.NetworkClient.LocalPlayer.ActorNumber);
			});
		}

		private void OnDisable()
		{
			QuantumEvent.Unsubscribe(subscription);
		}
	}
}