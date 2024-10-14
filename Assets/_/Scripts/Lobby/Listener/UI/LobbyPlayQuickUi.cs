using R3;
using Redbean.Network;
using UnityEngine;
using UnityEngine.UI;

namespace Redbean.Lobby
{
	public class LobbyPlayQuickUi : MonoBehaviour
	{
		[SerializeField] private Button button;

		private void Awake()
		{
			button.AsButtonObservable()
				.Subscribe(async _ =>
				{
					await NetworkManager.Default.ConnectAsync(ConnectionType.Matchmaking);
				}).AddTo(this);
		}
	}
}