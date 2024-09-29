using System;
using Quantum;
using R3;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace Redbean.Content
{
	public class InteractionButton : MonoBehaviour
	{
		[SerializeField]
		private GameObject Prefab;
		
		[SerializeField]
		private Button Button;
		
		private DispatcherSubscription subscription;
		private int Index;

		private void Awake()
		{
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

		private void Start()
		{
			Index = InteractionManager.GetIndex(GetInstanceID());
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

			if (Button.interactable)
			{
				
			}
		}

		private void SetInteraction(bool use)
		{
			Button.interactable = use;
		}
	}
}