using R3;
using UnityEngine;

namespace Redbean.Game
{
	public class StoneUnit : MonoBehaviour
	{
		[SerializeField] private GameObject mineGO;
		[SerializeField] private GameObject otherGO;

		private int x;
		private int y;

		public bool IsOwner;
		
		private void Awake()
		{
			RxGame.OnStoneHighlight
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
			
			mineGO.SetActive(isOwner);
			otherGO.SetActive(!isOwner);
		}
	}
}