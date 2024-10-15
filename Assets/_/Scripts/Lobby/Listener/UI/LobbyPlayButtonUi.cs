using Quantum;
using R3;
using Redbean.Network;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace Redbean.Lobby
{
	public class LobbyPlayButtonUi : MonoBehaviour
	{
		[SerializeField] private Button quickButton;
		[SerializeField] private Button partyButton;

		private void Awake()
		{
			quickButton.AsButtonObservable()
				.Subscribe(async _ =>
				{
					await NetworkManager.Default.ConnectAsync(SessionType.Quick);
				}).AddTo(this);

			partyButton.AsButtonObservable()
				.Subscribe(_ =>
				{
					RxLobby.SetMenuChanged(LobbyMenuType.Party);
				}).AddTo(this);
		}
	}
}