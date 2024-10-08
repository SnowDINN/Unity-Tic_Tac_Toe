using R3;
using UnityEngine;

namespace Redbean.Lobby
{
	public class LobbyCameraListener : MonoBehaviour
	{
		[SerializeField]
		private GameObject uiCamera;

		private void Awake()
		{
			LobbySubscriber.OnSceneChanged
				.Subscribe(_ =>
				{
					uiCamera.SetActive(_.buildIndex != 2);
				}).AddTo(this);
		}
	}
}