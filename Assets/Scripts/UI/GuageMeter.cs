using UnityEngine;
using System.Collections;

public class GuageMeter : MonoBehaviour {
	public Vector2 degreeRange = new Vector2(0, -180);
	public Vector2 valueRange = new Vector2(50f, 150f);
	public string suffix;

	public UILabel valueLabel;
	public Transform needle;
	
	float slope;

	void Start() {
		slope = (degreeRange.y-degreeRange.x)/(valueRange.y-valueRange.x);
	}

	public float Value {
		set {
			float clampedValue = Mathf.Clamp((value - valueRange.x)*slope, degreeRange.x, degreeRange.y);

			TweenRotation.Begin(needle.gameObject, .4f, Quaternion.Euler(0,0, clampedValue));

			valueLabel.text = string.Format("{0:F2} "+suffix, value);
		}
	}
}