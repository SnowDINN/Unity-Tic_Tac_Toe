using System;
using Photon.Deterministic;
using Quantum;
using R3;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace Redbean
{
	public static class MonoExtension
	{
		public static Observable<Unit> AsButtonObservable(this Button button, int inputThrottle = 200) =>
			inputThrottle > 0 
				? button.onClick.AsObservable().Share().ThrottleFirst(TimeSpan.FromMilliseconds(inputThrottle)) 
				: button.onClick.AsObservable().Share();

		public static void NetworkEventPublish<T>(this MonoBehaviour mono, T value) where T : DeterministicCommand =>
			QuantumRunner.DefaultGame.SendCommand(value);
	}
}