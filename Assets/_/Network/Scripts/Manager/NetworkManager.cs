using System.Threading.Tasks;
using Quantum.Menu;
using R3;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Redbean.Network
{
	public class NetworkManager : MonoBehaviour
	{
		[Header("Lobby System"), SerializeField]
		private QuantumMenuConnectionBehaviour connectionBehaviour;

		[Header("Lobby Config"), SerializeField]
		private QuantumMenuConfig connectionConfig;

		[Header("Lobby Args"), SerializeField]
		private QuantumMenuConnectArgs connectArgs;

		public static NetworkManager Default;

		private void Awake()
		{
			Default = this;
			DontDestroyOnLoad(this);

			connectArgs.SetDefaults(connectionConfig);
			connectionBehaviour.OnProgress
				.AsObservable()
				.Subscribe(RxNetwork.SetProgress)
				.AddTo(this);
		}

		private void OnEnable()
		{
			SceneManager.activeSceneChanged += OnActiveSceneChanged;
		}

		private void OnDisable()
		{
			SceneManager.activeSceneChanged -= OnActiveSceneChanged;
		}

		private void OnActiveSceneChanged(Scene current, Scene next)
		{
			RxNetwork.SetSceneChanged(next);
		}

		public async Task ConnectAsync()
		{
			RxNetwork.SetConnect(new EVT_ConnectionStatus
			{
				Status = ConnectionStatus.Before,
				ReasonCode = 0
			});
			
			var result = await connectionBehaviour.ConnectAsync(connectArgs);
			
			RxNetwork.SetConnect(new EVT_ConnectionStatus
			{
				Status = ConnectionStatus.After,
				ReasonCode = result.FailReason
			});
		}

		public async Task Disconnect(int reason)
		{
			RxNetwork.SetDisconnect(new EVT_ConnectionStatus
			{
				Status = ConnectionStatus.Before,
				ReasonCode = reason
			});
			
			await connectionBehaviour.DisconnectAsync(reason);
			
			RxNetwork.SetDisconnect(new EVT_ConnectionStatus
			{
				Status = ConnectionStatus.After,
				ReasonCode = reason
			});
		}
	}
}