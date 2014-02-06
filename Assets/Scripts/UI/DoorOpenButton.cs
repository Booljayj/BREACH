using UnityEngine;
using System.Collections;

public class DoorOpenButton : MonoBehaviour {
	public Door door;

	public float duration;
	
	public GameObject lArrow;
	public GameObject rArrow;
	public Vector3 lOpenPos = new Vector3(125,0,0);
	public Vector3 rOpenPos = new Vector3(-125,0,0);
	public Color lOpenCol = Color.white;
	public Color rOpenCol = Color.white;
	Vector3 lClosedPos;
	Vector3 rClosedPos;
	Color lClosedCol;
	Color rClosedCol;

	void Start() {
		door.Unsealing += IsOpen;
		door.Opening += IsOpen;
		door.Sealing += IsClosed;
		door.Closing += IsClosed;

		lClosedPos = lArrow.transform.localPosition;
		rClosedPos = rArrow.transform.localPosition;
		lClosedCol = lArrow.GetComponent<UISprite>().color;
		rClosedCol = rArrow.GetComponent<UISprite>().color;
	}

	void OnPress(bool isPressed) {
		if (isPressed) {
			door.Toggle();
		}
	}

	public void IsOpen() {
		TweenPosition.Begin(lArrow, duration, lOpenPos).method = UITweener.Method.EaseInOut;
		TweenPosition.Begin(rArrow, duration, rOpenPos).method = UITweener.Method.EaseInOut;
		TweenColor.Begin(lArrow, duration, lOpenCol).method = UITweener.Method.EaseInOut;
		TweenColor.Begin(rArrow, duration, rOpenCol).method = UITweener.Method.EaseInOut;
	}

	public void IsClosed() {
		TweenPosition.Begin(lArrow, duration, lClosedPos).method = UITweener.Method.EaseInOut;
		TweenPosition.Begin(rArrow, duration, rClosedPos).method = UITweener.Method.EaseInOut;
		TweenColor.Begin(lArrow, duration, lClosedCol).method = UITweener.Method.EaseInOut;
		TweenColor.Begin(rArrow, duration, rClosedCol).method = UITweener.Method.EaseInOut;
	}
}
