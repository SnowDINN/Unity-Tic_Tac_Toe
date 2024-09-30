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
		
		private int Index;

		private void Awake()
		{
			Button.AsButtonObservable()
				.Subscribe(_ =>
				{
					GameSubscriber.Interaction(Index);
				}).AddTo(this);

			GameSubscriber.OnSpawn
				.Subscribe(_ =>
				{
					if (_.index != Index)
						return;

					var go = Instantiate(Prefab, transform);
					var stone = go.GetComponent<StoneReceiver>();
					stone.UpdateView(_.Owner == QuantumRunner.Default.NetworkClient.LocalPlayer.ActorNumber);
						
					SetInteraction(false);
				}).AddTo(this);
		}

		private void Start()
		{
			Index = InteractionManager.GetIndex(GetInstanceID());
		}

		private void SetInteraction(bool use)
		{
			Button.interactable = use;
		}
	}
}