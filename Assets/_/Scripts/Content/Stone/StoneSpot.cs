using UnityEngine;

namespace Redbean.Content
{
	public class StoneSpot : MonoBehaviour
	{
		[SerializeField]
		private GameObject IsMine;

		[SerializeField]
		private GameObject IsOther;

		public bool IsOwner;

		public void UpdateView(bool isOwner)
		{
			IsOwner = isOwner;
			
			IsMine.SetActive(isOwner);
			IsOther.SetActive(!isOwner);
		}
	}
}