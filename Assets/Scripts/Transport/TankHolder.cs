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
		OnConnect();
	}

	public override void Disconnect() {
		connectedTank = null;
		OnDisconnect();
	}

	public override void Lock() {
		if (isLocked || connectedTank == null) return;
		interchange.AddTank(connectedTank, channel);
		isLocked = true;
		OnLock();
	}

	public override void Unlock() {
		if (!isLocked || connectedTank == null) return;
		interchange.RemoveTank(connectedTank, channel);
		isLocked = false;
		OnUnlock();
	}
}