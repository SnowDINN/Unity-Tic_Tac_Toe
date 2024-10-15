using System;
using Quantum;
using R3;
using TMPro;
using UnityEngine;

namespace Redbean.Lobby
{
	public class LobbyPopupQuickUi : MonoBehaviour
	{
		[Header("GameObject")]
		[SerializeField] private GameObject mainGO;
		
		[Header("UI Component")]
		[SerializeField] private TextMeshProUGUI progressText;
		
		private IDisposable disposable;
		private bool isConnecting;
		
		private void Awake()
		{
			RxLobby.OnConnect
				.Where(_ => _.Type is SessionType.Quick)
				.Subscribe(_ =>
				{
					var timer = 0;
					
					switch (_.OrderType)
					{
						case SessionOrderType.Before:
						{
							ConnectionActivator(true);
							break;
						}

						case SessionOrderType.After:
						{
							if (!isConnecting)
								return;
							
							disposable?.Dispose();
							disposable = Observable.Interval(TimeSpan.FromSeconds(1))
								.Select(_ => timer++)
								.Subscribe(_ =>
								{
									var time = TimeSpan.FromSeconds(_);
									progressText.text = $"Searching... {time.Minutes:D2}:{time.Seconds:D2}";
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
					progressText.text = _;
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