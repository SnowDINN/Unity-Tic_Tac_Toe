using R3;
using Redbean.Network;
using UnityEngine;
using UnityEngine.UI;

namespace Redbean.Lobby
{
	public class LobbyPopupQuickButtonUi : MonoBehaviour
	{
		[SerializeField] private Button cancelButton;

		private void Awake()
		{
			cancelButton.AsButtonObservable()
				.Subscribe(async _ =>
				{
					await NetworkManager.Default.Disconnect(NetworkCommonValue.UserLeave);
				}).AddTo(this);
		}
	}
}