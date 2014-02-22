using UnityEngine;
using System.Collections;

public class TankHolder : Holder {
	public GasInterchange interchange;
	public string channel = "None";
	public Animator animator;

	Tank connectedTank;

	void Start() {
		if (!socket) {
			enabled = false; return;
		}

		socket.Connected += Connect;
		socket.Disconnected += Disconnect;
	}

	public override void Connect(Item item) {
		connectedTank = item.GetComponent<Tank>();
		//Close();
	}

	public override void Disconnect() {
		connectedTank = null;
	}

	public override void Close() {
		animator.SetBool("toOpen", false);
	}

	public override void Open() {
		animator.SetBool("toOpen", true);
	}

	public override void Activate() {
		if (_isActivated || !animator.GetCurrentAnimatorStateInfo(0).IsName("IdleClosed")) return;
		interchange.AddTank(connectedTank, channel);
		isActivated = true;
	}

	public override void Deactivate() {
		if (!_isActivated) return;
		interchange.RemoveTank(connectedTank, channel);
		isActivated = false;
	}

	#region Event Functions

	#endregion
}