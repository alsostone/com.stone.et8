using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HUDIndicator {

	[AddComponentMenu("HUD Indicator/Indicator On Screen")]
	public class IndicatorOnScreen : Indicator {
		public IndicatorIconStyle style;

		protected override void CreateIndicatorCanvas(IndicatorRenderer renderer) {
			IndicatorCanvasOnScreen indicatorCanvasOnScreen = new IndicatorCanvasOnScreen();
			indicatorCanvasOnScreen.Create(this, renderer);

			indicatorsCanvas.Add(renderer, indicatorCanvasOnScreen);
		}
	}

}