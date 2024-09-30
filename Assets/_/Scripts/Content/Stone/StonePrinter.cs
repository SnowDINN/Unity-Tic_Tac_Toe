using Quantum;
using UnityEngine;

namespace Redbean.Content
{
	public class StonePrinter : MonoBehaviour
	{
		[SerializeField]
		private GameObject IsMine;

		[SerializeField]
		private GameObject IsOther;
		
		private DispatcherSubscription subscription;
		
		private void OnEnable()
		{
			subscription = QuantumEvent.Subscribe<EventOnInteraction>(this, OnInteraction);
		}

		private void OnDisable()
		{
			QuantumEvent.Unsubscribe(subscription);
		}

		private void OnInteraction(EventOnInteraction data)
		{

		}

		public void UpdateView(bool isOwner)
		{
			IsMine.SetActive(isOwner);
			IsOther.SetActive(!isOwner);
		}
	}
}