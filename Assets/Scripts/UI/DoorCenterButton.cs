/*
 * Created by copying the UIButtonScale script from NGUI, then modifying it
 */

using UnityEngine;
using System.Collections;

public class DoorCenterButton : MonoBehaviour {
	public Door door;

	public Vector3 hover = new Vector3(1.1f, 1.1f, 1.1f);
	public Vector3 pressed = new Vector3(0.9f, 0.9f, 0.9f);
	public Vector3 shutdown = new Vector3(.1f, .1f, .1f);
	public float duration = 0.2f;

	TweenScale tween;
	Vector3 mScale;
	bool mInitDone = false;
	bool mShutdown = false;
	
	void Start() {
		door.Sealed += On;
		door.Unsealing += Shutdown;
	}
	
	void OnEnable() {
		//Debug.Log ("Re-enabled");
		if (mShutdown) mShutdown = false;
		OnHover(false);
	}
	
	void OnDisable() {
//		Debug.Log("Disabled");
		TweenScale tc = GetComponent<TweenScale>();
		if (tc != null) {
			Destroy(tc);
		}
	}
	
	void Init() {
		mInitDone = true;
		mScale = transform.localScale;
	}
	
	void OnPress(bool isPressed) {
		if (enabled && isPressed && !mShutdown) {
			if (!mInitDone) Init();

			tween = TweenScale.Begin(gameObject, duration, Vector3.Scale(mScale, pressed));
			tween.method = UITweener.Method.EaseInOut;
			tween.eventReceiver = null;

			door.Open();
		}
	}
	
	void OnHover(bool isOver) {
		if (enabled && !mShutdown) {
			if (!mInitDone) Init();

			tween = TweenScale.Begin(gameObject, duration, isOver ? Vector3.Scale(mScale, hover) : mScale);
			tween.method = UITweener.Method.EaseInOut;
			tween.eventReceiver = null;
		}
	}

	public void Shutdown() {
//		Debug.Log("Shutting down");
		mShutdown = true;
		tween = TweenScale.Begin(gameObject, duration, shutdown);
		tween.method = UITweener.Method.EaseInOut;
		tween.eventReceiver = gameObject;
		tween.callWhenFinished = "Off";
	}

	public void Off() {
//		Debug.Log("Off");
		NGUITools.SetActive(transform.parent.gameObject, false);
	}

	public void On() {
//		Debug.Log("On");
		NGUITools.SetActive(transform.parent.gameObject, true);
	}
}
