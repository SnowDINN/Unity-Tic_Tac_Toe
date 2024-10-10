using R3;
using UnityEngine;

namespace Redbean.Lobby
{
	public class LobbyActivator : MonoBehaviour
	{
		private void Awake()
		{
			RxNetwork.OnSceneChanged
				.Subscribe(_ =>
				{
					gameObject.SetActive(_.buildIndex != 2);
				}).AddTo(this);
		}
	}
}