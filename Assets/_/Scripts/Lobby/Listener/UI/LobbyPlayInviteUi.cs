using R3;
using Redbean.Network;
using UnityEngine;
using UnityEngine.UI;

namespace Redbean.Lobby
{
	public class LobbyPlayInviteUi : MonoBehaviour
	{
		[SerializeField] private Button createButton;
		[SerializeField] private Button joinButton;

		private void Awake()
		{
			createButton.AsButtonObservable()
				.Subscribe(async _ =>
				{
					await NetworkManager.Default.ConnectAsync(ConnectionType.CreateRoom);
				}).AddTo(this);
			
			joinButton.AsButtonObservable()
				.Subscribe(async _ =>
				{
					await NetworkManager.Default.ConnectAsync(ConnectionType.JoinRoom);
				}).AddTo(this);
		}
	}
}