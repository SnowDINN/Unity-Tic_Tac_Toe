using Quantum;
using UnityEngine;

namespace Redbean.Network
{
	[CreateAssetMenu(fileName = "NetworkAsset", menuName = "Redbean/Network/NetworkAsset")]
	public class NetworkAssetResource : ScriptableObject
	{
		[Header("Map Asset")]
		public AssetRef<Map> Map;
	}

	public class NetworkAsset
	{
		private static NetworkAssetResource system => Resources.Load<NetworkAssetResource>("NetworkAsset");
		
		public static AssetRef<Map> Map => system.Map;
	}

	public class NetworkSetting
	{
		public static int LocalPlayerId => QuantumRunner.Default.NetworkClient.LocalPlayer.ActorNumber;
		
		public const int StoneDestroyTurn = 6;
	}
}