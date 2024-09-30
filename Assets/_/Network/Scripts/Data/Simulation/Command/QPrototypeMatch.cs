using Photon.Deterministic;
using Quantum;
using UnityEngine;

namespace Redbean.Network
{
	public class QPrototypeMatch : DeterministicCommand
	{
		public override void Serialize(BitStream stream)
		{
		}

		public void Match(Frame frame, PlayerRef player)
		{
			Debug.Log($"[ {frame.GetPlayerData(player).PlayerNickname} ] Match !!");
		}
	}
}