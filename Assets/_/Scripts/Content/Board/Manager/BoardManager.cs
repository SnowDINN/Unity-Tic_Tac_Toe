using System.Linq;
using Quantum;
using R3;
using Redbean.Network;
using UnityEngine;

namespace Redbean.Content
{
	public class BoardManager : MonoBehaviour
	{
		private static BoardLine[] components;
		
		private void Awake()
		{
			GameSubscriber.OnInteraction
				.Subscribe(_ =>
				{
					QuantumRunner.DefaultGame.SendCommand(new QCommandBoardInteraction
					{
						Entity = NetworkAsset.Stone,
						X = _.X,
						Y = _.Y,
					});
				}).AddTo(this);
			
			components = GetComponentsInChildren<BoardLine>();
			
			var componentArray = components.Select((value, index) => (value, index));
			foreach (var component in componentArray)
				component.value.SetPosition(component.index);
		}

		public static bool IsOwner(int x, int y)
		{
			if (y < 0 || y >= components.Length)
				return false;
			
			return components[y].GetStone(x) && components[y].GetStone(x).IsOwner;
		}
	}
}