using Quantum;
using Redbean.Content;
using UnityEngine;

namespace Redbean.Network
{
	public class StoneView : QuantumEntityViewComponent
	{
		[SerializeField]
		private int index;

		public override void OnActivate(Frame frame)
		{
			index = frame.Get<Stone>(EntityRef).Index.AsInt;

			GameSubscriber.Spawn(index);
		}
	}
}	