using R3;
using Redbean.Network;
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

		public StoneListener CurrentStone;

		private void Awake()
		{
			Button.AsButtonObservable()
				.Subscribe(_ =>
				{
					GameSubscriber.SetBoardSelect(new EVT_Position
					{
						X = x,
						Y = y
					});
				}).AddTo(this);

			GameSubscriber.OnStoneCreate
				.Subscribe(_ =>
				{
					if (_.Position.X != x || _.Position.Y != y)
						return;

					instance = Instantiate(Prefab, transform);
					CurrentStone = instance.GetComponent<StoneListener>();
					CurrentStone.UpdateView(x, y, _.OwnerId == NetworkSetting.LocalPlayerId);
					
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