using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Redbean.Lobby
{
	public class LobbyUiListener : MonoBehaviour
	{
		[SerializeField]
		private Button QuickPlayButton;

		private void Awake()
		{
			QuickPlayButton.AsButtonObservable()
				.Subscribe(async _ =>
				{
					await NetworkManager.Default.ConnectAsync();
				}).AddTo(this);
		}
	}
}