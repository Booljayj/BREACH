using UnityEngine;
using System.Collections;

public class DoorAutoButton : MonoBehaviour {
	public Door door;
	
	public float duration;
	
	public GameObject radio;
	public Quaternion autoRot = Quaternion.identity;
	public Color autoCol = Color.white;
	Quaternion manRot;
	Color manCol;
	
	void Start() {
		door.DoorAutomatic += IsAutomatic;
		door.DoorManual += IsManual;
		
		manRot = radio.transform.localRotation;
		manCol = radio.GetComponent<UISprite>().color;
	}
	
	void OnPress(bool isPressed) {
		if (isPressed) {
			door.Automatic = !door.Automatic;
		}
	}
	
	public void IsAutomatic() {
		TweenRotation.Begin(radio, duration, autoRot).method = UITweener.Method.EaseInOut;
		TweenColor.Begin(radio, duration, autoCol).method = UITweener.Method.EaseInOut;
	}
	
	public void IsManual() {
		TweenRotation.Begin(radio, duration, manRot).method = UITweener.Method.EaseInOut;
		TweenColor.Begin(radio, duration, manCol).method = UITweener.Method.EaseInOut;
	}
}

