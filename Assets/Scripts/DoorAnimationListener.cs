using UnityEngine;
using System.Collections;

public class DoorAnimationListener : MonoBehaviour {
	public Messenger message;

	private Door door;
	private Animator animator;

	public void Start() {
		if (message == null) {
			enabled = false;
			return;
		}

		door = transform.parent.GetComponent<Door>();
		animator = GetComponent<Animator>();

	}

	public void ProcessDoorState(DoorStateMessage m) {
		AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
		if (m == DoorStateMessage.Open) {
			animator.CrossFade("OpenState", .1f, 0, 1f-state.normalizedTime);
		} else if (m == DoorStateMessage.Close) {
			animator.CrossFade("CloseState", .1f, 0, 1f-state.normalizedTime);
		}
	}

	//called at the beginning of the Unsealing animation
	public void UnsealStart() {
		if (animator.GetBool("Open")) {
			door.OnUnsealing();
		} else {
			door.OnSealed();
		}
	}

	//called at the end of the Unsealing animation
	public void UnsealEnd() {
		if (animator.GetBool("Open")) {
			door.OnUnsealed();
		} else {
			door.OnSealing();
		}
	}

	//called at the beginning of the opening animation
	public void OpenStart() {
		if (animator.GetBool("Open")) {
			door.OnOpening();
		} else {
			door.OnClosed();
		}
	}

	//called at the end of the opening animation
	public void OpenEnd() {
		if (animator.GetBool("Open")) {
			door.OnOpened();
		} else {
			door.OnClosing();
		}
	}
}
