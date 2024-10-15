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
		[SerializeField] private TMP_InputField sessionText;

		private void Awake()
		{
			createButton.AsButtonObservable()
				.Subscribe(async _ =>
				{
					await NetworkManager.Default.ConnectAsync(SessionType.Create, sessionText.text);
				}).AddTo(this);
			
			joinButton.AsButtonObservable()
				.Subscribe(async _ =>
				{
					await NetworkManager.Default.ConnectAsync(SessionType.Join, sessionText.text);
				}).AddTo(this);
		}
	}
}