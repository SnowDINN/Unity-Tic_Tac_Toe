using Quantum;
using R3;
using Redbean.Network;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace Redbean.Lobby
{
	public class LobbyPartyPageUi : MonoBehaviour
	{
		[SerializeField] private GameObject mainGO;
		
		[Header("Buttons")]
		[SerializeField] private Button createButton;
		[SerializeField] private Button joinButton;
		[SerializeField] private Button closeButton;
		
		[Header("Session name")]
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

			closeButton.AsButtonObservable()
				.Subscribe(_ =>
				{
					RxLobby.SetMenuChanged(LobbyMenuType.Home);
				}).AddTo(this);

			RxLobby.OnMenuChanged
				.Subscribe(_ =>
				{
					mainGO.SetActive(_ == LobbyMenuType.Party);
				}).AddTo(this);
		}
	}
}