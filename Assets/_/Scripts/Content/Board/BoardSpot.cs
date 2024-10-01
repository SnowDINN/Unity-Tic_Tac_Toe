using Quantum;
using R3;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace Redbean.Content
{
	public class BoardSpot : MonoBehaviour
	{
		[SerializeField]
		private GameObject Prefab;
		
		[SerializeField]
		private Button Button;

		private GameObject instance;
		private int x;
		private int y;

		public StoneSpot CurrentStone;

		private void Awake()
		{
			Button.AsButtonObservable()
				.Subscribe(_ =>
				{
					GameSubscriber.Interaction(new PositionStream
					{
						X = x,
						Y = y
					});
				}).AddTo(this);

			GameSubscriber.OnStoneCreate
				.Subscribe(_ =>
				{
					if (_.X != x || _.Y != y)
						return;

					instance = Instantiate(Prefab, transform);
					CurrentStone = instance.GetComponent<StoneSpot>();
					CurrentStone.UpdateView(x, y, _.OwnerId == QuantumRunner.Default.NetworkClient.LocalPlayer.ActorNumber);
					
					SetInteraction(false);
				}).AddTo(this);

			GameSubscriber.OnStoneDestroy
				.Subscribe(_ =>
				{
					if (_.X != x || _.Y != y)
						return;
					
					if (instance)
						Destroy(instance);
					CurrentStone = default;
					
					SetInteraction(true);
				}).AddTo(this);
		}

		private void SetInteraction(bool use)
		{
			Button.interactable = use;
		}

		public void SetPosition(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
	}
}