using Quantum;
using UnityEditor;
using UnityEngine;

namespace Redbean.Network
{
	[CreateAssetMenu(fileName = "NetworkAsset", menuName = "Redbean/Network/NetworkAsset")]
	public class NetworkAssetSystem : ScriptableObject
	{
		[Header("Stone Asset")]
		public AssetRef<EntityPrototype> Stone;
	}

	public class NetworkAsset
	{
		private static NetworkAssetSystem system => Resources.Load<NetworkAssetSystem>("NetworkAsset");
		
		public static AssetRef<EntityPrototype> Stone => system.Stone;
	}
}