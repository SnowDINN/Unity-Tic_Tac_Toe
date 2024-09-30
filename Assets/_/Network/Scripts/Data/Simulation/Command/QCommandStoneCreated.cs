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

		public void StoneCreate(Frame frame, PlayerRef player, int DestroyTurn) => 
			frame.Set(frame.Create(Entity), new Stone
			{
				X = X,
				Y = Y,
				OwnerId = frame.PlayerToActorId(player).Value,
				DestroyTurn = frame.GetSingleton<Game>().TurnCount + DestroyTurn
			});
	}
}