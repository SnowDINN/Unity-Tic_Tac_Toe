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
			var component = frame.Get<Stone>(EntityRef);
			index = component.index.AsInt;

			GameSubscriber.Spawn(index);
		}
	}
}