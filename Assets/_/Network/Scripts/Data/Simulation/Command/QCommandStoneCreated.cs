using Photon.Deterministic;
using Quantum;

namespace Redbean.Network
{
	public class QCommandStoneCreated : DeterministicCommand
	{
		public AssetRef<EntityPrototype> Entity;
		public int X;
		public int Y;
		
		public override void Serialize(BitStream stream)
		{
			stream.Serialize(ref Entity);
			stream.Serialize(ref X);
			stream.Serialize(ref Y);
		}

		public void Spawn(Frame f, int Owner, int DestroyTurn) => 
			f.Set(f.Create(Entity), new Stone
			{
				X = X,
				Y = Y,
				Owner = Owner,
				DestroyTurn = f.GetSingleton<Game>().CurrentTurn + DestroyTurn
			});
	}
}