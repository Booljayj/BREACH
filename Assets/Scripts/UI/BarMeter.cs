using UnityEngine;
using System.Collections;

public class BarMeter : MonoBehaviour {
	public Vector2 valueRange = new Vector2(0f, 1f);
	public Vector2 sliderRange = new Vector2(.1f, 1f);

	public UILabel valueLabel;
	public UISlider slider;

	float maxHeight;
	float slope;

	void Start() {
		if (!valueLabel || !slider) {
			enabled = false;
			return;
		}

		maxHeight = slider.fullSize.y;
		slope = (sliderRange.y-sliderRange.x)/(valueRange.y-valueRange.x);
	}

	public float Value {
		set {
			float clampedValue = Mathf.Clamp(Mathf.Sqrt((value-valueRange.x)*slope), sliderRange.x, sliderRange.y);

			slider.sliderValue = clampedValue;
			TweenPosition.Begin(valueLabel.gameObject, .4f, new Vector3(0f, maxHeight*clampedValue, 0f));

			if (value >= .995f) valueLabel.text = "1.0";
			else valueLabel.text = string.Format(".{0}", Mathf.FloorToInt(value*100f));
		}
	}
}
