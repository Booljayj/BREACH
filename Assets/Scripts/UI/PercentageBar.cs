using UnityEngine;
using System.Collections;

public class PercentageBar : MonoBehaviour {
	public UILabel name;
	public UILabel val;
	public UISlider slider;
	
	float minValue = .1f;

	public string Name {
		get {
			return name.text;
		}
		set {
			name.text = value;
		}
	}
	
	public float Value {
		get {
			return slider.sliderValue;
		}
		set {
			float clampValue = Mathf.Clamp(Mathf.Sqrt(value), minValue, 1f);

			slider.sliderValue = clampValue;
			val.transform.localPosition = new Vector3(0f, slider.fullSize.x*clampValue);

			if (value >= .995f)
				val.text = "1.0";
			else 
				val.text = string.Format(".{0:F0}", value*100f);
		}
	}
}
