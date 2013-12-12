using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
	public enum DoorState {Closed, Opening, Open, Closing};

	public delegate void EventHandler();
	public event EventHandler DoorOpened;
	public event EventHandler DoorOpening;
	public event EventHandler DoorClosed;
	public event EventHandler DoorClosing;
	public event EventHandler DoorLocked;
	public event EventHandler DoorUnlocked;
	
	public Transform leftDoor;
	public Transform rightDoor;
	public Vector3 doorPosition = new Vector3(-1.3f,0f,0f);
	public float speed = 5f;
	
	public DoorState State {get; private set;}
	private bool _locked;
	public bool Locked {
		get {
			return _locked;
		}
		set {
			if (_locked != value) {
				_locked = value;
				if (_locked && DoorLocked != null) DoorLocked();
				if (!_locked && DoorUnlocked != null) DoorUnlocked();
			}
		}
	}

	private Airlock airlock;

	void Start() {
		DoorOpening += airlock.Open;
		DoorClosed	+= airlock.Close;
	}

	#region Activation
	public void Open() {
		if (_locked || State == DoorState.Open || State == DoorState.Opening) return;
		StopAllCoroutines();
		StartCoroutine("OpenDoors");
	}
	
	public void Close() {
		if (_locked || State == DoorState.Closed || State == DoorState.Closing) return;
		StopAllCoroutines();
		StartCoroutine("CloseDoors");
	}
	
	public void Toggle() {
		if (_locked) return;
		
		StopAllCoroutines();
		if (State == DoorState.Open || State == DoorState.Opening) StartCoroutine("CloseDoors");
		else StartCoroutine("OpenDoors");
	}
	#endregion

	#region Coroutines
	IEnumerator OpenDoors() {
		State = DoorState.Opening;
		if (DoorOpening != null) DoorOpening();
		
		while (Vector3.Distance(leftDoor.localPosition, doorPosition) > .01f) {
			leftDoor.localPosition = Vector3.Lerp(leftDoor.localPosition, -doorPosition, speed*Time.deltaTime);
			rightDoor.localPosition = Vector3.Lerp(rightDoor.localPosition, doorPosition, speed*Time.deltaTime);
			yield return null;
		}
		leftDoor.localPosition = -doorPosition;
		rightDoor.localPosition = doorPosition;
		
		State = DoorState.Open;
		if (DoorOpened != null) DoorOpened();
	}
	
	IEnumerator CloseDoors() {
		State = DoorState.Closing;
		if (DoorClosing != null) DoorClosing();
		
		while (Vector3.Distance(leftDoor.localPosition, Vector3.zero) > .01f) {
			leftDoor.localPosition = Vector3.Lerp(leftDoor.localPosition, Vector3.zero, speed*Time.deltaTime);
			rightDoor.localPosition = Vector3.Lerp(rightDoor.localPosition, Vector3.zero, speed*Time.deltaTime);
			yield return null;
		}
		leftDoor.localPosition = Vector3.zero;
		rightDoor.localPosition = Vector3.zero;
		
		State = DoorState.Closed;
		if (DoorClosed != null) DoorClosed();
	}
	#endregion
}
