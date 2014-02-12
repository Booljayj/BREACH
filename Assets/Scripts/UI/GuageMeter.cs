using UnityEngine;
using System.Collections;

public class GuageMeter : MonoBehaviour {
	public UILabel valueLabel;
	public Transform needle;

	public Vector2 degreeRange = new Vector2(0, -180);
	public Vector2 valueRange = new Vector2(50f, 150f);
	public string suffix;
	float slope;
	Vector2 clampRange;
	string format;

	void Start() {
		slope = (degreeRange.y-degreeRange.x)/(valueRange.y-valueRange.x);
		if (degreeRange.x > degreeRange.y)
			clampRange = new Vector2(degreeRange.y, degreeRange.x);
		else
			clampRange = new Vector2(degreeRange.x, degreeRange.y);
		format = "{0:F2} "+suffix;
	}

	public float Value {
		set {
			valueLabel.text = string.Format("{0:F2} "+suffix, value);

			TweenRotation.Begin(needle.gameObject, .4f, Quaternion.Euler(0,0, Mathf.Clamp((value - valueRange.x)*slope, clampRange.x, clampRange.y)));
		}
	}
}