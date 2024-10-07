using System;
using R3;
using UnityEngine.UI;

namespace Redbean
{
	public static class Extension
	{
		public static Observable<Unit> AsButtonObservable(this Button button, int inputThrottle = 200) =>
			inputThrottle > 0 
				? button.onClick.AsObservable().Share().ThrottleFirst(TimeSpan.FromMilliseconds(inputThrottle)) 
				: button.onClick.AsObservable().Share();
	}
}