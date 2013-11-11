using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
	public enum DoorState {Closed, Opening, Open, Closing};
	public Transform leftDoor;
	public Transform rightDoor;
	public Vector3 leftPosition = new Vector3(1,0,0);
	public Vector3 rightPosition = new Vector3(-1,0,0);
	
	public DoorState State {get; private set;}
	public bool Locked {get; set;}

	void Open() {
		if (Locked || State == DoorState.Open || State == DoorState.Opening) return;
		StopAllCoroutines();
		StartCoroutine("OpenDoors");
	}
	
	void Close() {
		if (Locked || State == DoorState.Closed || State == DoorState.Closing) return;
		StopAllCoroutines();
		StartCoroutine("CloseDoors");
	}
	
	void Toggle() {
		if (Locked) return;
		
		StopAllCoroutines();
		if (State == DoorState.Open || State == DoorState.Opening) StartCoroutine("CloseDoors");
		else StartCoroutine("OpenDoors");
	}
	
	IEnumerator OpenDoors() {
		State = DoorState.Opening;
		while (Vector3.Distance(leftDoor.localPosition, leftPosition) > .05f) {
			leftDoor.localPosition = Vector3.Lerp(leftDoor.localPosition, leftPosition, Time.deltaTime);
			rightDoor.localPosition = Vector3.Lerp(rightDoor.localPosition, rightPosition, Time.deltaTime);
			yield return null;
		}
		leftDoor.localPosition = leftPosition;
		rightDoor.localPosition = rightPosition;
		State = DoorState.Open;
	}
	
	IEnumerator CloseDoors() {
		State = DoorState.Closing;
		while (Vector3.Distance(leftDoor.localPosition, Vector3.zero) > .05f) {
			leftDoor.localPosition = Vector3.Lerp(leftDoor.localPosition, Vector3.zero, Time.deltaTime);
			rightDoor.localPosition = Vector3.Lerp(rightDoor.localPosition, Vector3.zero, Time.deltaTime);
			yield return null;
		}
		leftDoor.localPosition = Vector3.zero;
		rightDoor.localPosition = Vector3.zero;
		State = DoorState.Closed;
	}
}
