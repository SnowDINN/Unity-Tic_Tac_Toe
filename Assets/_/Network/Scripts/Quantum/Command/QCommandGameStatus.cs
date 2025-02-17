﻿using Photon.Deterministic;

namespace Quantum
{
	/// <summary>
	/// 게임 결과 동기화 네트워크 이벤트
	/// </summary>
	public class QCommandGameStatus : DeterministicCommand
	{
		public int Type;
		public PlayerRef TargetPlayer;
		
		public override void Serialize(BitStream stream)
		{
			stream.Serialize(ref Type);
			stream.Serialize(ref TargetPlayer);
		}
	}
}