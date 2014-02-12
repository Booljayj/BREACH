using UnityEngine;
using System.Collections;

public class BarMeter : MonoBehaviour {
	public UILabel valueLabel;
	public UISlider slider;
	
	public float minValue = .1f;
	public float barHeight = 10f;

	void Start() {
		barHeight = slider.fullSize.y;
		slider = GetComponent<UISlider>();
	}

	public float Value {
		set {
			float clampValue = Mathf.Clamp(Mathf.Sqrt(value), minValue, 1f);

			slider.sliderValue = clampValue;
			TweenPosition.Begin(valueLabel.gameObject, .4f, new Vector3(0f, barHeight*clampValue, 0f));

			if (value >= .995f)
				valueLabel.text = "1.0";
			else 
				valueLabel.text = string.Format(".{0}", Mathf.FloorToInt(value*100f));
		}
	}
}
