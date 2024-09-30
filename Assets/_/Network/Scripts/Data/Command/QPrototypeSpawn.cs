using Photon.Deterministic;
using Quantum;

namespace Redbean.Network
{
	public class SpawnCommand : DeterministicCommand
	{
		public AssetRef<EntityPrototype> Entity;
		public int Position;
		
		public override void Serialize(BitStream stream)
		{
			stream.Serialize(ref Entity);
			stream.Serialize(ref Position);
		}

		public void Spawn(Frame f, int Owner) =>
			f.Set(f.Create(Entity), new Stone { Position = Position, Owner = Owner});
	}
}