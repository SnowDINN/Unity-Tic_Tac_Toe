using Quantum;
using UnityEngine;

namespace Redbean.Content
{
	public class StoneListener : MonoBehaviour
	{
		[SerializeField]
		private GameObject IsMine;

		[SerializeField]
		private GameObject IsOther;

		private int x;
		private int y;

		public bool IsOwner;

		private DispatcherSubscription subscription;
		
		private void OnEnable()
		{
			subscription = QuantumEvent.Subscribe<EventStoneHighlight>(this, _ =>
			{
				if (x != _.Stone.X || y != _.Stone.Y)
					return;
				
				GetComponent<Animation>().Play("Highlight");
			});
		}

		private void OnDisable()
		{
			QuantumEvent.Unsubscribe(subscription);
		}

		public void UpdateView(int x, int y, bool isOwner)
		{
			this.x = x;
			this.y = y;
			IsOwner = isOwner;
			
			IsMine.SetActive(isOwner);
			IsOther.SetActive(!isOwner);
		}
	}
}