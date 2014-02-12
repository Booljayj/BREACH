using UnityEngine;
using System.Collections;

public class DoorLockButton : MonoBehaviour {
	public Door door;
	
	public float duration;
	
	public GameObject lockTop;
	public GameObject lockBottom;
	public Vector3 tLockedPos = new Vector3(0,0,0);
	public Vector3 bLockedPos = new Vector3(0,0,0);
	public Color tLockedCol = Color.white;
	public Color bLockedCol = Color.white;
	Vector3 tUnlockedPos;
	Vector3 bUnlockedPos;
	Color tUnlockedCol;
	Color bUnlockedCol;
	
	void Start() {
		door.Locked += IsLocked;
		door.Unlocked += IsUnlocked;
		
		tUnlockedPos = lockTop.transform.localPosition;
		bUnlockedPos = lockBottom.transform.localPosition;
		tUnlockedCol = lockTop.GetComponent<UISprite>().color;
		bUnlockedCol = lockBottom.GetComponent<UISprite>().color;
	}
	
	void OnPress(bool isPressed) {
		if (isPressed) {
			door.isLocked = !door.isLocked;
		}
	}
	
	public void IsLocked() {
		TweenPosition.Begin(lockTop, duration, tLockedPos).method = UITweener.Method.EaseInOut;
		TweenPosition.Begin(lockBottom, duration, bLockedPos).method = UITweener.Method.EaseInOut;
		TweenColor.Begin(lockTop, duration, tLockedCol).method = UITweener.Method.EaseInOut;
		TweenColor.Begin(lockBottom, duration, bLockedCol).method = UITweener.Method.EaseInOut;
	}
	
	public void IsUnlocked() {
		TweenPosition.Begin(lockTop, duration, tUnlockedPos).method = UITweener.Method.EaseInOut;
		TweenPosition.Begin(lockBottom, duration, bUnlockedPos).method = UITweener.Method.EaseInOut;
		TweenColor.Begin(lockTop, duration, tUnlockedCol).method = UITweener.Method.EaseInOut;
		TweenColor.Begin(lockBottom, duration, bUnlockedCol).method = UITweener.Method.EaseInOut;
	}
}

