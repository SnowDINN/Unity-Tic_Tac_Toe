using System.Linq;
using R3;
using Redbean.Network;
using UnityEngine;
using UnityEngine.UI;

namespace Redbean.Game
{
	public class GameDisconnectUi : MonoBehaviour
	{
		[SerializeField] private Button[] disconnectButtons;
		
		private void Awake()
		{
			disconnectButtons
				.Select(_ => _.AsButtonObservable())
				.Merge()
				.Subscribe(async _ =>
				{
					await NetworkManager.Default.Disconnect(NetworkCommonValue.UserLeave);
				}).AddTo(this);
		}
	}
}