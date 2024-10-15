using System;
using Quantum;
using R3;
using Redbean.Network;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace Redbean.Lobby
{
	public class LobbyQuickUi : MonoBehaviour
	{
		[Header("GameObject")]
		[SerializeField] private GameObject mainGO;
		
		[Header("UI Component")]
		[SerializeField] private Button cancelButton;
		[SerializeField] private TextMeshProUGUI text;
		
		private IDisposable disposable;
		private bool isConnecting;
		
		private void Awake()
		{
			cancelButton.AsButtonObservable()
				.Subscribe(async _ =>
				{
					await NetworkManager.Default.Disconnect(NetworkCommonValue.UserLeave);
				}).AddTo(this);

			RxLobby.OnConnect
				.Where(_ => _.Type is SessionType.Quick)
				.Subscribe(_ =>
				{
					var timer = 0;
					
					switch (_.OrderType)
					{
						case OrderType.Before:
						{
							ConnectionActivator(true);
							break;
						}

						case OrderType.After:
						{
							if (!isConnecting)
								return;
							
							disposable?.Dispose();
							disposable = Observable.Interval(TimeSpan.FromSeconds(1))
								.Select(_ => timer++)
								.Subscribe(_ =>
								{
									var time = TimeSpan.FromSeconds(_);
									text.text = $"Searching... {time.Minutes:D2}:{time.Seconds:D2}";
								});
							break;
						}
					}
				}).AddTo(this);

			RxLobby.OnDisconnect
				.Subscribe(_ =>
				{
					ConnectionActivator(false);
				}).AddTo(this);

			RxLobby.OnSceneChanged
				.Where(_ => _.buildIndex is 2)
				.Subscribe(_ =>
				{
					ConnectionActivator(false);
				}).AddTo(this);
			
			RxLobby.OnProgress
				.Subscribe(_ =>
				{
					text.text = _;
				}).AddTo(this);
		}

		private void OnDestroy()
		{
			ConnectionActivator(false);
		}

		private void ConnectionActivator(bool isConnect)
		{
			isConnecting = isConnect;
					
			mainGO.SetActive(isConnect);
			disposable?.Dispose();
		}
	}
}