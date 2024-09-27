using System.Linq;
using Quantum;
using R3;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace Redbean.Content
{
	public class InteractionPublisher : MonoBehaviour
	{
		[SerializeField]
		private GameObject Prefab;
		
		[SerializeField]
		private Button Button;
		
		[SerializeField]
		private int Index;
		
		private DispatcherSubscription subscription;
		private bool canInteraction;

		private void Awake()
		{
			Index = FindObjectsByType<InteractionPublisher>(FindObjectsSortMode.InstanceID)
				.Select((value, index) => (index, value))
				.FirstOrDefault(_ => _.value.GetInstanceID() == GetInstanceID()).index;

			Button.AsButtonObservable()
				.Subscribe(_ =>
				{
					GameSubscriber.Interaction(Index);
				}).AddTo(this);

			GameSubscriber.OnSpawn
				.Subscribe(_ =>
				{
					if (_ != Index)
						return;

					Instantiate(Prefab, transform);
					SetInteraction(false);
				}).AddTo(this);
		}

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
			if (data.Index != Index)
				return;
			
			if (canInteraction)
			{
				
			}
		}

		private void SetInteraction(bool use)
		{
			Button.interactable = use;
		}
	}
}