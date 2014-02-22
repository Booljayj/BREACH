using UnityEngine;
using System.Collections;

public class TankHolderAnimationListener : MonoBehaviour {
	TankHolder holder;
	Animator animator;
	
	void Start () {
		holder = transform.parent.GetComponent<TankHolder>();
		animator = GetComponent<Animator>();
	}

	public void AnimBegin() {
		if (animator.GetBool("toOpen")) {
			holder.OnOpened();
		} else {
			holder.OnClosing();
		}
	}

	public void AnimEnd() {
		if (animator.GetBool("toOpen")) {
			holder.OnOpening();
		} else {
			holder.OnClosed();
		}
	}
}
