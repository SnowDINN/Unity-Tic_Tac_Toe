using Quantum;
using UnityEngine;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public unsafe class OnSpawnSystem : SystemMainThreadFilter<OnSpawnSystem.Filter>, ISignalOnInteraction
	{
		public struct Filter
		{
			public EntityRef Entity;
			public LocalPlayer* LocalPlayer;
		}
		
		private int currentIndex;
		
		public override void Update(Frame f, ref Filter filter)
		{
			var input = f.GetPlayerInput(filter.LocalPlayer->Player);
			if (input->Index == currentIndex)
				return;

			currentIndex = input->Index;
			Debug.Log($"{f.GetPlayerData(filter.LocalPlayer->Player).PlayerNickname} : Select {currentIndex})");
			
			if (currentIndex > 0)
				f.Signals.OnInteraction(currentIndex);
		}

		public void OnInteraction(Frame f, int index) =>
			f.Set(f.Create(NetworkAsset.Stone), new Stone { Index = index });
	}
}