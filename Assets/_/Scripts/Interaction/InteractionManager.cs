using System.Linq;
using Photon.Deterministic;
using Quantum;
using R3;
using UnityEngine;
using Input = Quantum.Input;

namespace Redbean.Content
{
	public class InteractionManager : MonoBehaviour
	{
		[SerializeField]
		private InteractionButton[] buttons;
		
		private static InteractionButton[] staticButtons;
		
		private DispatcherSubscription subscription;
		private int index;

		private void Awake()
		{
			staticButtons = buttons;
			
			GameSubscriber.OnInteraction
				.Subscribe(_ =>
				{
					index = _;
				}).AddTo(this);
		}

		private void OnEnable()
		{
			subscription = QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback));
		}

		private void OnDisable()
		{
			QuantumCallback.Unsubscribe(subscription);
		}

		private void PollInput(CallbackPollInput callback)
		{
			callback.SetInput(new Input
			{
				Index = index
			}, DeterministicInputFlags.Repeatable);
		}

		public static int GetIndex(int instanceID)
		{
			return staticButtons.Select((value, index) => (index, value))
				.FirstOrDefault(_ => _.value.GetInstanceID() == instanceID).index + 1;
		}
	}
}