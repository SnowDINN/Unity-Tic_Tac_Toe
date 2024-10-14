using System.Threading.Tasks;
using Quantum;
using Quantum.Menu;
using R3;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

namespace Redbean.Network
{
	public class NetworkInitializer
	{
		[RuntimeInitializeOnLoadMethod]
		public static void Setup()
		{
			var networkGO = new GameObject("[Network Manager]");
			Object.DontDestroyOnLoad(networkGO);
			
			var connection = networkGO.AddComponent<QuantumMenuConnectionBehaviourSDK>();
			var network = networkGO.AddComponent<NetworkManager>();
			network.ConnectionSetup(connection);

			var eventGO = new GameObject("[Event System]", typeof(EventSystem), typeof(InputSystemUIInputModule));
			eventGO.transform.SetParent(networkGO.transform);

			NetworkManager.Default = network;
			NetworkAsset.ConnectionArgs.SetDefaults(NetworkAsset.ConnectionConfigure);
		}
	}
	
	public class NetworkManager : QuantumMonoBehaviour
	{
		public static NetworkManager Default;
		
		private QuantumMenuConnectArgs networkArgs => NetworkAsset.ConnectionArgs;
		private QuantumMenuConnectionBehaviour connection;
		
		public void ConnectionSetup(QuantumMenuConnectionBehaviour connection)
		{
			this.connection = connection;

			connection.OnProgress = new UnityEvent<string>();
			connection.OnProgress
				.AsObservable()
				.Subscribe(RxLobby.SetProgress)
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
			RxLobby.SetSceneChanged(next);
		}

		public async Task ConnectAsync(ConnectionType type, string session = default)
		{
			RxLobby.SetConnect(new EVT_ConnectionStatus
			{
				Type = type,
				Status = ConnectionStatus.Before,
				ReasonCode = 0
			});

			switch (type)
			{
				case ConnectionType.Matchmaking:
				{
					networkArgs.AppVersion = $"matchmaking_version_{Application.version}";
					networkArgs.Session = default;
					networkArgs.Creating = false;
					break;
				}

				case ConnectionType.CreateRoom:
				{
					networkArgs.AppVersion = $"party_version_{Application.version}";
					networkArgs.Session = session;
					networkArgs.Creating = true;
					break;
				}
				
				case ConnectionType.JoinRoom:
				{
					networkArgs.AppVersion = $"party_version_{Application.version}";
					networkArgs.Session = session;
					networkArgs.Creating = false;
					break;
				}
			}
			networkArgs.Region = networkArgs.PreferredRegion;
			
			var result = await connection.ConnectAsync(networkArgs);
			
			RxLobby.SetConnect(new EVT_ConnectionStatus
			{
				Type = type,
				Status = ConnectionStatus.After,
				ReasonCode = result.FailReason
			});
		}
		
		public async Task Disconnect(int reason)
		{
			RxLobby.SetDisconnect(new EVT_ConnectionStatus
			{
				Status = ConnectionStatus.Before,
				ReasonCode = reason
			});
			
			await connection.DisconnectAsync(reason);
			
			RxLobby.SetDisconnect(new EVT_ConnectionStatus
			{
				Status = ConnectionStatus.After,
				ReasonCode = reason
			});
		}
	}
}