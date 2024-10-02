using R3;
using UnityEngine;

namespace Redbean.Content
{
	public class GameWaitListener : MonoBehaviour
	{
		[SerializeField]
		private GameObject wait;
		
		private void Awake()
		{
			GameSubscriber.OnGameStatus
				.Subscribe(_ =>
				{
					wait.SetActive(_.Status == GameStatus.Wait);
				}).AddTo(this);
		}
	}
}