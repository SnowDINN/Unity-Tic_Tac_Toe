using System.Linq;
using Quantum;
using UnityEngine;

namespace Redbean.Network
{
	public class TableView : MonoBehaviour
	{
		[SerializeField]
		private int Index;
		
		private DispatcherSubscription subscription;
		private bool canInteraction;

		private void Awake()
		{
			Index = FindObjectsByType<TableView>(FindObjectsSortMode.InstanceID)
				.Select((value, index) => (index, value))
				.FirstOrDefault(_ => _.value.GetInstanceID() == GetInstanceID()).index;
		}

		private void OnEnable()
		{
			subscription = QuantumEvent.Subscribe<EventPlayerInteraction>(this, OnPlayerInteraction);
		}

		private void OnDisable()
		{
			QuantumEvent.Unsubscribe(subscription);
		}

		private void OnPlayerInteraction(EventPlayerInteraction data)
		{
			if (data.Index != Index)
				return;
			
			if (canInteraction)
			{
				
			}
		}
	}
}