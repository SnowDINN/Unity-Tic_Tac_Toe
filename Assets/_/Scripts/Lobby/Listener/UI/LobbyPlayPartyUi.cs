using R3;
using Redbean.Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Redbean.Lobby
{
	public class LobbyPlayPartyUi : MonoBehaviour
	{
		[SerializeField] private Button createButton;
		[SerializeField] private Button joinButton;
		[SerializeField] private TMP_InputField session;

		private void Awake()
		{
			createButton.AsButtonObservable()
				.Subscribe(async _ =>
				{
					await NetworkManager.Default.ConnectAsync(ConnectionType.CreateRoom, session.text);
				}).AddTo(this);
			
			joinButton.AsButtonObservable()
				.Subscribe(async _ =>
				{
					await NetworkManager.Default.ConnectAsync(ConnectionType.JoinRoom, session.text);
				}).AddTo(this);
		}
	}
}