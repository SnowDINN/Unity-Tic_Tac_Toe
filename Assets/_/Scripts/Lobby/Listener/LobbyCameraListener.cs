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
			LobbySubscriber.OnConnect
				.Where(_ => _.Status == ConnectStatus.After)
				.Subscribe(_ =>
				{
					uiCamera.SetActive(false);
				}).AddTo(this);
			
			LobbySubscriber.OnDisconnect
				.Where(_ => _.Status == ConnectStatus.After)
				.Subscribe(_ =>
				{
					uiCamera.SetActive(true);
				}).AddTo(this);
		}
	}
}