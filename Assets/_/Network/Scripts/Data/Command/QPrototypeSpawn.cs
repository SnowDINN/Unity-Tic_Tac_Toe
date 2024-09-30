using Photon.Deterministic;
using Quantum;

namespace Redbean.Network
{
	public class SpawnCommand : DeterministicCommand
	{
		public AssetRef<EntityPrototype> Entity;
		public int Index;
		
		public override void Serialize(BitStream stream)
		{
			stream.Serialize(ref Entity);
			stream.Serialize(ref Index);
		}

		public void Spawn(Frame f) =>
			f.Set(f.Create(Entity), new Stone { Index = Index });
	}
}