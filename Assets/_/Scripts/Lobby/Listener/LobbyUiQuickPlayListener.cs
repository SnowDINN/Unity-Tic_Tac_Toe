using System;
using Quantum.Menu;
using R3;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace Redbean.Lobby
{
	public class LobbyUiQuickPlayListener : MonoBehaviour
	{
		private IDisposable disposable;
		private bool isConnecting;
		
		[SerializeField] private Button QuickPlayButton;

		[Header("Match making")]
		[SerializeField] private GameObject matchmaking;
		[SerializeField] private Button matchmakingButton;
		[SerializeField] private TextMeshProUGUI matchmakingText;

		private void Awake()
		{
			QuickPlayButton.AsButtonObservable()
				.Subscribe(async _ =>
				{
					await NetworkManager.Default.ConnectAsync();
				}).AddTo(this);

			matchmakingButton.AsButtonObservable()
				.Subscribe(async _ =>
				{
					await NetworkManager.Default.Disconnect(ConnectFailReason.UserRequest);
				}).AddTo(this);

			LobbySubscriber.OnConnect
				.Subscribe(_ =>
				{
					switch (_.Status)
					{
						case ConnectStatus.Before:
							ConnectionPopup(true);
							break;

						case ConnectStatus.After:
							if (!isConnecting)
								return;
							
							var timer = 0;
							disposable?.Dispose();
							disposable = Observable.Interval(TimeSpan.FromSeconds(1))
								.Select(_ => timer++)
								.Subscribe(_ =>
								{
									var time = TimeSpan.FromSeconds(_);
									matchmakingText.text = $"Searching... {time.Minutes:D2}:{time.Seconds:D2}";
								});
							break;
					}
				}).AddTo(this);

			LobbySubscriber.OnDisconnect
				.Subscribe(_ =>
				{
					ConnectionPopup(false);
				}).AddTo(this);

			LobbySubscriber.OnSceneChanged
				.Where(_ => _.buildIndex == 2)
				.Subscribe(_ =>
				{
					ConnectionPopup(false);
				}).AddTo(this);
		}

		private void ConnectionPopup(bool isConnect)
		{
			isConnecting = isConnect;
					
			matchmaking.SetActive(isConnect);
			matchmakingText.text = isConnect ? "Connecting..." : "";
					
			disposable?.Dispose();
		}
	}
}