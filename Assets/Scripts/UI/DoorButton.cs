using UnityEngine;
using System;
using System.Collections.Generic;

public class DoorButton : MonoBehaviour {
	public Messenger message;

	public float duration = .5f;
	public List<ButtonState> states = new List<ButtonState>();
	ButtonState current;

	void Start() {
		if (message == null || states.Count == 0) {
			enabled = false;
			return;
		}

		current = states[0];
		message.Register<DoorStateMessage>(ProcessDoorState);
	}

	public void ProcessDoorState(DoorStateMessage m) {
		foreach (ButtonState state in states) {
			if (m == state.receive) {
				current = state;

				foreach (ButtonObjectState objstate in current.objectStates) {
					TweenPosition.Begin(objstate.obj, duration, objstate.position).method = UITweener.Method.EaseInOut;
					TweenRotation.Begin(objstate.obj, duration, Quaternion.Euler(objstate.rotation)).method = UITweener.Method.EaseInOut;
					TweenColor.Begin(objstate.obj, duration, objstate.color).method = UITweener.Method.EaseInOut;
				}

				return;
			}
		}
		current = null;
	}

	void OnPress(bool isPressed) {
		if (isPressed && current != null) {
			message.Send<ButtonMessage>(current.send);
		}
	}
}

[Serializable]
public class ButtonObjectState {
	public GameObject obj;
	public Vector3 position;
	public Vector3 rotation;
	public Color color = Color.white;
}

[Serializable]
public class ButtonState {
	public DoorStateMessage receive;
	public ButtonMessage send;
	public List<ButtonObjectState> objectStates = new List<ButtonObjectState>();
}