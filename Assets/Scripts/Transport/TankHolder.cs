using UnityEngine;
using System.Collections;

public class TankHolder : Holder {
	public GasInterchange interchange;
	public string channel = "None";

	Tank connectedTank;

	public override void Connect(Item item) {
		connectedTank = item.GetComponent<Tank>();
		if (connectedTank == null)
			throw new MissingComponentException("TankHolder was expecting a Tank component");
	}

	public override void Disconnect() {
		connectedTank = null;
	}

	public override void Lock() {
		if (locked || connectedTank == null) return;
		interchange.AddTank(connectedTank, channel);
		locked = true;
	}

	public override void Unlock() {
		if (!locked || connectedTank == null) return;
		interchange.RemoveTank(connectedTank, channel);
		locked = false;
	}
}