using Quantum;
using Quantum.Menu;
using UnityEngine;

namespace Redbean
{
	[CreateAssetMenu(fileName = "NetworkAsset", menuName = "Redbean/Network/NetworkAsset")]
	public class NetworkAssetResource : ScriptableObject
	{
		[Header("Quantum connection settings")]
		public QuantumMenuConfig connectionConfigure;
		public QuantumMenuConnectArgs connectionArgs;
		
		[Header("Map Asset")]
		public AssetRef<Map> Map;
	}

	public class NetworkAsset
	{
		private static NetworkAssetResource system => Resources.Load<NetworkAssetResource>("NetworkAsset");

		public static QuantumMenuConfig ConnectionConfigure => system.connectionConfigure;
		public static QuantumMenuConnectArgs ConnectionArgs => system.connectionArgs;
		
		public static AssetRef<Map> Map => system.Map;
	}

	public class NetworkPlayer
	{
		public static int LocalPlayerId => QuantumRunner.Default.NetworkClient.LocalPlayer.ActorNumber;
	}

	public class NetworkConst
	{
		public const int StoneDestroyTurn = 6;
		public const int UserLeave = 1;
	}
}