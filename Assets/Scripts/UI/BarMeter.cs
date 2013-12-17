using UnityEngine;
using System.Collections;

public class BarMeter : MonoBehaviour {
	public UILabel nameLabel;
	public UILabel valueLabel;
	public UISlider slider;
	
	public float minValue = .1f;
	public float barHeight = 10f;

	public string Name {
		set {nameLabel.text = value;}
	}
	
	public float Value {
		set {
			float clampValue = Mathf.Clamp(Mathf.Sqrt(value), minValue, 1f);

			slider.sliderValue = clampValue;
			valueLabel.transform.localPosition = new Vector3(0f, barHeight*clampValue);

			if (value >= .995f)
				valueLabel.text = "1.0";
			else 
				valueLabel.text = string.Format("{0:F2}", value);
		}
	}
}
