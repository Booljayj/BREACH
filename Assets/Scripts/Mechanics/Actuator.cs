using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ConfigurableJoint))]
public class Actuator : MonoBehaviour {
	public Messenger message;

	ConfigurableJoint joint;

	// Use this for initialization
	void Start () {
		if (message == null) {
			enabled = false;
			return;
		}
		message.Register<DoorStateMessage>(ProcessInput);

		joint = GetComponent<ConfigurableJoint>();
	}

	void ProcessInput(DoorStateMessage m) {
		if (m == DoorStateMessage.Open) {
			return;
		}
	}
}
