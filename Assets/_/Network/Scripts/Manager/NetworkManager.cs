using System.Threading.Tasks;
using Quantum.Menu;
using R3;
using Redbean.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Redbean.Lobby
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
			LobbySubscriber.SetSceneChanged(next);
		}

		public async Task ConnectAsync()
		{
			LobbySubscriber.SetConnect(new EVT_ConnectStatus
			{
				Status = ConnectStatus.Before,
				ReasonCode = 0
			});
			
			var result = await connectionBehaviour.ConnectAsync(connectArgs);
			
			LobbySubscriber.SetConnect(new EVT_ConnectStatus
			{
				Status = ConnectStatus.After,
				ReasonCode = result.FailReason
			});
		}

		public async Task Disconnect(int reason)
		{
			LobbySubscriber.SetDisconnect(new EVT_ConnectStatus
			{
				Status = ConnectStatus.Before,
				ReasonCode = reason
			});
			
			await connectionBehaviour.DisconnectAsync(reason);
			
			LobbySubscriber.SetDisconnect(new EVT_ConnectStatus
			{
				Status = ConnectStatus.After,
				ReasonCode = reason
			});
		}
	}
}