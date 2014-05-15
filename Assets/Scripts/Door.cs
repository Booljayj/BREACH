using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class Door : MonoBehaviour {
	#region Events
	public event EventHandler Unsealing;
	public event EventHandler Unsealed;
	public event EventHandler Opening;
	public event EventHandler Opened;
	public event EventHandler Closing;
	public event EventHandler Closed;
	public event EventHandler Sealing;
	public event EventHandler Sealed;

	public event EventHandler Locked;
	public event EventHandler Unlocked;

	public event EventHandler Automatic;
	public event EventHandler Manual;
	#endregion

	public bool open;

	#region Properties
	private bool _isLocked;
	public bool isLocked {
		get {
			return _isLocked;
		}
		set {
			if (_isLocked != value) {
				_isLocked = value;
				if (_isLocked && Locked != null) Locked();
				if (!_isLocked && Unlocked != null) Unlocked();
			}
		}
	}

	private bool _isAutomatic;
	public bool isAutomatic {
		get {
			return _isAutomatic;
		}
		set {
			if (_isAutomatic != value) {
				_isAutomatic = value;
				if (_isAutomatic && Automatic != null) Automatic();
				if (!_isAutomatic && Manual != null) Manual();
			}
		}
	}
	#endregion

	#region Variables
	private Airlock airlock;
	private Animator animator;
	private List<GameObject> obstructions = new List<GameObject>();
	#endregion

	#region Startup
	void Start() {
		airlock = GetComponent<Airlock>();
		animator = GetComponentInChildren<Animator>();

		if (airlock != null) {
			Opening += airlock.Open;
			Closed	+= airlock.Close;
		}
	}
	#endregion

	#region Activation
	public void Open() {
		if (_isLocked || animator.GetBool("Open")) return;

		animator.SetBool("Open", true);
		AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
		if (state.IsName("Sealing")) {
			animator.CrossFade("Unsealing", .05f, 0, 1f-state.normalizedTime);
			OnUnsealing();
		} else if (state.IsName("Closing")) {
			animator.CrossFade("Opening", .05f, 0, 1f-state.normalizedTime);
			OnOpening();
		}
	}
	
	public void Close() {
		if (_isLocked || !animator.GetBool("Open")) return;
		if (obstructions.Count > 0) return;

		animator.SetBool("Open", false);
		AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
		if (state.IsName("Unsealing")) {
			animator.CrossFade("Sealing", .05f, 0, 1f-state.normalizedTime);
			OnSealing();
		} else if (state.IsName("Opening")) {
			animator.CrossFade("Closing", .05f, 0, 1f-state.normalizedTime);
			OnClosing();
		}
	}
	
	public void Toggle() {
		if (animator.GetBool("Open")) {
			Close();
		} else {
			Open();
		}
	}
	#endregion

	#region Proximity
	void OnTriggerEnter(Collider other) {
		//Debug.Log("Obstruction detected");
		if (other != null) {
			obstructions.Add(other.gameObject);
		}
	}

	void OnTriggerExit(Collider other) {
		if (other != null) {
			obstructions.Remove(other.gameObject);
		}
	}
	#endregion

	#region Event Functions
	public void OnUnsealing() {
		if (Unsealing != null) Unsealing();
		//Debug.Log("Unsealing Door");
	}
	public void OnUnsealed() {
		if (Unsealed != null) Unsealed();
		//Debug.Log("Unsealed Door");
	}
	public void OnOpening() {
		if (Opening != null) Opening();
		//Debug.Log("Opening Door");
	}
	public void OnOpened() {
		if (Opened != null) Opened();
		//Debug.Log("Opened Door");
	}
	public void OnClosing() {
		if (Closing != null) Closing();
		//Debug.Log("Closing Door");
	}
	public void OnClosed() {
		if (Closed != null) Closed();
		//Debug.Log("Closed Door");
	}
	public void OnSealing() {
		if (Sealing != null) Sealing();
		//Debug.Log("Sealing Door");
	}
	public void OnSealed() {
		if (Sealed != null) Sealed();
		//Debug.Log("Sealed Door");
	}
	#endregion
}
