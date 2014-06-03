using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class DoorMotion : MonoBehaviour {
	public Messenger message;

	Animator animator;
	bool doOpen;

	// Use this for initialization
	void Start () {
		if (!message) {
			enabled = false;
			Debug.LogError(name + " requires a reference to a messenger");
			return;
		}

		message.Register<DoorStateMessage>(ProcessDoorState);

		animator = GetComponent<Animator>();
	}

	void ProcessDoorState(DoorStateMessage m) {
		AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

		if (m == DoorStateMessage.Open && !doOpen) {
			doOpen = true;
			if (state.IsName("Closed")) {
				animator.Play("Unsealing");
			} else if (state.IsName("Closing")) {
				animator.CrossFade("Opening", .1f, 0, 1f-state.normalizedTime);
			} else {
				animator.CrossFade("Unsealing", .1f, 0, 1f-state.normalizedTime);
			}
		} else if (m == DoorStateMessage.Close && doOpen) {
			doOpen = false;
			if (state.IsName("Opened")) {
				animator.Play("Closing");
			} else if (state.IsName("Opening")) {
				animator.CrossFade("Closing", .1f, 0, 1f-state.normalizedTime);
			} else {
				animator.CrossFade("Sealing", .1f, 0, 1f-state.normalizedTime);
			}
		}
	}

	void DoorUnsealStart() {
		if (doOpen) {
			message.Send<DoorStateMessage>(DoorStateMessage.Unsealing);
			message.Send<AirlockMessage>(AirlockMessage.PartialOpen);
		} else {
			message.Send<DoorStateMessage>(DoorStateMessage.Sealed);
			message.Send<AirlockMessage>(AirlockMessage.PartialClose);
		}
	}

	void DoorUnsealEnd() {
		if (doOpen) {
			message.Send<DoorStateMessage>(DoorStateMessage.Unsealed);
		} else {
			message.Send<DoorStateMessage>(DoorStateMessage.Sealing);
		}
	}

	void DoorOpenStart() {
		if (doOpen) {
			message.Send<DoorStateMessage>(DoorStateMessage.Opening);
			message.Send<AirlockMessage>(AirlockMessage.FullOpen);
		} else {
			message.Send<DoorStateMessage>(DoorStateMessage.Closed);
			message.Send<AirlockMessage>(AirlockMessage.FullClose);
		}
	}

	void DoorOpenEnd() {
		if (doOpen) {
			message.Send<DoorStateMessage>(DoorStateMessage.Opened);
		} else {
			message.Send<DoorStateMessage>(DoorStateMessage.Closing);
		}
	}
}
