using R3;
using UnityEngine;

namespace Redbean.Game
{
	public class StoneEntity : MonoBehaviour
	{
		[SerializeField]
		private GameObject IsMine;

		[SerializeField]
		private GameObject IsOther;

		private int x;
		private int y;

		public bool IsOwner;
		
		private void Awake()
		{
			GameSubscriber.OnStoneHighlight
				.Subscribe(_ =>
				{
					if (x != _.X || y != _.Y)
						return;
				
					GetComponent<Animation>().Play("Highlight");
				}).AddTo(this);
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