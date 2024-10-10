using System;
using R3;
using Redbean.Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Redbean.Lobby
{
	public class LobbyMatchmakingUi : MonoBehaviour
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
					await NetworkManager.Default.Disconnect(NetworkConst.UserLeave);
				}).AddTo(this);

			RxNetwork.OnConnect
				.Subscribe(_ =>
				{
					var timer = 0;
					
					switch (_.Status)
					{
						case ConnectionStatus.Before:
						{
							ConnectionActivator(true);
							break;
						}

						case ConnectionStatus.After:
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

			RxNetwork.OnDisconnect
				.Subscribe(_ =>
				{
					ConnectionActivator(false);
				}).AddTo(this);

			RxNetwork.OnProgress
				.Subscribe(_ =>
				{
					text.text = _;
				}).AddTo(this);

			RxNetwork.OnSceneChanged
				.Where(_ => _.buildIndex == 2)
				.Subscribe(_ =>
				{
					ConnectionActivator(false);
				}).AddTo(this);
		}
		
		private void ConnectionActivator(bool isConnect)
		{
			isConnecting = isConnect;
					
			mainGO.SetActive(isConnect);
			disposable?.Dispose();
		}
	}
}