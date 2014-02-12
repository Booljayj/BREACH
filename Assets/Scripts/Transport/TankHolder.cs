using UnityEngine;
using System.Collections;

public class TankHolder : Holder {
	public GasInterchange interchange;
	public string channel = "None";

	Tank connectedTank;

	public override void Connect(Item item) {
		if (animation.isPlaying) return;
		if (!isOpen) return;

		connectedTank = item.GetComponent<Tank>();
		if (connectedTank == null)
			throw new MissingComponentException("Tank holder was expecting a tank component");
		else {
			isOpen = false;
			animation.Play("Close");
		}
	}

	public override void Disconnect() {
		if (animation.isPlaying) return;
		if (isOpen || isActive) return;

		connectedTank = null;
	}

	public override void Activate() {
		if (isOpen || connectedTank == null) return;
		interchange.AddTank(connectedTank, channel);
		isOpen = true;
	}

	public override void Deactivate() {
		if (!isOpen || connectedTank == null) return;
		interchange.RemoveTank(connectedTank, channel);
		isOpen = false;
	}
}