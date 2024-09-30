using Quantum;
using R3;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace Redbean.Content
{
	public class InteractionButton : MonoBehaviour
	{
		[SerializeField]
		private GameObject Prefab;
		
		[SerializeField]
		private Button Button;
		
		private int Position;

		private void Awake()
		{
			Button.AsButtonObservable()
				.Subscribe(_ =>
				{
					GameSubscriber.Interaction(Position);
				}).AddTo(this);

			GameSubscriber.OnSpawn
				.Subscribe(_ =>
				{
					if (_.position != Position)
						return;

					var go = Instantiate(Prefab, transform);
					var stone = go.GetComponent<StoneReceiver>();
					stone.UpdateView(_.owner == QuantumRunner.Default.NetworkClient.LocalPlayer.ActorNumber);
						
					SetInteraction(false);
				}).AddTo(this);
		}

		private void Start()
		{
			Position = InteractionManager.GetIndex(GetInstanceID());
		}

		private void SetInteraction(bool use)
		{
			Button.interactable = use;
		}
	}
}