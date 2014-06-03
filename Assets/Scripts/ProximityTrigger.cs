using UnityEngine;
using System.Collections;

public class ProximityTrigger : MonoBehaviour {
	public Messenger message;

	public string tag;
	public ProximityMessage enterMessage;
	public ProximityMessage exitMessage;

	void OnTriggerEnter(Collider other) {
		if (!string.IsNullOrEmpty(tag) && other.tag != tag) {
			return;
		} else {
			if (enterMessage != ProximityMessage.Null) {
				message.Send<ProximityMessage>(enterMessage);
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (!string.IsNullOrEmpty(tag) && other.tag != tag) {
			return;
		} else {
			if (exitMessage != ProximityMessage.Null) {
				message.Send<ProximityMessage>(exitMessage);
			}
		}
	}
}
